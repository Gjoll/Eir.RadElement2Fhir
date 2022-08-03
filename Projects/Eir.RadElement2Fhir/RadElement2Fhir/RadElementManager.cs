using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RadElement2Fhir
{
    internal class RadElementManager
    {
        const String ClsName = nameof(RadElementManager);

        public async Task QueryRadElementSet(String radElementSetId)
        {
            const String fcn = $"{ClsName}.{nameof(QueryRadElementSet)}";

            Console.WriteLine($"{fcn}. Querying RadElement set {radElementSetId}");

            RestResponse response = await this.SendCommand($"sets?search={QueryRadElementSet}");
            //GetBreastCancerRiskByAssessment retVal = response.ParseResponse<GetBreastCancerRiskByAssessment>();
            //if (retVal.Data.AssessmentId != assessmentId)
            //    throw new Exception($"Invalid AssessmentId '{retVal.Data.AssessmentId}' returned, expected {assessmentId}");
            //return retVal;
        }


        private async Task<RestResponse> SendCommand(String command)
        {
            const String fcn = $"{ClsName}.{nameof(SendCommand)}";

            try
            {
                Console.WriteLine($"{fcn} Command '{command}'");
                RestClient client = new RestClient("http://api3.rsna.org/radelement/public/v1/");
                RestRequest request = new RestRequest(command);

                RestResponse response = await client.ExecuteGetAsync(request);

                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    Console.WriteLine($"{fcn} Response Status '{response.StatusCode}' '{response.StatusDescription}'");
                    if (String.IsNullOrEmpty(response.Content) == false)
                        Console.WriteLine($"{fcn} Response Content '{response.Content}'");
                    if (String.IsNullOrEmpty(response.ErrorMessage) == false)
                        Console.WriteLine($"{fcn} Response ErrorMessage '{response.ErrorMessage}'");
                    throw new Exception($"{fcn} Error ResponseStatus {response.ResponseStatus}");
                }

                if (String.IsNullOrEmpty(response.Content) == false)
                {
                    JObject jDoc = JObject.Parse(response.Content);
                    String fJson = JsonConvert.SerializeObject(jDoc, Formatting.Indented);
                    Console.WriteLine($"{fcn} Response Content\n{fJson.Indent()}");
                }

                if (response.IsSuccessful == false)
                    throw new Exception($"{fcn} HTTP Error code {response.StatusCode} returned");

                return response;
            }
            catch(Exception err)
            {
                throw err;
            }
        }
    }
}
