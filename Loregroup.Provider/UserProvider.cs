using Loregroup.Core.BusinessEntities;
using Loregroup.Core.Enumerations;
using Loregroup.Core.Exceptions;
using Loregroup.Core.Helpers;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Loregroup.Provider
{
    public class UserProvider : IUserProvider
    {
        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly IUtilities _utilities;
        //private readonly ISession _session;
        private readonly IConfigSettingProvider _configProvider;
        private readonly ICommonProvider _commonProvider;
        private IContentProvider _contentProvider;
        public static Int64? roleIdvalue = 0;
        public string Classifiedpath;

        public UserProvider(AppContext context, ISecurity security, IUtilities utilities, IConfigSettingProvider configProvider, ICommonProvider commonProvider)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _configProvider = configProvider;
            _commonProvider = commonProvider;
            //_session = session;
        }

        #region Convertors

        public UserDesignationViewModel ToUserDesignationViewModel(UserDesignation userDesignation)
        {
            return new UserDesignationViewModel()
            {
                Id = userDesignation.Id,
                Name = userDesignation.Name,
                CreatedById = userDesignation.CreatedById,
                CreationDate = userDesignation.CreationDate,
                ModifiedById = userDesignation.ModifiedById,
                ModificationDate = userDesignation.ModificationDate,
                StatusId = userDesignation.StatusId
            };
        }

        public SessionUser ToSessionUser(MasterUser user)//AppUser user)
        {
            return new SessionUser()
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                DateJoined = DateTime.Today,

                //Ismall = user.Ismall,
                //Isstore = user.Isstore,
                FirstName = user.FirstName,
                RoleId =user.RoleId,
                Role = (UserRole)user.RoleId
            };
        }

        public MasterUserViewModel ToMasterUserViewModel(MasterUser user)
        {

            return new MasterUserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = (Gender)user.Gender,
                Email = user.Email,
                UserName = user.UserName,
                Password = user.Password,
                Salt = user.Salt,
                Mobile = user.Mobile,
                //Fax=user.Fax,             
                CityId = user.CityId,
                RoleId = user.RoleId,
                StatusId = user.StatusId,
                CreatedById = user.CreatedById,
                //  CreationDate=user.CreationDate,
                ModificationDate = user.ModificationDate,
                ModifiedById = user.ModifiedById,
            };

        }

        #endregion

        #region Mandatory Checking App
        public SessionUser GetDomainUser(long id)
        {
            MasterUser user = _context.MasterUsers.FirstOrDefault(x => x.Id == id);
            //AppUser user = _context.AppUsers.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                return ToSessionUser(user);
            }
            throw new UserDoesNotExistsException();
        }




        public Int64 CheckLoginCredentials(string username, string password)
        {
            Int64 userId = 0;
            // MasterUserViewModel model = new MasterUserViewModel();
            var user = _context.MasterUsers.FirstOrDefault(x => x.UserName == username && x.StatusId == 1);

            if (user != null)
            {


                string hash = _security.ComputeHash(password, user.Salt);

                if (hash == user.Password && user.StatusId == (int)Status.Active)
                {
                    userId = user.Id;
                }
                else
                {
                    throw new InvalidPasswordException();
                }

            }
            else
            {
                throw new UserDoesNotExistsException();
            }
            return userId;
        }
        // get checkingapp master user 

        public MasterUserViewModel GetMaster_User(Int64 id)
        {
            MasterUser user = _context.MasterUsers.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                return ToMasterUserViewModel(user);
            }
            else
            {
                throw new UserDoesNotExistsException();
            }
        }


        #endregion

        public bool ChangePassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            //var user = _context.AppUsers.FirstOrDefault(x => x.Id == forgetPasswordViewModel.Id);
            var user = _context.MasterUsers.FirstOrDefault(x => x.Id == forgetPasswordViewModel.Id);
            String salt = Guid.NewGuid().ToString().Replace("-", "");
            string password = _security.ComputeHash(forgetPasswordViewModel.NewPassword, salt);
            if (user != null)
            {
                user.Password = password;
                user.Salt = salt;
                user.ModifiedById = forgetPasswordViewModel.Id;
                user.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }
            else
            {
                throw new UserDoesNotExistsException();
            }
            return true;
        }

        public UserDesignationViewModels GetAllUserDesignations(Status? status, int page = 0, int records = 0)
        {
            var userDesignationPredicate = PredicateBuilder.True<UserDesignation>();
            if (status != null)
            {
                userDesignationPredicate.And(x => x.StatusId == (int)status);
            }
            if (page > 0)
            {
                return new UserDesignationViewModels()
                {
                    DesignationViewModels = _context.UserDesignations.Where(userDesignationPredicate).Skip(page * records).Take(records)
                        .ToList()
                        .Select(ToUserDesignationViewModel)
                        .ToList()
                };
            }
            return new UserDesignationViewModels()
            {
                DesignationViewModels = _context.UserDesignations.Where(userDesignationPredicate)
                    .ToList()
                    .Select(ToUserDesignationViewModel)
                    .ToList()
            };
        }

        #region Roles


        public RoleViewModel GetRole(Int64 id)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (role != null)
            {
                return ToRoleViewModel(role);
            }
            else
            {
                throw new RoleDoesNotExistsException();
            }
        }

        public RoleViewModel SaveRole(RoleViewModel model)
        {
            try
            {
                Role role = _context.Roles.FirstOrDefault(x => x.Id == model.Id);
                if (role != null)
                {
                    role.Name = model.Name;
                    role.StatusId = (int)Status.Active;
                    role.ModificationDate = DateTime.UtcNow;
                    role.ModifiedById = model.ModifiedById;
                    _context.SaveChanges();
                    model.ModificationDate = role.ModificationDate;
                }
                else
                {
                    Role newRole = new Role()
                    {
                        Name = model.Name,
                        StatusId = (int)Status.Active,
                        CreatedById = model.CreatedById,
                        ModifiedById = model.ModifiedById
                    };
                    _context.Roles.Add(newRole);
                    _context.SaveChanges();
                    model.ModificationDate = newRole.ModificationDate;
                    model.CreationDate = newRole.CreationDate;
                    model.Id = newRole.Id;
                }
            }
            catch (Exception ex)
            {
                throw new RoleNotSavingException();
            }
            return model;
        }

        public void DeleteRole(int RoleId, int Userid)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.Id == RoleId);
            if (role != null)
            {
                role.StatusId = (int)Status.Deleted;
                role.ModifiedById = Userid;
                role.ModificationDate = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        public List<RoleViewModel> GetAllRoles()
        {
            List<RoleViewModel> roleviewmodel = new List<RoleViewModel>();
            List<Int64> roleids = _context.Roles.Where(x => x.StatusId == 1 && x.Id != 1).Select(m => m.Id).ToList();

            foreach (Int64 roleid in roleids)
            {
                var rolestable = _context.Roles.Where(x => x.Id == roleid).ToList();
                foreach (var r in rolestable)
                {
                    roleviewmodel.Add(new RoleViewModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        StatusId = r.StatusId,
                        Status = (Status)r.StatusId,
                        CreatedById = r.CreatedById,
                        ModifiedById = r.ModifiedById,
                        CreationDate = r.CreationDate,
                        ModificationDate = r.ModificationDate
                    });
                }

            }
            return roleviewmodel;
        }

        #endregion

        #region PermissionsMatrix

        public NavigationViewModel GetNavigationList(Int64 roleid)
        {
            NavigationViewModel model = new NavigationViewModel();
            model.PermissionList = _context.Navigations.Where(c => c.StatusId == (int)Status.Active)
            .Select(x => new { Id = x.Id, ActionUrl = x.ActionUrl, Text = x.Text, StatusId = (int)Status.Active, Order = x.Order, TypeHold = TypeHolding.Nav.ToString() })
            .Concat(_context.SubNavigations.Where(c => c.StatusId == (int)Status.Active)
            .Select(y => new { Id = y.Id, ActionUrl = y.ActionUrl, Text = y.Text, StatusId = (int)Status.Active, Order = y.Order, TypeHold = TypeHolding.SubNav.ToString() }))
            .Select(m => new { Id = m.Id, ActionUrl = m.ActionUrl, Text = m.Text, Order = m.Order, TypeHold = m.TypeHold })
            .OrderBy(m => m.TypeHold)
            .Select(f => new NavigationViewModel
            {
                Id = f.Id,                                          //Name = x.Name,
                ActionUrl = f.ActionUrl,                            //DisplayName = x.DisplayName,
                Text = f.Text,                                      //Description = x.Description,
                TypeHold = f.TypeHold,
                RoleList = _context.Roles.Where(z => z.StatusId == 1 & z.Id == roleid).Select(y => new RoleViewModel
                {
                    CreatedById = y.CreatedById,
                    ModifiedById = y.ModifiedById,
                    CreationDate = y.CreationDate,
                    ModificationDate = y.ModificationDate,
                    Id = y.Id,
                    Name = y.Name,
                    Value = (bool)_context.PermissionMatrixs.FirstOrDefault(s => s.AllNAvigationsId == f.Id && s.TypeHold == f.TypeHold && s.RoleId == y.Id).PermissionStatus ? true : false
                }).ToList()
            })
            .ToList();
            return model;
        }

        public List<WidgetViewModel> GetWidgetList(Int64 roleid)
        {
            List<WidgetViewModel> modeldata = new List<WidgetViewModel>();
            return modeldata;
        }

        public void SavePermissionMatrices(NavigationViewModel model)
        {
            try
            {
                if (model.NavList.Count != 0)
                {
                    for (var counter = 0; counter < model.NavList.Count(); counter++)
                    {
                        long permissionidn = model.NavList[counter].Id;
                        //------------------start check is navigation or is sub navigation
                        String menutype = model.NavList[counter].TypeHold;
                        //------------------end check is navigation or is sub navigation

                        for (var innerCounter = 0; innerCounter < model.NavList[counter].RoleList.Count(); innerCounter++)
                        {
                            long roleidn = model.NavList[counter].RoleList[innerCounter].Id;
                            bool statusvalue = model.NavList[counter].RoleList[innerCounter].Value;

                            try
                            {
                                var data = _context.PermissionMatrixs.FirstOrDefault(d => d.AllNAvigationsId == permissionidn && d.TypeHold == menutype && d.RoleId == roleidn);
                                if (data != null)
                                {
                                    data.RoleId = roleidn;
                                    data.PermissionStatus = statusvalue;
                                    data.StatusId = (int)Status.Active;
                                    data.TypeHold = menutype;
                                    data.AllNAvigationsId = permissionidn;
                                    data.CreatedById = model.NavList[counter].RoleList[innerCounter].CreatedById;
                                    data.ModifiedById = model.NavList[counter].RoleList[innerCounter].ModifiedById;
                                    data.CreationDate = model.NavList[counter].RoleList[innerCounter].CreationDate;
                                    data.ModificationDate = model.NavList[counter].RoleList[innerCounter].ModificationDate;
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    PermissionMatrix permissionmatrix = new PermissionMatrix()
                                    {
                                        RoleId = roleidn,
                                        PermissionStatus = statusvalue,
                                        StatusId = (int)Status.Active,
                                        TypeHold = menutype,
                                        AllNAvigationsId = permissionidn,
                                        CreatedById = model.NavList[counter].RoleList[innerCounter].Id,
                                        ModifiedById = model.NavList[counter].RoleList[innerCounter].Id,
                                        CreationDate = model.NavList[counter].RoleList[innerCounter].CreationDate,
                                        ModificationDate = model.NavList[counter].RoleList[innerCounter].ModificationDate
                                    };
                                    _context.PermissionMatrixs.Add(permissionmatrix);
                                    _context.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                if (model.SubNavList.Count != 0)
                {
                    for (var counter = 0; counter < model.SubNavList.Count(); counter++)
                    {
                        long permissionidn = model.SubNavList[counter].Id;
                        //------------------start check is navigation or is sub navigation
                        String menutype = model.SubNavList[counter].TypeHold;
                        //------------------end check is navigation or is sub navigation

                        for (var innerCounter = 0; innerCounter < model.SubNavList[counter].RoleList.Count(); innerCounter++)
                        {
                            long roleidn = model.SubNavList[counter].RoleList[innerCounter].Id;
                            bool statusvalue = model.SubNavList[counter].RoleList[innerCounter].Value;

                            try
                            {
                                var data = _context.PermissionMatrixs.FirstOrDefault(d => d.AllNAvigationsId == permissionidn && d.TypeHold == menutype && d.RoleId == roleidn);
                                if (data != null)
                                {
                                    data.RoleId = roleidn;
                                    data.PermissionStatus = statusvalue;
                                    data.StatusId = (int)Status.Active;
                                    data.TypeHold = menutype;
                                    data.AllNAvigationsId = permissionidn;
                                    data.CreatedById = model.SubNavList[counter].RoleList[innerCounter].CreatedById;
                                    data.ModifiedById = model.SubNavList[counter].RoleList[innerCounter].ModifiedById;
                                    data.CreationDate = model.SubNavList[counter].RoleList[innerCounter].CreationDate;
                                    data.ModificationDate = model.SubNavList[counter].RoleList[innerCounter].ModificationDate;
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    PermissionMatrix permissionmatrix = new PermissionMatrix()
                                    {
                                        RoleId = roleidn,
                                        PermissionStatus = statusvalue,
                                        StatusId = (int)Status.Active,
                                        TypeHold = menutype,
                                        AllNAvigationsId = permissionidn,
                                        CreatedById = model.SubNavList[counter].RoleList[innerCounter].Id,
                                        ModifiedById = model.SubNavList[counter].RoleList[innerCounter].Id,
                                        CreationDate = model.SubNavList[counter].RoleList[innerCounter].CreationDate,
                                        ModificationDate = model.SubNavList[counter].RoleList[innerCounter].ModificationDate
                                    };
                                    _context.PermissionMatrixs.Add(permissionmatrix);
                                    _context.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

                //Save Widgets
                //if (model.WidgetList.Count != 0)
                //{
                //    for (var counter = 0; counter < model.WidgetList.Count(); counter++)
                //    {
                //        long permissionidn = model.WidgetList[counter].Id;
                //        //------------------start check is navigation or is sub navigation
                //        String menutype = model.WidgetList[counter].TypeHold;
                //        //------------------end check is navigation or is sub navigation

                //        for (var innerCounter = 0; innerCounter < model.WidgetList[counter].RoleList.Count(); innerCounter++)
                //        {
                //            long roleidn = model.WidgetList[counter].RoleList[innerCounter].Id;
                //            bool statusvalue = model.WidgetList[counter].RoleList[innerCounter].Value;

                //            try
                //            {
                //                var data = _context.PermissionMatrixs.FirstOrDefault(d => d.AllNAvigationsId == permissionidn && d.TypeHold == menutype && d.RoleId == roleidn);
                //                if (data != null)
                //                {
                //                    data.RoleId = roleidn;
                //                    data.PermissionStatus = statusvalue;
                //                    data.StatusId = (int)Status.Active;
                //                    data.TypeHold = menutype;
                //                    data.AllNAvigationsId = permissionidn;
                //                    data.CreatedById = model.WidgetList[counter].RoleList[innerCounter].CreatedById;
                //                    data.ModifiedById = model.WidgetList[counter].RoleList[innerCounter].ModifiedById;
                //                    data.CreationDate = model.WidgetList[counter].RoleList[innerCounter].CreationDate;
                //                    data.ModificationDate = model.WidgetList[counter].RoleList[innerCounter].ModificationDate;
                //                    _context.SaveChanges();
                //                }
                //                else
                //                {
                //                    PermissionMatrix permissionmatrix = new PermissionMatrix()
                //                    {
                //                        RoleId = roleidn,
                //                        PermissionStatus = statusvalue,
                //                        StatusId = (int)Status.Active,
                //                        TypeHold = menutype,
                //                        AllNAvigationsId = permissionidn,
                //                        CreatedById = model.WidgetList[counter].RoleList[innerCounter].Id,
                //                        ModifiedById = model.WidgetList[counter].RoleList[innerCounter].Id,
                //                        CreationDate = model.WidgetList[counter].RoleList[innerCounter].CreationDate,
                //                        ModificationDate = model.WidgetList[counter].RoleList[innerCounter].ModificationDate
                //                    };
                //                    _context.PermissionMatrixs.Add(permissionmatrix);
                //                    _context.SaveChanges();
                //                }
                //            }
                //            catch (Exception ex)
                //            {

                //            }
                //        }
                //    }
                //}
                //////////////////////////////



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PermissionMatrixViewModel GetPermissionList()
        {
            PermissionMatrixViewModel model = new PermissionMatrixViewModel();
            model.PermissionList = _context.Permissions.Select(x => new PermissionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                RoleList = _context.Roles.Select(y => new RoleViewModel
                {
                    Id = y.Id,
                    Name = y.Name,
                    Value = false
                }).ToList()
            }).ToList();

            return model;
        }

        public JsonBoolModel SaveRolePermission(PermissionMatrixViewModel model)
        {
            JsonBoolModel result = new JsonBoolModel();
            if (model.PermissionList != null)
            {
                int i = 0;
                foreach (var dept in model.PermissionList)
                {
                    foreach (var section in dept.RoleList)
                    {
                        if (section.Value)
                        {
                            //  rp.PermissionId = model.PermissionList[i].Id;
                            //   rp.RoleId = section.Id;
                            //   rp.StatusId = (int)Status.Active;
                            //   _context.RolePermissions.Add(rp);
                            //   _context.SaveChanges();
                        }
                    }
                    i++;
                }
            }
            return result;
        }

        #endregion

        public List<RoleViewModel> GetAllRoles(Status? status, int page = 0, int records = 0)
        {
            var rolePredicate = PredicateBuilder.True<Role>();
            if (status != null)
            {
                rolePredicate.And(x => x.StatusId == (int)status);
            }
            if (page > 0)
            {
                return _context.Roles.Where(rolePredicate).Skip(page * records).Take(records)
                   .ToList()
                   .Select(ToRoleViewModel)
                   .ToList();
            }
            return _context.Roles.Where(y => y.StatusId == (int)Status.Active)
                   .ToList()
                   .Select(ToRoleViewModel)
                   .ToList();
        }

        public RoleViewModel ToRoleViewModel(Role role)
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

        public NavigationViewModel ToNavigationViewModel(Navigation navigation)
        {
            return new NavigationViewModel()
            {
                Id = navigation.Id,
                ActionUrl = navigation.ActionUrl,
                ActionUrlRequestType = (HttpRequestType)navigation.ActionUrlRequestType,
                Icon = navigation.Icon,
                Text = navigation.Text,
                HasSubMenu = navigation.HasSubMenu,
                SubNavigations =
                    _context.SubNavigations.Join(_context.PermissionMatrixs.Where(c => c.RoleId == roleIdvalue && c.TypeHold == "SubNav" && c.PermissionStatus == true), s => s.Id, d => d.AllNAvigationsId, (s, d) => s)
                        .Where(x => x.NavigationId == navigation.Id && x.StatusId == (int)Status.Active)
                        .ToList()
                        .OrderBy(x => x.Order)
                        .Select(ToSubNavigationViewModel)
                        .ToList()
            };
        }

        public SubNavigationViewModel ToSubNavigationViewModel(SubNavigation subNavigation)
        {
            return new SubNavigationViewModel()
            {
                Id = subNavigation.Id,
                ActionUrl = subNavigation.ActionUrl,
                ActionUrlRequestType = (HttpRequestType)subNavigation.ActionUrlRequestType,
                Icon = subNavigation.Icon,
                Text = subNavigation.Text
            };
        }

        public NavigationsViewModel GetNavigations(Int64? roleid)
        {
            try
            {
                roleIdvalue = roleid;
                return new NavigationsViewModel()
                {
                    Navigations =
                        _context.Navigations.Join(_context.PermissionMatrixs.Where(x => x.RoleId == roleid && x.TypeHold == "Nav" && x.PermissionStatus == true), s => s.Id, d => d.AllNAvigationsId, (s, d) => s)
                        .OrderBy(y => y.Order)
                        .ToList()
                        .Select(ToNavigationViewModel).ToList()
                };
            }
            catch (Exception)
            {
                return new NavigationsViewModel();
            }
        }

        public List<NotificationsViewModel> GetUsernotification(long userid, Status? status, int page = 0, int records = 0)
        {
            var MallPredicate = PredicateBuilder.True<Notification>();
            if (status != null)
            {
                MallPredicate.And(x => x.StatusId == (int)status);
            }

            return _context.Notifications.OrderBy(k => k.StatusId).Where(x => x.ReceiverId == userid && x.PackageId == 3 && x.StatusId != 4)
                  .ToList()
                  .Select(ToNotificationViewModel)
                  .ToList();

            throw new NotImplementedException();
        }

        public NotificationsViewModel ToNotificationViewModel(Notification Notifica)
        {
            NotificationsViewModel abc = new NotificationsViewModel();
            abc.notid = Notifica.Id;
            //abc.Name = Notifica.Name;
            abc.Description = Notifica.Description;
            //abc.NotificationMessage = Notifica.NotificationMessage;
            abc.NotificationType = Notifica.NotificationType;
            abc.StatusId = Notifica.StatusId;
            abc.Date = Notifica.CreationDate.ToString("dd/MM/yyyy");
            return abc;
        }

        public AppUserViewModel ToLeadViewModel(AppUser Auser)
        {
            return new AppUserViewModel()
            {
                Id = Auser.Id,
                FullName = Auser.FullName,
                Email = Auser.Email,
                IsLocked = Auser.IsLocked

            };
        }

        #region Users Alpha-Lore Group

        public int IsEmailExists(string emailid)
        {
            try
            {
                var record = _context.MasterUsers.FirstOrDefault(x => x.Email == emailid && x.StatusId != 4);
                if (record != null)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string SaveUser(CustomerViewModel model)
        {
            //var Result = "";
            try
            {
                string countryName = _context.Countries.FirstOrDefault(x => x.Id == model.CountryId).CountryName;

                if (model.CountryId == 2)
                {
                    model.StateName = _context.States.FirstOrDefault(w => w.Id == model.StateId).Statename;
                }
                else
                {
                    model.StateId = 0;
                }
                if (model.AgentsId == null||model.AgentsId==0)
                {
                    model.AgentsId = 0;
                }
                 else
                {
                 var aaa = _context.Agents.Where(x => x.Id == model.AgentsId && x.StatusId != 4).Select(y => new AgentViewModel{AgentId = y.AgentsId }).FirstOrDefault();
                    model.AgentsId = Convert.ToInt64(aaa.AgentId); 
                }
             
                //Get Lat Long Values
                try
                {
                    string address = "";
                    if (!String.IsNullOrEmpty(model.AddressLine1))
                        address += model.AddressLine1;
                    if (!String.IsNullOrEmpty(model.AddressLine2))
                        address += " , " + model.AddressLine2;
                    if (!String.IsNullOrEmpty(model.City))
                        address += " , " + model.City;
                    if (!String.IsNullOrEmpty(model.StateName))
                        address += " , " + model.StateName;
                    if (!String.IsNullOrEmpty(countryName))
                        address += " , " + countryName;

                    string url = "https://maps.google.com/maps/api/geocode/xml?address=" + address + "&sensor=false&key=AIzaSyDnLyynMxlsJHqXcRFXxn1D_7-vj4jnQmw";
                    WebRequest request = WebRequest.Create(url);

                    using (WebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            DataSet dsResult = new DataSet();
                            dsResult.ReadXml(reader);
                            //    DataTable dtCoordinates = new DataTable();
                            //    dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                            //new DataColumn("Address", typeof(string)),
                            //new DataColumn("Latitude",typeof(string)),
                            //new DataColumn("Longitude",typeof(string)) });
                            foreach (DataRow row in dsResult.Tables["result"].Rows)
                            {
                                string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                                DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                model.Latitude = location[0].ToString();
                                model.Longitude = location[1].ToString();
                                //dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                            }
                            //var Data = dtCoordinates;
                        }
                    }

                }
                catch (Exception ex)
                {
                    model.Latitude = "0";
                    model.Longitude = "0";
                }

                String salt = Guid.NewGuid().ToString().Replace("-", "");
                string password = _security.ComputeHash(model.Password, salt);
                 
                var chk = _context.MasterUsers.FirstOrDefault(x => x.Id == model.Id);
                if (chk == null)
                {
                    var ShopName = _context.MasterUsers.Where(x => x.ShopName == model.ShopName && x.StatusId != 4).FirstOrDefault();
                    if (ShopName != null)
                    {
                        return "Shop Name Aleardy Exist";
                    }
                    var tax = false;
                    if (model.CountryId == 3)
                    {
                        tax = true;
                    }
                    MasterUser user = new MasterUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.EmailId,
                        Password = password,
                        Keyword = model.Password,
                        RoleId = model.RoleId,
                        ShopName = model.ShopName,
                        CompanyName = model.CompanyName,
                        CompanyTaxId = model.CompanyTaxId,
                        CountryId = model.CountryId,
                        StateId = model.StateId,
                        CityId = 0,// model.TownId,
                        ZipCode = model.ZipCode,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        City = model.City,
                        StateName = model.StateName,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Mobile = model.MobileNo,
                        TelephoneNo = model.TelephoneNo,
                        TaxEnabled = tax,
                        Email = model.EmailId,
                        Fax = model.Fax,
                        Salt = salt,
                        CurrencyId = (int)model.Currency,
                        DistributionPointId = (int)model.DistributionPoint,
                        TaxId = model.TaxId,
                        CreatedById = model.CreatedById,
                        CreationDate = DateTime.UtcNow,
                        ShowOnMap = model.ShowOnMap,
                        ShopWebsite=model.WebSite,
                      territoryId = model.AgentsId
                    };
                    _context.MasterUsers.Add(user);
                }
                else
                {
                    var ShopName = _context.MasterUsers.Where(x => x.ShopName == model.ShopName && x.Id != model.Id && x.StatusId != 4).FirstOrDefault();
                    if (ShopName != null)
                    {
                        return "Shop Name Aleardy Exist";
                    }

                    var tax = false;
                    if (model.CountryId == 3)
                    {
                        tax = true;
                    }

                    chk.FirstName = model.FirstName;
                    chk.LastName = model.LastName;
                    chk.RoleId = model.RoleId;
                    chk.ShopName = model.ShopName;
                    chk.CompanyName = model.CompanyName;
                    chk.CompanyTaxId = model.CompanyTaxId;
                    chk.CountryId = model.CountryId;
                    chk.StateId = model.StateId;
                    chk.CityId = 0;//model.TownId;
                    chk.ZipCode = model.ZipCode;
                    chk.Longitude = model.Longitude;
                    chk.Latitude = model.Latitude;
                    chk.AddressLine1 = model.AddressLine1;
                    chk.AddressLine2 = model.AddressLine2;
                    chk.StateName = model.StateName;
                    chk.City = model.City;
                    chk.Mobile = model.MobileNo;
                    chk.TelephoneNo = model.TelephoneNo;
                    chk.Email = model.EmailId;
                    chk.CurrencyId = (int)model.Currency;
                    chk.DistributionPointId = (int)model.DistributionPoint;
                    chk.TaxId = model.TaxId;
                    chk.TaxEnabled = tax;
                    chk.ModificationDate = DateTime.UtcNow;
                    chk.ModifiedById = model.ModifiedById;// _session.CurrentUser.Id;
                    chk.ShowOnMap = model.ShowOnMap;
                    chk.ShopWebsite = model.WebSite;
                 chk.territoryId = model.AgentsId;
                }
                _context.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public List<CustomerViewModel> GetAllUsersForStoreLocator()
        {
            List<CustomerViewModel> newData = new List<CustomerViewModel>();

            try
            {
                List<CustomerViewModel> result = new List<CustomerViewModel>();

                result = _context.MasterUsers.Where(x => x.RoleId == 3 && x.StatusId != 4 && x.ShowOnMap == true)
                   .ToList().Select(ToCustomerViewModel).ToList();
                newData = result;
                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public List<CustomerViewModel> GetAllUsersForStoreLocator(Int64 countryid, Int64 stateid, string zipcd)
        {
            List<CustomerViewModel> newData = new List<CustomerViewModel>();

            try
            {
                List<CustomerViewModel> result = new List<CustomerViewModel>();

                IQueryable<MasterUser> query = _context.MasterUsers.Where(x => x.RoleId == 3 && x.StatusId != 4 && x.ShowOnMap == true);

                //result = _context.MasterUsers.Where(x => x.RoleId != 1 && x.StatusId != 4)
                //   .ToList().Select(ToCustomerViewModel).ToList();

                if (countryid > 0)
                {
                    query = query.Where(x => x.CountryId == countryid);
                }

                if (stateid > 0)
                {
                    query = query.Where(x => x.StateId == stateid);
                }

                if(zipcd!="" && zipcd!=null)
                {
                    query = query.Where(x => x.ZipCode == zipcd);
                }
                result = query.ToList().Select(ToCustomerViewModel).ToList();
                newData = result;
                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }


        public List<CustomerViewModel> GetAllUsers(int start, int length, string search, int filtercount, Int64 CurrentUserId)
        {
            List<CustomerViewModel> newData = new List<CustomerViewModel>();
            Int64 roleId = _context.MasterUsers.FirstOrDefault(x => x.Id == CurrentUserId).RoleId;
            try
            {
                List<CustomerViewModel> result = new List<CustomerViewModel>();

                if (roleId == 1)
                {
                    if (search == String.Empty)
                    {
                        result = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.RoleId != 1 && x.StatusId != 4 && x.Id != CurrentUserId)
                           .Skip(start).Take(length).ToList().Select(ToCustomerViewModel).ToList();

                        filtercount = result.Count;
                        newData = result;
                    }
                    if (search != String.Empty)
                    {
                        var value = search.Trim();
                        result = _context.MasterUsers.Where(p => p.UserName.Contains(value))
                            .OrderByDescending(x => x.Id).ToList().Skip(start).Take(length)
                            .Select(ToCustomerViewModel).ToList();

                        filtercount = result.Count;
                        newData = result;
                    }
                }
                else
                {
                    if (search == String.Empty)
                    {
                        result = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 7 && x.StatusId != 4 && x.Id != CurrentUserId)
                           .Skip(start).Take(length).ToList().Select(ToCustomerViewModel).ToList();

                        filtercount = result.Count;
                        newData = result;
                    }
                    if (search != String.Empty)
                    {
                        var value = search.Trim();
                        result = _context.MasterUsers.Where(x => x.RoleId != 1 && x.RoleId != 2 && x.RoleId != 7 && x.StatusId != 4 && x.Id != CurrentUserId)
                            .Where(p => p.UserName.Contains(value)).OrderByDescending(x => x.Id).Skip(start).Take(length).ToList()
                            .Select(ToCustomerViewModel).ToList();

                        filtercount = result.Count;
                        newData = result;
                    }
                }

                return newData;
            }
            catch (Exception ex)
            {
                return newData;
            }
        }

        public List<CustomerViewModel> GetUserByCountry(Int64 CountryId)
        {

            List<CustomerViewModel> UsersList = new List<CustomerViewModel>();

            UsersList = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.CountryId == CountryId && x.RoleId == 3 && x.StatusId != 4)        //&& x.ShowOnMap == true   //.Where(x => x.CountryId == CountryId && x.RoleId != 1 && x.StatusId != 4)
                      .ToList().Select(ToCustomerViewModel).ToList();
            if (UsersList != null)
            {
                return UsersList;
            }
            else
            {
                return new List<CustomerViewModel>();
            }

        }

        public List<UserLocation> GetAllUsersForMap()
        {
            List<UserLocation> UsersList = new List<UserLocation>();

            UsersList = _context.MasterUsers.OrderByDescending(x => x.Id).Where(x => x.RoleId == 3 && x.StatusId != 4)  //&& x.ShowOnMap == true         //.Where(x => x.RoleId != 1 && x.StatusId != 4)
                      .ToList().Select(ToUserLocation).ToList();
            if (UsersList != null)
            {
                return UsersList;
            }
            else
            {
                return new List<UserLocation>();
            }
        }

        private UserLocation ToUserLocation(MasterUser user)
        {
            try
            {
                int isonmp = 0;
                if (user.ShowOnMap == true)
                    isonmp = 1;
                string addinfo = null;
                if (user.AddressLine1 != null && user.AddressLine1 != "")
                    addinfo =  ", " + user.AddressLine1;
                if (user.AddressLine2 != null && user.AddressLine2 != "")
                    addinfo = addinfo + ", " + user.AddressLine2;
                if (user.Email != null && user.Email != "")
                    addinfo = addinfo + ", " + user.Email;
                if (user.TelephoneNo != null && user.TelephoneNo != "")
                    addinfo = addinfo + ", " + user.TelephoneNo;
                return new UserLocation()
                {
                    UserName = user.FirstName + " " + user.LastName,// + " " + addinfo,
                    ShopName = user.ShopName + " " + addinfo,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude,
                    isonmap= isonmp
                };
            }
            catch (Exception ex)
            {
                return new UserLocation();
            }
        }

        public CustomerViewModel ToCustomerViewModel(MasterUser user)
        {
            try
            {
                if (user.territoryId > 0)
                {

                    var bb = _context.Agents.Where(x => x.AgentsId == user.territoryId && x.StatusId != 4).Select(y => new AgentViewModel { Id = y.Id }).FirstOrDefault();
                    user.territoryId = Convert.ToInt64(bb.Id);

                }

                // string BrandName = _context.Products.Where(x => x.Id == product.Id).Where(x => x.StatusId == 1).Select(x => x.Id).FirstOrDefault();
                var CountryName = _context.Countries.Where(x => x.Id == user.CountryId).Where(x => x.StatusId == 1).Select(x => x.CountryName).FirstOrDefault();
                //var TownName = _context.Cities.Where(x => x.Id == user.CityId).Where(x => x.StatusId == 1).Select(x => x.CityName).FirstOrDefault();
                string stateName = "";
                if (user.StateId > 0)
                    stateName = _context.States.Where(x => x.Id == user.StateId).Where(x => x.StatusId == 1).Select(x => x.Statename).FirstOrDefault();
                var RoleName = _context.Roles.Where(x => x.Id == user.RoleId && x.StatusId != 4).Select(x => x.Name).FirstOrDefault();
                var Currency = (Currency)user.CurrencyId;
                return new CustomerViewModel()
                {
                    //UserId = User.Id,
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Salt = user.Salt,
                    Password = user.Password,
                    Keyword = user.Keyword,
                    RoleId = user.RoleId,
                    ShopName = user.ShopName,
                    CompanyName = user.CompanyName,
                    CompanyTaxId = user.CompanyTaxId,
                    CountryId = user.CountryId,
                    Country = CountryName,
                    Town = user.City,         //TownName,
                    StateId = user.StateId,
                    StateName = (stateName != "") ? stateName : user.StateName,
                    ShippingState = user.StateName,         //State,
                    TownId = user.CityId,
                    ZipCode = user.ZipCode,
                    RoleName = RoleName,
                    AddressLine1 = user.AddressLine1,
                    AddressLine2 = user.AddressLine2,
                    City = user.City,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude,
                    MobileNo = user.Mobile,
                    TelephoneNo = user.TelephoneNo,
                    EmailId = user.Email,
                    Currency = (Currency)user.CurrencyId,
                    CurrencyId = user.CurrencyId,
                    CustomerFullName = user.FirstName + " " + user.LastName,
                    CurrencyName = Currency.ToString(),
                    DistributionPointId = user.DistributionPointId,
                    DistributionPoint = (DistributionPoint)user.DistributionPointId,
                    Tax = user.TaxEnabled,
                    TaxId = user.TaxId,
                    CreatedById = Convert.ToInt64(user.CreatedById),
                    CreationDate = Convert.ToDateTime(user.CreationDate),
                    ModifiedById = Convert.ToInt64(user.ModifiedById),
                    ModificationDate = Convert.ToDateTime(user.ModificationDate),
                    StatusId = Convert.ToInt16(user.StatusId),
                    StatusString = GetStatus(user.StatusId),
                    ShowOnMap = user.ShowOnMap,
                    WebSite=user.ShopWebsite,
                     AgentsId=user.territoryId,
                    Edit = "<a href='/User/CreateUser?id=" + user.Id + "' title='Edit'>Edit</a>",
                    Delete = "<a href='/User/DeleteUser?id=" + user.Id + "' title='Delete' onclick='return Confirmation();' >Delete</a>"
                };
            }
            catch (Exception ex)
            {
                return new CustomerViewModel();
            }
        }

        public string GetStatus(int id)
        {
            if (id == 1)
            {
                return "<span class='label label-success'>" + ((Status)id).ToString() + "</span>";
            }
            else if (id == 2)
            {
                return "<span class='label label-info'>" + ((Status)id).ToString() + "</span>";
            }
            else if (id == 3)
            {
                return "<span class='label label-warning'>" + ((Status)id).ToString() + "</span>";
            }
            else if (id == 4)
            {
                return "<span class='label label-danger'>" + ((Status)id).ToString() + "</span>";
            }
            else
            {
                return ((Status)id).ToString();
            }
            //return ((Status)id).ToString();
        }

        public List<CustomerViewModel> AllUserList()
        {
            var result = _context.MasterUsers.Where(x => x.StatusId != 4)
                .Select(ToCustomerViewModel).ToList();
            return result;
        }

        public CustomerViewModel GetUser(Int64 id)
        
        {
            UserViewModel editcategory = new UserViewModel();
            MasterUser user = _context.MasterUsers.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                return ToCustomerViewModel(user);
            }
            else
            {
                return null;
            }
        }

        public void DeleteUser(Int64 id)
        {
            MasterUser user = _context.MasterUsers.Where(x => x.Id == id).FirstOrDefault();
            user.StatusId = 4;
            user.ModificationDate = DateTime.UtcNow;
            user.ModifiedById = 1;// _session.CurrentUser.Id;
            _context.SaveChanges();
        }

        #endregion

    }
}