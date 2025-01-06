using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class PendingDemandReminderWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public PendingDemandReminderWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);

            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            var delay = nextRun - now; 
            await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
            {
                await SendReminderEmails();
            }
            await Task.Delay(TimeSpan.FromDays(2), stoppingToken);
        }
    }

    private async Task SendReminderEmails()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var client = scope.ServiceProvider.GetRequiredService<HttpClient>();

            var request = new HttpRequestMessage(HttpMethod.Get, "http://172.30.44.13:5734/api/DemandReminder/ReminderDemand");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
}