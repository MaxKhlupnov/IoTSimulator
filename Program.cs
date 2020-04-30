using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace IoTSimulation
{
    
    class Program
    {

        private static ILogger logger;
        public static IConfigurationRoot Configuration;
        static void Main(string[] args)
        {


        using (var loggerFactory = LoggerFactory.Create(builder => {
                    builder.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("IoTSimulation.Program", LogLevel.Debug)
                        .AddConsole();
                }
            )){

        logger = loggerFactory.CreateLogger<Program>();

      
       logger.LogInformation("Logging information.");
            logger.LogCritical("Logging critical information.");
            logger.LogDebug("Logging debug information.");
            logger.LogError("Logging error information.");
            logger.LogTrace("Logging trace");
            logger.LogWarning("Logging warning.");
   

        };

        }
        
    }
}


