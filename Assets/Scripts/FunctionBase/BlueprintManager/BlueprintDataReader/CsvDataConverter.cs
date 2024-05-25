namespace FunctionBase.BlueprintManager.BlueprintDataReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Converter.Base;
    using FunctionBase.BlueprintManager.BlueprintBase;
    using FunctionBase.Extensions;
    using TheOneStudio.HyperCasual.Extensions;
    using Unity.VisualScripting;

    public class CsvDataConverter
    {
        public static T[] Deserialize<T>(string text) { return (T[])CreateDataArray<T>(typeof(T), ParseCsv(text)); }

        //public static T DeserializeSingleReCord<T>(string text) { return (T)CreateDataArray(typeof(T), ParseCsv(text)); }

        private static object CreateDataArray<T>(Type type, List<string[]> rows)
        {
            var arrayValue    = new List<T>();
            var table         = new Dictionary<string, int>();
            T   currentRecord = default;

            for (var i = 0; i < rows[0].Length; i++)
            {
                var id  = rows[0][i];
                var id2 = "";
                for (int j = 0; j < id.Length; j++)
                {
                    if ((id[j] >= 'a' && id[j] <= 'z') || (id[j] >= '0' && id[j] <= '9'))
                        id2 += ((char)id[j]).ToString();
                    else if (id[j] >= 'A' && id[j] <= 'Z')
                        id2 += ((char)(id[j] - 'A' + 'a')).ToString();
                }

                table.Add(id, i);
                if (!table.ContainsKey(id2))
                    table.Add(id2, i);
            }

            for (var i = 1; i < rows.Count; i++)
            {
                var fieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList().FirstOrDefault(e => e.GetCustomAttribute<KeyOfRecord>() != null);
                if (fieldInfo != null && !rows[i][table[fieldInfo.Name]].IsNullOrEmpty())
                {
                    var rowData = (T)CreateRecord(rows[i], table, type);
                    if (rowData.GetFieldInfoWithAttribute<KeyOfRecord>().First().GetValue(rowData) != null)
                    {
                        currentRecord = rowData;
                        arrayValue.Add(rowData);
                    }
                }

                else if (currentRecord != null)
                {
                    var rowData = (T)CreateRecord(rows[i], table, type, currentRecord);
                }
            }

            return arrayValue.ToArray();
        }

        private static object CreateRecord(string[] cols, Dictionary<string, int> table, Type type, object currentObj = null)
        {
            object v = currentObj ?? Activator.CreateInstance(type);

            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var         keyName   = fieldInfo.ToList().First(e => e.GetCustomAttribute<KeyOfRecord>() != null).Name;
            if (cols[table[keyName]].IsNullOrEmpty())
            {
                fieldInfo.ToList().Where(field => !table.ContainsKey(field.Name) && typeof(IBlueprintByRow).IsAssignableFrom(field.FieldType)).ToList().ForEach(field =>
                {
                    ((IBlueprintByRow)field.GetValue(v)).ReadRecord(cols, table);
                });
            }
            else
            {
                foreach (FieldInfo tmp in fieldInfo)
                {
                    if (table.ContainsKey(tmp.Name))
                    {
                        int idx = table[tmp.Name];
                        if (tmp.GetCustomAttribute<KeyOfRecord>() != null && cols[idx].IsNullOrEmpty()) continue;
                        if (idx < cols.Length)
                            SetValue(v, tmp, cols[idx]);
                    }
                    else if (typeof(IBlueprintByRow).IsAssignableFrom(tmp.FieldType))
                    {
                        ((IBlueprintByRow)tmp.GetValue(v)).ReadRecord(cols, table);
                    }
                }
            }


            return v;
        }

        public static void SetValue(object v, FieldInfo fieldInfo, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            var converter = ConverterManager.Instance.GetConverter(fieldInfo.FieldType);
            if (converter != null && converter.CanConvert(fieldInfo.FieldType))
            {
                var dataValue = converter.ConvertFromString(value, fieldInfo.FieldType);
                fieldInfo.SetValue(v, dataValue);
                return;
            }

            fieldInfo.SetValue(v, value);
        }


        private static List<string[]> ParseCsv(string text, char separator = ',')
        {
            List<string[]> lines  = new List<string[]>();
            List<string>   line   = new List<string>();
            StringBuilder  token  = new StringBuilder();
            bool           quotes = false;

            for (int i = 0; i < text.Length; i++)
            {
                if (quotes == true)
                {
                    if ((text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\"') || (text[i] == '\"' && i + 1 < text.Length && text[i + 1] == '\"'))
                    {
                        token.Append('\"');
                        i++;
                    }
                    else if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == 'n')
                    {
                        token.Append('\n');
                        i++;
                    }
                    else if (text[i] == '\"')
                    {
                        line.Add(token.ToString());
                        token  = new StringBuilder();
                        quotes = false;
                        if (i + 1 < text.Length && text[i + 1] == separator)
                            i++;
                    }
                    else
                    {
                        token.Append(text[i]);
                    }
                }
                else if (text[i] == '\r' || text[i] == '\n')
                {
                    if (token.Length > 0)
                    {
                        line.Add(token.ToString());
                        token = new StringBuilder();
                    }

                    if (line.Count > 0)
                    {
                        lines.Add(line.ToArray());
                        line.Clear();
                    }
                }
                else if (text[i] == separator)
                {
                    line.Add(token.ToString());
                    token = new StringBuilder();
                }
                else if (text[i] == '\"')
                {
                    quotes = true;
                }
                else
                {
                    token.Append(text[i]);
                }
            }

            if (token.Length > 0)
            {
                line.Add(token.ToString());
            }

            if (line.Count > 0)
            {
                lines.Add(line.ToArray());
            }

            return lines;
        }
    }
}