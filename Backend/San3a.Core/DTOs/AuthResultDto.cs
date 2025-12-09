using System;

namespace San3a.Core.DTOs
{
    public class AuthResultDto
    {
        #region Properties
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        #endregion
    }
}
