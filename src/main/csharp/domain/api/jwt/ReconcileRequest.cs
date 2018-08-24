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
  public class ReconcileRequest
  {
    public Guid applicationId;

    public string encodedJWT;

    public string device;

    public Guid identityProviderId;

    public string ipAddress;

    public MetaData metaData;

    public ReconcileRequest()
    {
    }

    public ReconcileRequest(Guid identityProviderId, Guid applicationId, string encodedJWT)
    {
      this.identityProviderId = identityProviderId;
      this.applicationId = applicationId;
      this.encodedJWT = encodedJWT;
    }

    public ReconcileRequest(Guid identityProviderId, Guid applicationId, string encodedJWT, String ipAddress)
    {
      this.applicationId = applicationId;
      this.encodedJWT = encodedJWT;
      this.identityProviderId = identityProviderId;
      this.ipAddress = ipAddress;
    }
  }
}