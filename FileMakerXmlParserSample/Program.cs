using System;
using System.Collections.Generic;
using System.Diagnostics;
using tojc.FileMakerXml.XmlObject;

namespace tojc.FileMakerXml.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlFileName = @"..\..\sample.xml";
            FileMakerXmlParser xmlParser = new FileMakerXmlParser();
            xmlParser.SanitizeAndParse(xmlFileName);

            foreach (KeyValuePair<int, FileMakerField> col in xmlParser.FmpXmlResult.Metadata.Fields)
            {
                Debug.WriteLine("FILD:  " + col.Value.Name);
            }

            foreach (KeyValuePair<long, FileMakerRow> row in xmlParser.FmpXmlResult.ResultSet.Rows)
            {
                Debug.WriteLine("ROW: " + row.Key.ToString());

                foreach (KeyValuePair<int, FileMakerCol> col in row.Value.Cols)
                {
                    Debug.WriteLine("COL:  " + col.Value.ToString());

                    if (col.Value.IsRepeat())
                    {
                        for (int index = 0; index > col.Value.Field.MaxRepeat; index++)
                        {
                            Debug.WriteLine("DATA:  " + row.Value.GetDataValue(col.Value.Field.Name, index));
                        }
                    }
                    else
                    {
                        Debug.WriteLine("DATA:  " + row.Value.GetDataValue(col.Value.Field.Name));
                    }
                }
            }
        }
    }
}
