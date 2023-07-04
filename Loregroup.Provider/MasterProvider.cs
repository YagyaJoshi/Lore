using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System;
using Loregroup.Data;
using System.Collections.Generic;
using Loregroup.Core.Helpers;
using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.Interfaces;
using Loregroup.Core;


namespace Loregroup.Provider
{
    public class MasterProvider : IMasterProvider
    {

        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly ISession _session;
        private readonly IUtilities _utilities;
        public MasterProvider(AppContext context, ISecurity security, IUtilities utilities, ISession session)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _session = session;
        }
        
        //old
        #region: Status and Notification
        public void Updatestate_status(long uid)
        {
            try
            {
                var State = _context.States.FirstOrDefault(x => x.Id == uid);
                State.StatusId = 1;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCity_status(long uid)
        {
            try
            {
                var City = _context.Cities.FirstOrDefault(x => x.Id == uid);
                City.StatusId = 1;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateCompany_status(long uid)
        //{
        //    try
        //    {
        //        var City = _context.CompanyLeads.FirstOrDefault(x => x.Id == uid);
        //        City.StatusId = 1;
        //        _context.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void UpdateDistrict_status(long uid)
        {
            try
            {
                var City = _context.Districts.FirstOrDefault(x => x.Id == uid);
                City.StatusId = 1;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Getprofimage(long userid)
        {
            string pic = "";
            try
            {
                //byte[] proim = _context.AppUsers.Where(x => x.Id == userid).Select(x => x.ProfileImage).FirstOrDefault();
                // byte[] proim = _context.MasterUsers.Where(x => x.Id == userid).Select(x => x.ProfileImage).FirstOrDefault();
                //if (proim != null)
                //{
                //    var base64 = Convert.ToBase64String(proim);

                //    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

                //    pic = imgSrc;
                //}
                //else
                //{

                pic = "data:image/gif;base64,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";
                // }
            }
            catch (Exception ex)
            { }
            return pic;
        }
        public List<NotificationsViewModel> GetAdminnotification(Status? status, int page = 0, int records = 0)
        {
            var MallPredicate = PredicateBuilder.True<Notification>();
            if (status != null)
            {
                MallPredicate.And(x => x.StatusId == (int)status);
            }

            //if (page > 0)
            //{
            //    return _context.Classifieds.Where(x => x.StatusId == 3)
            //       .ToList()
            //       .Select(ToNotificationViewModel)
            //       .ToList();
            //}

            return _context.Notifications.OrderBy(k => k.StatusId).Where(x => x.ReceiverId == 1 && x.PackageId == 1 && x.StatusId != 4)
                  .ToList()
                  .Select(ToNotificationViewModel)
                  .ToList();

            throw new NotImplementedException();



        }
        public NotificationsViewModel ToNotificationViewModel(Notification Notifica)
        {


            NotificationsViewModel abc = new NotificationsViewModel();
            abc.notid = Notifica.Id;
            abc.Name = Notifica.Name;
            abc.Description = Notifica.Description;
            //abc.NotificationMessage = Notifica.NotificationMessage;
            abc.NotificationType = Notifica.NotificationType;
            abc.StatusId = Notifica.StatusId;
            abc.Date = Notifica.CreationDate.ToString("dd/MM/yyyy");

            return abc;



        }
        //sulabh 09/08
        public NotificationsViewModel Getnotification(long NotificationID)
        {
            Notification notific = _context.Notifications.FirstOrDefault(x => x.Id == NotificationID);
            if (notific.StatusId == 1)
            {
                notific.StatusId = 2;
                _context.SaveChanges();
            }
            {
                return TosingleNotificationViewModel(notific);
            }
        }
        public NotificationsViewModel TosingleNotificationViewModel(Notification Notifica)
        {


            NotificationsViewModel abc = new NotificationsViewModel();
            abc.notid = Notifica.Id;
            abc.Name = Notifica.Name;
            abc.Description = Notifica.Description;
            abc.NotificationMessage = Notifica.NotificationMessage;


            abc.Date = Notifica.CreationDate.ToString("dd/MM/yyyy");

            return abc;



        }
        //delete notification
        public void DeleteNotification(Int64 UserId, int modifiedByid)
        {
            Notification notifi = _context.Notifications.FirstOrDefault(x => x.Id == UserId);
            //return new WidgetViewModel()
            //{}

            if (notifi != null)
            {
                notifi.StatusId = (int)Status.Deleted;
                notifi.ModifiedById = modifiedByid;
                notifi.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

        }
        public long SendStoreNotification(NotificationsViewModel model)
        {
            long retid = 0;
            try
            {
                //String FirstName = _context.StoreMasters.Where(x => x.Id == id).Select(x => x.StoreName).FirstOrDefault();
                Notification notif = new Notification()
                {

                    Name = model.Name,
                    SenderId = model.SenderId,
                    ReceiverId = model.ReceiverId,
                    NotificationMessage = model.NotificationMessage,
                    Description = model.Description,
                    RoleId = 2,
                    StatusId = 1,
                    NotificationType = model.NotificationType,
                    PackageId = model.PackageId,

                };
                _context.Notifications.Add(notif);
                _context.SaveChanges();
                return retid;
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void SaveStoreReview(string review, Int64 id, Int64 storeid)
        {
            Notification Newreview = new Notification()
            {
                SenderId = id,
                NotificationMessage = review,
                ReceiverId = storeid,
                NotificationType = 2,
                Description = "Review",
                PackageId = 2
            };
            _context.Notifications.Add(Newreview);
            _context.SaveChanges();
        }
        public void PostCongratulation(string review, Int64 id, Int64 newopeningid)
        {
            Notification Newreview = new Notification()
            {
                SenderId = id,
                NotificationMessage = review,
                ReceiverId = newopeningid,
                NotificationType = 2,
                Description = "Congratulation",
                PackageId = 2
            };
            _context.Notifications.Add(Newreview);
            _context.SaveChanges();
        }
        public void SaveMallReview(string review, Int64 id, Int64 mallid)
        {
            Notification newReview = new Notification()
            {
                SenderId = id,
                NotificationMessage = review,
                ReceiverId = mallid,
                NotificationType = 2,
                Description = "Review",
                PackageId = 4
            };
            _context.Notifications.Add(newReview);
            _context.SaveChanges();
        }
        #endregion

        #region: Block

        public BlocksViewModel ToBlockViewModel(Block block)
        {

            return new BlocksViewModel()
            {
                Id = block.Id,
                BlockName = block.BlockName,
                StatusId = block.StatusId,
                DistricId = block.DistricId,
            };
        }



        public List<BlocksViewModel> GetAllBlock(bool? isVerified, bool? isLocked, Status? status, int page = 0, int records = 0)
        {

            var StatePredicate = PredicateBuilder.True<Block>();

            if (status != null)
            {
                StatePredicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                return _context.Blocks.Where(x => x.StatusId == 1)
                   .ToList()
                   .Select(ToBlockViewModel)
                   .ToList();
            }
            return _context.Blocks.Where(StatePredicate)
                   .ToList()
                   .Select(ToBlockViewModel)
                   .ToList(); throw new NotImplementedException();
        }

        public long SaveBlock(BlocksViewModel model)
        {
            long i = 0;
            try
            {
                Block block = new Block()
                {
                    BlockName = model.BlockName,
                    DistricId = model.DistricId
                };
                _context.SaveChanges();
                i = block.Id;

            }
            catch (Exception ex)
            { }
            return i;
        }

        public void UpdateBlock_status(long uid)
        {
            try
            {
                var res = _context.Blocks.FirstOrDefault(x => x.Id == uid);
                res.StatusId = 1;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BlocksViewModel GetBlock(long id)
        {

            Block Blocks = _context.Blocks.FirstOrDefault(x => x.Id == id);

            {
                return ToBlockViewModel(Blocks);
            }

        }
        public long UpdateBlock(BlocksViewModel model)
        {
            try
            {
                var blocks = _context.Blocks.FirstOrDefault(x => x.Id == model.Id);
                blocks.BlockName = model.BlockName;
                blocks.DistricId = model.DistricId;
                _context.SaveChanges();
                return blocks.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteBlock(Int64 UserId, int modifiedByid)
        {
            Block Blocks = _context.Blocks.FirstOrDefault(x => x.Id == UserId);

            if (Blocks != null)
            {
                Blocks.StatusId = (int)Status.Deleted;
                Blocks.ModifiedById = modifiedByid;
                Blocks.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

        }

        public List<UserViewModel> GetUserList()
        {
            List<UserViewModel> userlist = new List<UserViewModel>();
            var res = _context.MasterUsers.Where(x => x.StatusId == (int)Status.Active).ToList();
            if (res != null)
            {
                foreach (var item in res)
                {
                    UserViewModel obj = new UserViewModel();
                    obj.Id = item.Id;
                    obj.UserName = item.FirstName;//+" "+item.LastName;
                    userlist.Add(obj);
                }


            }
            return userlist;

        }

        #endregion
        
        //Delivery Slip 

        #region : State

        public long SaveState(StateViewModel model)
        {
            //Add by 
            long i = 0;

            var CountryName = _context.Countries.Where(x => x.Id == model.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
            try
            {
                State State = new State()
                {
                    Statename = model.Statename,
                    CountryId = model.CountryId,
                    CountryName = CountryName
                };
                _context.States.Add(State);
                _context.SaveChanges();
                i = State.Id;
            }
            catch (Exception ex)
            { }
            return i;
        }

        public List<StateViewModel> GetAllStates(bool? isVerified, bool? isLocked, Status? status, int page = 0, int records = 0)
        {

            var StatePredicate = PredicateBuilder.True<State>();

            if (status != null)
            {
                StatePredicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                return _context.States.Where(x => x.StatusId == 1)
                   .ToList()
                   .Select(ToStateViewModel)
                   .ToList();
            }
            return _context.States.Where(x => x.StatusId != 4)
                   .ToList()
                   .Select(ToStateViewModel)
                   .ToList(); throw new NotImplementedException();


        }

        public StateViewModel ToStateViewModel(State State)
        {
            //Add by 
            //var CountryName = _context.Countries.Where(y => y.StatusId != 4).Where(x => x.Id == State.CountryId).Select(x => x.CountryName).FirstOrDefault();
            return new StateViewModel()
            {
                Id = State.Id,
                Statename = State.Statename,
                Countryname = State.CountryName,
                CountryId = State.CountryId,
                StatusId = State.StatusId,
                Edit = "<a href='/Master/EditState?id=" + State.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Master/DeleteState?id=" + State.Id + "' title='Edit'>Delete</a>"

            };
        }

        public StateViewModel GetState(long id)
        {
            State State = _context.States.FirstOrDefault(x => x.Id == id);

            {
                return ToStateViewModel(State);
            }
        }

        public long UpdateState(StateViewModel model)
        {
            try
            {
                var CountryName = _context.Countries.Where(x => x.Id == model.CountryId && x.StatusId != 4).Select(x => x.CountryName).FirstOrDefault();
               
                var State = _context.States.FirstOrDefault(x => x.Id == model.Id);
                State.Statename = model.Statename;
                State.CountryId = model.CountryId;
                State.CountryName = CountryName;
                _context.SaveChanges();
                return State.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteState(Int64 UserId, int modifiedByid)
        {
            State States = _context.States.FirstOrDefault(x => x.Id == UserId);
            //return new WidgetViewModel()
            //{}

            if (States != null)
            {
                States.StatusId = (int)Status.Deleted;
                States.ModifiedById = modifiedByid;
                States.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

        }

        public List<StateViewModel> GetAllStates()
        {
            try
            {
                return _context.States.Where(x => x.StatusId != 4)
                    .ToList()
                    .Select(ToStateViewModel)
                    .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<StateViewModel> GetStates(Int64 Id = 0)
        {
            try
            {
                return _context.States.Where(x => x.StatusId != 4).Where(k => k.CountryId == Id)
                .ToList()
                .Select(ToStateViewModel)
                .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<StateViewModel> GetAllStates(int start, int length, string search, int filtercount)
        {
            List<StateViewModel> newData = new List<StateViewModel>();

            try
            {
                List<StateViewModel> result = new List<StateViewModel>();

                if (search == String.Empty)
                {
                    result = _context.States.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToStateViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.States.Where(p => p.Statename.Contains(value) || p.CountryName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToStateViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public void DeleteState(Int64 id)
        {
            State state = _context.States.Where(x => x.Id == id).FirstOrDefault();
            state.StatusId = 4;
            state.ModificationDate = DateTime.UtcNow;
            state.ModifiedById = _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

        #region: District
        public List<DistrictsViewModel> GetAllDistrict(bool? isVerified, bool? isLocked, Status? status, int page = 0, int records = 0)
        {
            //Add by 
            var StatePredicate = PredicateBuilder.True<District>();

            if (status != null)
            {
                StatePredicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                return _context.Districts.Where(x => x.StatusId == 1)
                   .ToList()
                   .Select(ToDistrictsViewModel)
                   .ToList();
            }
            return _context.Districts.Where(x => x.StatusId == 1)
                   .ToList()
                   .Select(ToDistrictsViewModel)
                   .ToList(); throw new NotImplementedException();
        }
        public DistrictsViewModel ToDistrictsViewModel(District District)
        {
            //Add by 

            try
            {
                return new DistrictsViewModel()
                {
                    Id = District.Id,
                    DistrictName = District.DistricName,
                    StatusId = District.StatusId,
                    StateId = District.StateId,
                };
            }

            catch (Exception ex) { return new DistrictsViewModel(); }
        }
        public long SaveDistric(DistrictsViewModel model)
        {
            long i = 0;
            try
            {
                District District = new District()
                {
                    DistricName = model.DistrictName,
                    StateId = model.StateId,
                };
                _context.Districts.Add(District);
                _context.SaveChanges();
                i = District.Id;
            }
            catch (Exception ex)
            { }
            return i;
        }
        public long UpdateDistric(DistrictsViewModel model)
        {
            try
            {
                var Districts = _context.Districts.FirstOrDefault(x => x.Id == model.Id);
                Districts.DistricName = model.DistrictName;
                Districts.StateId = model.StateId;
                _context.SaveChanges();
                return Districts.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteDistric(Int64 UserId, int modifiedByid)
        {
            District Districts = _context.Districts.FirstOrDefault(x => x.Id == UserId);

            if (Districts != null)
            {
                Districts.StatusId = (int)Status.Deleted;
                Districts.ModifiedById = modifiedByid;
                Districts.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

        }
        public DistrictsViewModel GetDistric(long id)
        {
            District Districts = _context.Districts.FirstOrDefault(x => x.Id == id);

            {
                return ToDistrictsViewModel(Districts);
            }

        }
        public List<DistrictsViewModel> GetDistricts(Int64 SelectedDistrictId)
        {

            return _context.Districts.Where(k => k.StateId == SelectedDistrictId && k.StatusId != 4).ToList().Select(ToDistrictsViewModel).ToList();

        }
        #endregion

        #region: City
        public long SaveCity(CityViewModel model)
        {
            long i = 0;
            try
            {
                City City = new City()
                {
                    CityName = model.Cityname,
                    CountryId = model.CountryId,
                    DistricId = model.DistricId,
                    StateId = model.StateId
                };
                _context.Cities.Add(City);
                _context.SaveChanges();
                i = City.Id;
            }
            catch (Exception ex)
            { }
            return i;
        }
        public long UpdateCity(CityViewModel model)
        {
            try
            {
                var City = _context.Cities.FirstOrDefault(x => x.Id == model.Id);
                City.CityName = model.Cityname;
                City.DistricId = model.DistricId;
                City.CountryId = model.CountryId;
                City.StateId = model.StateId;
                _context.SaveChanges();
                return City.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteCity(Int64 UserId, int modifiedByid)
        {
            City cities = _context.Cities.FirstOrDefault(x => x.Id == UserId);

            if (cities != null)
            {
                cities.StatusId = (int)Status.Deleted;
                cities.ModifiedById = _session.CurrentUser.Id;
                cities.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

        }
        public void DeleteCity(Int64 UserId)
        {
            City cities = _context.Cities.FirstOrDefault(x => x.Id == UserId);

            if (cities != null)
            {
                cities.StatusId = (int)Status.Deleted;
                cities.ModifiedById = _session.CurrentUser.Id;
                cities.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

        }
        public CityViewModel ToCityViewModel(City city)
        {

            string district = "";
            string state = "";

            if (city.DistricId != null)
            {
                var d = GetDistric(city.DistricId);
                district = d.DistrictName;

            }

            if (city.StateId != null)
            {
                var d = GetState(city.StateId);
                state = d.Statename;

            }

            string st = "";
            if (city.StatusId == 1)
            {
                st = "<span class='label label-success'>Active</span>";

            }
            else if (city.StatusId == 2)
            {
                st = "<span class='label label-primary'>In-Active</span>";
            }
            else if (city.StatusId == 3)
            {
                st = "<span class='label label-warning'>Pending</span>";
            }
            else if (city.StatusId == 4)
            {
                st = "<span class='label label-danger'>Deleted</span>";
            }

            var CountryName = _context.Countries.Where(y => y.StatusId != 4).Where(x => x.Id == city.CountryId).Select(x => x.CountryName).FirstOrDefault();
            var StateName = _context.States.Where(y => y.StatusId != 4).Where(x => x.Id == city.StateId).Select(x => x.Statename).FirstOrDefault();
            return new CityViewModel()
            {
                Id = city.Id,
                Cityname = city.CityName,
                CountryId = city.CountryId,
                CountryName = CountryName,
                StateId = city.StateId,
                DistricId = city.DistricId,
                StatusId = city.StatusId,
                DistrictName = district,
                StateName = StateName,
                Education = st,
                Edit = "<a href='/Master/EditCity?id=" + city.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Master/DeleteCity?id=" + city.Id + "' title='Edit'>Delete</a>"
            };
        }

        public CityViewModel GetCity(long id)
        {
            City Cities = _context.Cities.FirstOrDefault(x => x.Id == id);
            {
                return ToCityViewModel(Cities);
            }

        }
        public List<CityViewModel> GetAllCity(Status? status, int records, int page = 0)
        {
            var CityPredicate = PredicateBuilder.True<City>();
            if (status != null)
            {
                CityPredicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                //return _context.Cities.Where(x => x.StatusId == 1)
                //   .ToList()
                //   .Select(ToCityViewModel)
                //   .ToList();
            }
            return _context.Cities.Where(x => x.StatusId != 4)
                    .ToList()
                    .Select(ToCityViewModel)
                    .ToList();

            throw new NotImplementedException();
        }

        public List<CityViewModel> GetCities(Int64 StateId)
        {
            return _context.Cities.Where(k => k.StateId == StateId && k.StatusId != 4).ToList().Select(ToCityViewModel).ToList();
        }

        public List<CityViewModel> GetAllCityAccId(Int64 Id, Status? status, int records, int page = 0)
        {
            var CityPredicate = PredicateBuilder.True<City>();
            if (status != null)
            {
                CityPredicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                //return _context.Cities.Where(x => x.StatusId == 1)
                //   .ToList()
                //   .Select(ToCityViewModel)
                //   .ToList();
            }
            if (records == 0)
            {
                return _context.Cities.Where(x => x.StatusId == 1 && x.DistricId == Id)
                       .ToList()
                       .Select(ToCityViewModel)
                       .ToList();
            }
            else
            {
                return _context.Cities
                   .ToList()
                   .Select(ToCityViewModel)
                   .ToList();
            }
            throw new NotImplementedException();

        }
        public string GetCityName(Int64? CityId)
        {
            string city = "";
            try
            {
                city = _context.Cities.FirstOrDefault(x => x.Id == CityId).CityName;
                return city;
            }
            catch (Exception ex)
            {
                return city;
            }
        }

        public List<CityViewModel> GetAllCities(int start, int length, string search, int filtercount)
        {
            List<CityViewModel> newData = new List<CityViewModel>();

            try
            {
                List<CityViewModel> result = new List<CityViewModel>();

                if (search == String.Empty)
                {
                    result = _context.Cities.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                       .ToList().Select(ToCityViewModel).ToList();

                    filtercount = result.Count;
                    newData = result;
                }
                if (search != String.Empty)
                {
                    var value = search.Trim();
                    result = _context.Cities.Where(p => p.CityName.Contains(value))
                        .OrderByDescending(x => x.Id).ToList()
                        .Select(ToCityViewModel).ToList();


                    filtercount = result.Count;
                    newData = result;
                }

                return newData;

            }
            catch (Exception ex)
            {
                return newData;
            }
        }               

        #endregion

        #region : User
        public List<MasterUserViewModel> GetAllUser(Status? status, int page = 0, int records = 0)
        {
            var UserPredicate = PredicateBuilder.True<AppUser>();
            if (status != null)
            {
                UserPredicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                return _context.MasterUsers.Where(x => x.StatusId != 4)
                   .ToList()
                   .Select(ToUserViewModel)
                   .ToList();
            }
            if (_session.CurrentUser.Id == 1)
            {
                return _context.MasterUsers.Where(x => x.StatusId != 4).OrderBy(x => x.FirstName)
                   .ToList()
                   .Select(ToUserViewModel)
                   .ToList();


            }

            else
            {

                return _context.MasterUsers.Where(x => x.StatusId != 4 && x.CreatedById == _session.CurrentUser.Id).OrderBy(x => x.FirstName)
                       .ToList()
                       .Select(ToUserViewModel)
                       .ToList();

            }
            throw new NotImplementedException();
        }
        public MasterUserViewModel ToUserViewModel(MasterUser User)//AppUser User)
        {

            String RoleName = "";
            if (User.RoleId == 1)
            {
                RoleName = "SuperAdmin";

            }
            if (User.RoleId == 2)
            {
                RoleName = "Admin";

            }
            if (User.RoleId == 3)
            {
                RoleName = "Marketing Manager";

            }
            if (User.RoleId == 4)
            {
                RoleName = "Distributor";

            }
            if (User.RoleId == 5)
            {
                RoleName = "User";

            }

            return new MasterUserViewModel()
            {
                Id = User.Id,
                UserName = User.UserName,
                Email = User.Email,
                FirstName = User.FirstName,
                Gender = (Gender)User.Gender,
                LastName = User.LastName,
                SelectedStateId = User.StateId,
                SelectedDistrictId = User.DistrictId,
                CityId = User.CityId,
                Mobile = User.Mobile,
                // Fax = User.fax,
                RoleId = User.RoleId,
                Password = User.Password,
                RoleName = RoleName,
                StatusId = User.StatusId


            };
        }
        public List<RoleViewModel> GetAllRole(Status? status, int page = 0, int records = 0)
        {
            var StatePredicate = PredicateBuilder.True<Role>();

            if (status != null)
            {
                StatePredicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                return _context.Roles.Where(StatePredicate).Skip(page * records).Take(records)
                   .ToList()
                   .Select(ToRoleViewModel)
                   .ToList();
            }
            return _context.Roles.Where(StatePredicate)
                   .ToList()
                   .Select(ToRoleViewModel)
                   .ToList(); throw new NotImplementedException();
        }
        public RoleViewModel ToRoleViewModel(Role Role)
        {

            //Add by 
            return new RoleViewModel()
            {
                Id = Role.Id,
                Name = Role.Name,
                StatusId = Role.StatusId,

            };
        }
        public List<MasterUserViewModel> GetAllAppUsers()
        {
            //var UserPredicate = PredicateBuilder.True<User>();
            //if (status != null)
            //{
            //    UserPredicate.And(x => x.StatusId == (int)status);
            //}

            ////if (page > 0)
            ////{
            ////    //return _context.AppUsers.Where(UserPredicate).Skip(page * records).Take(records)
            ////    return _context.AppUsers.Where(UserPredicate)
            ////       .ToList()
            ////       .Select(ToUserRegistrationViewModel)
            ////       .ToList();
            ////}
            //return _context.AppUsers.Where(UserPredicate)
            //       .ToList()
            //       .Select(ToUserRegistrationViewModel)

            //       .ToList();
            //throw new NotImplementedException();
            //  var CityPredicate = PredicateBuilder.True<AppUser>();
            //if (status != null)
            //{
            //    CityPredicate.And(x => x.StatusId == (int)status);
            //}

            //if (page > 0)
            //{

            //}
            //if (records == 0)
            //{
            //return _context.AppUsers.Where(x => x.StatusId == 1)
            //       .ToList()
            //       .Select(ToUserRegistrationViewModel)
            //       .ToList();
            //}
            //else
            //{

            if (_session.CurrentUser.Id == 1)
            {


                return _context.MasterUsers.Where(x => x.StatusId == 1)//.AppUsers
                      .ToList()
                      .Select(ToUserRegistrationViewModel)
                      .ToList();



            }

            else
            {

                return _context.MasterUsers.Where(x => x.StatusId == 1 && x.CreatedById == _session.CurrentUser.Id).OrderBy(x => x.FirstName)
                          .ToList()
                          .Select(ToUserViewModel)
                          .ToList();
            }


            //}
            throw new NotImplementedException();
        }
        public MasterUserViewModel ToUserRegistrationViewModel(MasterUser User)//AppUser User)
        {
            String RoleName = "";
            if (User.RoleId == 1)
            {
                RoleName = "SuperAdmin";

            }
            if (User.RoleId == 2)
            {
                RoleName = "Admin";

            }
            if (User.RoleId == 3)
            {
                RoleName = "Marketing Manager";

            }
            if (User.RoleId == 4)
            {
                RoleName = "Distributor";

            }
            if (User.RoleId == 5)
            {
                RoleName = "User";

            }
            return new MasterUserViewModel()
            {
                Email = User.Email,
                FirstName = User.FirstName,
                Password = User.Password,
                Mobile = User.Mobile,
                StatusId = User.StatusId,
                Id = User.Id,
                RoleId = User.RoleId,
                RoleName = RoleName,
                UserName = User.UserName,




            };
        }
        public void DeleteMasterUser(int id)
        {
            MasterUser AppUsers = _context.MasterUsers.FirstOrDefault(x => x.Id == id);

            if (AppUsers != null)
            {

                AppUsers.StatusId = (int)Status.Deleted;
                AppUsers.ModifiedById = id;
                AppUsers.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }

        }
        public MasterUserViewModel GetMasterUserForEdit(Int64 Id)
        {
            MasterUserViewModel user = new MasterUserViewModel();

            var res = _context.MasterUsers.FirstOrDefault(x => x.Id == Id);
            if (res != null)
            {
                user.FirstName = res.FirstName;
                user.LastName = res.LastName;
                user.Email = res.Email;
                user.SelectedStateId = res.StateId;
                user.SelectedDistrictId = res.DistrictId;
                user.CityId = res.CityId;
                user.Gender = (Gender)res.Gender;
                user.UserName = res.UserName;
                user.Password = res.Keyword;
                user.Keyword = res.Keyword;
                user.Fax = res.Fax;
                user.Mobile = res.Mobile;
                user.RoleId = res.RoleId;
                user.Id = res.Id;

            }
            return user;

        }
        public bool EditUpdateUser(MasterUserViewModel model)
        {
            try
            {

                String salt = Guid.NewGuid().ToString().Replace("-", "");
                string password = _security.ComputeHash(model.Password, salt);

                var result = _context.MasterUsers.FirstOrDefault(x => x.Id == model.Id);
                if (result != null)
                {
                    result.Salt = salt;
                    result.Password = password;
                    result.FirstName = model.FirstName;
                    result.LastName = model.LastName;
                    result.Email = model.Email;
                    result.UserName = model.UserName;
                    result.Gender = (int)result.Gender;
                    result.Mobile = model.Mobile;
                    result.CityId = model.CityId;
                    result.Fax = model.Fax;
                    result.StatusId = 1;
                    result.Keyword = model.Password;
                    result.CreatedById = model.CreatedById;
                    result.ModifiedById = model.ModifiedById;
                    result.CreationDate = model.CreationDate;
                    result.ModificationDate = model.ModificationDate;
                    result.StateId = model.SelectedStateId;
                    result.District = model.District;
                    result.CityId = model.CityId;
                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }



            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public long UserRegistration(MasterUserViewModel model)
        {
            try
            {
                String salt = Guid.NewGuid().ToString().Replace("-", "");
                string password = _security.ComputeHash(model.Password, salt);
                string verificationCode = _utilities.GenerateRandomString(4);

                MasterUser user = new MasterUser()
                {

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = password,
                    Salt = salt,
                    Gender = (int)model.Gender,
                    Mobile = model.Mobile,
                    Fax = model.Fax,
                    RoleId = model.RoleId,
                    PackageId = 0,
                    //GetPackageId(_session.CurrentUser.RoleId), 
                    StatusId = 1,
                    StateId = model.SelectedStateId,
                    DistrictId = model.SelectedDistrictId,
                    CityId = model.CityId,
                    CreatedById = _session.CurrentUser.Id,
                    CreationDate = System.DateTime.UtcNow,
                    Keyword = model.Keyword
                };
                _context.MasterUsers.Add(user);
                _context.SaveChanges();
                UpdatePackageId(_session.CurrentUser.RoleId, user.Id, model);
                return user.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MasterUserViewModel GetUser(Int64 UserId)
        {

            MasterUser user = new MasterUser();
            try
            {
                user = _context.MasterUsers.FirstOrDefault(x => x.Id == UserId);
                return ToUserViewModel(user);

            }
            catch (Exception ex) { return new MasterUserViewModel(); }


        }

        public Int64 GetPackageId(Int64 UserId)
        {

            Int64 PackageId = 0;
            try
            {
                //if (_session.CurrentUser.RoleId == 2)
                //{
                //    PackageId = _session.CurrentUser.Id;
                //    return PackageId;
                //}
                //else
                //{
                PackageId = _context.MasterUsers.FirstOrDefault(x => x.Id == UserId).PackageId;
                return PackageId;
                //}
            }
            catch (Exception ex) { return PackageId; }
        }

        public Int64 UpdatePackageId(Int64 Role, Int64 UserId, MasterUserViewModel model)
        {

            Int64 packageId = 0;
            try
            {
                if (_session.CurrentUser.RoleId == 2)
                {
                    packageId = _session.CurrentUser.Id;
                    MasterUser UserData = _context.MasterUsers.FirstOrDefault(x => x.Id == UserId);
                    if (UserData != null)
                    {
                        UserData.PackageId = packageId;
                        _context.SaveChanges();
                    }
                    return packageId;
                }
                else
                {
                    if (model.RoleId == 2)
                    {
                        MasterUser UserData = _context.MasterUsers.FirstOrDefault(x => x.Id == UserId);

                        if (UserData != null)
                        {
                            UserData.PackageId = UserId;
                            _context.SaveChanges();
                        }
                        return UserId;
                    }
                    else
                    {
                        packageId = _context.MasterUsers.FirstOrDefault(x => x.Id == _session.CurrentUser.Id).PackageId;
                        MasterUser UserData = _context.MasterUsers.FirstOrDefault(x => x.Id == UserId);

                        if (UserData != null)
                        {
                            UserData.PackageId = packageId;
                            _context.SaveChanges();
                        }
                        return packageId;
                        //return PackageId;
                    }
                }
            }
            catch (Exception ex) { return packageId; }

        }

        public Int64 GetPackageIdForAPI(Int64 UserId)
        {
            Int64 PackageId = 0;
            try
            {
                PackageId = _context.MasterUsers.FirstOrDefault(x => x.Id == UserId).PackageId;
                return PackageId;
            }
            catch (Exception ex) { return PackageId; }
        }
        #endregion

        #region : Country

        public List<CountryViewModel> getAllCountries()
        {
            var result = _context.Countries.Where(x => x.StatusId != 4)
                .Select(ToCountryViewModel).ToList();
            return result;
        }

        public List<CountryViewModel> getAllCountries(int start, int length, string search, int filtercount)
        {
            //  var result = _context.Countries.Where(x => x.StatusId != 4)
            //    .Select(ToCountryViewModel).ToList();
            var result = _context.Countries.OrderByDescending(x => x.Id).Where(x => x.StatusId != 4)
                .Skip(start).Take(length).ToList()
                .Select(ToCountryViewModel).ToList();
            filtercount = result.Count;
            if (search != String.Empty)
            {
                var value = search.Trim();
                result = _context.Countries.Where(p => p.CountryName.Contains(value) || p.CurrencyName.Contains(value) || p.CurrencySymbol.Contains(value))
                    .OrderByDescending(x => x.Id).Skip(start).Take(length).ToList()
                    .Select(ToCountryViewModel).ToList();
                filtercount = result.Count;
            }
            return result;
        }

        public CountryViewModel ToCountryViewModel(Country country)
        {
            return new CountryViewModel()
            {
                Id = country.Id,
                countryName = country.CountryName,
                currencyName = country.CurrencyName,
                currencySymbol = country.CurrencySymbol,
                StatusId = Convert.ToInt32(country.StatusId),
                Edit = "<a href='/Master/EditCountry?id=" + country.Id + "' title='Edit'>Edit</a>",
                Delete = "<a href='/Master/DeleteCountry?id=" + country.Id + "' title='Delete'>Delete</a>"
            };
        }

        public long saveCountry(CountryViewModel model)
        {
            Int64 i = 0;
            int? status = _context.Countries.Where(x => x.CountryName == model.countryName).Select(x => x.StatusId).FirstOrDefault();
            if (status == 4)
            {
                try
                {
                    var res = _context.Countries.Where(x => x.CountryName == model.countryName).FirstOrDefault();
                    res.StatusId = 1;
                    res.CurrencyName = model.currencyName;
                    res.CurrencySymbol = model.currencySymbol;
                    res.ModificationDate = DateTime.UtcNow;
                    res.ModifiedById = _session.CurrentUser.Id;
                    _context.SaveChanges();
                    i = res.Id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    Country country = new Country()
                    {
                        CountryName = model.countryName,
                        CurrencyName = model.currencyName,
                        CurrencySymbol = model.currencySymbol,
                        CreatedById = model.CreatedById
                    };
                    _context.Countries.Add(country);
                    _context.SaveChanges();
                    i = country.Id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }

        public void deleteCountry(Int64 id)
        {
            Country country = _context.Countries.Where(x => x.Id == id).FirstOrDefault();
            if (country != null)
            {
                country.StatusId = (int)Status.Deleted;
                country.ModificationDate = DateTime.UtcNow;
                country.ModifiedById = _session.CurrentUser.Id;
                _context.SaveChanges();
            }
        }

        public CountryViewModel getCountry(Int64 id)
        {
            Country country = _context.Countries.FirstOrDefault(x => x.Id == id);
            return ToCountryViewModel(country);
        }

        public Int64 updateCountry(CountryViewModel model)
        {
            try
            {
                var res = _context.Countries.FirstOrDefault(x => x.Id == model.Id);
                res.CountryName = model.countryName;
                res.CurrencyName = model.currencyName;
                res.CurrencySymbol = model.currencySymbol;
                res.ModificationDate = DateTime.UtcNow;
                res.ModifiedById = model.ModifiedById;
                _context.SaveChanges();
                return res.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region : RefillRate

        public RefillRateViewModel ToRefillRateViewModel(RefillRate refill)
        {
            try
            {

                return new RefillRateViewModel()
                {

                    Id = refill.Id,
                    PackageId = refill.PackageId,
                    DateFrom = refill.DateFrom,
                    NewRefillRate = refill.NewRefillRate,
                    OldRefillRate = refill.OldRefillRate,
                    datefrm = refill.DateFrom.ToString("dd/MM/yyyy")

                };
            }
            catch (Exception ex) { return new RefillRateViewModel(); }


        }

        public decimal GetOldRate()
        {
            decimal oldrate = 0;
            Int64 pid = 0;
            pid = GetPackageId(_session.CurrentUser.Id);
            try
            {
                oldrate = _context.RefillRates.Where(x => x.PackageId == pid).Select(y => y.OldRefillRate).Sum();
                return oldrate;
            }
            catch (Exception ex)
            {
                return oldrate;
            }
        }

        public decimal GetNewRate()
        {
            Int64 pid = 0;
            decimal newrate = 0;
            pid = GetPackageId(_session.CurrentUser.Id);
            try
            {
                newrate = _context.RefillRates.Where(x => x.PackageId == pid).Select(y => y.NewRefillRate).Sum();
                return newrate;
            }
            catch (Exception ex)
            {
                return newrate;
            }
        }

        public RefillRateViewModel GetRefillRate(Int64 id)
        {

            RefillRate refill = new RefillRate();
            try
            {
                refill = _context.RefillRates.FirstOrDefault(x => x.PackageId == id);
                return ToRefillRateViewModel(refill);


            }
            catch (Exception ex) { return new RefillRateViewModel(); }

        }

        public Int64 SaveRefilleRate(RefillRateViewModel model)
        {

            RefillRate rr = new RefillRate();
            Int64 pid = 0;
            try
            {
                RefillRate rr1 = new RefillRate()
                {
                    NewRefillRate = model.NewRefillRate,
                    DateFrom = model.DateFrom,
                    CreatedById = _session.CurrentUser.Id,
                    CreationDate = System.DateTime.UtcNow,
                    PackageId = GetPackageId(_session.CurrentUser.Id),


                };
                _context.RefillRates.Add(rr1);
                _context.SaveChanges();
                return model.PackageId;

            }
            catch (Exception ex) { return model.PackageId; }


        }

        public bool UpdateRefillRate(RefillRateViewModel model)
        {
            RefillRate rr = new RefillRate();
            Int64 pid = 0;
            try
            {
                rr = _context.RefillRates.FirstOrDefault(x => x.PackageId == model.PackageId);
                if (rr != null)
                {
                    rr.OldRefillRate = GetNewRate();
                    rr.NewRefillRate = model.NewRefillRate;
                    rr.ModifiedById = _session.CurrentUser.Id;
                    rr.DateFrom = model.DateFrom;
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex) { return false; }

        }
        #endregion

        #region : DSLog

        public Int64 SaveDSLog(CheckStatusViewModel model)
        {

            Int64 i = 0;
            try
            {
                DSLog log = new DSLog()
                {
                    Message = model.dslog.Message,
                    Number = model.ConsumerNo,
                    CreatedById = _session.CurrentUser.Id,
                    TimeStamp = System.DateTime.UtcNow,
                    CreationDate = model.CreationDate


                };
                _context.DSLogs.Add(log);
                i = _context.SaveChanges();
                return i;

            }
            catch (Exception ex) { return i; }

        }



        #endregion

        #region: UserLog

        public void SaveUserLog(string module, string msg, Int64 userId)
        {
            long rId = 0;
            try
            {
                rId = _context.MasterUsers.FirstOrDefault(x => x.Id == userId).RoleId;
                if (rId == 2 || rId == 3|| rId==7||rId==2||rId==5)
                {
                    UserLog log = new UserLog()
                    {
                        Module = module,
                        MessageLog = msg,
                        RoleId = rId,
                        StatusId = 1,
                        CreatedById = userId,
                        CreationDate = DateTime.Now
                    };
                    _context.UserLogs.Add(log);
                    _context.SaveChanges();
                }                             
            }
            catch (Exception ex)
            { }            
        }

       

        #endregion

    }
}
