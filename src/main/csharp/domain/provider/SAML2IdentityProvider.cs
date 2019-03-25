/*
 * Copyright (c) 2019, FusionAuth, All Rights Reserved
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
  public class SAML2IdentityProvider : Buildable<SAML2IdentityProvider>, IdentityProvider
  {
    public readonly IdentityProviderType type = IdentityProviderType.SAML2;

    public IDictionary<Guid, SAML2ApplicationConfiguration>
      applicationConfiguration = new Dictionary<Guid, SAML2ApplicationConfiguration>();

    public Uri buttonImageURL;

    public string buttonText;

    public IDictionary<string, string> claimMap;

    public ICollection<string> domains;

    public string emailClaim;

    public bool enabled;

    public Guid id;

    public string idpEndpoint;

    public string name;

    public string requestPrivateKey;

    public string requestPublicKey;

    public string responsePublicKey;

    public string rolesClaim;

    public SAML2IdentityProvider With(Action<SAML2IdentityProvider> action)
    {
      action(this);
      return this;
    }
  }
}