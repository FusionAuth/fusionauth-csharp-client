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

namespace FusionAuth.Domain
{
  public class RegistrationRequest
  {
    public bool generateAuthenticationToken;

    public UserRegistration registration;

    public bool sendSetPasswordEmail;

    public bool skipRegistrationVerification;

    public bool skipVerification;

    public User user;

    public RegistrationRequest()
    {
    }

    public RegistrationRequest(User user, UserRegistration registration)
    {
      this.user = user;
      this.registration = registration;
    }

    public RegistrationRequest(User user, UserRegistration registration, bool sendSetPasswordEmail,
                               bool skipVerification)
    {
      this.user = user;
      this.registration = registration;
      this.sendSetPasswordEmail = sendSetPasswordEmail;
      this.skipVerification = skipVerification;
    }
  }
}