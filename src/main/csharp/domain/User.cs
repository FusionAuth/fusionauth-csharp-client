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
  public class User : Buildable<User>
  {
    public readonly Dictionary<string, object> data = new Dictionary<string, object>();
    
    public List<GroupMember> memberships = new List<GroupMember>();

    public List<UserRegistration> registrations = new List<UserRegistration>();

    public bool active;

    public string birthDate;

    public Guid? cleanSpeakId;

    public string email;

    public string encryptionScheme;

    public DateTimeOffset? expiry;

    public int? factor;

    public string firstName;

    public string fullName;

    public Guid? id;

    public Uri imageUrl;

    public DateTimeOffset? insertInstant;

    public DateTimeOffset? lastLoginInstant;

    public string lastName;

    public string middleName;

    public string mobilePhone;

    public string password;

    public bool passwordChangeRequired;

    public readonly List<CultureInfo> preferredLanguages = new List<CultureInfo>();

    public string salt;

    public Guid? tenantId;

    public string timezone;

    public TwoFactorDelivery twoFactorDelivery;

    public bool twoFactorEnabled;

    public string twoFactorSecret;

    public string username;

    public ContentStatus usernameStatus;

    public bool verified;

    public User()
    {
    }

    public User(Guid? id, string email, string username, string password, string salt, string birthDate,
                string fullName, string firstName, string middleName, string lastName, string encryptionScheme,
                DateTimeOffset? expiry, bool active, string timezone, Guid? cleanSpeakId,
                Dictionary<string, object> data, bool verified,
                ContentStatus usernameStatus, TwoFactorDelivery twoFactorDelivery, bool twoFactorEnabled,
                string twoFactorSecret, Uri imageUrl, params UserRegistration[] registrations)
    {
      this.id = id;
      this.email = email;
      this.password = password;
      this.salt = salt;
      this.birthDate = birthDate;
      this.encryptionScheme = encryptionScheme;
      this.expiry = expiry;
      this.active = active;
      this.username = username;
      this.timezone = timezone;
      this.fullName = fullName;
      this.firstName = firstName;
      this.middleName = middleName;
      this.lastName = lastName;
      this.cleanSpeakId = cleanSpeakId;
      this.verified = verified;
      this.usernameStatus = usernameStatus;
      this.twoFactorDelivery = twoFactorDelivery;
      this.twoFactorEnabled = twoFactorEnabled;
      this.twoFactorSecret = twoFactorSecret;
      this.imageUrl = imageUrl;

      for (var i = 0; i < registrations.Length; i++)
      {
        this.registrations.Add(registrations[i]);
      }
    }

    public User With(Action<User> action)
    {
      action(this);
      return this;
    }

    public Dictionary<string, object> GetDataForApplication(Guid id)
    {
      var registration = GetRegistrations().Find(r => r.id.Equals(id));
      return registration == null ? null : registration.data;
    }

    public string GetName()
    {
      if (fullName != null)
      {
        return fullName;
      }

      if (firstName != null)
      {
        return firstName + (lastName != null ? " " + lastName : "");
      }

      return null;
    }

    public UserRegistration GetRegistrationForApplication(Guid id)
    {
      var registration = GetRegistrations().Find(r => r.id.Equals(id));
      return registration;
    }

    public List<UserRegistration> GetRegistrations()
    {
      return registrations;
    }

    public ICollection<string> GetRoleNamesForApplication(Guid id)
    {
      var registration = GetRegistrations().Find(r => r.id.Equals(id));
      return registration == null ? null : registration.roles;
    }
  }
}