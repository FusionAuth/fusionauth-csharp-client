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

namespace FusionAuth.Domain
{
  public class RefreshToken : Buildable<RefreshToken>
  {
    public Guid applicationId;

    public string device;

    public DateTimeOffset? insertInstant;

    public MetaData metaData = new MetaData();

    public DateTimeOffset? startInstant;

    public string token;

    public Guid userId;

    public RefreshToken With(Action<RefreshToken> action)
    {
      action(this);
      return this;
    }
  }
}