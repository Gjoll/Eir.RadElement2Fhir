using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadElement2Fhir
{
    static class Extensions
    {
        const String ClsName = nameof(Extensions);

        public static String Indent(this string value, String indent = "  > ")
        {
            StringBuilder sb = new StringBuilder();
            foreach (String line in value.Split('\n'))
            {
                String clean = line.Replace("\r", "");
                sb.AppendLine($"{indent}{clean}");
            }
            return sb.ToString();
        }
    }
}
