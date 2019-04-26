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
using System.Collections.Generic;

namespace FusionAuth.Domain
{
  public class OpenIdConnectIdentityProvider : Buildable<OpenIdConnectIdentityProvider>, IdentityProvider
  {
    public IDictionary<Guid, OpenIdConnectApplicationConfiguration> applicationConfiguration =
      new Dictionary<Guid, OpenIdConnectApplicationConfiguration>();

    public Uri buttonImageURL;

    public string buttonText;

    public bool enabled;

    public Guid id;

    public string name;

    public IdentityProviderOAuth2Configuration oauth2 = new IdentityProviderOAuth2Configuration();

    public readonly IdentityProviderType type = IdentityProviderType.OpenIDConnect;

    public LambdaConfiguration lambdaConfiguration = new LambdaConfiguration();

    public OpenIdConnectIdentityProvider With(Action<OpenIdConnectIdentityProvider> action)
    {
      action(this);
      return this;
    }
  }
}