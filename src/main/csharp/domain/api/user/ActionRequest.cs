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
  public class ActionRequest
  {
    public ActionData action;

    public bool broadcast;

    public ActionRequest()
    {
    }

    public ActionRequest(ActionData action, bool broadcast)
    {
      this.action = action;
      this.broadcast = broadcast;
    }

    public class ActionData : Buildable<ActionData>
    {
      public Guid actioneeUserId;

      public Guid actionerUserId;

      public List<Guid> applicationIds;

      public string comment;

      public bool emailUser;

      public DateTimeOffset? expiry;

      /**
       * Flag instructing webhooks to notify the user
       */
      public bool notifyUser;

      public string option;

      public Guid? reasonId;

      public Guid userActionId;

      public ActionData()
      {
      }

      public ActionData(Guid userActionId, Guid actioneeUserId, Guid actionerUserId, string comment,
                        DateTimeOffset? expiry,
                        bool notifyUser, bool emailUser, string option, Guid? reasonId,
                        params Guid[] applicationIds)
      {
        this.userActionId = userActionId;
        this.actioneeUserId = actioneeUserId;
        this.actionerUserId = actionerUserId;
        this.comment = comment;
        this.expiry = expiry;
        this.notifyUser = notifyUser;
        this.emailUser = emailUser;
        this.option = option;
        this.reasonId = reasonId;
        for (var i = 0; i < applicationIds.Length; i++)
        {
          this.applicationIds.Add(applicationIds[i]);
        }
      }

      public ActionData With(Action<ActionData> action)
      {
        action(this);
        return this;
      }
    }
  }
}