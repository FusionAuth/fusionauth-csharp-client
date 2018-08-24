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
  public class UserData : Buildable<UserData>
  {
    public readonly Dictionary<string, object> attributes = new Dictionary<string, object>();

    public readonly List<CultureInfo> preferredLanguages = new List<CultureInfo>();

    public string timezone;

    public UserData()
    {
    }

    public UserData(Dictionary<string, object> attributes, List<CultureInfo> preferredLanguages)
    {
      if (attributes != null)
      {
        foreach (var attribute in attributes)
        {
          this.attributes.Add(attribute.Key, attribute.Value);
        }
      }
      if (preferredLanguages != null)
      {
        foreach (var language in preferredLanguages)
        {
          this.preferredLanguages.Add(language);
        }
      }
    }

    public UserData With(Action<UserData> action)
    {
      action(this);
      return this;
    }
  }
}