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
using System.Globalization;

namespace FusionAuth.Domain
{
  public class UserRegistration : Buildable<UserRegistration>
  {
    public Application application;

    public Guid applicationId;

    public string authenticationToken;

    public Guid? cleanSpeakId;

    public readonly Dictionary<string, object> data = new Dictionary<string, object>();

    public readonly List<CultureInfo> preferredLanguages = new List<CultureInfo>();

    public string timezone;

    public Guid? id;

    public DateTimeOffset? insertInstant;

    public DateTimeOffset? lastLoginInstant;

    public List<string> roles = new List<string>();

    public Guid userId;

    public string username;

    public ContentStatus usernameStatus;

    public bool verified;

    public UserRegistration()
    {
    }

    public UserRegistration(Guid? id, Guid applicationId, Guid userId, DateTimeOffset? lastLoginInstant,
                            string username,
                            ContentStatus usernameStatus, Guid? cleanSpeakId, Dictionary<string, object> data,
                            params string[] roles)
    {
      this.id = id;
      this.applicationId = applicationId;
      this.userId = userId;
      this.cleanSpeakId = cleanSpeakId;
      this.data = data;
      this.lastLoginInstant = lastLoginInstant;
      this.username = username;
      this.usernameStatus = usernameStatus;
      for (var i = 0; i < roles.Length; i++)
      {
        this.roles.Add(roles[i]);
      }
    }

    public UserRegistration With(Action<UserRegistration> action)
    {
      action(this);
      return this;
    }
  }
}