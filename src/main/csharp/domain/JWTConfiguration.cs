﻿/*
 * Copyright (c) 2017, FusionAuth, All Rights Reserved
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
  public class JWTConfiguration : Buildable<JWTConfiguration>
  {
    public Algorithm algorithm;

    public bool enabled;

    public string issuer;

    public string privateKey;

    public string publicKey;

    public int? refreshTokenTimeToLiveInMinutes;

    public string secret;

    public int timeToLiveInSeconds;

    public JWTConfiguration With(Action<JWTConfiguration> action)
    {
      action(this);
      return this;
    }
  }

  public enum Algorithm
  {
    HS256,

    HS384,

    HS512,

    RS256,

    RS384,

    RS512,

    none
  }
}