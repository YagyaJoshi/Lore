using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Loregroup.Core.Exceptions.BaseExceptions;
using Loregroup.Core.Interfaces.Utilities;
using NLog;

namespace Loregroup.Core.Utilities
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IUtilities _utilities;
        public ErrorHandler(IUtilities utilities)
        {
            _utilities = utilities;
        }

        public BaseException HandleError(Exception ex, String className, String methodName, NameValueCollection methodParams)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Error("Class Name : " + className + "\r\nMethod Name : " + methodName + "\r\nMethod Params : " + (methodParams != null && methodParams.HasKeys() ? _utilities.Serialize(methodParams) : "") + "\r\nException Message : " + ex.Message + "\r\nException Message : " + ex.StackTrace);

            return new BaseException(ex.Message, ex);
        }

        public void HandleInfo(String info)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info(info);

        }

    }
}
