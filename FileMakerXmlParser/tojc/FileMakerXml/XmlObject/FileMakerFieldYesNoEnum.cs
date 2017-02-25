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
    public enum FileMakerFieldYesNoEnum
    {
        NO = 0,
        YES = 1
    }

    public static class FileMakerFieldYesNoEnumExtensions
    {
        public static string GetName(this FileMakerFieldYesNoEnum fieldYesNoEnum)
        {
            return Enum.GetName(typeof(FileMakerFieldYesNoEnum), fieldYesNoEnum);
        }

        public static int GetValue(this FileMakerFieldYesNoEnum fieldYesNoEnum)
        {
            return (int)fieldYesNoEnum;
        }

        public static FileMakerFieldYesNoEnum ConvertOrDefault(this string value, FileMakerFieldYesNoEnum defaultYesNoEnum)
        {
            FileMakerFieldYesNoEnum result = defaultYesNoEnum;
            foreach (FileMakerFieldYesNoEnum fieldYesNoEnum in Enum.GetValues(typeof(FileMakerFieldYesNoEnum)))
            {
                if (string.Compare(value, fieldYesNoEnum.ToString(), true) == 0)
                {
                    result = fieldYesNoEnum;
                    break;
                }
            }
            return result;
        }

    }
}
