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
using Newtonsoft.Json;

namespace FusionAuth.Domain
{
  public class IdentityProviderOAuth2Configuration : Buildable<IdentityProviderOAuth2Configuration>
  {
    public Uri authorization_endpoint;

    public string client_id;

    public string client_secret;

    public string issuer;

    public string scope;

    public Uri token_endpoint;

    public Uri userinfo_endpoint;

    public IdentityProviderOAuth2Configuration With(Action<IdentityProviderOAuth2Configuration> action)
    {
      action(this);
      return this;
    }
  }
}