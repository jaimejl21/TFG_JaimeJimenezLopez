using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class DallEServiceImpl : MonoBehaviour
{
    private const string OpenAiPath = "https://api.openai.com/v1";
    private Auth auth;
    private ApiKey apiKey;

    /**
     * Create image with a given prompt
     *
     * @param DallERequestDTO the request object
     * @return {@code Task<DallEResponse/>} The response object
     */
    public async Task<DallEResponse> GetImage(DallERequestDTO request)
    {
        var path = $"{OpenAiPath}/images/generations";
        var payload = CreatePayload(request);
        return await ExecuteRequest<DallEResponse>(path, UnityWebRequest.kHttpVerbPOST, payload);
    }

    /// <summary>
    ///     Create byte array payload from the given request object that contains the parameters.
    /// </summary>
    /// <param name="request">The request object that contains the parameters of the payload.</param>
    /// <typeparam name="T">type of the request object.</typeparam>
    /// <returns>Byte array payload.</returns>
    private byte[] CreatePayload<T>(T request)
    {
        var json = JsonConvert.SerializeObject(request, jsonSerializerSettings);
        return Encoding.UTF8.GetBytes(json);
    }

    /**
     * This JSON SERIALIZER ALLOW US TO CONVERT THE REQUEST OBJECT TO JSON IN SNAKE_CASE
     */
    private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CustomStrategy()
        },
        MissingMemberHandling = MissingMemberHandling.Error,
        Culture = CultureInfo.InvariantCulture
    };

    /**
     * Makes an HTTP request
     * 
     * @param path the path request
     * @param method method to execute
     * @param payload byte array payload
     * @return {@code Task<T>} Task that contains the response of the API
     */
    private async Task<T> ExecuteRequest<T>(string path, string method, byte[] payload = null) where T : IBaseResponse
    {
        T data;

        using (var request = UnityWebRequest.Put(path, payload))
        {
            request.method = method;

            if (PlayerPrefs.GetString("organization") == null && PlayerPrefs.GetString("private_api_key") == null)
            {
                auth = new Auth();
            }
            else if (PlayerPrefs.GetString("organization") == null && PlayerPrefs.GetString("private_api_key") != null)
            {
                apiKey = new ApiKey();
                apiKey.private_api_key = PlayerPrefs.GetString("private_api_key");
                request.SetHeaders(apiKey, ApiDataUtilities.ContentType.ApplicationJson);
            }
            else if (PlayerPrefs.GetString("organization") != null && PlayerPrefs.GetString("private_api_key") != null)
            {
                auth = new Auth();
                auth.organization = PlayerPrefs.GetString("organization");
                auth.private_api_key = PlayerPrefs.GetString("private_api_key");
                request.SetHeaders(auth, ApiDataUtilities.ContentType.ApplicationJson);
            }

            var asyncOperation = request.SendWebRequest();
            while (!asyncOperation.isDone) await Task.Yield();
            data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text, jsonSerializerSettings);
        }

        if (data?.Error != null)
        {
            ApiError error = data.Error;
            Debug.LogError($"Error Message: {error.Message} \nError Type: {{error.Type}}\n");
        }

        return data;
    }
}