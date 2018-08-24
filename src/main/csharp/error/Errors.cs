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
using Newtonsoft.Json;

namespace FusionAuth.Error
{
  /**
  * Standard error domain object that can also be used as the response from an API call.
  */
  public class Errors
  {
    public readonly IDictionary<string, List<Error>> fieldErrors = new Dictionary<string, List<Error>>();

    public readonly IList<Error> generalErrors = new List<Error>();

    /**
      * Search through all errors, general and field, and if any error.code is prefixed by the codePrefix, passed in,
      * return true. Otherwise return false.
      * <p>
      * This is a little loose, making the caller know which code prefixes the Validator is using, an enum or something
      * might work nicely... for now keeping this.
      */
    public bool ContainsError(string codePrefix)
    {
      foreach (var error in generalErrors)
      {
        if (error.code.StartsWith(codePrefix, StringComparison.Ordinal))
        {
          return true;
        }
      }

      foreach (var errors in fieldErrors.Values)
      {
        foreach (var error in errors)
        {
          if (error.code.StartsWith(codePrefix, StringComparison.Ordinal))
          {
            return true;
          }
        }
      }

      return false;
    }

    /**
    * This is named <strong>empty</strong> because <strong>isEmpty</strong> is a JavaBean property name and Jackson
    * serializes that out.
    *
    * @return True if this Errors is empty, false otherwise.
    */
    public bool Empty()
    {
      return generalErrors.Count == 0 && fieldErrors.Count == 0;
    }

    public override bool Equals(object obj)
    {
      if (this == obj) return true;
      if (obj == null || GetType() != obj.GetType()) return false;

      var errors = (Errors) obj;
      return fieldErrors.Equals(errors.fieldErrors) && generalErrors.Equals(errors.fieldErrors);
    }

    public override int GetHashCode()
    {
      var result = generalErrors.GetHashCode();
      result = 31 * result + fieldErrors.GetHashCode();
      return result;
    }

    /**
     * @return the total count of all errors. All field errors and general errors
    */
    public int Size()
    {
      return generalErrors.Count + fieldErrors.Values.Count;
    }

    public override string ToString()
    {
      return JsonConvert.SerializeObject(this);
    }
  }
}