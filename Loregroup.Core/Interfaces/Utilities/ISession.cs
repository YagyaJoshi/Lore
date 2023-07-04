using Loregroup.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Utilities
{
    public interface ISession {
        SessionUser CurrentUser { get; }
    }
}
