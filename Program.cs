using System.Text;
using DotEnv.Core;

namespace OpenAI_API_Call
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load environment variables from .env file
            var env = new EnvLoader().Load();
            
            // Get the AI Credentials from environment variables
            string endpoint = env["OPENAI_API_ENDPOINT"];
            string apiKey = env["OPENAI_API_KEY"];
            string model = env["OPENAI_MODEL"];
            string userMessage = env["OPENAI_USER_MESSAGE"];
            string temperature = env["OPENAI_TEMPERATURE"];
            
            // Check if API key is set
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Error: OPENAI_API_KEY not found or not set properly in .env file.");
                Console.WriteLine("Please set your OpenAI API key in the .env file.");
                return;
            }
            
            // Create HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                // Set the API key in the headers
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                
                // Build the JSON payload from environment variables
                string json = $@"{{
                    ""model"": ""{model}"",
                    ""messages"": [
                        {{
                            ""role"": ""user"",
                            ""content"": ""{userMessage}""
                        }}
                    ],
                    ""temperature"": {temperature}
                }}";
                
                // Store JSON payload as Content 
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                try
                {
                    // Make the API call to OpenAI
                    HttpResponseMessage response = await client.PostAsync(
                        endpoint, 
                        content);
                    
                    // Read and display the response
                    string responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response from OpenAI:");
                    Console.WriteLine(responseString);
                }
                catch (Exception ex)
                {
                    // Display Error if API call fails
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
