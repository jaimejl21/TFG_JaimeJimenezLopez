using UnityEngine.Networking;

public static class ApiDataUtilities
{
    public static class ContentType
    {
        public const string MultipartFormData = "multipart/form-data";
        public const string ApplicationJson = "application/json";
    }

    public static class ImageFormat
    {
        public const string Url = "url";
        public const string Base64Json = "b64_json";
    }
    /// <summary>
    ///     Set headers of the HTTP request with user credentials.
    /// </summary>
    /// <param name="request">this UnityWebRequest</param>
    /// <param name="authValues">Configuration file that contains user credentials.</param>
    /// <param name="type">The value of the Accept header for an HTTP request.</param>
    ///
    /**
     * Set the HTTP Headers
     * 
     */
    public static void SetHeaders(this UnityWebRequest request, Auth authValues, string type = null)
    {
        if (authValues.organization != null)
        {
            request.SetRequestHeader("OpenAI-Organization", authValues.organization);
        }

        if (type != null)
        {
            request.SetRequestHeader("Content-Type", type);
        }

        request.SetRequestHeader("Authorization", "Bearer " + authValues.private_api_key);
    }

    public static void SetHeaders(this UnityWebRequest request, ApiKey apiKey, string type = null)
    {
        if (type != null)
        {
            request.SetRequestHeader("Content-Type", type);
        }

        request.SetRequestHeader("Authorization", "Bearer " + apiKey.private_api_key);
    }
}


