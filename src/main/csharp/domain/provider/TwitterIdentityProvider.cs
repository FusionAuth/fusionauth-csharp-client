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
  public class TwitterIdentityProvider : Buildable<TwitterIdentityProvider>, IdentityProvider
  {
    public IDictionary<Guid, TwitterApplicationConfiguration> applicationConfiguration =
      new Dictionary<Guid, TwitterApplicationConfiguration>();

    public string buttonText;

    public string consumerKey;

    public string consumerSecret;

    public bool enabled;

    public Guid id;

    public string name;

    public readonly IdentityProviderType type = IdentityProviderType.Twitter;

    public TwitterIdentityProvider With(Action<TwitterIdentityProvider> action)
    {
      action(this);
      return this;
    }
  }
}