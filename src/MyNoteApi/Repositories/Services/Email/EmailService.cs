﻿using MyNoteApi.Models.ViewModels.Email;
using MyNoteApi.Repositories.Interfaces.Email;
using System.Net;
using System.Net.Mail;

namespace MyNoteApi.Repositories.Services.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Send(SendEmailViewModel model)
    {
        SmtpClient smtpClient = new SmtpClient(_configuration["EmailProvider:Host"], int.Parse(_configuration["EmailProvider:Port"]))
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_configuration["EmailProvider:Username"], _configuration["EmailProvider:Password"]),
            EnableSsl = true
        };
        var message = new MailMessage
        {
            From = new MailAddress(_configuration["EmailProvider:Username"]),
            Subject = model.title,
            Body = model.message,
            IsBodyHtml = true
        };
        message.To.Add(model.to);
        smtpClient.Send(message);
        smtpClient.Dispose();
        return ;
    }
}
