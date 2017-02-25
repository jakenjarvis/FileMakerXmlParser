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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using tojc.FileMakerXml.XmlObject;
using tojc.Utility;

namespace tojc.FileMakerXml
{
    public class FileMakerXmlParser
    {
        public const string D_FMPXMLRESULT_NAMESPACE = @"http://www.filemaker.com/fmpxmlresult";

        public Encoding Encoding { get; set; }
        public XmlDocument Document { get; set; }
        public FileMakerFmpXmlResult FmpXmlResult { get; protected set; }

        public FileMakerXmlParser()
        {
            this.Encoding = Encoding.GetEncoding("UTF-8");
            this.Document = new XmlDocument();
            this.FmpXmlResult = new FileMakerFmpXmlResult();
        }

        public void SanitizeAndParse(string srcFileName, string destFileName, bool escapeWhenSanitizing = false)
        {
            FileMakerXmlParser.SanitizeToSave(srcFileName, destFileName, this.Encoding, escapeWhenSanitizing);
            this.Parse(destFileName);
        }

        public void SanitizeAndParse(string srcFileName, bool escapeWhenSanitizing = false)
        {
            string sanitizingXml = FileMakerXmlParser.Sanitize(srcFileName, this.Encoding, escapeWhenSanitizing);
            this.ParseXml(sanitizingXml);
        }

        public void Parse(string filename)
        {
            this.Document.Load(new StreamReader(filename));
            this.executeXmlParse();
        }

        public void Parse(Stream inStream)
        {
            this.Document.Load(inStream);
            this.executeXmlParse();
        }

        public void Parse(XmlReader reader)
        {
            this.Document.Load(reader);
            this.executeXmlParse();
        }

        public void Parse(TextReader txtReader)
        {
            this.Document.Load(txtReader);
            this.executeXmlParse();
        }

        public void ParseXml(string xml)
        {
            this.Document.LoadXml(xml);
            this.executeXmlParse();
        }

        protected void executeXmlParse()
        {
            // https://www.filemaker.com/help/14/fmp/ja/html/import_export.18.29.html

            XmlNamespaceManager xmlNsManager = new XmlNamespaceManager(this.Document.NameTable);
            xmlNsManager.AddNamespace("ns", D_FMPXMLRESULT_NAMESPACE);

            XmlElement root = this.Document.DocumentElement;

            // FMPXMLRESULT / ERRORCODE
            XmlNode nodeErrorCode = root.SelectSingleNode("/ns:FMPXMLRESULT/ns:ERRORCODE", xmlNsManager);
            //Debug.WriteLine(nodeErrorCode.InnerText);
            this.FmpXmlResult.ErrorCode = nodeErrorCode.InnerText.TryParse<int>();//

            // FMPXMLRESULT / PRODUCT
            XmlNode nodeProduct = root.SelectSingleNode("/ns:FMPXMLRESULT/ns:PRODUCT", xmlNsManager);
            this.FmpXmlResult.Product.Name = nodeProduct.Attributes.GetNamedItem("NAME").Value;//
            this.FmpXmlResult.Product.Version = nodeProduct.Attributes.GetNamedItem("VERSION").Value;//
            this.FmpXmlResult.Product.Build = nodeProduct.Attributes.GetNamedItem("BUILD").Value;//

            // FMPXMLRESULT / DATABASE
            XmlNode nodeDatabase = root.SelectSingleNode("/ns:FMPXMLRESULT/ns:DATABASE", xmlNsManager);
            this.FmpXmlResult.Database.Name = nodeDatabase.Attributes.GetNamedItem("NAME").Value;//
            this.FmpXmlResult.Database.Records = nodeDatabase.Attributes.GetNamedItem("RECORDS").Value.TryParse<long>();//
            this.FmpXmlResult.Database.DateFormat = nodeDatabase.Attributes.GetNamedItem("DATEFORMAT").Value;//
            this.FmpXmlResult.Database.TimeFormat = nodeDatabase.Attributes.GetNamedItem("TIMEFORMAT").Value;//
            this.FmpXmlResult.Database.Layout = nodeDatabase.Attributes.GetNamedItem("LAYOUT").Value;//

            // FMPXMLRESULT / METADATA / FIELD
            XmlNodeList nodeFieldList = root.SelectNodes("/ns:FMPXMLRESULT/ns:METADATA/ns:FIELD", xmlNsManager);
            for (int intIndex = 0; intIndex < nodeFieldList.Count; intIndex++)
            {
                XmlElement nodeField = (XmlElement)nodeFieldList[intIndex];
                Debug.WriteLine(nodeField.GetAttribute("NAME"));
                this.FmpXmlResult.Metadata.CreateField(
                    nodeField.GetAttribute("NAME"),
                    nodeField.GetAttribute("TYPE"),
                    nodeField.GetAttribute("EMPTYOK"),
                    nodeField.GetAttribute("MAXREPEAT"));//
            }

            // FMPXMLRESULT / RESULTSET
            XmlNode nodeResultSet = root.SelectSingleNode("/ns:FMPXMLRESULT/ns:RESULTSET", xmlNsManager);
            this.FmpXmlResult.ResultSet.Found = nodeResultSet.Attributes.GetNamedItem("FOUND").Value.TryParse<int>();//

            // FMPXMLRESULT / RESULTSET / ROW
            XmlNodeList nodeRowList = nodeResultSet.SelectNodes("ns:ROW", xmlNsManager);
            for (int intRowIndex = 0; intRowIndex < nodeRowList.Count; intRowIndex++)
            {
                Debug.WriteLine("ROW:" + intRowIndex);
                XmlElement nodeRow = (XmlElement)nodeRowList[intRowIndex];

                FileMakerRow fmpRow = this.FmpXmlResult.ResultSet.CreateRow();//
                fmpRow.ModId = nodeRow.GetAttribute("MODID").TryParse<int>();//
                fmpRow.RecordId = nodeRow.GetAttribute("RECORDID").TryParse<int>();//

                // FMPXMLRESULT / RESULTSET / ROW / COL
                XmlNodeList nodeColumnList = nodeRow.SelectNodes("ns:COL", xmlNsManager);
                for (int intColumnIndex = 0; intColumnIndex < nodeColumnList.Count; intColumnIndex++)
                {
                    XmlElement nodeColumn = (XmlElement)nodeColumnList[intColumnIndex];

                    FileMakerCol fmpCol = fmpRow.CreateCol();//

                    // FMPXMLRESULT / RESULTSET / ROW / COL / DATA
                    XmlNodeList nodeDataList = nodeColumn.GetElementsByTagName("DATA");
                    if (nodeDataList.Count == 1)
                    {
                        // general field
                        //Debug.WriteLine(nodeDataList.Item(0).InnerText);
                        fmpCol.CreateData(nodeDataList.Item(0).InnerText);//
                    }
                    else if (nodeDataList.Count >= 2)
                    {
                        // repeating fields
                        for (int intDataIndex = 0; intDataIndex < nodeDataList.Count; intDataIndex++)
                        {
                            XmlElement nodeData = (XmlElement)nodeDataList[intDataIndex];
                            //Debug.WriteLine(nodeData.InnerText);
                            fmpCol.CreateData(nodeData.InnerText);//
                        }
                    }
                }
            }

        }

        public static void SanitizeToSave(string srcFileName, string destFileName, Encoding encoding = null, bool escapeWhenSanitizing = false)
        {
            using (StreamWriter swDst = new StreamWriter(destFileName, false, encoding))
            {
                swDst.Write(Sanitize(srcFileName, encoding, escapeWhenSanitizing));
            }
        }

        public static string Sanitize(string filename, Encoding encoding = null, bool escapeWhenSanitizing = false)
        {
            StringBuilder result = new StringBuilder();

            if (encoding == null)
            {
                encoding = Encoding.GetEncoding("UTF-8");
            }

            string buffer = "";
            using (StreamReader srOrg = new StreamReader(filename, encoding))
            {
                buffer = srOrg.ReadToEnd();
            }

            // XML Sanitizing. Delete invalid character code.
            // https://support.microsoft.com/en-us/kb/325694
            foreach (char character in buffer)
            {
                int code = (int)character;
                if (code == 0x9 ||
                    code == 0xa ||
                    code == 0xd ||
                    (0x20 <= code && code <= 0xd7ff) ||
                    (0xe000 <= code && code <= 0xfffd) ||
                    (0x10000 <= code && code <= 0x10ffff))
                {
                    // valid character
                    result.Append(character);
                }
                else
                {
                    // invalid character
                    //Debug.WriteLine("Invalid Character:" + code);

                    if (escapeWhenSanitizing)
                    {
                        string escape = string.Format("&#x{0:x};", code);
                        result.Append(escape);
                    }
                }
            }

            return result.ToString();
        }

    }
}
