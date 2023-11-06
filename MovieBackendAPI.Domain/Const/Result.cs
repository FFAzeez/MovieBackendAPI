using System.Text.Json.Serialization;

namespace MovieBackendAPI.Domain.Const
{
    public class Result
    {
        [JsonPropertyName("statusCode")]
        public string StatusCode { get; set; }
        [JsonPropertyName("statusMessage")]
        public string StatusMessage { get; set; }
        [JsonPropertyName("successful")]
        public bool Successful => StatusCode == ResponseCode.SUCCESSFUL || StatusCode == "0";
    }

}
