using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cit_eTrike.Models
{
    public class Helper
    {
        public bool IsPropertyExist(dynamic obj, string name)
        {
            if (obj is ExpandoObject)
            {
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            }

            return obj.GetType().GetProperty(name) != null;
        }
        public string GetDirectory()
        {
            var config = configuration();
            string directory = config.GetSection("StaticFiles").GetSection("StaticFilesFolder").Value;

            return directory;
        }
        public IConfigurationRoot configuration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

    }
    public static class Extension
    {
        public static string GetNumbers(this string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
    }
}
