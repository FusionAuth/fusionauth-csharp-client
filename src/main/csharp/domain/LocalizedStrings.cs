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

using System.Collections.Generic;
using System.Globalization;

namespace FusionAuth.Domain
{
  public class LocalizedStrings : Dictionary<CultureInfo, string>
  {
    public LocalizedStrings()
    {
    }

    public LocalizedStrings(CultureInfo locale, string str)
    {
      Add(locale, str);
    }

    public LocalizedStrings(CultureInfo locale, string str, CultureInfo locale2, string str2)
    {
      Add(locale, str);
      Add(locale2, str2);
    }
  }
}