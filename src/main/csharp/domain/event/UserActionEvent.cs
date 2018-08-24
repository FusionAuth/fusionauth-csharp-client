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
  public class UserActionEvent : BaseEvent, ApplicationEvent
  {
    public static DateTimeOffset? Infinite = DateTimeOffset.MaxValue;

    public readonly IList<Guid> applicationIds = new List<Guid>();

    public string action;

    public Guid actionId;

    public Guid actioneeUserId;

    public Guid actionerUserId;

    public string comment;

    public Email email;

    public DateTimeOffset? expiry;

    public string localizedAction;

    public string localizedDuration;

    public string localizedOption;

    public string localizedReason;

    public bool notifyUser;

    public string option;

    public bool emailedUser;

    public UserActionPhase phase;

    public string reason;

    public string reasonCode;

    public bool active()
    {
      return expiry != null && DateTime.UtcNow.CompareTo(expiry) < 0;
    }

    ICollection<Guid> ApplicationEvent.applicationIds()
    {
      return applicationIds;
    }

    public override string type()
    {
      return "user.action";
    }
  }
}