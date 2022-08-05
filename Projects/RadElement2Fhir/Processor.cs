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
        Dictionary<String, SystemCodes> systemDict = new Dictionary<string, SystemCodes>();

        Options options;

        public Processor(Options options)
        {
            this.options = options;
        }

        public async Task Execute()
        {
            await CreateValueSets();
            await CreateCodeSystems();
        }

        void AddSystemCode(IndexCode indexCode)
        {
            if (String.IsNullOrEmpty(indexCode.System))
                return;

            String systemName = indexCode.System.Trim().ToUpper();

            if (this.systemDict.TryGetValue(systemName, out SystemCodes? systemCodes) == false)
            {
                systemCodes = new SystemCodes();
                this.systemDict.Add(systemName, systemCodes);
            }
            systemCodes.Codes.Add(indexCode);
        }

        async Task CreateCodeSystems()
        {
            foreach (Options.CodeSystem cs in options.CodeSystems)
                await CreateCodeSystem(cs.Name, cs.ID, cs.Title);
        }

        async Task CreateCodeSystem(String csName,
            String csId,
            String csTitle)
        {
            String name = $"{csName.ToMachineName()}CS";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"CodeSystem: {name}");
            sb.AppendLine($"Id: {csId}");
            sb.AppendLine($"Title: \"{csTitle}\"");

            if (this.systemDict.TryGetValue(csName.Trim().ToUpper(), out SystemCodes? systemCodes) == true)
            {
                foreach (var code in syste)
            }

            String fhirCodeSystem = sb.ToString();
            String csOutputDir = options.VSOutput;
            Trace.WriteLine(fhirCodeSystem);
            if (String.IsNullOrEmpty(csOutputDir) == true)
            {
                Console.WriteLine(fhirCodeSystem);
                return;
            }

            String csOutputPath = Path.Combine(csOutputDir, name);
            File.WriteAllText(csOutputPath, fhirCodeSystem);
        }




        async Task CreateValueSets()
        {
            foreach (Options.ValueSet vs in options.ValueSets)
                await CreateValueSet(vs.Id);
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
            sb.Append(data.Definition?.Break("    "));
            sb.AppendLine($"    \"\"\"");

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
                foreach (IndexCode indexCode in data.Index_Codes)
                {
                    String? system = indexCode.System;
                    String? code = indexCode.Code;
                    String? display = indexCode.Display;

                    sb.AppendLine($"{system}#{code} \"{display}\"");
                    AddSystemCode(indexCode);
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
