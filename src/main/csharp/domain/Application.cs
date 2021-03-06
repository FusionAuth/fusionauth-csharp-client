﻿/*
 * Copyright (c) 2018-2019, FusionAuth, All Rights Reserved
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
using System.Collections.Generic;

namespace FusionAuth.Domain
{
  public class Application : Buildable<Application>
  {
    public static readonly Guid FUSIONAUTH_APP_ID = new Guid("3C219E58-ED0E-4B18-AD48-F4F92793AE32");

    public bool active;

    public AuthenticationTokenConfiguration authenticationTokenConfiguration;

    public CleanSpeakConfiguration cleanSpeakConfiguration;

    public IDictionary<string, object> data = new Dictionary<string, object>();

    public Guid? id;

    public JWTConfiguration jwtConfiguration;

    public LambdaConfiguration lambdaConfiguration = new LambdaConfiguration();

    public LoginConfiguration loginConfiguration = new LoginConfiguration();

    public string name;

    public OAuth2Configuration oauthConfiguration = new OAuth2Configuration();

    public PasswordlessConfiguration passwordlessConfiguration = new PasswordlessConfiguration();

    public RegistrationConfiguration registrationConfiguration = new RegistrationConfiguration();

    public List<ApplicationRole> roles = new List<ApplicationRole>();

    public SAMLv2Configuration samlv2Configuration = new SAMLv2Configuration();

    public Guid? tenantId;

    public Guid? verificationEmailTemplateId;

    public bool verifyRegistration;

    public Application()
    {
    }

    public Application(string name)
    {
      this.name = name;
    }

    public Application(Guid? id, string name, bool active, CleanSpeakConfiguration cleanSpeakConfiguration, params
                         ApplicationRole[] roles)
    {
      this.id = id;
      this.name = name;
      this.active = active;
      this.cleanSpeakConfiguration = cleanSpeakConfiguration;
      foreach (var t in roles)
      {
        this.roles.Add(t);
      }
    }

    public Application(Guid? id, string name, bool active, CleanSpeakConfiguration cleanSpeakConfiguration,
                       OAuth2Configuration oAuthConfiguration, params ApplicationRole[] roles)
    {
      this.id = id;
      this.name = name;
      this.active = active;
      this.cleanSpeakConfiguration = cleanSpeakConfiguration;
      oauthConfiguration = oAuthConfiguration;
      foreach (var role in roles)
      {
        this.roles.Add(role);
      }
    }

    public ApplicationRole GetRole(string name)
    {
      foreach (var role in roles)
      {
        if (role.name.Equals(name))
        {
          return role;
        }
      }

      return null;
    }

    public Application With(Action<Application> action)
    {
      action(this);
      return this;
    }

    public class LambdaConfiguration : Buildable<LambdaConfiguration>
    {
      public Guid? accessTokenPopulateId;

      public Guid? idTokenPopulateId;

      public Guid? samlv2PopulateId;

      public LambdaConfiguration With(Action<LambdaConfiguration> action)
      {
        action(this);
        return this;
      }
    }
  }
}