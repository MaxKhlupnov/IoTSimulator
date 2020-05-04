
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace IoTSimulation {

 public class EventsLoader{

        private ILogger Logger;
        private SampleEventModel [] Events;
        public string device_id {get; private set;}
        private string topic;
        private YaIoTClient devClient;
        /** Time interval between device messges in seconds  */
        private int msg_interval; 
        public bool isStopped {get; set;}
        
        public EventsLoader(IConfigurationSection config,ILogger Logger){
            
            this.Logger = Logger; 
            this.device_id = config["device_id"];

           if (!int.TryParse(config["msg_interval"], out this.msg_interval))
            msg_interval = 10; // if interval is not specified - default is 10 sec.

            this.topic = YaIoTClient.TopicName(this.device_id, EntityType.Device, TopicType.Events);
            this.devClient = new YaIoTClient();
           
            if (!string.IsNullOrEmpty(config["cert"]))
            {
                this.devClient.Start(config["cert"]);
            }else{
                this.devClient.Start(config["device_id"], config["pwd"]);
            }
            

            var sampleFilePath = Path.Combine(Directory.GetCurrentDirectory(), config["events_file"]);

            Logger.LogInformation($"Loading events from file {sampleFilePath}");
            Events = File.ReadAllLines(sampleFilePath)
                .Skip(1)
                .Select(SplitAndFill)
                .ToArray();
            if (Events.Length == 0){
                string errMsg = $"No events defined in the file {sampleFilePath}";
                Logger.LogError(errMsg);
                throw new ArgumentException(errMsg);
            }
            
            Thread workerThread = new Thread(this.Start);
            workerThread.Start();
        }
        public async void Start(){
            try{

                    Logger.LogInformation($"Connecting device {device_id}");
                if (!this.devClient.WaitConnected())
                {
                     Logger.LogError($"Device {device_id} connection error");
                    return;
                }else{
                     Logger.LogInformation($"Device {device_id} connected successfully");
                }
                    while(!this.isStopped){
                        for(int i = 0; i < Events.Length; i++){
                        SampleEventModel nextEvent = Events[i];
                            nextEvent.device_id = this.device_id;  // initialize device_id from configuration file and datetime 
                            nextEvent.datetime = DateTime.Now.ToString();
                        string eventData = JsonSerializer.Serialize(nextEvent);
                        await this.devClient.Publish(topic, eventData, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
                        Logger.LogInformation($"Event {eventData} sucessfully sent");
                        if (this.isStopped){
                            Logger.LogInformation($"Shutting down event generation for device {device_id}");
                            break;
                        }else
                            await Task.Delay(msg_interval * 1000); // in msec
                        }
                    }
            }finally{               
                this.devClient.Stop();
            }
            Logger.LogInformation($"Event generation for device {device_id} stopped");
        }
        
            
        

         private static SampleEventModel SplitAndFill(string line)
        {
            var sampleAlert = new SampleEventModel();
            
            var props = typeof(SampleEventModel).GetProperties();

            var values = line.Split(";");
            for (var i = 0; i < values.Length - 1; i++)
            {
                props[i].SetValue(sampleAlert, values[i]);
            }

            return sampleAlert;
        }


 }


}

