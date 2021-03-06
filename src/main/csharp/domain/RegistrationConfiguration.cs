﻿/*
 * Copyright (c) 2019, FusionAuth, All Rights Reserved
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
  public class RegistrationConfiguration : Buildable<RegistrationConfiguration>
  {
    public bool enabled;

    public Requirable birthDate = new Requirable();

    public bool confirmPassword;

    public Requirable firstName = new Requirable();

    public Requirable fullName = new Requirable();

    public Requirable lastName = new Requirable();

    public LoginIdType loginIdType = LoginIdType.email;

    public Requirable middleName = new Requirable();

    public Requirable mobilePhone = new Requirable();

    public RegistrationConfiguration With(Action<RegistrationConfiguration> action)
    {
      action(this);
      return this;
    }

    public enum LoginIdType
    {
      email,

      username
    }
  }
}