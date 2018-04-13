/*Copyright 2018 Alaa Ben Fatma.
License: MIT
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TinySerializer
{
    public class TinySerializer
    {
        /// <summary>
        /// Serialize an object by converting its properties into textual representations.
        /// </summary>
        /// <param name="obj">The object that will be serialized.</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("<?");
            var myType = obj.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(obj, null);
                sb.AppendLine();
                sb.Append(@"	[" + prop.Name + "=" + propValue + "]");
            }
            sb.AppendLine();
            sb.Append("?>");
            return sb.ToString();
        }
        /// <summary>
        /// Returns a list of the serialized objects.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startString"></param>
        /// <param name="endString"></param>
        /// <param name="raw">Enabled: Will return the raw representation without any filtering.</param>
        /// <returns></returns>
        public static List<string> ExtractData(
            string text, string startString = "<?", string endString = "?>", bool raw = false)
        {
            var matched = new List<string>();
            var exit = false;
            while (!exit)
            {
                var indexStart = text.IndexOf(startString, StringComparison.Ordinal);
                var indexEnd = text.IndexOf(endString, StringComparison.Ordinal);
                if (indexStart != -1 && indexEnd != -1)
                {
                    if (raw)
                        matched.Add("<?" + text.Substring(indexStart + startString.Length,
                                        indexEnd - indexStart - startString.Length) + "?>");
                    else
                        matched.Add(text.Substring(indexStart + startString.Length,
                            indexEnd - indexStart - startString.Length));
                    text = text.Substring(indexEnd + endString.Length);
                }
                else
                {
                    exit = true;
                }
            }
            return matched;
        }
        /// <summary>
        /// Returns a list of all the serialized properties.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<Data> ExtractValuesFromData(string text)
        {
            var listOfData = new List<Data>();
            var allData = ExtractData(text, "[", "]");
            foreach (var data in allData)
            {
                var pName = data.Substring(0, data.IndexOf("=", StringComparison.Ordinal));
                var pValue = data.Substring(data.IndexOf("=", StringComparison.Ordinal) + 1);
                listOfData.Add(new Data {PropertyName = pName, Value = pValue});
            }
            return listOfData;
        }
        /// <summary>
        /// Deserialize an object based on some serialized data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeData"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(string serializeData, T target) where T : new()
        {
            var deserializedObjects = ExtractData(serializeData);

            foreach (var obj in deserializedObjects)
            {
                var properties = ExtractValuesFromData(obj);
                foreach (var property in properties)
                {
                    var propInfo = target.GetType().GetProperty(property.PropertyName);
                    propInfo?.SetValue(target,
                        Convert.ChangeType(property.Value, propInfo.PropertyType), null);
                }
            }
            return target;
        }
        public struct Data
        {
            public string PropertyName;
            public string Value;
        }
    }
}