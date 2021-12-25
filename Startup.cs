using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using smarthome.Model;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using smarthome.Schedules;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using smarthome.Helper;
using smarthome.Config;
using Microsoft.AspNetCore.HttpOverrides;
using smarthome.Hubs;
using smarthome.Services;
using Microsoft.AspNetCore.SignalR;
using System.IO.Ports;

namespace smarthome
{
    public class Startup
    {
        public Startup(IConfiguration configuration , IWebHostEnvironment _env)
        {
            Configuration = configuration;
            environment = _env;
        }

        public IConfiguration Configuration { get; }
        public readonly IWebHostEnvironment environment;

        public readonly string AllowOrigin = "AllowOrigin";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(
                AllowOrigin , builder => {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                        
                }
            ));

            string conStr = Configuration.GetConnectionString("Connection");
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))    
            {
                conStr = Configuration.GetConnectionString("ConnectionRasp");
            }

            Console.WriteLine(conStr);

            services.AddDbContext<DbContextModel>(options =>{
                options.UseMySQL(conStr);
            });

            //services.AddSingleton<IInitBoard, InitBoard>();

            services.AddHostedService<QuartzHostedService>();

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<JobHandler>();

            services.AddSingleton<SerialPort>();

            services.AddSingleton<SocketService>();
            services.AddSingleton<BluetoothService>();

            services.AddScoped<SmartPlugData_Schedule>();        
            services.AddScoped<DeviceService>();
            services.AddScoped<ModuleService>();

            services.AddScoped<InitBoard>();

            services.AddTransient<SmartPlugService>();
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            
            services.AddSignalR();

            services.AddControllers();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env 
                                , DbContextModel contextModel 
                                , IHubContext<EntryHub> _entryHub
                                , DeviceService _deviceService
                                , SocketService _socketService
                                , BluetoothService _bluetoothService
                                , InitBoard initBoard)
        {
            initBoard.Init().Wait();

            BluetoothService bluetoothService = _bluetoothService;
            bluetoothService.PairingMode();
            bluetoothService.MonitorSignal();

            app.UseCors(AllowOrigin);

            // ShellHelper.Run("" , "rtsp-simple-server" , false);

            config.makeWPASupplicant();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<EntryHub>("/socket");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });  

            app.UseWebSockets();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
