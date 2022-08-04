using Newtonsoft.Json.Linq;
using RadElement2Fhir.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadElement2Fhir
{
    internal class RadElementToFhir
    {
        public String OutputPath = String.Empty;
        public String RadElementId = String.Empty;

        public async Task Execute()
        {
            if (String.IsNullOrEmpty(RadElementId))
                throw new Exception($"Rad element id not specified");
            GetElement element = await LoadRadElementSet(this.RadElementId);
            String text = CreateFshValueSet(element);
            if (String.IsNullOrEmpty(OutputPath) == false)
            {
                File.WriteAllText(OutputPath, text);
                return;
            }
            Console.WriteLine(text);
        }

        String CreateFshValueSet(GetElement element)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ValueSet: {element.Data?.Name.ToMachineName()}VS");
            sb.AppendLine($"Id: {element.Data?.Id.ToMachineName()}");
            sb.AppendLine($"Title: \"{element.Data?.Name}\"");
            sb.AppendLine($"Description: \"\"\"");
            sb.Append(element.Data?.Definition?.Break("             "));
            sb.AppendLine($"             \"\"\"");
            return sb.ToString();
        }

        async Task<GetElement> LoadRadElementSet(String radElementId)
        {
            RadElementManager mgr = new RadElementManager();
            return await mgr.QueryRadElementSet(radElementId);
        }
    }
}
