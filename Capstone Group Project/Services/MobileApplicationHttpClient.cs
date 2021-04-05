using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Newtonsoft.Json;
using System;
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


        public static async Task<Type> PostObjectAsynchronouslyAndReturnResultAsSpecificedType<Type>(Object objectToConvertToJson)
        {
            String jsonStringRepresentation = JsonConvert.SerializeObject(objectToConvertToJson);
            String responseJsonString = await PostJsonStringAsynchronouslyAndReturnResultAsJsonString(jsonStringRepresentation);
            // Once we start receiving JSON objects back from the cloud, we can convert them into an instance of a specific class:
            return JsonConvert.DeserializeObject<Type>(responseJsonString);
        }


        private static async Task<String> PostJsonStringAsynchronouslyAndReturnResultAsJsonString(String jsonStringToSend)
        {
            try
            {
                StringContent content = new StringContent(jsonStringToSend, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await httpClient.PostAsync(baseURL, content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    String responseJsonString = await responseMessage.Content.ReadAsStringAsync();
                    return responseJsonString;
                }
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine("\nHttpRequestException Caught!");
                Console.WriteLine("Message :{0} ", exception.Message);
            }
            return null;
        }


        private static void DeserializationDemo()
        {
            Item item = JsonConvert.DeserializeObject<Item>("{ 'Id' : 'blah', 'Text' : 'blahblah', 'Description' : 'blahblahblah' }");
            String result = item.Id + " " + item.Text + " " + item.Description;
            // Place a breakpoint on the below line to inspect the values of the above variables:
            return;
        }


        private static void DeserializationTest1()
        {
            // We can see that the 'Hello' field is ommitted:
            CurrentLoginState myLoginState = JsonConvert.DeserializeObject<CurrentLoginState>("{ 'Hello' : 3, 'Account_ID' : 1234, 'Public_Key' : 'MYPUBLICKEY', 'Private_Key' : 'MYPRIVATEKEY', 'IdsOfConversationsUserIsParticipantIn' : [111, 222, 333], 'ConversationInvitations' : [ { 'Conversation_ID' : 777, 'Account_Username_Of_Sender' : 'Jamal' } ] }");
            String stringRepresentation = myLoginState.Account_ID + "\n" +
                myLoginState.Public_Key + "\n" +
                myLoginState.Private_Key + "\n" +
                myLoginState.IdsOfConversationsUserIsParticipantIn.ToString() + "\n" +
                myLoginState.ConversationInvitations.ToString();
            // Place a breakpoint on the below line to inspect the values of the above variables:
            return;
        }


        private static void DeserializationTest2()
        {
            // The below line crashes, since the input JSON String was clearly never a String Object in the first place.
            String result = JsonConvert.DeserializeObject<String>("{ 'Account_ID' : 1234, 'Public_Key' : 'MYPUBLICKEY', 'Private_Key' : 'MYPRIVATEKEY', 'IdsOfConversationsUserIsParticipantIn' : [111, 222, 333], 'ConversationInvitations' : [ { 'Conversation_ID' : 777, 'Account_Username_Of_Sender' : 'Jamal' } ] }");
            // Place a breakpoint on the below line to inspect the values of the above variable:
            return;
        }
    }
}
