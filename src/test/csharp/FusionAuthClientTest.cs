/*
 * Copyright (c) 2018, FusionAuth, All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific
 * language governing permissions and limitations under the License.
 */

using System;
using System.Net;
using FusionAuth;
using FusionAuth.Domain;
using Inversoft.Restify;
using NUnit.Framework;

namespace FusionAuthCSharpClientTest
{
  public class TestBuilder
  {
    public const string ApiKey = "bf69486b-4733-4470-a592-f1bfce7af580";

    public static readonly Guid ApplicationId = new Guid("4eedf18a-9360-40f6-a36c-88269ed5ec55");

    public const string EmailAddress = "csharpclient@fusionauth.io";

    public Application Application;

    public FusionAuthClient Client;

    public string Token;

    public User User;

    public TestBuilder()
    {
      Client = new FusionAuthClient(ApiKey, "http://localhost:9011");
    }

    public FusionAuthClient NewClientWithTenantId(Guid tenantId)
    {
      return new FusionAuthClient(ApiKey, "http://localhost:9011", tenantId.ToString());
    }

    public TestBuilder AssertSuccess<T, U>(ClientResponse<T, U> response)
    {
      var message = response.exception == null ? "No Errors" : response.exception.ToString();
      Assert.AreEqual(200, response.status,
                      response.errorResponse != null ? response.errorResponse.ToString() : message);
      Assert.IsNull(response.exception);
      Assert.IsNull(response.errorResponse);

      return this;
    }

    public TestBuilder AssertStatusCode<T, U>(ClientResponse<T, U> response, int expectedCode)
    {
      Assert.AreEqual(expectedCode, response.status,
                      response.errorResponse != null ? response.errorResponse.ToString() : "No errors");
      Assert.IsNull(response.exception);
      if (expectedCode == 400)
      {
        Assert.IsNotNull(response.errorResponse);
      }
      else
      {
        Assert.IsNull(response.errorResponse);
      }

      Assert.IsNull(response.successResponse);

      return this;
    }

    public TestBuilder AssertMissing<T, U>(ClientResponse<T, U> response)
    {
      Assert.AreEqual(404, response.status,
                      response.errorResponse != null ? response.errorResponse.ToString() : "No errors");
      Assert.IsNull(response.exception);
      Assert.IsNull(response.errorResponse);
      Assert.IsNull(response.successResponse);

      return this;
    }

    public TestBuilder CallClient(Action<FusionAuthClient> action)
    {
      action(Client);
      return this;
    }

    public TestBuilder ProxyClient(IWebProxy proxy)
    {
      Client = new FusionAuthClient(ApiKey, "http://localhost:9011", proxy);
      return this;
    }

    public TestBuilder CreateUser()
    {
      var retrieveResponse = Client.RetrieveUserByEmail(EmailAddress);
      if (retrieveResponse.WasSuccessful())
      {
        AssertSuccess(Client.DeleteUser((Guid) retrieveResponse.successResponse.user.id));
      }

      var newUser = new User()
                    .With(u => u.email = EmailAddress)
                    .With(u => u.username = "csharpclient")
                    .With(u => u.password = "password");

      var newRegistration = new UserRegistration()
                            .With(r => r.applicationId = ApplicationId)
                            .With(r => r.username = "csharpclient");

      var response = Client.Register(null, new RegistrationRequest(newUser, newRegistration, false, true));
      AssertSuccess(response);
      Assert.AreEqual(newUser.username, response.successResponse.user.username);

      User = response.successResponse.user;
      return this;
    }

    public TestBuilder Login()
    {
      var response = Client.Login(new LoginRequest(Application.id, User.email, "password"));
      AssertSuccess(response);

      Token = response.successResponse.token;
      User = response.successResponse.user;
      return this;
    }

    public TestBuilder UpdateApplication(Application application)
    {
      var response = Client.UpdateApplication(ApplicationId, new ApplicationRequest(application, null));
      AssertSuccess(response);

      Application = response.successResponse.application;
      return this;
    }


    public TestBuilder CreateApplication()
    {
      var retrieveResponse = Client.RetrieveApplication(ApplicationId);
      if (retrieveResponse.WasSuccessful())
      {
        AssertSuccess(Client.DeleteApplication(ApplicationId));
      }

      var application = new Application("CSharp Client Test");
      var response = Client.CreateApplication(ApplicationId,
                                              new ApplicationRequest(new Application("CSharp Client Test"), null));

      AssertSuccess(response);
      Assert.AreEqual(application.name, response.successResponse.application.name);

      Application = response.successResponse.application;
      return this;
    }
  }

  [TestFixture]
  public class FusionAuthClientTest
  {
    private TestBuilder _test;

    [SetUp]
    public void Initialize()
    {
      _test = new TestBuilder();
    }

    [Test]
    public void Retrieve_Application_Test()
    {
      _test.CreateApplication()
           .CallClient(client => client.RetrieveApplication(_test.Application.id));

      var response = _test.Client.RetrieveApplication(_test.Application.id);
      Assert.AreEqual("CSharp Client Test", response.successResponse.application.name);
      _test.AssertSuccess(response);
    }

    [Test]
    public void Retrieve_RefreshTokens_Test()
    {
      _test.CreateApplication()
           .CreateUser();

      var response = _test.Client.RetrieveRefreshTokens((Guid) _test.User.id);
      _test.AssertSuccess(response);
      Assert.IsNull(response.successResponse.refreshTokens);
    }

    [Test]
    public void Update_Application_Test()
    {
      _test.CreateApplication()
           .UpdateApplication(_test.Application.With(a => a.name = "CSharp Client Test (Updated)"));

      var application = new Application("CSharp Client Test (updated)");
      var response = _test.Client.UpdateApplication(TestBuilder.ApplicationId,
                                                    new ApplicationRequest(application, null));
      Assert.AreEqual("CSharp Client Test (updated)", response.successResponse.application.name);
      _test.AssertSuccess(response);
    }

    [Test]
    public void Validate_JWT_Test()
    {
      _test.CreateApplication().CreateUser().Login();

      var response = _test.Client.ValidateJWT(_test.Token);
      _test.AssertSuccess(response);

      Assert.AreEqual(response.successResponse.jwt["sub"].ToString(), _test.User.id.ToString());
      Assert.AreEqual(response.successResponse.jwt["applicationId"].ToString(), _test.Application.id.ToString());
    }


    [Test]
    public void Retrieve_Public_Keys_Test()
    {
      _test.CreateApplication().CreateUser();

      // No Application Specific Public Keys
      var response = _test.Client.RetrieveJWTPublicKey(_test.Application.id.ToString());
      _test.AssertMissing(response);

      response = _test.Client.RetrieveJWTPublicKeys();
      _test.AssertSuccess(response);
    }

    [Test]
    public void Deactivate_Application_Test()
    {
      var response = _test.Client.DeactivateApplication(TestBuilder.ApplicationId);
      _test.AssertSuccess(response);
    }

    [Test]
    public void Reactivate_Application_Test()
    {
      Deactivate_Application_Test();

      var response = _test.Client.ReactivateApplication(TestBuilder.ApplicationId);
      _test.AssertSuccess(response);

      var retrieveResponse = _test.Client.RetrieveApplication(TestBuilder.ApplicationId);
      Assert.AreEqual("CSharp Client Test", retrieveResponse.successResponse.application.name);
      Assert.IsTrue(retrieveResponse.successResponse.application.active);
      _test.AssertSuccess(retrieveResponse);
    }

    [Test]
    public void Register_Test()
    {
      _test.CreateApplication().CreateUser();

      //test retrieval
      var testRetrieve = _test.Client.RetrieveRegistration((Guid) _test.User.id, TestBuilder.ApplicationId);
      Assert.AreEqual(_test.User.username, testRetrieve.successResponse.registration.username);
      _test.AssertSuccess(testRetrieve);

      //test update      
      var userRegistration = new UserRegistration(null, TestBuilder.ApplicationId, (Guid) _test.User.id, null,
                                                  _test.User.username, ContentStatus.ACTIVE,
                                                  new Guid("9af3fc1d-9236-4793-93df-aeac5f67f23e"), null, null);
      var updateResponse = _test.Client.UpdateRegistration((Guid) _test.User.id,
                                                           new RegistrationRequest(null, userRegistration));
      Assert.AreEqual(_test.User.username, updateResponse.successResponse.registration.username);
      _test.AssertSuccess(updateResponse);

      // Delete Registration and User
      _test.AssertSuccess(_test.Client.DeleteRegistration((Guid) _test.User.id, TestBuilder.ApplicationId));
      _test.AssertSuccess(_test.Client.DeleteUser((Guid) _test.User.id));

      // test empty retrieval
      var randomUserId = new Guid("f64992f5-c705-47b2-bc88-4046ac8a82ee");
      _test.AssertMissing(_test.Client.RetrieveRegistration(randomUserId, TestBuilder.ApplicationId));
    }

    [Test]
    public void SystemConfiguration()
    {
      var response = _test.Client.RetrieveSystemConfiguration();
      _test.AssertSuccess(response);
    }

    [Test]
    public void Groups()
    {
      var retrieveResponse = _test.Client.RetrieveGroups();
      _test.AssertSuccess(retrieveResponse);

      if (retrieveResponse.successResponse.groups != null && retrieveResponse.successResponse.groups.Count > 0)
      {
        retrieveResponse.successResponse.groups.ForEach(g =>
        {
          if (g.name.Equals("C# Group"))
          {
            _test.Client.DeleteGroup(g.id);
          }
        });
      }

      var createResponse = _test.Client.CreateGroup(null, new GroupRequest(new Group().With(g => g.name = "C# Group")));
      _test.AssertSuccess(createResponse);
      retrieveResponse = _test.Client.RetrieveGroups();
      _test.AssertSuccess(retrieveResponse);

      // Use a tenantId
      var tenantResponse = _test.Client.RetrieveTenants();
      _test.AssertSuccess(tenantResponse);

      var tenantId = tenantResponse.successResponse.tenants[0].id;
      var tenantClient = _test.NewClientWithTenantId(tenantId);

      var tenantGroupRetrieveResponse = tenantClient.RetrieveGroup(createResponse.successResponse.group.id);
      _test.AssertSuccess(tenantGroupRetrieveResponse);

      // 400, bad tenant Id
      var badTenantClient = _test.NewClientWithTenantId(new Guid("40602225-d65c-4801-8696-9654e731b5da"));

      tenantGroupRetrieveResponse = badTenantClient.RetrieveGroup(createResponse.successResponse.group.id);
      _test.AssertStatusCode(tenantGroupRetrieveResponse, 400);

      // 404, Wrong tenant Id
      var createTenantResponse =
        _test.Client.CreateTenant(null, new TenantRequest(new Tenant().With(t => t.name = "C# Tenant")));
      _test.AssertSuccess(createTenantResponse);

      var wrongTenantClient = _test.NewClientWithTenantId(createTenantResponse.successResponse.tenant.id);

      tenantGroupRetrieveResponse = wrongTenantClient.RetrieveGroup(createResponse.successResponse.group.id);
      _test.AssertMissing(tenantGroupRetrieveResponse);

      var deleteResponse = _test.Client.DeleteTenant(createTenantResponse.successResponse.tenant.id);
      _test.AssertSuccess(deleteResponse);
    }

    [Test]
    public void IdentityProviders()
    {
      var retrieveResponse = _test.Client.RetrieveIdentityProviders();
      _test.AssertSuccess(retrieveResponse);

      if (retrieveResponse.successResponse.identityProviders != null &&
          retrieveResponse.successResponse.identityProviders.Count > 0)
      {
        retrieveResponse.successResponse.identityProviders.ForEach(idp =>
        {
          if (idp.name.Equals("C# IdentityProvider"))
          {
            _test.Client.DeleteIdentityProvider(idp.id);
          }
        });
      }

      var createResponse = _test.Client.CreateIdentityProvider(null, new IdentityProviderRequest(new IdentityProvider()
                                                                                                 .With(idp => idp.name =
                                                                                                         "C# IdentityProvider")
                                                                                                 .With(idp => idp
                                                                                                           .headerKeyParameter
                                                                                                         = "kid")
                                                                                                 .With(idp =>
                                                                                                         idp
                                                                                                             .uniqueIdentityClaim
                                                                                                           = "username")
                                                                                                 .With(idp =>
                                                                                                         idp
                                                                                                             .uniqueIdentityClaimType
                                                                                                           = UniqueIdentityClaimType
                                                                                                             .Username)));
      _test.AssertSuccess(createResponse);
      retrieveResponse = _test.Client.RetrieveIdentityProviders();
      _test.AssertSuccess(retrieveResponse);
    }

    [Test]
    public void Integrations()
    {
      var response = _test.Client.RetrieveIntegration();
      _test.AssertSuccess(response);

      Assert.IsNotNull(response.successResponse.integrations.cleanspeak);
      Assert.IsNotNull(response.successResponse.integrations.twilio);
    }

    [Test]
    public void Login_Test_Proxy()
    {
      // Complete proxy construction to test against a real proxy.
      IWebProxy webProxy = null;
      //IWebProxy webProxy = new WebProxy("http://proxy.test.com");
      //webProxy.Credentials = new NetworkCredential("user", "password");

      _test.ProxyClient(webProxy)
           .CreateApplication()
           .CreateUser();

      var response = _test.Client.Login(new LoginRequest(TestBuilder.ApplicationId, TestBuilder.EmailAddress,
                                                         "password", "10.0.1.129"));
      _test.AssertSuccess(response);
    }

    [Test]
    public void Login_Test()
    {
      _test.CreateApplication()
           .CreateUser()
           .CallClient(client => _test.AssertSuccess(client.Login(new LoginRequest(TestBuilder.ApplicationId,
                                                                                   TestBuilder.EmailAddress,
                                                                                   "password", "10.0.1.129"))));
    }

    [Test]
    public void Tenant_Test()
    {
      var response = _test.Client.RetrieveTenants();
      _test.AssertSuccess(response);

      Assert.IsNotNull(response.successResponse.tenants);
      Assert.AreEqual(response.successResponse.tenants[0].name, "default");

      var createResponse =
        _test.Client.CreateTenant(null, new TenantRequest(new Tenant().With(t => t.name = "C# Tenant")));
      _test.AssertSuccess(createResponse);
      Assert.AreEqual(createResponse.successResponse.tenant.name, "C# Tenant");
      Assert.IsNotNull(createResponse.successResponse.tenant.id);

      var deleteResponse = _test.Client.DeleteTenant(createResponse.successResponse.tenant.id);
      _test.AssertSuccess(deleteResponse);
    }
  }
}