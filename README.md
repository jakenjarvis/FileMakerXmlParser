FileMakerXmlParser
==============================

# Overview
This is a C# library for parsing XML file of the FMPXMLRESULT format.  
File output from FileMaker has several problems. I have made this library to avoid it.  

* For ODBC connection and Excel output, the value of the repeating field is deleted!
* FileMaker outputs a character string including a special control code.

We need to output FMPXMLRESULT in order to correctly interpret [repeating fields](http://www.filemaker.com/help/12/fmp/html/create_db.8.23.html).

# Tutorials
## Use FileMakerXmlParser library to parse FMPXMLRESULT.

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


## sample FMPXMLRESULT format file. [XML FMPXMLRESULT grammar](http://www.filemaker.com/help/13/fmp/en/html/import_export.17.33.html)

    <?xml version="1.0" encoding="UTF-8"?>
    <FMPXMLRESULT xmlns="http://www.filemaker.com/fmpxmlresult">
        <ERRORCODE>0</ERRORCODE>
        <PRODUCT BUILD="5/23/2002" NAME="FileMaker Pro"
        VERSION="7.0"/>
        <DATABASE DATEFORMAT="MM/dd/yy" LAYOUT="summary"
        NAME="Employees.fmp12" RECORDS="23" TIMEFORMAT="hh:mm:ss"/>
        <METADATA>
            <FIELD EMPTYOK="NO" MAXREPEAT="1" NAME="First Name" TYPE="TEXT"/>
            <FIELD EMPTYOK="NO" MAXREPEAT="1" NAME="Last Name" TYPE="TEXT"/>
            <FIELD EMPTYOK="YES" MAXREPEAT="1" NAME="Department" TYPE="TEXT"/>
        </METADATA>
        <RESULTSET FOUND="2">
            <ROW MODID="47" RECORDID="34">
                <COL>
                    <DATA>Joe</DATA>
                </COL>
                <COL>
                    <DATA>Smith</DATA>
                </COL>
                <COL>
                    <DATA>Engineering</DATA>
                </COL>
            </ROW>
            <ROW MODID="89" RECORDID="78">
                <COL>
                    <DATA>Susan</DATA>
                </COL>
                <COL>
                    <DATA>Jones</DATA>
                </COL>
                <COL>
                    <DATA>Marketing</DATA>
                </COL>
            </ROW>
        </RESULTSET>
    </FMPXMLRESULT>


## Output result

    FILD:  First Name
    FILD:  Last Name
    FILD:  Department
    ROW: 0
    COL:  [First Name={Joe}]
    DATA:  Joe
    COL:  [Last Name={Smith}]
    DATA:  Smith
    COL:  [Department={Engineering}]
    DATA:  Engineering
    ROW: 1
    COL:  [First Name={Susan}]
    DATA:  Susan
    COL:  [Last Name={Jones}]
    DATA:  Jones
    COL:  [Department={Marketing}]
    DATA:  Marketing


# Apache License, Version 2.0

Copyright (c) 2017, Jaken Jarvis (jaken.jarvis@gmail.com).  

Licensed under the Apache License, Version 2.0 (the "License");  
you may not use this file except in compliance with the License.  
You may obtain a copy of the License at  

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software  
distributed under the License is distributed on an "AS IS" BASIS,  
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
See the License for the specific language governing permissions and  
limitations under the License.  

The author may be contacted via  
https://github.com/jakenjarvis/FileMakerXmlParser

