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
  public class UserAction : IComparable<UserAction>, Buildable<UserAction>
  {
    public bool active;

    /**
     * Only time-based actions. Template to use when cancelled
     */
    public Guid? cancelEmailTemplateId;

    /**
     * Only time-based actions. Template to use when ended
     */
    public Guid? endEmailTemplateId;

    public Guid? id;

    public bool includeEmailInEventJSON;

    public LocalizedStrings localizedNames;

    /**
     * Only time-based actions. Template to use when modified
     */
    public Guid? modifyEmailTemplateId;

    public string name;

    public List<UserActionOption> options = new List<UserActionOption>();

    public bool preventLogin;

    /**
     * Only time-based actions. This indicates FusionAuth will send an event when the action expires
     */
    public bool sendEndEvent;

    /**
     * All actions. The template to be used when an action is first taken
     */
    public Guid? startEmailTemplateId;

    public bool temporal;

    /**
     * FusionAuth emailing
     */
    public bool userEmailingEnabled;

    public bool userNotificationsEnabled;

    public UserAction()
    {
    }

    public UserAction(string name)
    {
      this.name = name;
    }

    public UserAction(Guid? id, string name, bool active, LocalizedStrings localizedNames, bool preventLogin,
                      bool sendEndEvent, bool temporal, bool userNotificationsEnabled,
                      bool userEmailingEnabled, bool includeEmailInEventJSON,
                      Guid? startEmailTemplateId, Guid? modifyEmailTemplateId, Guid? cancelEmailTemplateId,
                      Guid? endEmailTemplateId, params UserActionOption[] options)
    {
      this.id = id;
      this.active = active;
      this.name = name;
      this.includeEmailInEventJSON = includeEmailInEventJSON;
      this.localizedNames = localizedNames;
      this.preventLogin = preventLogin;
      this.sendEndEvent = sendEndEvent;
      this.temporal = temporal;
      this.userNotificationsEnabled = userNotificationsEnabled;
      this.startEmailTemplateId = startEmailTemplateId;
      this.modifyEmailTemplateId = modifyEmailTemplateId;
      this.cancelEmailTemplateId = cancelEmailTemplateId;
      this.endEmailTemplateId = endEmailTemplateId;
      this.userEmailingEnabled = userEmailingEnabled;
      for (var i = 0; i < options.Length; i++)
      {
        this.options.Add(options[i]);
      }
    }

    public int CompareTo(UserAction o)
    {
      return string.Compare(name, o.name, StringComparison.Ordinal);
    }

    public UserActionOption GetOption(string name)
    {
      if (name == null)
      {
        return null;
      }

      foreach (var key in options)
      {
        if (key.name.Equals(name))
        {
          return key;
        }
      }

      return null;
    }

    public void SortOptions()
    {
      options.Sort();
    }

    public UserAction With(Action<UserAction> action)
    {
      action(this);
      return this;
    }
  }
}