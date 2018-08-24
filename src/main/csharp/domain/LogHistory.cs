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
  public class LogHistory : Buildable<LogHistory>
  {
    public readonly IList<HistoryItem> historyItems = new List<HistoryItem>();

    public LogHistory()
    {
    }

    public LogHistory(Guid actionerUserId, string comment, DateTimeOffset? createInstant, DateTimeOffset? expiry)
    {
      Add(actionerUserId, comment, createInstant, expiry);
    }

    public LogHistory Add(Guid actionerUserId, string comment, DateTimeOffset? createInstant, DateTimeOffset? expiry)
    {
      historyItems.Add(new HistoryItem(actionerUserId, comment, createInstant, expiry));
      return this;
    }

    public HistoryItem Earliest()
    {
      if (historyItems.Count == 0)
      {
        return null;
      }

      return historyItems[0];
    }

    public HistoryItem Latest()
    {
      if (historyItems.Count == 0)
      {
        return null;
      }

      return historyItems[historyItems.Count - 1];
    }

    public class HistoryItem
    {
      public Guid actionerUserId;

      public string comment;

      public DateTimeOffset? createInstant;

      public DateTimeOffset? expiry;

      public HistoryItem()
      {
      }

      public HistoryItem(Guid actionerUserId, string comment, DateTimeOffset? createInstant, DateTimeOffset? expiry)
      {
        this.actionerUserId = actionerUserId;
        this.comment = comment;
        this.createInstant = createInstant;
        this.expiry = expiry;
      }
    }

    public LogHistory With(Action<LogHistory> action)
    {
      action(this);
      return this;
    }
  }
}