﻿/*
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
  public class PasswordValidationRules : Buildable<PasswordValidationRules>
  {
    public int maxLength;

    public int minLength;

    public RememberPreviousPasswords rememberPreviousPasswords;

    public bool requireMixedCase;

    public bool requireNonAlpha;

    public bool requireNumber;

    public PasswordValidationRules()
    {
    }

    public PasswordValidationRules(int minLength, int maxLength, bool requireMixedCase,
                                   bool requireNonAlpha)
    {
      this.maxLength = maxLength;
      this.minLength = minLength;
      this.requireMixedCase = requireMixedCase;
      this.requireNonAlpha = requireNonAlpha;
    }

    public PasswordValidationRules With(Action<PasswordValidationRules> action)
    {
      action(this);
      return this;
    }
  }
}