using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServiceDemo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = await client.GetAsync("http://45.32.110.207:300/login");
                    if (result.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"App is running.status code {result.StatusCode}");
                    }
                    else
                    {
                        _logger.LogInformation($"App is not running.status code {result.StatusCode}");
                    }
                    await Task.Delay(1000, stoppingToken);
                }
                catch(Exception e) { }
               
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
