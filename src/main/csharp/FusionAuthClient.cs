/*
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

using FusionAuth.Error;
using FusionAuth.Domain;
using Inversoft.Restify;
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FusionAuth
{
  /**
   * Client that connects to a FusionAuth server and provides access to the full set of FusionAuth APIs.
   */
  public class FusionAuthClient
  {
    // Add our serializer here that includes our IdentityProviderConvert
    private static readonly JsonSerializer serializer = new JsonSerializer();

    static FusionAuthClient()
    {
      serializer.Converters.Add(new StringEnumConverter());
      serializer.Converters.Add(new DateTimeOffsetConverter());
      serializer.Converters.Add(new IdentityProviderConverter());
      serializer.NullValueHandling = NullValueHandling.Ignore;
    }

    public const string TENANT_ID_HEADER = "X-FusionAuth-TenantId";

    private readonly string apiKey;

    private readonly string baseUrl;

    private readonly string tenantId;

    private readonly IWebProxy webProxy;

    public int timeout = 2000;

    public int readWriteTimeout = 2000;

    public FusionAuthClient(string apiKey, string baseUrl)
    {
      this.apiKey = apiKey;
      this.baseUrl = baseUrl;
    }

    public FusionAuthClient(string apiKey, string baseUrl, string tenantId)
    {
      this.apiKey = apiKey;
      this.baseUrl = baseUrl;
      this.tenantId = tenantId;
    }

    public FusionAuthClient(string apiKey, string baseUrl, string tenantId, IWebProxy webProxy)
    {
      this.apiKey = apiKey;
      this.baseUrl = baseUrl;
      this.tenantId = tenantId;
      this.webProxy = webProxy;
    }

    public FusionAuthClient(string apiKey, string baseUrl, IWebProxy webProxy)
    {
      this.apiKey = apiKey;
      this.baseUrl = baseUrl;
      this.webProxy = webProxy;
    }

    /**
     * Takes an action on a user. The user being actioned is called the "actionee" and the user taking the action is called the
     * "actioner". Both user ids are required. You pass the actionee's user id into the method and the actioner's is put into the
     * request object.
     *
     * @param actioneeUserId The actionee's user id.
     * @param request The action request that includes all of the information about the action being taken including
     * the id of the action, any options and the duration (if applicable).
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> ActionUser(Guid actioneeUserId, ActionRequest request)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlSegment(actioneeUserId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Cancels the user action.
     *
     * @param actionId The action id of the action to cancel.
     * @param request The action request that contains the information about the cancellation.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> CancelAction(Guid actionId, ActionRequest request)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlSegment(actionId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Delete()
                                          .Go();
    }

    /**
     * Changes a user's password using the change password Id. This usually occurs after an email has been sent to the user
     * and they clicked on a link to reset their password.
     *
     * @param changePasswordId The change password Id used to find the user. This value is generated by FusionAuth once the change password workflow has been initiated.
     * @param request The change password request that contains all of the information used to change the password.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ChangePasswordResponse, Errors> ChangePassword(string changePasswordId, ChangePasswordRequest request)
    {
        return Start<ChangePasswordResponse, Errors>().Uri("/api/user/change-password")
                                          .UrlSegment(changePasswordId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Changes a user's password using their identity (login id and password). Using a loginId instead of the changePasswordId
     * bypasses the email verification and allows a password to be changed directly without first calling the #forgotPassword
     * method.
     *
     * @param request The change password request that contains all of the information used to change the password.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> ChangePasswordByIdentity(ChangePasswordRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/change-password")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Adds a comment to the user's account.
     *
     * @param request The request object that contains all of the information used to create the user comment.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> CommentOnUser(UserCommentRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/comment")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates an application. You can optionally specify an Id for the application, if not provided one will be generated.
     *
     * @param applicationId (Optional) The Id to use for the application. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the application.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, Errors> CreateApplication(Guid? applicationId, ApplicationRequest request)
    {
        return Start<ApplicationResponse, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a new role for an application. You must specify the id of the application you are creating the role for.
     * You can optionally specify an Id for the role inside the ApplicationRole object itself, if not provided one will be generated.
     *
     * @param applicationId The Id of the application to create the role on.
     * @param roleId (Optional) The Id of the role. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the application role.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, Errors> CreateApplicationRole(Guid applicationId, Guid? roleId, ApplicationRequest request)
    {
        return Start<ApplicationResponse, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .UrlSegment("role")
                                          .UrlSegment(roleId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates an audit log with the message and user name (usually an email). Audit logs should be written anytime you
     * make changes to the FusionAuth database. When using the FusionAuth App web interface, any changes are automatically
     * written to the audit log. However, if you are accessing the API, you must write the audit logs yourself.
     *
     * @param request The request object that contains all of the information used to create the audit log entry.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<AuditLogResponse, Errors> CreateAuditLog(AuditLogRequest request)
    {
        return Start<AuditLogResponse, Errors>().Uri("/api/system/audit-log")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates an email template. You can optionally specify an Id for the template, if not provided one will be generated.
     *
     * @param emailTemplateId (Optional) The Id for the template. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the email template.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<EmailTemplateResponse, Errors> CreateEmailTemplate(Guid? emailTemplateId, EmailTemplateRequest request)
    {
        return Start<EmailTemplateResponse, Errors>().Uri("/api/email/template")
                                          .UrlSegment(emailTemplateId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a group. You can optionally specify an Id for the group, if not provided one will be generated.
     *
     * @param groupId (Optional) The Id for the group. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the group.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<GroupResponse, Errors> CreateGroup(Guid? groupId, GroupRequest request)
    {
        return Start<GroupResponse, Errors>().Uri("/api/group")
                                          .UrlSegment(groupId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a member in a group.
     *
     * @param request The request object that contains all of the information used to create the group member(s).
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<MemberResponse, Errors> CreateGroupMembers(MemberRequest request)
    {
        return Start<MemberResponse, Errors>().Uri("/api/group/member")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates an identity provider. You can optionally specify an Id for the identity provider, if not provided one will be generated.
     *
     * @param identityProviderId (Optional) The Id of the identity provider. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the identity provider.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<IdentityProviderResponse, Errors> CreateIdentityProvider(Guid? identityProviderId, IdentityProviderRequest request)
    {
        return Start<IdentityProviderResponse, Errors>().Uri("/api/identity-provider")
                                          .UrlSegment(identityProviderId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a Lambda. You can optionally specify an Id for the lambda, if not provided one will be generated.
     *
     * @param lambdaId (Optional) The Id for the lambda. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the lambda.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LambdaResponse, Errors> CreateLambda(Guid? lambdaId, LambdaRequest request)
    {
        return Start<LambdaResponse, Errors>().Uri("/api/lambda")
                                          .UrlSegment(lambdaId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a tenant. You can optionally specify an Id for the tenant, if not provided one will be generated.
     *
     * @param tenantId (Optional) The Id for the tenant. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the tenant.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<TenantResponse, Errors> CreateTenant(Guid? tenantId, TenantRequest request)
    {
        return Start<TenantResponse, Errors>().Uri("/api/tenant")
                                          .UrlSegment(tenantId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a user. You can optionally specify an Id for the user, if not provided one will be generated.
     *
     * @param userId (Optional) The Id for the user. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> CreateUser(Guid? userId, UserRequest request)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlSegment(userId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a user action. This action cannot be taken on a user until this call successfully returns. Anytime after
     * that the user action can be applied to any user.
     *
     * @param userActionId (Optional) The Id for the user action. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the user action.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionResponse, Errors> CreateUserAction(Guid? userActionId, UserActionRequest request)
    {
        return Start<UserActionResponse, Errors>().Uri("/api/user-action")
                                          .UrlSegment(userActionId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a user reason. This user action reason cannot be used when actioning a user until this call completes
     * successfully. Anytime after that the user action reason can be used.
     *
     * @param userActionReasonId (Optional) The Id for the user action reason. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the user action reason.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionReasonResponse, Errors> CreateUserActionReason(Guid? userActionReasonId, UserActionReasonRequest request)
    {
        return Start<UserActionReasonResponse, Errors>().Uri("/api/user-action-reason")
                                          .UrlSegment(userActionReasonId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Creates a webhook. You can optionally specify an Id for the webhook, if not provided one will be generated.
     *
     * @param webhookId (Optional) The Id for the webhook. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the webhook.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<WebhookResponse, Errors> CreateWebhook(Guid? webhookId, WebhookRequest request)
    {
        return Start<WebhookResponse, Errors>().Uri("/api/webhook")
                                          .UrlSegment(webhookId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Deactivates the application with the given Id.
     *
     * @param applicationId The Id of the application to deactivate.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeactivateApplication(Guid applicationId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deactivates the user with the given Id.
     *
     * @param userId The Id of the user to deactivate.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeactivateUser(Guid userId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user")
                                          .UrlSegment(userId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deactivates the user action with the given Id.
     *
     * @param userActionId The Id of the user action to deactivate.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeactivateUserAction(Guid userActionId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user-action")
                                          .UrlSegment(userActionId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deactivates the users with the given ids.
     *
     * @param userIds The ids of the users to deactivate.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeactivateUsers(ICollection<Guid> userIds)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/bulk")
                                          .UrlParameter("userId", userIds)
                                          .Delete()
                                          .Go();
    }

    /**
     * Hard deletes an application. This is a dangerous operation and should not be used in most circumstances. This will
     * delete the application, any registrations for that application, metrics and reports for the application, all the
     * roles for the application, and any other data associated with the application. This operation could take a very
     * long time, depending on the amount of data in your database.
     *
     * @param applicationId The Id of the application to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteApplication(Guid applicationId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .UrlParameter("hardDelete", true)
                                          .Delete()
                                          .Go();
    }

    /**
     * Hard deletes an application role. This is a dangerous operation and should not be used in most circumstances. This
     * permanently removes the given role from all users that had it.
     *
     * @param applicationId The Id of the application to deactivate.
     * @param roleId The Id of the role to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteApplicationRole(Guid applicationId, Guid roleId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .UrlSegment("role")
                                          .UrlSegment(roleId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the email template for the given Id.
     *
     * @param emailTemplateId The Id of the email template to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteEmailTemplate(Guid emailTemplateId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/email/template")
                                          .UrlSegment(emailTemplateId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the group for the given Id.
     *
     * @param groupId The Id of the group to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteGroup(Guid groupId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/group")
                                          .UrlSegment(groupId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Removes users as members of a group.
     *
     * @param request The member request that contains all of the information used to remove members to the group.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteGroupMembers(MemberDeleteRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/group/member")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the identity provider for the given Id.
     *
     * @param identityProviderId The Id of the identity provider to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteIdentityProvider(Guid identityProviderId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/identity-provider")
                                          .UrlSegment(identityProviderId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the key for the given Id.
     *
     * @param keyOd The Id of the key to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteKey(Guid keyOd)
    {
        return Start<RESTVoid, Errors>().Uri("/api/key")
                                          .UrlSegment(keyOd)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the lambda for the given Id.
     *
     * @param lambdaId The Id of the lambda to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteLambda(Guid lambdaId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/lambda")
                                          .UrlSegment(lambdaId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the user registration for the given user and application.
     *
     * @param userId The Id of the user whose registration is being deleted.
     * @param applicationId The Id of the application to remove the registration for.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteRegistration(Guid userId, Guid applicationId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/registration")
                                          .UrlSegment(userId)
                                          .UrlSegment(applicationId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the tenant for the given Id.
     *
     * @param tenantId The Id of the tenant to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteTenant(Guid tenantId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/tenant")
                                          .UrlSegment(tenantId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the user for the given Id. This permanently deletes all information, metrics, reports and data associated
     * with the user.
     *
     * @param userId The Id of the user to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteUser(Guid userId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user")
                                          .UrlSegment(userId)
                                          .UrlParameter("hardDelete", true)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the user action for the given Id. This permanently deletes the user action and also any history and logs of
     * the action being applied to any users.
     *
     * @param userActionId The Id of the user action to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteUserAction(Guid userActionId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user-action")
                                          .UrlSegment(userActionId)
                                          .UrlParameter("hardDelete", true)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the user action reason for the given Id.
     *
     * @param userActionReasonId The Id of the user action reason to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteUserActionReason(Guid userActionReasonId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user-action-reason")
                                          .UrlSegment(userActionReasonId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the users with the given ids.
     *
     * @param request The ids of the users to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteUsers(UserDeleteRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/bulk")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Delete()
                                          .Go();
    }

    /**
     * Deletes the webhook for the given Id.
     *
     * @param webhookId The Id of the webhook to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DeleteWebhook(Guid webhookId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/webhook")
                                          .UrlSegment(webhookId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Disable Two Factor authentication for a user.
     *
     * @param userId The Id of the User for which you're disabling Two Factor authentication.
     * @param code The Two Factor code used verify the the caller knows the Two Factor secret.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> DisableTwoFactor(Guid userId, string code)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/two-factor")
                                          .UrlParameter("userId", userId)
                                          .UrlParameter("code", code)
                                          .Delete()
                                          .Go();
    }

    /**
     * Enable Two Factor authentication for a user.
     *
     * @param userId The Id of the user to enable Two Factor authentication.
     * @param request The two factor enable request information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> EnableTwoFactor(Guid userId, TwoFactorRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/two-factor")
                                          .UrlSegment(userId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Exchange a refresh token for a new JWT.
     *
     * @param request The refresh request.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RefreshResponse, Errors> ExchangeRefreshTokenForJWT(RefreshRequest request)
    {
        return Start<RefreshResponse, Errors>().Uri("/api/jwt/refresh")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Begins the forgot password sequence, which kicks off an email to the user so that they can reset their password.
     *
     * @param request The request that contains the information about the user so that they can be emailed.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ForgotPasswordResponse, Errors> ForgotPassword(ForgotPasswordRequest request)
    {
        return Start<ForgotPasswordResponse, Errors>().Uri("/api/user/forgot-password")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Generate a new Email Verification Id to be used with the Verify Email API. This API will not attempt to send an
     * email to the User. This API may be used to collect the verificationId for use with a third party system.
     *
     * @param email The email address of the user that needs a new verification email.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<VerifyEmailResponse, RESTVoid> GenerateEmailVerificationId(string email)
    {
        return Start<VerifyEmailResponse, RESTVoid>().Uri("/api/user/verify-email")
                                          .UrlParameter("email", email)
                                          .UrlParameter("sendVerifyPasswordEmail", false)
                                          .Put()
                                          .Go();
    }

    /**
     * Generate a new RSA or EC key pair or an HMAC secret.
     *
     * @param keyId (Optional) The Id for the key. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the key.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<KeyResponse, Errors> GenerateKey(Guid? keyId, KeyRequest request)
    {
        return Start<KeyResponse, Errors>().Uri("/api/key/generate")
                                          .UrlSegment(keyId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Generate a new Application Registration Verification Id to be used with the Verify Registration API. This API will not attempt to send an
     * email to the User. This API may be used to collect the verificationId for use with a third party system.
     *
     * @param email The email address of the user that needs a new verification email.
     * @param applicationId The Id of the application to be verified.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<VerifyRegistrationResponse, RESTVoid> GenerateRegistrationVerificationId(string email, Guid applicationId)
    {
        return Start<VerifyRegistrationResponse, RESTVoid>().Uri("/api/user/verify-registration")
                                          .UrlParameter("email", email)
                                          .UrlParameter("sendVerifyPasswordEmail", false)
                                          .UrlParameter("applicationId", applicationId)
                                          .Put()
                                          .Go();
    }

    /**
     * Generate a Two Factor secret that can be used to enable Two Factor authentication for a User. The response will contain
     * both the secret and a Base32 encoded form of the secret which can be shown to a User when using a 2 Step Authentication
     * application such as Google Authenticator.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<SecretResponse, RESTVoid> GenerateTwoFactorSecret()
    {
        return Start<SecretResponse, RESTVoid>().Uri("/api/two-factor/secret")
                                          .Get()
                                          .Go();
    }

    /**
     * Generate a Two Factor secret that can be used to enable Two Factor authentication for a User. The response will contain
     * both the secret and a Base32 encoded form of the secret which can be shown to a User when using a 2 Step Authentication
     * application such as Google Authenticator.
     *
     * @param encodedJWT The encoded JWT (access token).
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<SecretResponse, RESTVoid> GenerateTwoFactorSecretUsingJWT(string encodedJWT)
    {
        return Start<SecretResponse, RESTVoid>().Uri("/api/two-factor/secret")
                                          .Authorization("JWT " + encodedJWT)
                                          .Get()
                                          .Go();
    }

    /**
     * Handles login via third-parties including Social login, external OAuth and OpenID Connect, and other
     * login systems.
     *
     * @param request The third-party login request that contains information from the third-party login
     * providers that FusionAuth uses to reconcile the user's account.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginResponse, Errors> IdentityProviderLogin(IdentityProviderLoginRequest request)
    {
        return Start<LoginResponse, Errors>().Uri("/api/identity-provider/login")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Import an existing RSA or EC key pair or an HMAC secret.
     *
     * @param keyId (Optional) The Id for the key. If not provided a secure random UUID will be generated.
     * @param request The request object that contains all of the information used to create the key.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<KeyResponse, Errors> ImportKey(Guid? keyId, KeyRequest request)
    {
        return Start<KeyResponse, Errors>().Uri("/api/key/import")
                                          .UrlSegment(keyId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Bulk imports multiple users. This does some validation, but then tries to run batch inserts of users. This reduces
     * latency when inserting lots of users. Therefore, the error response might contain some information about failures,
     * but it will likely be pretty generic.
     *
     * @param request The request that contains all of the information about all of the users to import.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> ImportUsers(ImportRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/user/import")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Issue a new access token (JWT) for the requested Application after ensuring the provided JWT is valid. A valid
     * access token is properly signed and not expired.
     * <p>
     * This API may be used in an SSO configuration to issue new tokens for another application after the user has
     * obtained a valid token from authentication.
     *
     * @param applicationId The Application Id for which you are requesting a new access token be issued.
     * @param encodedJWT The encoded JWT (access token).
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<IssueResponse, Errors> IssueJWT(Guid applicationId, string encodedJWT)
    {
        return Start<IssueResponse, Errors>().Uri("/api/jwt/issue")
                                          .Authorization("JWT " + encodedJWT)
                                          .UrlParameter("applicationId", applicationId)
                                          .Get()
                                          .Go();
    }

    /**
     * Logs a user in.
     *
     * @param request The login request that contains the user credentials used to log them in.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginResponse, Errors> Login(LoginRequest request)
    {
        return Start<LoginResponse, Errors>().Uri("/api/login")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Sends a ping to FusionAuth indicating that the user was automatically logged into an application. When using
     * FusionAuth's SSO or your own, you should call this if the user is already logged in centrally, but accesses an
     * application where they no longer have a session. This helps correctly track login counts, times and helps with
     * reporting.
     *
     * @param userId The Id of the user that was logged in.
     * @param applicationId The Id of the application that they logged into.
     * @param callerIPAddress (Optional) The IP address of the end-user that is logging in. If a null value is provided
     * the IP address will be that of the client or last proxy that sent the request.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> LoginPing(Guid userId, Guid applicationId, string callerIPAddress)
    {
        return Start<RESTVoid, Errors>().Uri("/api/login")
                                          .UrlSegment(userId)
                                          .UrlSegment(applicationId)
                                          .UrlParameter("ipAddress", callerIPAddress)
                                          .Put()
                                          .Go();
    }

    /**
     * The Logout API is intended to be used to remove the refresh token and access token cookies if they exist on the
     * client and revoke the refresh token stored. This API does nothing if the request does not contain an access
     * token or refresh token cookies.
     *
     * @param global When this value is set to true all of the refresh tokens issued to the owner of the
     * provided token will be revoked.
     * @param refreshToken (Optional) The refresh_token as a request parameter instead of coming in via a cookie.
     * If provided this takes precedence over the cookie.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, RESTVoid> Logout(bool global, string refreshToken)
    {
        return Start<RESTVoid, RESTVoid>().Uri("/api/logout")
                                          .UrlParameter("global", global)
                                          .UrlParameter("refreshToken", refreshToken)
                                          .Post()
                                          .Go();
    }

    /**
     * Retrieves the identity provider for the given domain. A 200 response code indicates the domain is managed
     * by a registered identity provider. A 404 indicates the domain is not managed.
     *
     * @param domain The domain or email address to lookup.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LookupResponse, RESTVoid> LookupIdentityProvider(string domain)
    {
        return Start<LookupResponse, RESTVoid>().Uri("/api/identity-provider/lookup")
                                          .UrlParameter("domain", domain)
                                          .Get()
                                          .Go();
    }

    /**
     * Modifies a temporal user action by changing the expiration of the action and optionally adding a comment to the
     * action.
     *
     * @param actionId The Id of the action to modify. This is technically the user action log id.
     * @param request The request that contains all of the information about the modification.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> ModifyAction(Guid actionId, ActionRequest request)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlSegment(actionId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Complete a login request using a passwordless code
     *
     * @param request The passwordless login request that contains all of the information used to complete login.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginResponse, Errors> PasswordlessLogin(PasswordlessLoginRequest request)
    {
        return Start<LoginResponse, Errors>().Uri("/api/passwordless/login")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Reactivates the application with the given Id.
     *
     * @param applicationId The Id of the application to reactivate.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, Errors> ReactivateApplication(Guid applicationId)
    {
        return Start<ApplicationResponse, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .UrlParameter("reactivate", true)
                                          .Put()
                                          .Go();
    }

    /**
     * Reactivates the user with the given Id.
     *
     * @param userId The Id of the user to reactivate.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> ReactivateUser(Guid userId)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlSegment(userId)
                                          .UrlParameter("reactivate", true)
                                          .Put()
                                          .Go();
    }

    /**
     * Reactivates the user action with the given Id.
     *
     * @param userActionId The Id of the user action to reactivate.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionResponse, Errors> ReactivateUserAction(Guid userActionId)
    {
        return Start<UserActionResponse, Errors>().Uri("/api/user-action")
                                          .UrlSegment(userActionId)
                                          .UrlParameter("reactivate", true)
                                          .Put()
                                          .Go();
    }

    /**
     * Reconcile a User to FusionAuth using JWT issued from another Identity Provider.
     *
     * @param request The reconcile request that contains the data to reconcile the User.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginResponse, Errors> ReconcileJWT(IdentityProviderLoginRequest request)
    {
        return Start<LoginResponse, Errors>().Uri("/api/jwt/reconcile")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Registers a user for an application. If you provide the User and the UserRegistration object on this request, it
     * will create the user as well as register them for the application. This is called a Full Registration. However, if
     * you only provide the UserRegistration object, then the user must already exist and they will be registered for the
     * application. The user id can also be provided and it will either be used to look up an existing user or it will be
     * used for the newly created User.
     *
     * @param userId (Optional) The Id of the user being registered for the application and optionally created.
     * @param request The request that optionally contains the User and must contain the UserRegistration.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RegistrationResponse, Errors> Register(Guid? userId, RegistrationRequest request)
    {
        return Start<RegistrationResponse, Errors>().Uri("/api/user/registration")
                                          .UrlSegment(userId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Re-sends the verification email to the user.
     *
     * @param email The email address of the user that needs a new verification email.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<VerifyEmailResponse, RESTVoid> ResendEmailVerification(string email)
    {
        return Start<VerifyEmailResponse, RESTVoid>().Uri("/api/user/verify-email")
                                          .UrlParameter("email", email)
                                          .Put()
                                          .Go();
    }

    /**
     * Re-sends the application registration verification email to the user.
     *
     * @param email The email address of the user that needs a new verification email.
     * @param applicationId The Id of the application to be verified.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<VerifyRegistrationResponse, RESTVoid> ResendRegistrationVerification(string email, Guid applicationId)
    {
        return Start<VerifyRegistrationResponse, RESTVoid>().Uri("/api/user/verify-registration")
                                          .UrlParameter("email", email)
                                          .UrlParameter("applicationId", applicationId)
                                          .Put()
                                          .Go();
    }

    /**
     * Retrieves a single action log (the log of a user action that was taken on a user previously) for the given Id.
     *
     * @param actionId The Id of the action to retrieve.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> RetrieveAction(Guid actionId)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlSegment(actionId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the actions for the user with the given Id. This will return all time based actions that are active,
     * and inactive as well as non-time based actions.
     *
     * @param userId The Id of the user to fetch the actions for.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> RetrieveActions(Guid userId)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlParameter("userId", userId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the actions for the user with the given Id that are currently preventing the User from logging in.
     *
     * @param userId The Id of the user to fetch the actions for.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> RetrieveActionsPreventingLogin(Guid userId)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlParameter("userId", userId)
                                          .UrlParameter("preventingLogin", true)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the actions for the user with the given Id that are currently active.
     * An active action means one that is time based and has not been canceled, and has not ended.
     *
     * @param userId The Id of the user to fetch the actions for.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> RetrieveActiveActions(Guid userId)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlParameter("userId", userId)
                                          .UrlParameter("active", true)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the application for the given id or all of the applications if the id is null.
     *
     * @param applicationId (Optional) The application id.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, RESTVoid> RetrieveApplication(Guid? applicationId)
    {
        return Start<ApplicationResponse, RESTVoid>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the applications.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, RESTVoid> RetrieveApplications()
    {
        return Start<ApplicationResponse, RESTVoid>().Uri("/api/application")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves a single audit log for the given Id.
     *
     * @param auditLogId The Id of the audit log to retrieve.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<AuditLogResponse, Errors> RetrieveAuditLog(int auditLogId)
    {
        return Start<AuditLogResponse, Errors>().Uri("/api/system/audit-log")
                                          .UrlSegment(auditLogId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the daily active user report between the two instants. If you specify an application id, it will only
     * return the daily active counts for that application.
     *
     * @param applicationId (Optional) The application id.
     * @param start The start instant as UTC milliseconds since Epoch.
     * @param end The end instant as UTC milliseconds since Epoch.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<DailyActiveUserReportResponse, Errors> RetrieveDailyActiveReport(Guid? applicationId, long start, long end)
    {
        return Start<DailyActiveUserReportResponse, Errors>().Uri("/api/report/daily-active-user")
                                          .UrlParameter("applicationId", applicationId)
                                          .UrlParameter("start", start)
                                          .UrlParameter("end", end)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the email template for the given Id. If you don't specify the id, this will return all of the email templates.
     *
     * @param emailTemplateId (Optional) The Id of the email template.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<EmailTemplateResponse, RESTVoid> RetrieveEmailTemplate(Guid? emailTemplateId)
    {
        return Start<EmailTemplateResponse, RESTVoid>().Uri("/api/email/template")
                                          .UrlSegment(emailTemplateId)
                                          .Get()
                                          .Go();
    }

    /**
     * Creates a preview of the email template provided in the request. This allows you to preview an email template that
     * hasn't been saved to the database yet. The entire email template does not need to be provided on the request. This
     * will create the preview based on whatever is given.
     *
     * @param request The request that contains the email template and optionally a locale to render it in.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<PreviewResponse, Errors> RetrieveEmailTemplatePreview(PreviewRequest request)
    {
        return Start<PreviewResponse, Errors>().Uri("/api/email/template/preview")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Retrieves all of the email templates.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<EmailTemplateResponse, RESTVoid> RetrieveEmailTemplates()
    {
        return Start<EmailTemplateResponse, RESTVoid>().Uri("/api/email/template")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves a single event log for the given Id.
     *
     * @param eventLogId The Id of the event log to retrieve.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<EventLogResponse, Errors> RetrieveEventLog(int eventLogId)
    {
        return Start<EventLogResponse, Errors>().Uri("/api/system/event-log")
                                          .UrlSegment(eventLogId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the group for the given Id.
     *
     * @param groupId The Id of the group.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<GroupResponse, Errors> RetrieveGroup(Guid groupId)
    {
        return Start<GroupResponse, Errors>().Uri("/api/group")
                                          .UrlSegment(groupId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the groups.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<GroupResponse, RESTVoid> RetrieveGroups()
    {
        return Start<GroupResponse, RESTVoid>().Uri("/api/group")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the identity provider for the given id or all of the identity providers if the id is null.
     *
     * @param identityProviderId (Optional) The identity provider id.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<IdentityProviderResponse, RESTVoid> RetrieveIdentityProvider(Guid? identityProviderId)
    {
        return Start<IdentityProviderResponse, RESTVoid>().Uri("/api/identity-provider")
                                          .UrlSegment(identityProviderId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the identity providers.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<IdentityProviderResponse, RESTVoid> RetrieveIdentityProviders()
    {
        return Start<IdentityProviderResponse, RESTVoid>().Uri("/api/identity-provider")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the actions for the user with the given Id that are currently inactive.
     * An inactive action means one that is time based and has been canceled or has expired, or is not time based.
     *
     * @param userId The Id of the user to fetch the actions for.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ActionResponse, Errors> RetrieveInactiveActions(Guid userId)
    {
        return Start<ActionResponse, Errors>().Uri("/api/user/action")
                                          .UrlParameter("userId", userId)
                                          .UrlParameter("active", false)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the applications that are currently inactive.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, RESTVoid> RetrieveInactiveApplications()
    {
        return Start<ApplicationResponse, RESTVoid>().Uri("/api/application")
                                          .UrlParameter("inactive", true)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the user actions that are currently inactive.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionResponse, RESTVoid> RetrieveInactiveUserActions()
    {
        return Start<UserActionResponse, RESTVoid>().Uri("/api/user-action")
                                          .UrlParameter("inactive", true)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the available integrations.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<IntegrationResponse, RESTVoid> RetrieveIntegration()
    {
        return Start<IntegrationResponse, RESTVoid>().Uri("/api/integration")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the Public Key configured for verifying JSON Web Tokens (JWT) by the key Id. If the key Id is provided a
     * single public key will be returned if one is found by that id. If the optional parameter key Id is not provided all
     * public keys will be returned.
     *
     * @param keyId (Optional) The Id of the public key.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<PublicKeyResponse, RESTVoid> RetrieveJWTPublicKey(string keyId)
    {
        return Start<PublicKeyResponse, RESTVoid>().Uri("/api/jwt/public-key")
                                          .UrlSegment(keyId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all Public Keys configured for verifying JSON Web Tokens (JWT).
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<PublicKeyResponse, RESTVoid> RetrieveJWTPublicKeys()
    {
        return Start<PublicKeyResponse, RESTVoid>().Uri("/api/jwt/public-key")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the key for the given Id.
     *
     * @param keyId The Id of the key.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<KeyResponse, Errors> RetrieveKey(Guid keyId)
    {
        return Start<KeyResponse, Errors>().Uri("/api/key")
                                          .UrlSegment(keyId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the keys.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<KeyResponse, RESTVoid> RetrieveKeys()
    {
        return Start<KeyResponse, RESTVoid>().Uri("/api/key")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the lambda for the given Id.
     *
     * @param lambdaId The Id of the lambda.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LambdaResponse, Errors> RetrieveLambda(Guid lambdaId)
    {
        return Start<LambdaResponse, Errors>().Uri("/api/lambda")
                                          .UrlSegment(lambdaId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the lambdas.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LambdaResponse, RESTVoid> RetrieveLambdas()
    {
        return Start<LambdaResponse, RESTVoid>().Uri("/api/lambda")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the lambdas for the provided type.
     *
     * @param type The type of the lambda to return.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LambdaResponse, RESTVoid> RetrieveLambdasByType(LambdaType type)
    {
        return Start<LambdaResponse, RESTVoid>().Uri("/api/lambda")
                                          .UrlParameter("type", type)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the login report between the two instants. If you specify an application id, it will only return the
     * login counts for that application.
     *
     * @param applicationId (Optional) The application id.
     * @param start The start instant as UTC milliseconds since Epoch.
     * @param end The end instant as UTC milliseconds since Epoch.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginReportResponse, Errors> RetrieveLoginReport(Guid? applicationId, long start, long end)
    {
        return Start<LoginReportResponse, Errors>().Uri("/api/report/login")
                                          .UrlParameter("applicationId", applicationId)
                                          .UrlParameter("start", start)
                                          .UrlParameter("end", end)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the monthly active user report between the two instants. If you specify an application id, it will only
     * return the monthly active counts for that application.
     *
     * @param applicationId (Optional) The application id.
     * @param start The start instant as UTC milliseconds since Epoch.
     * @param end The end instant as UTC milliseconds since Epoch.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<MonthlyActiveUserReportResponse, Errors> RetrieveMonthlyActiveReport(Guid? applicationId, long start, long end)
    {
        return Start<MonthlyActiveUserReportResponse, Errors>().Uri("/api/report/monthly-active-user")
                                          .UrlParameter("applicationId", applicationId)
                                          .UrlParameter("start", start)
                                          .UrlParameter("end", end)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the Oauth2 configuration for the application for the given Application Id.
     *
     * @param applicationId The Id of the Application to retrieve OAuth configuration.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<OAuthConfigurationResponse, Errors> RetrieveOauthConfiguration(Guid applicationId)
    {
        return Start<OAuthConfigurationResponse, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .UrlSegment("oauth-configuration")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the password validation rules.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<PasswordValidationRulesResponse, RESTVoid> RetrievePasswordValidationRules()
    {
        return Start<PasswordValidationRulesResponse, RESTVoid>().Uri("/api/system-configuration/password-validation-rules")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the last number of login records.
     *
     * @param offset The initial record. e.g. 0 is the last login, 100 will be the 100th most recent login.
     * @param limit (Optional, defaults to 10) The number of records to retrieve.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RecentLoginResponse, Errors> RetrieveRecentLogins(int offset, int limit)
    {
        return Start<RecentLoginResponse, Errors>().Uri("/api/user/recent-login")
                                          .UrlParameter("offset", offset)
                                          .UrlParameter("limit", limit)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the refresh tokens that belong to the user with the given Id.
     *
     * @param userId The Id of the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RefreshResponse, Errors> RetrieveRefreshTokens(Guid userId)
    {
        return Start<RefreshResponse, Errors>().Uri("/api/jwt/refresh")
                                          .UrlParameter("userId", userId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user registration for the user with the given id and the given application id.
     *
     * @param userId The Id of the user.
     * @param applicationId The Id of the application.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RegistrationResponse, Errors> RetrieveRegistration(Guid userId, Guid applicationId)
    {
        return Start<RegistrationResponse, Errors>().Uri("/api/user/registration")
                                          .UrlSegment(userId)
                                          .UrlSegment(applicationId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the registration report between the two instants. If you specify an application id, it will only return
     * the registration counts for that application.
     *
     * @param applicationId (Optional) The application id.
     * @param start The start instant as UTC milliseconds since Epoch.
     * @param end The end instant as UTC milliseconds since Epoch.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RegistrationReportResponse, Errors> RetrieveRegistrationReport(Guid? applicationId, long start, long end)
    {
        return Start<RegistrationReportResponse, Errors>().Uri("/api/report/registration")
                                          .UrlParameter("applicationId", applicationId)
                                          .UrlParameter("start", start)
                                          .UrlParameter("end", end)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the system configuration.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<SystemConfigurationResponse, RESTVoid> RetrieveSystemConfiguration()
    {
        return Start<SystemConfigurationResponse, RESTVoid>().Uri("/api/system-configuration")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the tenant for the given Id.
     *
     * @param tenantId The Id of the tenant.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<TenantResponse, Errors> RetrieveTenant(Guid tenantId)
    {
        return Start<TenantResponse, Errors>().Uri("/api/tenant")
                                          .UrlSegment(tenantId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the tenants.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<TenantResponse, RESTVoid> RetrieveTenants()
    {
        return Start<TenantResponse, RESTVoid>().Uri("/api/tenant")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the totals report. This contains all of the total counts for each application and the global registration
     * count.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<TotalsReportResponse, RESTVoid> RetrieveTotalReport()
    {
        return Start<TotalsReportResponse, RESTVoid>().Uri("/api/report/totals")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user for the given Id.
     *
     * @param userId The Id of the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> RetrieveUser(Guid userId)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlSegment(userId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user action for the given Id. If you pass in null for the id, this will return all of the user
     * actions.
     *
     * @param userActionId (Optional) The Id of the user action.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionResponse, RESTVoid> RetrieveUserAction(Guid? userActionId)
    {
        return Start<UserActionResponse, RESTVoid>().Uri("/api/user-action")
                                          .UrlSegment(userActionId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user action reason for the given Id. If you pass in null for the id, this will return all of the user
     * action reasons.
     *
     * @param userActionReasonId (Optional) The Id of the user action reason.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionReasonResponse, RESTVoid> RetrieveUserActionReason(Guid? userActionReasonId)
    {
        return Start<UserActionReasonResponse, RESTVoid>().Uri("/api/user-action-reason")
                                          .UrlSegment(userActionReasonId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all the user action reasons.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionReasonResponse, RESTVoid> RetrieveUserActionReasons()
    {
        return Start<UserActionReasonResponse, RESTVoid>().Uri("/api/user-action-reason")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the user actions.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionResponse, RESTVoid> RetrieveUserActions()
    {
        return Start<UserActionResponse, RESTVoid>().Uri("/api/user-action")
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user by a change password Id. The intended use of this API is to retrieve a user after the forgot
     * password workflow has been initiated and you may not know the user's email or username.
     *
     * @param changePasswordId The unique change password Id that was sent via email or returned by the Forgot Password API.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> RetrieveUserByChangePasswordId(string changePasswordId)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlParameter("changePasswordId", changePasswordId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user for the given email.
     *
     * @param email The email of the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> RetrieveUserByEmail(string email)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlParameter("email", email)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user for the loginId. The loginId can be either the username or the email.
     *
     * @param loginId The email or username of the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> RetrieveUserByLoginId(string loginId)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlParameter("loginId", loginId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user for the given username.
     *
     * @param username The username of the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> RetrieveUserByUsername(string username)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlParameter("username", username)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user by a verificationId. The intended use of this API is to retrieve a user after the forgot
     * password workflow has been initiated and you may not know the user's email or username.
     *
     * @param verificationId The unique verification Id that has been set on the user object.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> RetrieveUserByVerificationId(string verificationId)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlParameter("verificationId", verificationId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all of the comments for the user with the given Id.
     *
     * @param userId The Id of the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserCommentResponse, Errors> RetrieveUserComments(Guid userId)
    {
        return Start<UserCommentResponse, Errors>().Uri("/api/user/comment")
                                          .UrlSegment(userId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the login report between the two instants for a particular user by Id. If you specify an application id, it will only return the
     * login counts for that application.
     *
     * @param applicationId (Optional) The application id.
     * @param userId The userId id.
     * @param start The start instant as UTC milliseconds since Epoch.
     * @param end The end instant as UTC milliseconds since Epoch.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginReportResponse, Errors> RetrieveUserLoginReport(Guid? applicationId, Guid userId, long start, long end)
    {
        return Start<LoginReportResponse, Errors>().Uri("/api/report/login")
                                          .UrlParameter("applicationId", applicationId)
                                          .UrlParameter("userId", userId)
                                          .UrlParameter("start", start)
                                          .UrlParameter("end", end)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the login report between the two instants for a particular user by login Id. If you specify an application id, it will only return the
     * login counts for that application.
     *
     * @param applicationId (Optional) The application id.
     * @param loginId The userId id.
     * @param start The start instant as UTC milliseconds since Epoch.
     * @param end The end instant as UTC milliseconds since Epoch.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginReportResponse, Errors> RetrieveUserLoginReportByLoginId(Guid? applicationId, string loginId, long start, long end)
    {
        return Start<LoginReportResponse, Errors>().Uri("/api/report/login")
                                          .UrlParameter("applicationId", applicationId)
                                          .UrlParameter("loginId", loginId)
                                          .UrlParameter("start", start)
                                          .UrlParameter("end", end)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the last number of login records for a user.
     *
     * @param userId The Id of the user.
     * @param offset The initial record. e.g. 0 is the last login, 100 will be the 100th most recent login.
     * @param limit (Optional, defaults to 10) The number of records to retrieve.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RecentLoginResponse, Errors> RetrieveUserRecentLogins(Guid userId, int offset, int limit)
    {
        return Start<RecentLoginResponse, Errors>().Uri("/api/user/recent-login")
                                          .UrlParameter("userId", userId)
                                          .UrlParameter("offset", offset)
                                          .UrlParameter("limit", limit)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the user for the given Id. This method does not use an API key, instead it uses a JSON Web Token (JWT) for authentication.
     *
     * @param encodedJWT The encoded JWT (access token).
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> RetrieveUserUsingJWT(string encodedJWT)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .Authorization("JWT " + encodedJWT)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the webhook for the given Id. If you pass in null for the id, this will return all the webhooks.
     *
     * @param webhookId (Optional) The Id of the webhook.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<WebhookResponse, RESTVoid> RetrieveWebhook(Guid? webhookId)
    {
        return Start<WebhookResponse, RESTVoid>().Uri("/api/webhook")
                                          .UrlSegment(webhookId)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves all the webhooks.
     *
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<WebhookResponse, RESTVoid> RetrieveWebhooks()
    {
        return Start<WebhookResponse, RESTVoid>().Uri("/api/webhook")
                                          .Get()
                                          .Go();
    }

    /**
     * Revokes a single refresh token, all tokens for a user or all tokens for an application. If you provide a user id
     * and an application id, this will delete all the refresh tokens for that user for that application.
     *
     * @param token (Optional) The refresh token to delete.
     * @param userId (Optional) The user id whose tokens to delete.
     * @param applicationId (Optional) The application id of the tokens to delete.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> RevokeRefreshToken(string token, Guid? userId, Guid? applicationId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/jwt/refresh")
                                          .UrlParameter("token", token)
                                          .UrlParameter("userId", userId)
                                          .UrlParameter("applicationId", applicationId)
                                          .Delete()
                                          .Go();
    }

    /**
     * Searches the audit logs with the specified criteria and pagination.
     *
     * @param request The search criteria and pagination information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<AuditLogSearchResponse, RESTVoid> SearchAuditLogs(AuditLogSearchRequest request)
    {
        return Start<AuditLogSearchResponse, RESTVoid>().Uri("/api/system/audit-log/search")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Searches the event logs with the specified criteria and pagination.
     *
     * @param request The search criteria and pagination information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<EventLogSearchResponse, RESTVoid> SearchEventLogs(EventLogSearchRequest request)
    {
        return Start<EventLogSearchResponse, RESTVoid>().Uri("/api/system/event-log/search")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Retrieves the users for the given ids. If any id is invalid, it is ignored.
     *
     * @param ids The user ids to search for.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<SearchResponse, Errors> SearchUsers(ICollection<Guid> ids)
    {
        return Start<SearchResponse, Errors>().Uri("/api/user/search")
                                          .UrlParameter("ids", ids)
                                          .Get()
                                          .Go();
    }

    /**
     * Retrieves the users for the given search criteria and pagination.
     *
     * @param request The search criteria and pagination constraints. Fields used: queryString, numberOfResults, startRow,
     * and sort fields.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<SearchResponse, Errors> SearchUsersByQueryString(SearchRequest request)
    {
        return Start<SearchResponse, Errors>().Uri("/api/user/search")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Send an email using an email template id. You can optionally provide <code>requestData</code> to access key value
     * pairs in the email template.
     *
     * @param emailTemplateId The id for the template.
     * @param request The send email request that contains all of the information used to send the email.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<SendResponse, Errors> SendEmail(Guid emailTemplateId, SendRequest request)
    {
        return Start<SendResponse, Errors>().Uri("/api/email/send")
                                          .UrlSegment(emailTemplateId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Send a passwordless authentication code in an email to complete login.
     *
     * @param request The passwordless send request that contains all of the information used to send an email containing a code.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> SendPasswordlessCode(PasswordlessSendRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/passwordless/send")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Send a Two Factor authentication code to assist in setting up Two Factor authentication or disabling.
     *
     * @param request The request object that contains all of the information used to send the code.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> SendTwoFactorCode(TwoFactorSendRequest request)
    {
        return Start<RESTVoid, Errors>().Uri("/api/two-factor/send")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Send a Two Factor authentication code to allow the completion of Two Factor authentication.
     *
     * @param twoFactorId The Id returned by the Login API necessary to complete Two Factor authentication.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, Errors> SendTwoFactorCodeForLogin(string twoFactorId)
    {
        return Start<RESTVoid, Errors>().Uri("/api/two-factor/send")
                                          .UrlSegment(twoFactorId)
                                          .Post()
                                          .Go();
    }

    /**
     * Complete login using a 2FA challenge
     *
     * @param request The login request that contains the user credentials used to log them in.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LoginResponse, Errors> TwoFactorLogin(TwoFactorLoginRequest request)
    {
        return Start<LoginResponse, Errors>().Uri("/api/two-factor/login")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Post()
                                          .Go();
    }

    /**
     * Updates the application with the given Id.
     *
     * @param applicationId The Id of the application to update.
     * @param request The request that contains all of the new application information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, Errors> UpdateApplication(Guid applicationId, ApplicationRequest request)
    {
        return Start<ApplicationResponse, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the application role with the given id for the application.
     *
     * @param applicationId The Id of the application that the role belongs to.
     * @param roleId The Id of the role to update.
     * @param request The request that contains all of the new role information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ApplicationResponse, Errors> UpdateApplicationRole(Guid applicationId, Guid roleId, ApplicationRequest request)
    {
        return Start<ApplicationResponse, Errors>().Uri("/api/application")
                                          .UrlSegment(applicationId)
                                          .UrlSegment("role")
                                          .UrlSegment(roleId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the email template with the given Id.
     *
     * @param emailTemplateId The Id of the email template to update.
     * @param request The request that contains all of the new email template information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<EmailTemplateResponse, Errors> UpdateEmailTemplate(Guid emailTemplateId, EmailTemplateRequest request)
    {
        return Start<EmailTemplateResponse, Errors>().Uri("/api/email/template")
                                          .UrlSegment(emailTemplateId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the group with the given Id.
     *
     * @param groupId The Id of the group to update.
     * @param request The request that contains all of the new group information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<GroupResponse, Errors> UpdateGroup(Guid groupId, GroupRequest request)
    {
        return Start<GroupResponse, Errors>().Uri("/api/group")
                                          .UrlSegment(groupId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the identity provider with the given Id.
     *
     * @param identityProviderId The Id of the identity provider to update.
     * @param request The request object that contains the updated identity provider.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<IdentityProviderResponse, Errors> UpdateIdentityProvider(Guid identityProviderId, IdentityProviderRequest request)
    {
        return Start<IdentityProviderResponse, Errors>().Uri("/api/identity-provider")
                                          .UrlSegment(identityProviderId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the available integrations.
     *
     * @param request The request that contains all of the new integration information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<IntegrationResponse, Errors> UpdateIntegrations(IntegrationRequest request)
    {
        return Start<IntegrationResponse, Errors>().Uri("/api/integration")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the key with the given Id.
     *
     * @param keyId The Id of the key to update.
     * @param request The request that contains all of the new key information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<KeyResponse, Errors> UpdateKey(Guid keyId, KeyRequest request)
    {
        return Start<KeyResponse, Errors>().Uri("/api/key")
                                          .UrlSegment(keyId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the lambda with the given Id.
     *
     * @param lambdaId The Id of the lambda to update.
     * @param request The request that contains all of the new lambda information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<LambdaResponse, Errors> UpdateLambda(Guid lambdaId, LambdaRequest request)
    {
        return Start<LambdaResponse, Errors>().Uri("/api/lambda")
                                          .UrlSegment(lambdaId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the registration for the user with the given id and the application defined in the request.
     *
     * @param userId The Id of the user whose registration is going to be updated.
     * @param request The request that contains all of the new registration information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RegistrationResponse, Errors> UpdateRegistration(Guid userId, RegistrationRequest request)
    {
        return Start<RegistrationResponse, Errors>().Uri("/api/user/registration")
                                          .UrlSegment(userId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the system configuration.
     *
     * @param request The request that contains all of the new system configuration information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<SystemConfigurationResponse, Errors> UpdateSystemConfiguration(SystemConfigurationRequest request)
    {
        return Start<SystemConfigurationResponse, Errors>().Uri("/api/system-configuration")
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the tenant with the given Id.
     *
     * @param tenantId The Id of the tenant to update.
     * @param request The request that contains all of the new tenant information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<TenantResponse, Errors> UpdateTenant(Guid tenantId, TenantRequest request)
    {
        return Start<TenantResponse, Errors>().Uri("/api/tenant")
                                          .UrlSegment(tenantId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the user with the given Id.
     *
     * @param userId The Id of the user to update.
     * @param request The request that contains all of the new user information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserResponse, Errors> UpdateUser(Guid userId, UserRequest request)
    {
        return Start<UserResponse, Errors>().Uri("/api/user")
                                          .UrlSegment(userId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the user action with the given Id.
     *
     * @param userActionId The Id of the user action to update.
     * @param request The request that contains all of the new user action information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionResponse, Errors> UpdateUserAction(Guid userActionId, UserActionRequest request)
    {
        return Start<UserActionResponse, Errors>().Uri("/api/user-action")
                                          .UrlSegment(userActionId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the user action reason with the given Id.
     *
     * @param userActionReasonId The Id of the user action reason to update.
     * @param request The request that contains all of the new user action reason information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<UserActionReasonResponse, Errors> UpdateUserActionReason(Guid userActionReasonId, UserActionReasonRequest request)
    {
        return Start<UserActionReasonResponse, Errors>().Uri("/api/user-action-reason")
                                          .UrlSegment(userActionReasonId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Updates the webhook with the given Id.
     *
     * @param webhookId The Id of the webhook to update.
     * @param request The request that contains all of the new webhook information.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<WebhookResponse, Errors> UpdateWebhook(Guid webhookId, WebhookRequest request)
    {
        return Start<WebhookResponse, Errors>().Uri("/api/webhook")
                                          .UrlSegment(webhookId)
                                          .BodyHandler(new JSONBodyHandler(request, serializer))
                                          .Put()
                                          .Go();
    }

    /**
     * Validates the provided JWT (encoded JWT string) to ensure the token is valid. A valid access token is properly
     * signed and not expired.
     * <p>
     * This API may be used to verify the JWT as well as decode the encoded JWT into human readable identity claims.
     *
     * @param encodedJWT The encoded JWT (access token).
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<ValidateResponse, RESTVoid> ValidateJWT(string encodedJWT)
    {
        return Start<ValidateResponse, RESTVoid>().Uri("/api/jwt/validate")
                                          .Authorization("JWT " + encodedJWT)
                                          .Get()
                                          .Go();
    }

    /**
     * Confirms a email verification. The Id given is usually from an email sent to the user.
     *
     * @param verificationId The email verification id sent to the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, RESTVoid> VerifyEmail(string verificationId)
    {
        return Start<RESTVoid, RESTVoid>().Uri("/api/user/verify-email")
                                          .UrlSegment(verificationId)
                                          .Post()
                                          .Go();
    }

    /**
     * Confirms an application registration. The Id given is usually from an email sent to the user.
     *
     * @param verificationId The registration verification Id sent to the user.
     * @return When successful, the response will contain the log of the action. If there was a validation error or any
     * other type of error, this will return the Errors object in the response. Additionally, if FusionAuth could not be
     * contacted because it is down or experiencing a failure, the response will contain an Exception, which could be an
     * IOException.
     */
    public ClientResponse<RESTVoid, RESTVoid> VerifyRegistration(string verificationId)
    {
        return Start<RESTVoid, RESTVoid>().Uri("/api/user/verify-registration")
                                          .UrlSegment(verificationId)
                                          .Post()
                                          .Go();
    }

    // Start initializes and returns RESTClient
    private RESTClient<T, U> Start<T, U>()
    {
        var client = new RESTClient<T, U>().Authorization(apiKey)
                                   .SuccessResponseHandler(typeof(T) == typeof(RESTVoid) ? null : new JSONResponseHandler<T>(serializer))
                                   .ErrorResponseHandler(typeof(U) == typeof(RESTVoid) ? null : new JSONResponseHandler<U>(serializer))
                                   .Url(baseUrl)
                                   .Timeout(timeout)
                                   .ReadWriteTimeout(readWriteTimeout)
                                   .Proxy(webProxy);


        if (tenantId != null) {
          client.Header(TENANT_ID_HEADER, tenantId);
        }

        return client;
    }
  }
}