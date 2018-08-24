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
  public class Webhook : Buildable<Webhook>
  {
    public List<Guid> applicationIds = new List<Guid>();

    public int? timeout;

    public string description;

    public bool global;

    public NotificationHeaders headers = new NotificationHeaders();

    public string httpAuthenticationPassword;

    public string httpAuthenticationUsername;

    public Guid? id;

    public int? readWriteTimeout;

    public string sslCertificate;

    public Uri url;

    public Webhook()
    {
    }

    public Webhook(Uri url) : this(null, url, false, null, null, null, null, 1000, 2000)
    {
    }

    public Webhook(Guid? id, Uri url, bool global, string httpAuthenticationUsername,
                   string httpAuthenticationPassword, string sslCertificate, Dictionary<string, string> headers,
                   int timeout, int readWriteTimeout, params Guid[] applicationIds)
    {
      this.id = id;
      this.url = url;
      this.global = global;
      this.httpAuthenticationUsername = httpAuthenticationUsername;
      this.httpAuthenticationPassword = httpAuthenticationPassword;
      this.sslCertificate = sslCertificate;

      if (headers != null)
      {
        foreach (var header in headers)
        {
          this.headers.Add(header.Key, header.Value);
        }
      }

      this.timeout = timeout;
      this.readWriteTimeout = readWriteTimeout;
      for (var i = 0; i < applicationIds.Length; i++)
      {
        this.applicationIds.Add(applicationIds[i]);
      }
    }

    public Webhook With(Action<Webhook> action)
    {
      action(this);
      return this;
    }
  }
}