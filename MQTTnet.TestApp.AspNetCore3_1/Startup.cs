using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCoreEx;
using MQTTnet.Server;

namespace MQTTnet.TestApp.AspNetCore3_1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mqttServerOptions = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(1883)
                .WithClientId("sdfsd")
                .Build();

            services
                .AddHostedMqttServer(mqttServerOptions)
                .AddMqttConnectionHandler()
                .AddMqttWebSocketServerAdapter()
                .AddMqttTcpServerAdapter()
                .AddConnections();
                

            //services.AddHostedMqttServerEx(mqttServerOptions);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});


            //app.UseConnections(c => c.MapConnectionHandler<MqttConnectionHandler>("/mqtt", options =>
            //{
            //    options.WebSockets.SubProtocolSelector = MQTTnet.AspNetCore.ApplicationBuilderExtensions.SelectSubProtocol;
            //}));

            app.UseMqttEndpoint("/mqtt");
            app.UseMqttServer(server =>
            {
                
                server.StartedHandler = new MqttServerStartedHandlerDelegate((args) =>
                {

                    return Task.Run(() =>
                    {
                        Console.WriteLine($"----->start: {args.ToString()}");
                        //LogProvide.WriteFile("mqtt_connect", $"----->connected: {args.ClientId}");
                    });
                });

                

                server.UseClientConnectedHandler(async args => {
                    var msg = new MqttApplicationMessageBuilder()
                        .WithPayload("Mqtt is awesome")
                        .WithTopic("message");
                    await server.PublishAsync(msg.Build());
                });
            });
            //app.UseMqttServerEx(server =>
            //{

            //    server.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(args => Server_ClientConnected(server, args));
            //    //ClientConnectionValidatorHandler 
            //    server.ClientConnectionValidatorHandler = new MqttServerClientConnectionValidatorHandlerDelegate(args => Server_ClientConnectionValidator(server, args));
            //});

            app.Use((context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Request.Path = "/Index.html";
                }

                return next();
            });

            app.UseStaticFiles();


            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules"))
            });
        }

        private void Server_ClientConnectionValidator(IMqttServerEx server, MqttServerClientConnectionValidatorEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void Server_ClientConnected(IMqttServerEx server, MqttServerClientConnectedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
