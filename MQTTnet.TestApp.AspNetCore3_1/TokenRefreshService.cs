using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTnet.TestApp.AspNetCore3_1
{
    internal class TokenRefreshService : IHostedService, IDisposable
    {
        private Timer _timer;

        public TokenRefreshService()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Service starting");
            Console.WriteLine("Service starting");
            _timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void Refresh(object state)
        {
            //_logger.LogInformation(DateTime.Now.ToLongTimeString() + ": Refresh Token!"); //在此写需要执行的任务
            Console.WriteLine(DateTime.Now.ToLongTimeString() + ": Refresh Token!");

            var msg = new MqttApplicationMessageBuilder()
                        .WithPayload("Mqtt is awesome")
                        .WithTopic("message");

            try { 
            var _mqttServer = (MQTTnet.Server.MqttServer) ServiceProvides.Instance.GetService(typeof(MQTTnet.Server.IMqttServer));
            Task.Run(()=>_mqttServer.PublishAsync(msg.Build()));
            }catch(Exception ex)
            {
                Console.WriteLine("error" + ex.Message);
                    
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Service stopping");
            Console.WriteLine("Service stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
