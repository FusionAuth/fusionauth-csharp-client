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

namespace FusionAuth.Domain
{
  public class ApplicationRole : Buildable<ApplicationRole>, IComparable<ApplicationRole>
  {
    public string description;

    public Guid? id;

    public bool isDefault;

    public bool isSuperRole;

    public string name;

    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName) : this(null, roleName, false, false, null)
    {
    }

    public ApplicationRole(Guid? id, string name, bool isDefault, bool isSuperRole, string description)
    {
      this.id = id;
      this.name = name;
      this.isDefault = isDefault;
      this.isSuperRole = isSuperRole;
      this.description = description;
    }

    public int CompareTo(ApplicationRole o)
    {
      return string.Compare(name, o.name, StringComparison.Ordinal);
    }

    public ApplicationRole With(Action<ApplicationRole> action)
    {
      action(this);
      return this;
    }
  }
}