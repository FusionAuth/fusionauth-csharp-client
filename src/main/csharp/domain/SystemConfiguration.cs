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

namespace FusionAuth.Domain
{
  public class SystemConfiguration : Buildable<SystemConfiguration>
  {
    public string cookieEncryptionIV;

    public string cookieEncryptionKey;

    public EmailConfiguration emailConfiguration;

    public EventConfiguration eventConfiguration;

    public EventLogConfiguration eventLogConfiguration = new EventLogConfiguration();

    public ExternalIdentifierConfiguration externalIdentifierConfiguration;

    public FailedAuthenticationConfiguration failedAuthenticationConfiguration;

    public int httpSessionMaxInactiveInterval = 3600;

    public string issuer;

    public JWTConfiguration jwtConfiguration;

    public Uri logoutURL;

    public MaximumPasswordAge maximumPasswordAge = new MaximumPasswordAge();

    public MinimumPasswordAge minimumPasswordAge = new MinimumPasswordAge();

    public PasswordEncryptionConfiguration passwordEncryptionConfiguration;

    public PasswordValidationRules passwordValidationRules;

    public string reportTimezone;

    public UIConfiguration uiConfiguration;

    public SystemConfiguration With(Action<SystemConfiguration> action)
    {
      action(this);
      return this;
    }
  }
}