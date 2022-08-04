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

        public static String ToMachineName(this string text)
        {
            bool startOfWordFlag = true;
            StringBuilder sb = new StringBuilder();
            foreach (Char c in text)
            {
                if (Char.IsWhiteSpace(c))
                    startOfWordFlag = true;
                if ((Char.IsLetterOrDigit(c)) || (c == '_'))
                {
                    if (startOfWordFlag)
                    {
                        sb.Append(Char.ToUpper(c));
                        startOfWordFlag = false;
                    }
                    else
                        sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static String Break(this string text, String indent)
        {
            const Int32 LineLength = 30;
            StringBuilder sb = new StringBuilder();

            int index = 0;
            string result = "";
            foreach (char c in text)
            {
                //if smaller then 30 add to result
                if ((index <= LineLength) || (Char.IsWhiteSpace(c) == false))
                {
                    //increase char index
                    index++;
                    result += c;
                }
                else
                {
                    //if index hits the first 30 chars add to list and clear result and index
                    sb.AppendLine($"{indent}{result}");
                    result = "";
                    index = 0;
                }
            }

            if (result.Length > 0)
                sb.AppendLine($"{indent}{result}");
            return sb.ToString();
        }

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
