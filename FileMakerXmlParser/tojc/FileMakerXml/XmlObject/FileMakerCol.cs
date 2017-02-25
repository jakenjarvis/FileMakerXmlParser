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
    public class FileMakerCol
    {
        public FileMakerRow Row { get; protected set; }
        public FileMakerField Field { get; protected set; }
        public SortedList<int, FileMakerData> Datas { get; protected set; }

        public FileMakerCol(FileMakerRow row, FileMakerField field)
        {
            this.Row = row;
            this.Field = field;
            this.Datas = new SortedList<int, FileMakerData>();
        }

        public FileMakerData CreateData(string value)
        {
            int index = this.Datas.Count;
            FileMakerData result = new FileMakerData(this, index, value);
            this.Datas.Add(index, result);
            return result;
        }

        public bool IsRepeat()
        {
            bool result = this.Field.IsRepeat();
            if (this.Datas.Count >= 2)
            {
                result = true;
            }
            return result;
        }

        public override string ToString()
        {
            string result = "[" + this.Field.Name + "={";

            if(!this.IsRepeat())
            {
                result += "" + this.Datas[0].ToString() + "";
            }
            else
            {
                foreach (KeyValuePair<int, FileMakerData> data in this.Datas)
                {
                    result += "" + data.Value.ToString() + ",";
                }
            }
            result += "}]";
            return result;
        }

    }
}
