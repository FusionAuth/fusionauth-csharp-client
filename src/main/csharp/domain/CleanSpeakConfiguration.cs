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
  public class CleanSpeakConfiguration : Buildable<CleanSpeakConfiguration>
  {
    public string apiKey;

    public List<Guid> applicationIds = new List<Guid>();

    public bool enabled;

    public Uri url;

    public UsernameModeration usernameModeration = new UsernameModeration();

    public CleanSpeakConfiguration()
    {
    }

    public CleanSpeakConfiguration(string apiKey, Uri url, UsernameModeration usernameModeration,
                                   params Guid[] applicationIds)
    {
      this.apiKey = apiKey;
      this.url = url;
      this.usernameModeration = usernameModeration;
      this.applicationIds.AddRange(applicationIds);
    }

    public class UsernameModeration
    {
      public Guid applicationId;

      public bool enabled;
    }

    public CleanSpeakConfiguration With(Action<CleanSpeakConfiguration> action)
    {
      action(this);
      return this;
    }
  }
}