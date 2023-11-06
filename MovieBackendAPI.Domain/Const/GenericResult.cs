using System.Text.Json.Serialization;

namespace MovieBackendAPI.Domain.Const
{
    public class Result<T>
    {
        #region properies
        [JsonPropertyName("statusCode")]
        public string StatusCode { get; set; }
        [JsonPropertyName("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonPropertyName("successful")]
        public bool Successful => StatusCode == ResponseCode.SUCCESSFUL;

        [JsonPropertyName("responseObject")]
        public T ResponseObject { get; private set; } = default;
        #endregion

        public static Result<T> Success(T instance, string message = "Successful")
        {
            return new Result<T>
            {
                StatusCode = ResponseCode.SUCCESSFUL,
                StatusMessage = message,
                ResponseObject = instance
            };
        }

        public static Result<T> Pending(string message, string code) => new Result<T>
        {
            StatusCode = code,
            StatusMessage = message
        };

        public static Result<T> Failure(string error = "Failed")
        {
            return new Result<T>
            {
                StatusCode = ResponseCode.GENERIC_EXCEPTION,
                StatusMessage = error
            };
        }
    }

}
