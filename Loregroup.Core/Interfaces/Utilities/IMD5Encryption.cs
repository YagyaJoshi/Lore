using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loregroup.Core.Interfaces.Utilities
{
    public interface IMD5Encryption
    {
        string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes);
        bool VerifyHash(string plainText, string hashAlgorithm, string hashValue);
    }
}
