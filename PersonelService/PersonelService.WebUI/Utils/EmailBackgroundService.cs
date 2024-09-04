using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PersonelService.WebUI.Utils 
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope()) 
                {
                    var mailHelper = scope.ServiceProvider.GetRequiredService<MailHelper>();
                    await mailHelper.SendEmailToCustomersAsync(); 
                }

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken); 
            }
        }
    }
}
