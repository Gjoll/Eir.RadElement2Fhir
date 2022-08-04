using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadElement2Fhir.Packages
{
    public class GetElementData
    {
        public String? Id { get; set; }
        public String? Name { get; set; }
        public String? Definition { get; set; }
        public String? Question { get; set; }
        public Version? Version { get; set; }
        public IndexCode[]? Index_Codes { get; set; }
    }
}
