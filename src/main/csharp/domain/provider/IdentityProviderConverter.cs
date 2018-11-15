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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FusionAuth.Domain
{
  public class IdentityProviderConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        return;
      }

      writer.WriteValue(value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var json = JObject.Load(reader);
      if (json["type"].Value<string>() == "Facebook")
      {
        return json.ToObject<FacebookIdentityProvider>(serializer);
      }

      if (json["type"].Value<string>() == "Google")
      {
        return json.ToObject<GoogleIdentityProvider>(serializer);
      }

      if (json["type"].Value<string>() == "Twitter")
      {
        return json.ToObject<TwitterIdentityProvider>(serializer);
      }

      if (json["type"].Value<string>() == "ExternalJWT")
      {
        return json.ToObject<ExternalJWTIdentityProvider>(serializer);
      }

      if (json["type"].Value<string>() == "OpenIDConnect")
      {
        return json.ToObject<OpenIdConnectIdentityProvider>(serializer);
      }

      return null;
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(IdentityProvider);
    }
  }
}