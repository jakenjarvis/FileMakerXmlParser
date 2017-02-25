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
    public class FileMakerRow
    {
        public FileMakerResultSet ResultSet { get; protected set; }
        public FileMakerMetadata Metadata { get; protected set; }
        public long Index { get; protected set; }

        public SortedList<int, FileMakerCol> Cols { get; protected set; }

        public long ModId { get; set; }
        public long RecordId { get; set; }

        public FileMakerRow(FileMakerResultSet resultSet, FileMakerMetadata metadata, long index)
        {
            this.ResultSet = resultSet;
            this.Metadata = metadata;
            this.Index = index;

            this.Cols = new SortedList<int, FileMakerCol>();

            this.ModId = 0;
            this.RecordId = 0;
        }

        public FileMakerCol CreateCol()
        {
            int index = this.Cols.Count;
            FileMakerField field = this.Metadata.GetField(index);
            FileMakerCol result = new FileMakerCol(this, field);
            this.Cols.Add(index, result);
            return result;
        }

        public string GetDataValue(string fieldName)
        {
            FileMakerField field = this.Metadata.GetFieldByName(fieldName);
            if (field.IsRepeat())
            {
                throw new InvalidOperationException("There is no index specification for fields with multiple data.");
            }
            return this.GetDataValue(field.Index, 0);
        }

        public string GetDataValue(string fieldName, int dataIndex)
        {
            FileMakerField field = this.Metadata.GetFieldByName(fieldName);
            return this.GetDataValue(field.Index, dataIndex);
        }

        protected string GetDataValue(int fieldIndex, int dataIndex)
        {
            string result = "";
            if ((fieldIndex < 0) || (fieldIndex >= this.Metadata.Fields.Count))
            {
                throw new ArgumentException("Index range is invalid.");
            }
            FileMakerCol col = this.Cols[fieldIndex];

            if ((dataIndex < 0) || (dataIndex >= col.Datas.Count))
            {
                throw new ArgumentException("Index range is invalid.");
            }
            result = col.Datas[dataIndex].Value;
            return result;
        }

        public void SetDataValue(string fieldName, string value)
        {
            FileMakerField field = this.Metadata.GetFieldByName(fieldName);
            if (field.IsRepeat())
            {
                throw new InvalidOperationException("There is no index specification for fields with multiple data.");
            }
            this.SetDataValue(field.Index, 0, value);
        }

        public void SetDataValue(string fieldName, int dataIndex, string value)
        {
            FileMakerField field = this.Metadata.GetFieldByName(fieldName);
            this.SetDataValue(field.Index, dataIndex, value);
        }

        protected void SetDataValue(int fieldIndex, int dataIndex, string value)
        {
            if ((fieldIndex < 0) || (fieldIndex >= this.Metadata.Fields.Count))
            {
                throw new ArgumentException("Index range is invalid.");
            }
            FileMakerCol col = this.Cols[fieldIndex];

            if ((dataIndex < 0) || (dataIndex >= col.Datas.Count))
            {
                throw new ArgumentException("Index range is invalid.");
            }
            col.Datas[dataIndex].Value = value;
        }

        public override string ToString()
        {
            string result = "FileMakerRow: [Index=" + this.Index + "],"
                + "[ModId=" + this.ModId + "],"
                + "[RecordId=" + this.RecordId + "],"
                + "[Cols.Count=" + this.Cols.Count + "],"
                + "{";

            foreach (KeyValuePair<int, FileMakerCol> row in this.Cols)
            {
                result += "" + row.Value.ToString() + ",";
            }

            result += "}";
            return result;
        }
    }
}
