using System.IO;
using Newtonsoft.Json;

namespace YY.Siemens.NORIS.Client
{
    public class AppSetting
    {
        public string BaseUrl { get; set; }

        public string Id { get; set; }

        public string Password { get; set; }

        public static void Export(AppSetting appSetting, string filePath)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(appSetting));
        }

        public static AppSetting Import(string filePath)
        {
            return JsonConvert.DeserializeObject<AppSetting>(File.ReadAllText(filePath));
        }
    }
}