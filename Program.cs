using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace IoTSimulation
{    
    class Program
    {
        private static ILogger Logger;
        public static IConfiguration Configuration;
        
        public static bool isStopped = false;
        static void Main(string[] args)
        {
        using (var loggerFactory = LoggerFactory.Create(builder => {
                    builder.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("IoTSimulation", LogLevel.Debug)
                        .AddConsole();
                }
            )){

        Logger = loggerFactory.CreateLogger<Program>();

        Configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", true, true)
          .Build();

        IConfigurationSection devicesConfigSection =  Configuration.GetSection("devices");
    
       foreach(IConfigurationSection deviceConfig in devicesConfigSection.GetChildren()){
            Logger.LogInformation($"Loading data for {deviceConfig["device_id"]}.");
          
            EventsLoader evnts_loader = new EventsLoader(deviceConfig, Logger);
       }
   

        };

        }
        
    }
}


