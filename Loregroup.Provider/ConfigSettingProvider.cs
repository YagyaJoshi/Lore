using Loregroup.Core.Interfaces.Providers;
using Loregroup.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Data.Entities;

namespace Loregroup.Provider
{
    public class ConfigSettingProvider:IConfigSettingProvider {
        private readonly AppContext _context;

        public ConfigSettingProvider(AppContext context)
        {
            _context = context;
        }

        public T GetSetting<T>(Core.Enumerations.ConfigSetting configurationSetting) where T : class
        {
            Configuration configuration = _context.Configurations.FirstOrDefault(x => x.Id == (Int64)configurationSetting);
            if (configuration != null)
                return (T)Convert.ChangeType(configuration.Value, typeof(T), CultureInfo.InvariantCulture);
            else
                throw new Exception();
        }
    }
}
