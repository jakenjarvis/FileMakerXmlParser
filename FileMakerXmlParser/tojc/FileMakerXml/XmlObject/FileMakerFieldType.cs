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

namespace tojc.FileMakerXml.XmlObject
{
    public enum FileMakerFieldType
    {
        TEXT = 0,
        DATE = 1,
        NUMBER = 2,
        TIME = 3,
        TIMESTAMP = 4,
        CONTAINER = 5
    }

    public static class FileMakerFieldTypeExtensions
    {
        public static string GetName(this FileMakerFieldType fieldType)
        {
            return Enum.GetName(typeof(FileMakerFieldType), fieldType);
        }

        public static int GetValue(this FileMakerFieldType fieldType)
        {
            return (int)fieldType;
        }

        public static FileMakerFieldType ConvertOrDefault(this string value, FileMakerFieldType defaultType)
        {
            FileMakerFieldType result = defaultType;
            foreach (FileMakerFieldType fieldType in Enum.GetValues(typeof(FileMakerFieldType)))
            {
                if (string.Compare(value, fieldType.ToString(), true) == 0)
                {
                    result = fieldType;
                    break;
                }
            }
            return result;
        }

    }

}
