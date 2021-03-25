using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Group_Project.Services
{
    static class MobileApplicationHttpClient
    {
        private static HttpClient httpClient;
        private static String baseURL = "https://ptsv2.com/t/scud0-1616693073/post";
        // You can view the "dumps", which are the POST requests (including the JSON data sent) at: https://ptsv2.com/t/scud0-1616693073


        static MobileApplicationHttpClient()
        {
            httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(baseURL);
        }


        public static async Task <Object> PostObjectAsynchronouslyAndReturnResultAsObject(Object objectToConvertToJson)
        {
            try
            {
                String jsonStringRepresentation = JsonConvert.SerializeObject(objectToConvertToJson);
                StringContent content = new StringContent(jsonStringRepresentation, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await httpClient.PostAsync(baseURL, content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    String responseJsonString = await responseMessage.Content.ReadAsStringAsync();
                    // Once we start receiving JSON objects back from the cloud, we can convert them into normal C# objects,
                    // and then cast them to an instance of a specific class:
                    //return JsonConvert.DeserializeObject(responseJsonString);
                    // But for now:
                    return responseJsonString;
                }
            }
            catch(HttpRequestException exception)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", exception.Message);
            }
            return null;
        }
    }
}
