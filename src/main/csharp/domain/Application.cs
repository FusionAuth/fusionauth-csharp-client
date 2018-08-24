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
  public class Application : Buildable<Application>
  {
    public static readonly Guid FUSIONAUTH_APP_ID = new Guid("3C219E58-ED0E-4B18-AD48-F4F92793AE32");

    public bool active;

    public ApplicationPasswordConfiguration passwordConfiguration;

    public AuthenticationTokenConfiguration authenticationTokenConfiguration;

    public CleanSpeakConfiguration cleanSpeakConfiguration;

    public JWTConfiguration jwtConfiguration;

    public Guid? id;

    public string name;

    public OAuth2Configuration oauthConfiguration = new OAuth2Configuration();

    public List<ApplicationRole> roles = new List<ApplicationRole>();

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
  }
}