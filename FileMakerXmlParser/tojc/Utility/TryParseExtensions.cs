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
using System.ComponentModel;

namespace tojc.Utility
{
    public static class TryParseExtensions
    {
        public static T TryParse<T>(this string text) where T : struct
        {
            return TryParseOrDefault(text, default(T));
        }

        public static T TryParseOrDefault<T>(this string text, T defaultValue) where T : struct
        {
            T result = defaultValue;
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if ((converter != null) && (converter.CanConvertFrom(typeof(string))))
            {
                try
                {
                    result = (T)converter.ConvertFromString(text);
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }

    }
}
