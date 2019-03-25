﻿/*
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
  public class MemberRequest
  {
    public IDictionary<Guid, List<GroupMember>> members;

    public MemberRequest()
    {
    }

    public MemberRequest(Dictionary<Guid, List<GroupMember>> members)
    {
      this.members = members;
    }

    public MemberRequest(Guid groupId, List<GroupMember> members)
    {
      this.members = new Dictionary<Guid, List<GroupMember>>(1) {{groupId, members}};
    }
  }
}