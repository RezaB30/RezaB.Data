using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Data.Files
{
    public static class CSVGenerator
    {
        public static Stream GetStream<T>(IEnumerable<T> dataRows, string columnSeperator)
        {
            MemoryStream stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            var line = new List<string>();
            // write titles
            MemberInfo[] propertyInfos = typeof(T).GetProperties();
            // order by declaration order
            propertyInfos = propertyInfos.OrderBy(info => info.MetadataToken).ToArray();
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                var currentAttribute = propertyInfos[i].GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                if (currentAttribute == null)
                {
                    line.Add(propertyInfos[i].Name.Replace(columnSeperator, " ").Replace(Environment.NewLine, " "));
                }
                else
                {
                    line.Add(currentAttribute.GetName().Replace(columnSeperator, " ").Replace(Environment.NewLine, " "));
                }
            }
            writer.WriteLine(string.Join(columnSeperator, line.ToArray()));
            writer.Flush();
            // write data
            foreach (var row in dataRows)
            {
                line.Clear();
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    line.Add(Convert.ToString(typeof(T).GetProperty(propertyInfos[i].Name).GetValue(row)).Replace(columnSeperator, " ").Replace(Environment.NewLine, " "));
                }
                writer.WriteLine(string.Join(columnSeperator, line.ToArray()));
                writer.Flush();
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
