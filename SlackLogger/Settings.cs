using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SlackLogger
{
    internal class Settings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public bool IncludeScopes
        {
            get
            {
                bool includeScopes;
                var value = _configuration["IncludeScopes"];
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }
                else if (bool.TryParse(value, out includeScopes))
                {
                    return includeScopes;
                }
                else
                {
                    var message = $"Configuration value '{value}' for setting '{nameof(IncludeScopes)}' is not supported.";
                    throw new InvalidOperationException(message);
                }
            }
        }

        public bool TryGetSwitch(string name, out LogLevel level)
        {
            var switches = _configuration.GetSection("LogLevel");
            if (switches == null)
            {
                level = LogLevel.None;
                return false;
            }

            var value = switches[name];
            if (string.IsNullOrEmpty(value))
            {
                level = LogLevel.None;
                return false;
            }
            else if (Enum.TryParse<LogLevel>(value, out level))
            {
                return true;
            }
            else
            {
                var message = $"Configuration value '{value}' for category '{name}' is not supported.";
                throw new InvalidOperationException(message);
            }
        }
    }
}