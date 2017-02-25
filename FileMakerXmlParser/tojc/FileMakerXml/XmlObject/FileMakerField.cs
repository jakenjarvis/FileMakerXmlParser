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
using tojc.Utility;

namespace tojc.FileMakerXml.XmlObject
{
    public class FileMakerField
    {
        public int Index { get; protected set; }

        public string Name { get; set; }
        public FileMakerFieldType Type { get; set; }
        public FileMakerFieldYesNoEnum EmptyOk { get; set; }
        public int MaxRepeat { get; set; }

        public FileMakerField(int index, string name, string type, string emptyOk, string maxRepeat)
        {
            this.Index = index;

            this.Name = name;
            this.Type = type.ConvertOrDefault(FileMakerFieldType.TEXT);
            this.EmptyOk = emptyOk.ConvertOrDefault(FileMakerFieldYesNoEnum.NO);
            this.MaxRepeat = maxRepeat.TryParseOrDefault<int>(1);
        }

        public bool IsRepeat()
        {
            bool result = false;
            if (this.MaxRepeat >= 2)
            {
                result = true;
            }
            return result;
        }

    }
}
