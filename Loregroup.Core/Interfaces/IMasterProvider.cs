using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using Core.ViewModels;

using Loregroup.Data.Entities;
using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using Loregroup.Core.ViewModels;

namespace Loregroup.Core.Interfaces
{
    public interface IMasterProvider
    {       
        //old
        List<BlocksViewModel> GetAllBlock(bool? isVerified, bool? isLocked, Status? status, int page = 0, int records = 0);
        long SaveBlock(BlocksViewModel model);
        void UpdateBlock_status(long uid);
        BlocksViewModel GetBlock(long id);
        long UpdateBlock(BlocksViewModel model);
        void DeleteBlock(Int64 UserId, int modifiedByid);
        void UpdateDistrict_status(long uid);
        string Getprofimage(long userid);      
        List<NotificationsViewModel> GetAdminnotification(Status? status, int page = 0, int records = 0);
        NotificationsViewModel Getnotification(long NotificationID);
        void DeleteNotification(Int64 UserId, int modifiedByid);
        long SendStoreNotification(NotificationsViewModel model);      
        void SaveStoreReview(string review, Int64 id, Int64 storeid);
        void SaveMallReview(string review, Int64 id, Int64 mallid);
        void PostCongratulation(string review, Int64 id, Int64 newopeningid);    
        void DeleteMasterUser(int id);
       
        //log
        Int64 SaveDSLog(CheckStatusViewModel model);

        //Country
        List<CountryViewModel> getAllCountries();
        List<CountryViewModel> getAllCountries(int start, int length, string search, int filtercount);
        long saveCountry(CountryViewModel model);
        void deleteCountry(Int64 id);
        CountryViewModel getCountry(Int64 id);
        Int64 updateCountry(CountryViewModel model);

        //State 
        Int64 SaveState(StateViewModel model);
        List<StateViewModel> GetAllStates(bool? isVerified, bool? isLocked, Status? status, int page = 0, int records = 0);
        StateViewModel GetState(Int64 id);
        Int64 UpdateState(StateViewModel model);
        void DeleteState(Int64 UserId, int modifiedByid);
        List<StateViewModel> GetAllStates();
        List<StateViewModel> GetStates(Int64 Id);
        List<StateViewModel> GetAllStates(int start, int length, string search, int filtercount);
        void DeleteState(Int64 id);
        void Updatestate_status(long uid);
        
        //City
        List<CityViewModel> GetCities(Int64 SelectedDistrictId);
        Int64 SaveCity(CityViewModel model);
        Int64 UpdateCity(CityViewModel model);
        void DeleteCity(Int64 UserId, int modifiedByid);
        CityViewModel ToCityViewModel(City city);
        List<CityViewModel> GetAllCity(Status? status, int records, int page = 0);
        CityViewModel GetCity(Int64 id);
        List<CityViewModel> GetAllCityAccId(Int64 Id, Status? status, int records, int page = 0);
        void UpdateCity_status(long uid);
        string GetCityName(Int64? CityId);
        List<CityViewModel> GetAllCities(int start, int length, string search, int filtercount);

        void DeleteCity(Int64 UserId);

        //User
        MasterUserViewModel GetUser(Int64 UserId);
        List<MasterUserViewModel> GetAllUser(Status? status, int page = 0, int records = 0);
        MasterUserViewModel ToUserViewModel(MasterUser User);//AppUser User);
        List<UserViewModel> GetUserList();//17/june/2017
        List<RoleViewModel> GetAllRole(Status? status, int page = 0, int records = 0);
        RoleViewModel ToRoleViewModel(Role Role);
        long UserRegistration(MasterUserViewModel model);
        List<MasterUserViewModel> GetAllAppUsers();
        MasterUserViewModel GetMasterUserForEdit(Int64 Id);
        bool EditUpdateUser(MasterUserViewModel model);
        Int64 GetPackageId(Int64 UserId);
        Int64 GetPackageIdForAPI(Int64 UserId);       

        //district
        List<DistrictsViewModel> GetAllDistrict(bool? isVerified, bool? isLocked, Status? status, int page = 0, int records = 0);
        long SaveDistric(DistrictsViewModel model);
        long UpdateDistric(DistrictsViewModel model);
        void DeleteDistric(Int64 UserId, int modifiedByid);
        DistrictsViewModel GetDistric(long id);
        List<DistrictsViewModel> GetDistricts(Int64 SelectedDistrictId);

        // Refill Rate      
        decimal GetNewRate();
        decimal GetOldRate();
        Int64 SaveRefilleRate(RefillRateViewModel model);
        bool UpdateRefillRate(RefillRateViewModel model);
        RefillRateViewModel GetRefillRate(Int64 id);

        void SaveUserLog(string module, string msg, Int64 userId);

      
    }
}
