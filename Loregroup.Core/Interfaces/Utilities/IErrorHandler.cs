using Loregroup.Core.Exceptions.BaseExceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Utilities
{
    public interface IErrorHandler {
        BaseException HandleError(Exception ex, String className, String methodName, NameValueCollection methodParams);
        void HandleInfo(String info);

    }
}
