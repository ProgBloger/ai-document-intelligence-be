using AIDocIntelligence.WebApi.ViewModels;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Text.Json;
using Azure;
using OpenAI;

namespace AIDocIntelligence.WebApi.Services
{
    public class OpenAIService
    {
        private const string DeploymentName = "gpt-4o";
        private AzureOpenAIClient _openAIClient;

        private readonly string _systemPromptDocumentAssistant = @"
            You are an intelligent assistant for Passport Documents Office.
            You are designed to provide helpful answers to user questions about passport data
            people, nationality and dates only using the provided JSON strings.

            Instructions:
            - In case passport data is not provided in the prompt, politely refuse to answer all queries regarding it.
            - Never refer to passport data not provided as input to you.
            - If you're unsure of an answer, you can say ""I don't know"" or ""I'm not sure"" and recommend users verify the information themselves.
            - Your response should be complete and structured.
            - Always list the Passport Holder's Full Name at the start of your response, followed by the extracted passport details in a clear format.
            - Assume the user is not an expert in passport verification or document processing.
            - Format the content so that it can be printed to the Command Line in a structured way.
            - In case there is more than one matching passport record, prompt the user to select the most appropriate entry.";

        public OpenAIService(AzureOpenAIClient openAIClient)
        {
            _openAIClient = openAIClient;
        }

        public async Task<List<CorrectedProperty>> ValidatePassportData(PassportDataToVerify passportData, CancellationToken cancellationToken)
        {
            var result = new List<CorrectedProperty>();
            List<ChatMessage> messages = new()
            {
                new UserChatMessage(
                @$"Task:
                    1. Parse the Machine-Readable Zone (MRZ) of a passport into a structured format following ICAO standards.
                    2. Compare the parsed MRZ values with the provided passport data.
                    3. Use only the property names from the provided passport data for the results, ensuring no false positives.
                    4. Return a structured JSON response containing validation results for each property. Ensure that the resulting list retains the same properties as the input passportData.

                    ### Step 1: Parsing the MRZ
                    - Parse the MRZ into the following properties:
                      - `documentType`: Type of the document (e.g., ""P"").
                      - `issuingCountry`: Country code of the issuing authority (e.g., ""UTO"").
                      - `lastName`: Surname of the passport holder.
                      - `givenNames`: Given names of the passport holder (space-separated).
                      - `passportNumber`: Passport number.
                      - `nationality`: Nationality code.
                      - `dateOfBirth`: Date of birth in ISO format (YYYY-MM-DD).
                      - `sex`: Sex of the passport holder (""M"", ""F"", or ""<"" for unspecified).
                      - `dateOfExpiration`: Passport expiry date in ISO format (YYYY-MM-DD).
                      - `optionalData`: Optional data field.

                    ### Step 2: Validating Passport Data
                    - Compare each parsed MRZ property with the corresponding field in the provided passport data, ensuring strict validation.
                    - Ensure the following rules are met:
                      - Use only the property names from the provided passport data for validation and results.
                      - Before comparing dates, any dates in the passport data (such as dateOfBirth, dateOfExpiration, etc.) should be converted to the YYYY-MM-DD format.
                      - If a date in the passport data is in a non-standard format (e.g., ""12 JUL/JUI/JUL 21"" or ""13/04/1990""), it must be normalized to the YYYY-MM-DD format.
                        - For example:
                          - ""12 JUL 21"" should be converted to ""2021-07-12"".
                          - ""13/04/1990"" should be converted based on the region or MRZ context:
                            - If the MRZ is in the ""YYYY-MM-DD"" format, use that to interpret the date.
                            - If the issuing country is from a region using the ""DD/MM/YYYY"" format (e.g., most European countries), convert the date as ""1990-04-13"".
                            - If the issuing country is from a region using the ""MM/DD/YYYY"" format (e.g., USA), convert the date as ""1990-04-13"" but **verify this against MRZ date patterns**.
                      - The conversion should respect regional date conventions where the MRZ data provides no clear indication.
                      - Compare names (surname and given_names) by ignoring case sensitivity and accounting for missing or extra spaces.
                      - If no value exists for a property in the MRZ, set `SuggestedValue` to the value from the passport data.
                      - Ensure that `SuggestedValue` is always a valid string and never `null`.
                      - If the value in the SuggestedValue is a date, it should always be in the YYYY-MM-DD format.
                      - Avoid false positives by strictly ensuring all validations check exact matches or explicitly defined formats.

                    Passport Data:
                    {JsonSerializer.Serialize(passportData)}
                ")
            };

            ChatCompletionOptions options = new()
            {
                Temperature = 0.2f, // Lower temperature for deterministic and strict responses
                TopP = 0.1f, // Consider only the most probable completions
                FrequencyPenalty = 0, // No penalty for reusing similar text
                PresencePenalty = 0, // No encouragement for introducing new text

                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "passport_validation",
                jsonSchema: BinaryData.FromString("""
                {
                    "type": "object",
                    "properties": {
                        "results": {
                            "type": "array",
                            "items": {
                                "type": "object",
                                "properties": {
                                    "PropertyName": { "type": "string" },
                                    "SuggestedValue": { "type": ["string", "null"] },
                                    "IsValid": { "type": ["boolean", "null"] }
                                },
                                "required": ["PropertyName", "SuggestedValue", "IsValid"],
                                "additionalProperties": false
                            }
                        }
                    },
                    "required": ["results"],
                    "additionalProperties": false
                }
                """),
                jsonSchemaIsStrict: true)
            };

            var client = _openAIClient.GetChatClient(DeploymentName);
            ChatCompletion completion = await client.CompleteChatAsync(messages, options, cancellationToken);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);

            JsonElement results = structuredJson.RootElement.GetProperty("results");

            foreach (JsonElement property in results.EnumerateArray())
            {
                var verifiedProperty = new CorrectedProperty
                {
                    PropertyName = property.GetProperty("PropertyName").GetString()!,
                    SuggestedValue = property.GetProperty("SuggestedValue").GetString()!
                };

                if (property.GetProperty("IsValid").ValueKind != JsonValueKind.Null)
                {
                    verifiedProperty.IsValid = property.GetProperty("IsValid").GetBoolean();
                }

                result.Add(verifiedProperty);
            }

            return result;
        }

        public async Task<float[]> GetEmbeddingsAsync(object data, CancellationToken cancellationToken)
        {
            var endpoint = "https://oai-ai-doc-intelligence-eus.openai.azure.com/";
            // TODO: Add some key
            var clientOptions = new OpenAIClientOptions
            {
                Endpoint = new Uri(endpoint),
            };

            var _openAIClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

            var embeddingClient = _openAIClient.GetEmbeddingClient("text-embedding-3-small");

            var serializedData = JsonSerializer.Serialize(data);

            var embedding = await embeddingClient.GenerateEmbeddingAsync(serializedData, cancellationToken: cancellationToken);

            return embedding.Value.ToFloats().Span.ToArray();
        }

        public async Task<string> GetChatCompletionAsync(
            string userPrompt, string documents, CancellationToken cancellationToken)
        {
            var client = _openAIClient.GetChatClient(DeploymentName);

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(_systemPromptDocumentAssistant + documents),
                new UserChatMessage(userPrompt)
            };

            var options = new ChatCompletionOptions
            {
                MaxOutputTokenCount = 4000,
                TopP = 0.95f,
                Temperature = 0.5f, //0.3f,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            var result = await client.CompleteChatAsync(messages, options, cancellationToken);

            return result.Value.Content.Last().Text;
        }
    }
}
