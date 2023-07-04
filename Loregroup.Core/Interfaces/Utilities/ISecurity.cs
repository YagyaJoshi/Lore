using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loregroup.Core.Interfaces.Utilities
{
    public interface ISecurity
    {
        String ComputeHash(String password, String salt);
        String GenerateNewPassword();
    }
}
