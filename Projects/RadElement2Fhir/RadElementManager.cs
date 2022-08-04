using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RadElement2Fhir.Packages;

namespace RadElement2Fhir
{
    internal class RadElementManager
    {
        const String ClsName = nameof(RadElementManager);

        public async Task<GetElement> QueryRadElementSet(String radElementSetId)
        {
            const String fcn = $"{ClsName}.{nameof(QueryRadElementSet)}";

            Console.WriteLine($"{fcn}. Querying RadElement set {radElementSetId}");

            RestResponse response = await this.SendCommand($"elements/{radElementSetId}?page=1");
            if (String.IsNullOrEmpty(response.Content) == true)
                throw new Exception($"Empty content returned from API call");
            GetElement? element = JsonConvert.DeserializeObject<GetElement>(response.Content);
            if (element == null)
                throw new Exception($"Error deserializing element");
            return element;
        }


        private async Task<RestResponse> SendCommand(String command)
        {
            const String fcn = $"{ClsName}.{nameof(SendCommand)}";

            //Console.WriteLine($"{fcn} Command '{command}'");
            RestClient client = new RestClient("https://api3.rsna.org/radelement/public/v1/");
            RestRequest request = new RestRequest(command);

            RestResponse response = await client.ExecuteGetAsync(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.Error.WriteLine($"{fcn} Response Status '{response.StatusCode}' '{response.StatusDescription}'");
                if (String.IsNullOrEmpty(response.Content) == false)
                    Console.Error.WriteLine($"{fcn} Response Content '{response.Content}'");
                if (String.IsNullOrEmpty(response.ErrorMessage) == false)
                    Console.Error.WriteLine($"{fcn} Response ErrorMessage '{response.ErrorMessage}'");
                throw new Exception($"{fcn} Error ResponseStatus {response.ResponseStatus}");
            }

            if (response.IsSuccessful == false)
                throw new Exception($"{fcn} HTTP Error code {response.StatusCode} returned");

            return response;
        }
    }
}
