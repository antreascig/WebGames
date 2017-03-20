using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using YamlDotNet.Serialization.NamingConventions;

namespace WebGames.Libs
{
    public class SettingsConfig
    {
        public SecurityModel security { get; set; }
    }

    public class SecurityModel
    {
        public List<PersistentUser> persistentUsers { get; set; }
    }


    public class PersistentUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public List<string> roles { get; set; }
    }

    public class SettingsManager
    {
        static SettingsConfig config = null;

        public static SettingsConfig GetConfig()
        {
            if (config != null) return config;

            try
            {
                string path = System.IO.Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);

                using (var conf = System.IO.File.OpenRead(System.IO.Path.Combine(path, "settings.yml")))
                {
                    using (var rdr = new System.IO.StreamReader(conf))
                    {
                        var deserializer = new YamlDotNet.Serialization.Deserializer(namingConvention: new CamelCaseNamingConvention());

                        config = deserializer.Deserialize<SettingsConfig>(rdr);

                    }
                }
            }
            catch
            {
                // stub config
                config = new SettingsConfig()
                {
                    security = new SecurityModel()
                    {
                        persistentUsers = new List<PersistentUser>()
                    }
                };
            }

            return config;
        }
    }
}