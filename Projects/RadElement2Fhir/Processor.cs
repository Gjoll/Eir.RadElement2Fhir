using Newtonsoft.Json.Linq;
using RadElement2Fhir.Packages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadElement2Fhir
{
    public class Processor
    {
        Options options;

        public Processor(Options options)
        {
            this.options = options;
        }

        public async Task Execute()
        {
            foreach (Options.ValueSet vs in options.ValueSets)
            {
                await CreateValueSet(vs.Id);
            }
        }

        async Task CreateValueSet(String radElementId)
        {
            if (String.IsNullOrEmpty(radElementId))
                throw new Exception($"Rad element id not specified");
            GetElement element = await LoadRadElementSet(radElementId);
            if (CreateFshValueSet(element, out String vsName, out String fhirValueSet) == false)
                return;
            String vsOutputDir = options.VSOutput;

            Trace.WriteLine(fhirValueSet);
            if (String.IsNullOrEmpty(vsOutputDir) == true)
            {
                Console.WriteLine(fhirValueSet);
                return;
            }

            String vsOutputPath = Path.Combine(vsOutputDir, vsName);
            File.WriteAllText(vsOutputPath, fhirValueSet);
        }

        bool CreateFshValueSet(GetElement element, out String vsName, out String vsText)
        {
            if (element.Data == null)
                throw new Exception($"GetElement data field is empty");
            GetElementData data = element.Data;

            StringBuilder sb = new StringBuilder();
            vsName = $"{data.Name?.ToMachineName()}VS";
            sb.AppendLine($"ValueSet: {vsName}");
            sb.AppendLine($"Id: {data.Id?.ToMachineName()}");
            sb.AppendLine($"Title: \"{data.Name}\"");
            sb.AppendLine($"Description: \"\"\"");
            sb.Append(data.Definition?.Break("             "));
            sb.AppendLine($"             \"\"\"");
            {
                Packages.Version? version = data.Version;
                if (version != null)
                {
                    if (version.Date != null)
                        sb.AppendLine($" * ^date = {version.Date}");
                    if (version.Name != null)
                        sb.AppendLine($" * ^version = \"{version.Name}\"");
                    switch (version.Status?.Trim().ToUpper())
                    {
                        case null:
                            break;
                        case "PUBLISHED":
                            sb.AppendLine($" * ^status = #active");
                            break;
                        default:
                            throw new Exception($"Unknown version status '{version.Status}");
                    }
                }
            }

            if (data.Index_Codes != null)
            {
                foreach (Index_Code indexCode in data.Index_Codes)
                {
                    String? system  = indexCode.System;
                    String? code = indexCode.Code;
                    String? display = indexCode.Display;

                    sb.AppendLine($"{system}#{code} \"{display}\"");
                }
            }


            vsText = sb.ToString();
            return true;
        }

        async Task<GetElement> LoadRadElementSet(String radElementId)
        {
            RadElementManager mgr = new RadElementManager();
            return await mgr.QueryRadElementSet(radElementId);
        }
    }
}
