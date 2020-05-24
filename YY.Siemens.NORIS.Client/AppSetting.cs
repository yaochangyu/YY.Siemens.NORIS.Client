using System.IO;
using Newtonsoft.Json;

namespace YY.Siemens.NORIS.Client
{
    public class AppSetting
    {
        public static string BaseUrl { get; set; }

        public static string Id { get; set; }

        public static string Password { get; set; }

        public static void Export(string filePath)
        {
            var config = new {BaseUrl, Id, Password};
            File.WriteAllText(filePath, JsonConvert.SerializeObject(config));
        }

        public static void Import(string filePath)
        {
            dynamic config = JsonConvert.DeserializeObject(File.ReadAllText(filePath));

            BaseUrl  = config.BaseUrl;
            Id       = config.Id;
            Password = config.Password;
        }
    }
}