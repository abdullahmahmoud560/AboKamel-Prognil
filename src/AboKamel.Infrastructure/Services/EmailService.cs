using MailKit.Net.Smtp;
using MimeKit;
using Services.Core.Contracts;
using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Logging;

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
        var smtpPort = int.Parse(Environment.GetEnvironmentVariable("Email__SmtpPort") ?? "587");
        var smtpUser = Environment.GetEnvironmentVariable("Email__SmtpUsername");
        var smtpPass = Environment.GetEnvironmentVariable("Email__SmtpPassword");
        var fromEmail = Environment.GetEnvironmentVariable("Email__FromEmail");
        var fromName = Environment.GetEnvironmentVariable("Email__FromName");

        _logger.LogInformation("Starting email process for {ToEmail}", toEmail);

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
                client.Timeout = 10000;

                _logger.LogInformation("Connecting to {Host}:{Port}...", smtpHost, smtpPort);
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.Auto);

                _logger.LogInformation("Authenticating user {User}...", smtpUser);
                await client.AuthenticateAsync(smtpUser, smtpPass);

                _logger.LogInformation("Sending email...");
                await client.SendAsync(message);
                
                await client.DisconnectAsync(true);
                _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending email to {ToEmail}: {Message}", toEmail, ex.Message);
            throw;
        }
    }
}