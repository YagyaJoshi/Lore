using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface IConfigSettingProvider {
        T GetSetting<T>(ConfigSetting configurationSetting) where T : class;
    }
}
