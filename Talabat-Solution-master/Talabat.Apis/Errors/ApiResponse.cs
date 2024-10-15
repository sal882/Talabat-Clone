namespace Talabat.Apis.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetFaultMessageForStatusCode(statusCode);
        }

        private string? GetFaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "a Bad Request you have made",
                401 => "Authoriezed, you are not",
                404 => "Resource was not found",
                500 => "Errors are the path to the dark side",
                _ => null,
            };
        }
    }
}
