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
using System.Collections.Generic;

namespace tojc.FileMakerXml.XmlObject
{
    public class FileMakerResultSet
    {
        protected FileMakerFmpXmlResult Parent { get; set; }

        public SortedList<long, FileMakerRow> Rows { get; protected set; }

        public long Found { get; set; }

        public FileMakerResultSet(FileMakerFmpXmlResult parent)
        {
            this.Parent = parent;

            this.Rows = new SortedList<long, FileMakerRow>();

            this.Found = 0;
        }

        public FileMakerRow CreateRow()
        {
            long index = this.Rows.Count;
            FileMakerRow result = new FileMakerRow(this, this.Parent.Metadata, index);
            this.Rows.Add(index, result);
            return result;
        }

    }
}
