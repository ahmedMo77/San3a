using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Interfaces
{
    public interface IEmailService 
    {
        Task SendVerificationCodeAsync(string toEmail, string verificationToken);
        Task SendPasswordResetCodeAsync(string toEmail, string resetCode);
    }
}
