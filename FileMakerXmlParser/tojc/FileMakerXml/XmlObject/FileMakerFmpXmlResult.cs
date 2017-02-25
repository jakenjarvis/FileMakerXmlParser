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
namespace tojc.FileMakerXml.XmlObject
{
    public class FileMakerFmpXmlResult
    {
        public FileMakerProduct Product { get; protected set; }
        public FileMakerDatabase Database { get; protected set; }
        public FileMakerMetadata Metadata { get; protected set; }
        public FileMakerResultSet ResultSet { get; protected set; }

        public int ErrorCode { get; set; }

        public FileMakerFmpXmlResult()
        {
            this.Product = new FileMakerProduct(this);
            this.Database = new FileMakerDatabase(this);
            this.Metadata = new FileMakerMetadata(this);
            this.ResultSet = new FileMakerResultSet(this);

            this.ErrorCode = 0;
        }
    }
}
