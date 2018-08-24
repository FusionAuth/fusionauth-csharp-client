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
  public class UserActionLog : Buildable<UserActionLog>
  {
    public Guid actioneeUserId;

    public Guid actionerUserId;

    public IList<Guid> applicationIds = new List<Guid>();

    public string comment;

    public DateTimeOffset? createInstant;

    /**
     * FusionAuth will email the user when the action ends.
     */
    public bool emailUserOnEnd;

    public bool endEventSent;

    public DateTimeOffset? expiry;

    public LogHistory history;

    public Guid? id;

    public string localizedOption;

    public string localizedReason;

    public UserActionEvent @event;

    /**
     * Webhooks will use this to determine if they should notify the user
     */
    public bool notifyUserOnEnd;

    public string option;

    public string reason;

    public string reasonCode;

    public Guid userActionId;

    public UserActionLog()
    {
    }

    public UserActionLog(Guid actioneeUserId, Guid actionerUserId, Guid userActionId, List<Guid> applicationIds,
                         string comment, DateTimeOffset? expiry, string option, string localizedOption, string reason,
                         string localizedReason, string reasonCode, DateTimeOffset? createInstant,
                         bool endEventSent, LogHistory history, bool notifyUserOnEnd,
                         bool emailUserOnEnd)
    {
      this.actioneeUserId = actioneeUserId;
      this.actionerUserId = actionerUserId;
      this.userActionId = userActionId;

      if (applicationIds != null)
      {
        this.applicationIds = applicationIds;
      }

      this.comment = comment;
      this.expiry = expiry;
      this.option = option;
      this.reason = reason;
      this.reasonCode = reasonCode;
      this.createInstant = createInstant;
      this.endEventSent = endEventSent;
      this.history = history;
      this.localizedOption = localizedOption;
      this.localizedReason = localizedReason;
      this.emailUserOnEnd = emailUserOnEnd;
      this.notifyUserOnEnd = notifyUserOnEnd;
    }

    public UserActionLog With(Action<UserActionLog> action)
    {
      action(this);
      return this;
    }
  }
}