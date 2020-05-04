using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTSimulation
{    
    class Program
    {
        private static ILogger Logger;
        public static IConfiguration Configuration;
        private static List<EventsLoader> loadTasks = new  List<EventsLoader>();
             
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
                    Logger.LogInformation($"Loading data for device {deviceConfig["device_id"]}.");
                     EventsLoader tmpLoader = new EventsLoader(deviceConfig, Logger);
                    loadTasks.Add(tmpLoader);
            }
            
   
            Console.Read();
            //shutdown loade thread
            foreach(EventsLoader loader in loadTasks)
                loader.isStarted = false;

            Task.Delay(1000); //  wait a sec before task shutdown
        };

        }
        
    }
}


