using System.Collections.Generic;

namespace San3a.Core.DTOs.Base
{
    public class ApiResponse<T>
    {
        #region Properties
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        #endregion

        #region Constructors
        public ApiResponse()
        {
            Errors = new List<string>();
        }
        #endregion

        #region Public Methods
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> FailureResponse(string message, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
        #endregion
    }
}
