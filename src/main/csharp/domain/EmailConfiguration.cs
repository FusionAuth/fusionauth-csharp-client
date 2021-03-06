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


using System;

namespace FusionAuth.Domain
{
  public class EmailConfiguration : Buildable<EmailConfiguration>
  {
    public bool enabled;
    
    public Guid? forgotPasswordEmailTemplateId;

    public string host;

    public string password;

    public Guid? passwordlessEmailTemplateId;

    public int? port;

    public EmailSecurityType security;
    
    public Guid? setPasswordEmailTemplateId;

    public string username;
    
    public Guid? verificationEmailTemplateId;

    public bool verifyEmail;

    public bool verifyEmailWhenChanged;

    public EmailConfiguration With(Action<EmailConfiguration> action)
    {
      action(this);
      return this;
    }
  }

  public enum EmailSecurityType
  {
    NONE,

    SSL,

    TLS
  }
}