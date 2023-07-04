using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Helpers;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System.Web;
using System.IO;
using Loregroup.Core.Exceptions;

namespace Loregroup.Provider
{
    public class CommonProvider : ICommonProvider
    {
        private readonly AppContext _context;

        public CommonProvider(AppContext context)
        {
            _context = context;
        }
        public RoleViewModel ToRoleViewModel(Role role, int depth)
        {
            if (depth == 1)
            {
                return new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    CreatedById = role.CreatedById,
                    CreationDate = role.CreationDate,
                    ModifiedById = role.ModifiedById,
                    ModificationDate = role.ModificationDate,
                    StatusId = role.StatusId
                };
            }
            return new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                CreatedById = role.CreatedById,
                CreationDate = role.CreationDate,
                ModifiedById = role.ModifiedById,
                ModificationDate = role.ModificationDate,
                StatusId = role.StatusId
            };
        }

        public List<RoleViewModel> GetAllRole(Status? status, int page = 0, int records = 0)
        {

            try
            {
                return _context.Roles
                    .ToList()
                    .Select(ToRoleViewModel)
                    .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
    }
}
