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
  public class OAuth2Configuration
  {
    public List<Uri> authorizedOriginURLs = new List<Uri>();

    public List<Uri> authorizedRedirectURLs = new List<Uri>();

    public string clientId;

    public string clientSecret;

    public bool generateRefreshTokens = true;

    public Uri logoutURL;

    public bool requireClientAuthentication;

    public OAuth2Configuration()
    {
    }

    public OAuth2Configuration(string clientId, string clientSecret)
    {
      this.clientId = clientId;
      this.clientSecret = clientSecret;
    }
  }
}