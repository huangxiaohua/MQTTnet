using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore;

namespace MQTTnet.TestApp.AspNetCore3_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        //private static IWebHost BuildWebHost(string[] args) =>
        //    Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
        //        //.UseKestrel(o => {
        //        //    o.ListenAnyIP(1883, l => l.UseMqtt());
        //        //    o.ListenAnyIP(5000); // default http pipeline
        //        //})
        //        .UseStartup<Startup>()
        //        .Build();
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseKestrel(o =>
                    {
                        o.ListenAnyIP(1883, l => l.UseMqtt());
                        o.ListenAnyIP(5000); // default http pipeline
                    });
                });
    }
}
