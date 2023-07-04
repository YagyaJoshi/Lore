using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface IUserProvider
    {

        bool ChangePassword(ForgetPasswordViewModel forgetPasswordViewModel);
        UserDesignationViewModels GetAllUserDesignations(Status? status, int page = 0, int records = 0);
        List<RoleViewModel> GetAllRoles(Status? status, int page = 0, int records = 0);
        RoleViewModel GetRole(Int64 id);
        RoleViewModel SaveRole(RoleViewModel model);
        List<RoleViewModel> GetAllRoles();
        void DeleteRole(int RoleId, int Userid);
        SessionUser ToSessionUser(MasterUser user); //AppUser user);
        SessionUser GetDomainUser(Int64 id);
        Int64 CheckLoginCredentials(string username, string password);
        MasterUserViewModel GetMaster_User(Int64 id);
        PermissionMatrixViewModel GetPermissionList();
        List<NotificationsViewModel> GetUsernotification(long userid, Status? status, int page = 0, int records = 0);
        NavigationViewModel GetNavigationList(Int64 roleid);
        List<WidgetViewModel> GetWidgetList(Int64 roleid);
        void SavePermissionMatrices(NavigationViewModel model);

        String SaveUser(CustomerViewModel model);
        List<CustomerViewModel> GetAllUsers(int start, int length, string search, int filtercount , Int64 CurrentUserId);
        List<CustomerViewModel> AllUserList();
        CustomerViewModel GetUser(Int64 id);
        void DeleteUser(Int64 id);
        List<CustomerViewModel> GetUserByCountry(Int64 CountryId);
        List<UserLocation> GetAllUsersForMap();

        List<CustomerViewModel> GetAllUsersForStoreLocator();
        List<CustomerViewModel> GetAllUsersForStoreLocator(Int64 countryid, Int64 stateid, string zipcd);

        int IsEmailExists(string emailid);
    }
}
