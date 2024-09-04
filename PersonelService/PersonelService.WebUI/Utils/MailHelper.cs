using Microsoft.EntityFrameworkCore;
using PersonelService.Data;
using PersonelService.Entities;
using PersonelService.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PersonelService.WebUI.Utils
{
    public class MailHelper
    {
        private readonly DatabaseContext _context;

        public MailHelper(DatabaseContext context, IService<Musteri> serviceMusteri)
        {
            _context = context;

        }

        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "mailtestim33@gmail.com";
        private readonly string _smtpPass = "erxjwrwhhueemsbf";

        public async Task<List<(string Name, string Email,string Soyadi)>> GetCustomerEmailsAsync()
        {
            var customerEmails = await _context.Musteriler
                .Where(c => !string.IsNullOrEmpty(c.Email))
                .Select(c => new { c.Adi, c.Email,c.Soyadi})
                .ToListAsync();

            return customerEmails.Select(c => (c.Adi, c.Email,c.Soyadi)).ToList();
        }

        public async Task SendEmailToCustomersAsync()
        {
            try
            {
                var customerEmails = await GetCustomerEmailsAsync();

                using (SmtpClient smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                    smtpClient.EnableSsl = true;

                    foreach (var (name, email,soyadi) in customerEmails)
                    {
                        MailMessage message = new MailMessage
                        {
                            From = new MailAddress("mailtestim33@gmail.com"),
                            Subject = "Sanrı Otomotiv Hizmetleri",
                            Body = $"Sayın {name} {soyadi} Bey, Oto Servisimize hoş geldiniz.",
                            IsBodyHtml = true
                        };

                        message.To.Add(email);

                        await smtpClient.SendMailAsync(message);
                        Console.WriteLine($"E-posta başarıyla gönderildi: {email}");
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Hatası: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Genel Hata: {ex.Message}");
            }
        }
    }
}
