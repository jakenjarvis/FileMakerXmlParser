/*
 * This file is part of the FileMakerXmlParser package.
 *
 * Copyright (c) 2017, Jaken Jarvis (jaken.jarvis@gmail.com).
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * The author may be contacted via
 * https://github.com/jakenjarvis/FileMakerXmlParser
 */
using System;
using System.Collections.Generic;

namespace tojc.FileMakerXml.XmlObject
{
    public class FileMakerMetadata
    {
        protected FileMakerFmpXmlResult Parent { get; set; }

        public SortedList<int, FileMakerField> Fields { get; protected set; }
        protected Dictionary<String, int> NamedKeys { get; set; }

        public FileMakerMetadata(FileMakerFmpXmlResult parent)
        {
            this.Parent = parent;

            this.Fields = new SortedList<int, FileMakerField>();
            this.NamedKeys = new Dictionary<String, int>();
        }

        public FileMakerField CreateField(string name, string type, string emptyOk, string maxRepeat)
        {
            int index = this.Fields.Count;
            FileMakerField result = new FileMakerField(index, name, type, emptyOk, maxRepeat);
            this.Fields.Add(index, result);
            this.NamedKeys.Add(name, index);
            return result;
        }

        public FileMakerField GetField(int index)
        {
            if ((index < 0) || (index >= this.Fields.Count))
            {
                throw new ArgumentException("Index range is invalid.");
            }
            return this.Fields[index];
        }

        public FileMakerField GetFieldByName(String fieldName)
        {
            FileMakerField result = FindFieldByName(fieldName);
            if (result == null)
            {
                throw new ArgumentException("Field name was not found.");
            }
            return result;
        }

        public FileMakerField FindFieldByName(String fieldName)
        {
            FileMakerField result = null;
            if (this.NamedKeys.ContainsKey(fieldName))
            {
                result = this.GetField(this.NamedKeys[fieldName]);
            }
            return result;
        }

    }
}
