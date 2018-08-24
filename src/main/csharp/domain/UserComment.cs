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
  public class UserComment : Buildable<UserComment>
  {
    public string comment;

    public Guid commenterId;

    public DateTimeOffset? createInstant;

    public Guid? id;

    public Guid userId;

    public UserComment()
    {
    }

    public UserComment(string comment, DateTimeOffset? createInstant, Guid? id, Guid userId, Guid commenterId)
    {
      this.comment = comment;
      this.createInstant = createInstant;
      this.id = id;
      this.userId = userId;
      this.commenterId = commenterId;
    }

    public UserComment With(Action<UserComment> action)
    {
      action(this);
      return this;
    }
  }
}