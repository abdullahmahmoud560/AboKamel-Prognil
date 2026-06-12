using MailKit.Net.Smtp;
using MimeKit;
using Services.Core.Contracts;
using System;
using System.Threading.Tasks;
using MailKit.Security;
using System.Net.Security;
using Microsoft.Extensions.Logging; // إضافة مكتبة الـ Logger

namespace Services.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpHost = Environment.GetEnvironmentVariable("Email__SmtpHost");
        var smtpPort = int.Parse(Environment.GetEnvironmentVariable("Email__SmtpPort") ?? "2525");
        var smtpUser = Environment.GetEnvironmentVariable("Email__SmtpUsername");
        var smtpPass = Environment.GetEnvironmentVariable("Email__SmtpPassword");
        var fromEmail = Environment.GetEnvironmentVariable("Email__FromEmail");
        var fromName = Environment.GetEnvironmentVariable("Email__FromName");

        _logger.LogInformation("Attempting to send email to {ToEmail} via {Host}:{Port}", toEmail, smtpHost, smtpPort);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        try
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                _logger.LogInformation("Connecting to SMTP server...");
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                
                _logger.LogInformation("Authenticating user: {User}", smtpUser);
                await client.AuthenticateAsync(smtpUser, smtpPass);
                
                _logger.LogInformation("Sending message...");
                await client.SendAsync(message);
                
                await client.DisconnectAsync(true);
                _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending email to {ToEmail}: {Message}", toEmail, ex.Message);
            throw;
        }
    }
}