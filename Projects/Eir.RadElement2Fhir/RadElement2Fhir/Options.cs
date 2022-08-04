using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RadElement2Fhir
{
    public class Options
    {
        /// <summary>
        /// Json compatability level
        /// </summary>
        public Int32 Compatability { get; set; } = 1;

        public String UrlBase { get; set; } = "https://api3.rsna.org/radelement/public/v1/";
        public String VSOutput { get; set; } = String.Empty;

        /// <summary>
        /// Describes one SQL connection string
        /// </summary>
        public class ValueSet
        {
            public String Id { get; set; }  = String.Empty;
        }

        /// <summary>
        /// Accounts
        /// </summary>
        public List<ValueSet> ValueSets { get; set; } = new List<ValueSet>();


        public void Save(String path)
        {
            String json = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, json);
        }

        public static Options Load(String path)
        {
            String json = File.ReadAllText(path);
            Options? options = JsonConvert.DeserializeObject<Options>(json);
            if (options == null)
                throw new Exception($"Error loading options '{path}'");
            return options;
        }
    }
}
