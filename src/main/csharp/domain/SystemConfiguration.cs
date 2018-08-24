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
using System.Collections.Generic;

namespace FusionAuth.Domain
{
  public class SystemConfiguration : Buildable<SystemConfiguration>
  {
    public List<Uri> backendServers;

    public string cookieEncryptionIV;

    public string cookieEncryptionKey;

    public EmailConfiguration emailConfiguration;

    public EventConfiguration eventConfiguration;

    public ExternalIdentifierConfiguration externalIdentifierConfiguration;

    public FailedAuthenticationConfiguration failedAuthenticationConfiguration;

    public Guid? userActionId;

    public Guid? forgotPasswordEmailTemplateId;

    public int httpSessionMaxInactiveInterval = 3600;

    public JWTConfiguration jwtConfiguration;

    public MaximumPasswordAge maximumPasswordAge = new MaximumPasswordAge();

    public MinimumPasswordAge minimumPasswordAge = new MinimumPasswordAge();

    public PasswordEncryptionConfiguration passwordEncryptionConfiguration;

    public Uri logoutURL;

    [Obsolete("Prefer maximumPasswordAge.days instead.")]
    public int? passwordExpirationDays;

    public PasswordValidationRules passwordValidationRules;

    public string reportTimezone;

    public Guid? setPasswordEmailTemplateId;

    public UIConfiguration uiConfiguration;

    public Guid? verificationEmailTemplateId;

    public bool verifyEmail;

    public bool verifyEmailWhenChanged;

    public SystemConfiguration With(Action<SystemConfiguration> action)
    {
      action(this);
      return this;
    }
  }
}