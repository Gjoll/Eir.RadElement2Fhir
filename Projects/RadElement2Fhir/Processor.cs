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
            if (String.IsNullOrEmpty(vsOutputDir) == true)
            {
                Trace.WriteLine(fhirValueSet);
                Console.WriteLine(fhirValueSet);
                return;
            }

            String vsOutputPath = Path.Combine(vsOutputDir, vsName);
            File.WriteAllText(vsOutputPath, fhirValueSet);
        }

        bool CreateFshValueSet(GetElement element, out String vsName, out String vsText)
        {
            StringBuilder sb = new StringBuilder();
            vsName = $"{element.Data?.Name?.ToMachineName()}VS";
            sb.AppendLine($"ValueSet: {vsName}");
            sb.AppendLine($"Id: {element.Data?.Id?.ToMachineName()}");
            sb.AppendLine($"Title: \"{element.Data?.Name}\"");
            sb.AppendLine($"Description: \"\"\"");
            sb.Append(element.Data?.Definition?.Break("             "));
            sb.AppendLine($"             \"\"\"");
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
