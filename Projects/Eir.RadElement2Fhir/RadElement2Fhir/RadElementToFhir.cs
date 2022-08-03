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
            await LoadRadElementSet(this.RadElementId);
        }

        async Task LoadRadElementSet(String radElementId)
        {
            RadElementManager mgr = new RadElementManager();
            await mgr.QueryRadElementSet(radElementId);
        }
    }
}
