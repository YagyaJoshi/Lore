using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Utilities
{
    public interface ICacheService
    {
        T Get<T>(string cacheID, Func<T> getItemCallback) where T : class;
        void DeleteCache(string cacheId);
    }
}
