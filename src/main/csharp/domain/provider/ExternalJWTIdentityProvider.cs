﻿/*
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
  public class ExternalJWTIdentityProvider : BaseIdentityProvider<ExternalJWTApplicationConfiguration>,
                                             Buildable<ExternalJWTIdentityProvider>
  {
    public IDictionary<string, string> claimMap;

    public ICollection<string> domains;

    public string headerKeyParameter;

    public IDictionary<string, string> keys;

    public IdentityProviderOAuth2Configuration oauth2;

    public string uniqueIdentityClaim;

    public ExternalJWTIdentityProvider With(Action<ExternalJWTIdentityProvider> action)
    {
      action(this);
      return this;
    }

    public override IdentityProviderType getType()
    {
      return IdentityProviderType.ExternalJWT;
    }
  }
}