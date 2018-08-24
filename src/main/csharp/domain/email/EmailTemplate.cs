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
  public class EmailTemplate : Buildable<EmailTemplate>
  {
    public string defaultFromName;

    public string defaultHtmlTemplate;

    public string defaultSubject;

    public string defaultTextTemplate;

    public string fromEmail;

    public Guid? id;

    public LocalizedStrings localizedFromNames = new LocalizedStrings();

    public LocalizedStrings localizedHtmlTemplates = new LocalizedStrings();

    public LocalizedStrings localizedSubjects = new LocalizedStrings();

    public LocalizedStrings localizedTextTemplates = new LocalizedStrings();

    public string name;

    public EmailTemplate()
    {
    }

    public EmailTemplate(Guid? id, string name, string defaultFromName, string fromEmail, string defaultSubject,
                         string defaultHtmlTemplate, string defaultTextTemplate)
    {
      this.defaultFromName = defaultFromName;
      this.fromEmail = fromEmail;
      this.defaultHtmlTemplate = defaultHtmlTemplate;
      this.defaultSubject = defaultSubject;
      this.defaultTextTemplate = defaultTextTemplate;
      this.id = id;
      this.name = name;
    }

    public EmailTemplate(Guid? id, string name, string defaultFromName, string fromEmail, string defaultSubject,
                         string defaultHtmlTemplate, string defaultTextTemplate, LocalizedStrings localizedFromNames,
                         LocalizedStrings localizedSubjects, LocalizedStrings localizedHtmlTemplates,
                         LocalizedStrings localizedTextTemplates)
    {
      this.fromEmail = fromEmail;
      this.defaultFromName = defaultFromName;
      this.defaultHtmlTemplate = defaultHtmlTemplate;
      this.defaultSubject = defaultSubject;
      this.defaultTextTemplate = defaultTextTemplate;
      this.id = id;
      this.localizedFromNames = localizedFromNames;
      this.localizedHtmlTemplates = localizedHtmlTemplates;
      this.localizedSubjects = localizedSubjects;
      this.localizedTextTemplates = localizedTextTemplates;
      this.name = name;
    }

    public EmailTemplate With(Action<EmailTemplate> action)
    {
      action(this);
      return this;
    }
  }
}