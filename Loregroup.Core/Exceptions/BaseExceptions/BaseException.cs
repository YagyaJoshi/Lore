using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Exceptions.BaseExceptions
{
    [Serializable]
    public class BaseException : Exception {
        public BaseException() { }

        public BaseException(string message)
            : base(message) { }

        public BaseException(string message, Exception innerException)
            : base(message, innerException) { }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
