using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace asset_pipeline
{
    public class PipelineConfiguration
    {
        public static Dictionary<string, string> config;

        public static string Get(string key)
        {
            return config[key];
        }

        public static void LoadConfiguration(string path)
        {
            Console.WriteLine("Loading configuration");
            Console.WriteLine(path);
            string text = File.ReadAllText(path);
            config = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            Console.WriteLine("Configuration loaded");
        }
    }
}
