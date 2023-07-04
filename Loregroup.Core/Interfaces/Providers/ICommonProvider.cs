using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using System.Web;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface ICommonProvider
    {

        List<RoleViewModel> GetAllRole(Status? status, int page = 0, int records = 0);

      
    }
}
