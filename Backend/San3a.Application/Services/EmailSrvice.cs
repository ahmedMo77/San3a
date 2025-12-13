using Microsoft.Extensions.Configuration;
using San3a.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendVerificationCodeAsync(string toEmail, string verificationCode)
        {
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@san3a.com";

            using (var smtpClient = new SmtpClient(host, port))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(username, password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, "San3a Platform"),
                    Subject = "Email Verification Code - San3a",
                    Body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f9f9f9;'>
                    <div style='background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333; margin-top: 0;'>Email Verification</h2>
                        <p style='color: #666; font-size: 16px;'>Thank you for registering! Please use the verification code below to verify your email address:</p>
                        
                        <div style='background-color: #f0f7ff; border: 2px dashed #007bff; border-radius: 8px; padding: 20px; margin: 30px 0; text-align: center;'>
                            <p style='color: #666; margin: 0 0 10px 0; font-size: 14px;'>Your verification code is:</p>
                            <h1 style='color: #007bff; font-size: 42px; letter-spacing: 8px; margin: 10px 0; font-family: monospace;'>{verificationCode}</h1>
                        </div>
                        
                        <div style='background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0;'>
                            <p style='margin: 0; color: #856404;'><strong>⏱️ Important:</strong> This code will expire in <strong>5 minutes</strong>.</p>
                        </div>
                        
                        <p style='color: #666; font-size: 14px; margin-top: 20px;'>If you didn't create an account, please ignore this email.</p>
                        
                        <hr style='border: none; border-top: 1px solid #ddd; margin: 30px 0;'>
                        
                        <p style='color: #999; font-size: 12px; text-align: center; margin: 0;'>This is an automated message, please do not reply.</p>
                        <p style='color: #999; font-size: 12px; text-align: center; margin: 5px 0 0 0;'>© 2024 San3a Platform. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public async Task SendPasswordResetCodeAsync(string toEmail, string resetCode)
        {
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@San3a.com";

            using (var smtpClient = new SmtpClient(host, port))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(username, password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, "San3a Platform"),
                    Subject = "Password Reset Code - San3a",
                    Body = $@"
            <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f9f9f9;'>
                    <div style='background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333; margin-top: 0;'>Password Reset</h2>
                        <p style='color: #666; font-size: 16px;'>You requested to reset your password. Please use the code below:</p>
                        
                        <div style='background-color: #fff5f5; border: 2px dashed #dc3545; border-radius: 8px; padding: 20px; margin: 30px 0; text-align: center;'>
                            <p style='color: #666; margin: 0 0 10px 0; font-size: 14px;'>Your password reset code is:</p>
                            <h1 style='color: #dc3545; font-size: 42px; letter-spacing: 8px; margin: 10px 0; font-family: monospace;'>{resetCode}</h1>
                        </div>
                        
                        <div style='background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0;'>
                            <p style='margin: 0; color: #856404;'><strong>⏱️ Important:</strong> This code will expire in <strong>5 minutes</strong>.</p>
                        </div>
                        
                        <p style='color: #666; font-size: 14px; margin-top: 20px;'>If you didn't request this, please ignore this email or contact support if you have concerns.</p>
                        
                        <hr style='border: none; border-top: 1px solid #ddd; margin: 30px 0;'>
                        
                        <p style='color: #999; font-size: 12px; text-align: center; margin: 0;'>This is an automated message, please do not reply.</p>
                        <p style='color: #999; font-size: 12px; text-align: center; margin: 5px 0 0 0;'>© 2024 San3a Platform. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);
            }
        }

    }
}