Desertfire.AppUsers = {};
Desertfire.SymbolLibraries = {};
Desertfire.CustomerRegistrations = {};
Desertfire.AgentRegistrations = {};
Desertfire.TreatmentDetails = {};
Desertfire.CustomerRegistrationss = {};
Desertfire.CustomerRegistrationsss = {};
//Desertfire.CustomerPolicyMasters = {};
Desertfire.CustomerPolicyMasterss = {};
Desertfire.Minal = {};
Desertfire.AppUsersNew = {};
Desertfire.TotalPolicy = {};
Desertfire.TotalDoctor = {};
Desertfire.TotalDoctom = {};

Desertfire.Commissionlevel = {};

Desertfire.DoctorRegistrations = {};


Desertfire.AppUsers.datatable_setup = function (parent) {
    Desertfire.AppUsers.datatable = new DataTableWrapper($(parent), 'AppUsersDatatable', false);
    var dt = Desertfire.AppUsers.datatable;
    
    dt.DataSource = '/UserManagement/GetAllAppUsers';
    
    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            return '<a href="#" onclick="Desertfire.AppUsers.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    
    dt.AddColumn('Name', 'FullName');
    dt.AddColumn('Email ID', 'Email');
   
    dt.AddColumn('Phone Number', 'PhoneNumber');
    dt.AddColumn('Locked', 'IsLocked');
    dt.AddColumn('Verified', 'IsVerified');
    
    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/UserManagement/AddEditUser/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    dt.AddActionDeleteButton('Delete', function () {
        var hubs = dt.GetSelectedAttribute('Id');
        DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    //dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {

    //}, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.AppUsers.GetSingleUserTurnover = function(userId) {
   
    Comm.send_json("/UserManagement/GetSingleUserdetail", { id: userId }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

Desertfire.AppUsers.GetMessageTurnover = function (messageId, folderName) {
   
    Comm.send_json("/UserManagement/GetSingleMessage", { id: messageId, folderName: folderName }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

Desertfire.AppUsers.GetMessagesFromFolder = function(folderName) {
    Comm.send_json("/UserManagement/GetAllMessagesFromFolder", { folderName: folderName }, function (data) {
        if (data.Result) {
            $(".table-mailbox").html("");
            for (var i = 0; i < data.List.length; i++) {
                var $tr = $('<tr />');
                $tr.append('<td><input type="checkbox" /></td>');
                $tr.append('<td class="name"><a href="#">' + data.List[i].SenderName + '</a></td>');
                $tr.append('<td class="subject"><a href="#" data-message-id="' + data.List[i].MessageId + '" onclick="Desertfire.AppUsers.GetMessageTurnover(' + data.List[i].MessageId, '\'' + data.List[i].FolderName + '\'' + ')">' + data.List[i].Subject + '</a></td>');
                $tr.append('<td class="time">' + data.List[i].DateTime + '</td>');
                $(".table-mailbox").append($tr);
            }
            //$(".table-mailbox")
        }
    });
}

//m2
Desertfire.AgentRegistrationsNew = {};
Desertfire.AgentRegistrationsNew.datatable_setup = function (parent) {
    Desertfire.AgentRegistrationsNew.datatable = new DataTableWrapper($(parent), 'policyDatatable', false);
    var dt = Desertfire.AgentRegistrationsNew.datatable;
    dt.DataSource = '/Agent/GetAllAgentsForKyc';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Customer Name', 'fullname');
    dt.AddColumn('Customer Code', 'AdvisorCode');
    dt.AddColumn('Nominee', 'nominee');
   
 

    dt.AddColumn('Bank', 'bankName');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('city', 'CityName');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-primary'>For KVC</span>"
            }
            return "";
        }
    });

    dt.AddActionEditButton('Kyc Agent Detail', function () {
        Desertfire.ChangeLocation("/Agent/AgentDetail/" + dt.GetSelectedAttribute('id'));
    });

    dt.AddActionDeleteButton('Approve Agent', function () {
        Desertfire.AgentRegistrationsNew.DisapprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Agent', function () {
        Desertfire.AgentRegistrationsNew.ApprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.AgentRegistrationsNew.DisapprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/ApproveAgentKYC", { id: selectedids }, function (data) {
            Desertfire.AgentRegistrationsNew.datatable.RetrieveData(true);
        });
    }
    Desertfire.AgentRegistrationsNew.ApprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/DisApproveAgentKYC", { id: selectedids }, function (data) {
            Desertfire.AgentRegistrationsNew.datatable.RetrieveData(true);
        });
    }
}


Desertfire.Roles = {};
Desertfire.Roles.datatable_setup = function (parent) {
    Desertfire.Roles.datatable = new DataTableWrapper($(parent), 'rolesDatatable', false);
    var dt = Desertfire.Roles.datatable;

    dt.DataSource = '/Settings/GetAllRoles';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Name', 'Name');

    dt.AddActionNewButton('Add New Role', function () {
        Desertfire.ChangeLocation("/Settings/AddEditRole/0");
    });

    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Settings/AddEditRole/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });

    //dt.AddActionDeleteButton('Delete', function () {
    //    Desertfire.Roles.DeleteRole(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });

    //dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {

    //}, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.Roles.DeleteRole = function (id) {
    var selectedids = id.toString();
    Comm.send_json("/Settings/DeleteRole", { roleid: selectedids }, function (data) {
        Desertfire.Roles.datatable.RetrieveData(true);
    });
}



Desertfire.Permissions = {};
Desertfire.Permissions.MoveToRoleChange = function (id) {
    var selectedPermissions = Desertfire.Permissions.datatable.GetSelected();
    var selectedPermissionIds = [];
    for (var i = 0; i < selectedPermissions.length; i++) {
        selectedPermissionIds.push(selectedPermissions[i].Id);
    }

    var roleToPermisoinSetViewModel = {
        RoleId: id,
        Permissions: selectedPermissionIds
    };

    console.log(roleToPermisoinSetViewModel);
    Comm.send_json("/Settings/AddRoleToPermisoinSet", roleToPermisoinSetViewModel, function () { });
    //console.log(selectedPermissions);
};

Desertfire.Permissions.CalculateRoles = function () {
    var selectedPermissions = Desertfire.Permissions.datatable.GetSelected();
    console.log(selectedPermissions);
}

Desertfire.Permissions.datatable_setup = function (parent) {
    Desertfire.Permissions.datatable = new DataTableWrapper($(parent), 'permissionsDatatable', false);
    var dt = Desertfire.Permissions.datatable;

    dt.DataSource = '/Settings/GetAllPermissions';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Display Name', 'DisplayName');
    dt.AddColumn('Description', 'Description');

    dt.AddActionNewButton('Add New Permission', function () {
        Desertfire.ChangeLocation("/Settings/AddEditPermission/0");
    });

    //var rolesActions = new DataTableActionSelection('Add/Remove from Roles', 'glyphicon glyphicon-folder-open', '', { UseRightMenu: true });
    //rolesActions.IsMultiSelect = false;
    //rolesActions.OnOpen = function () {
    //    Desertfire.Permissions.CalculateRoles();
    //};
    //rolesActions.AddFromElement($('#roles_move_template'));
    //rolesActions.UseRightMenu = false;
    //dt.AddAction(rolesActions);

    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Settings/AddEditPermission/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    dt.AddActionDeleteButton('Delete', function () {
        Desertfire.Permissions.DeletePermissions(dt.GetSelectedAttribute('Id'));

        var hubs = dt.GetSelectedAttribute('Id');
        DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    //dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {

    //}, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.Permissions.DeletePermissions = function (id) {
    var selectedids = id.toString();
    Comm.send_json("/Settings/DeletePermissions", { permissionid: selectedids }, function (data) {
        Desertfire.Permissions.datatable.RetrieveData(true);
    });
}

//All User List
Desertfire.AppUsers.UserData = function (parent) {
    Desertfire.AppUsers.datatable = new DataTableWrapper($(parent), 'AppUsersDatatable', false);
    var dt = Desertfire.AppUsers.datatable;
    dt.DataSource = '/Settings/GetUserDetails';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
   
    //dt.AddColumn('', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {         
    //        return '<a href="#" onclick="Desertfire.AppUsers.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});

    dt.AddColumn('Username', 'UserName');
    dt.AddColumn('Display Name', 'FullName');
    dt.AddColumn('Email', 'Email');
    dt.AddColumn('Role', 'UserRole')
    //dt.AddColumn('Role', 'Role', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "SuperAdmin".fontcolor('Blue');
    //        } else if (data == 3) {
    //            return "SystemOwner".fontcolor('Blue');
    //        }
    //        else if (data == 4) {
    //            return "ITAdministrator".fontcolor('Blue');
    //        }
    //        else if (data == 5) {
    //            return "Author".fontcolor('Blue');
    //        }
    //        else if (data == 6)
    //        {
    //            return "Reviewer".fontcolor('Blue');
    //        }
    //        else if (data == 7)
    //        {
    //            return "EndUser".fontcolor('Blue');
    //        }
    //        return "";
    //    }
    //});
    dt.AddColumn('Contact Number', 'PhoneNumber');
    dt.AddColumn('Status', 'Status', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"
                //return "Active".fontcolor('Blue');
            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
                //  return "In-Active".fontcolor('Red');
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-danger'>DisApproved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-warning'>KYCPending</span>"
            }
            else if (data == 8) {
                return "<span class='label label-primary'>KYCApproved</span>"
            }
            else if (data == 9) {
                return "<span class='label label-danger'>KYCDisApproved</span>"
            }
            return "";
        }
    });
        
    dt.AddActionNewButton('Add New User', function () {
        Desertfire.ChangeLocation("/Settings/AddEditAppUsers/0");
    });

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Settings/AddEditAppUsers/" + dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Settings/EditUser/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });


    dt.AddActionDisApproveButton('DisApprove User', function () {
        Desertfire.AppUsers.DisapproveUser(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });

    dt.AddActionApproveButton('Approve User', function () {
        Desertfire.AppUsers.ApproveUser(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('Delete User', function () {
        Desertfire.AppUsers.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });

    
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    //  dt.AddColumn('', '');
    
    Desertfire.AppUsers.DisapproveUser = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Settings/DisApproveUser", { id: selectedids }, function (data) {
            Desertfire.AppUsers.datatable.RetrieveData(true);
        });
    }
    Desertfire.AppUsers.ApproveUser = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Settings/ApproveUser", { id: selectedids }, function (data) {
            Desertfire.AppUsers.datatable.RetrieveData(true);
        });
    }
    Desertfire.AppUsers.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Settings/DeleteUser", { id: selectedids }, function (data) {
            Desertfire.AppUsers.datatable.RetrieveData(true);
        });
    }

}



//Inactive User List
Desertfire.AppUsers.InactiveUserData = function (parent) {
    Desertfire.AppUsers.Inactiveuserdatatable = new DataTableWrapper($(parent), 'InactiveAppUsersDatatable', false);
    var dt = Desertfire.AppUsers.Inactiveuserdatatable;
    dt.DataSource = '/Settings/GetInactiveUserDetails';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    //dt.AddColumn('', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {
    //        return '<a href="#" onclick="Desertfire.AppUsers.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});

    dt.AddColumn('Username', 'UserName');
    dt.AddColumn('Display Name', 'FullName');
    dt.AddColumn('Email', 'Email');
    dt.AddColumn('Role', 'UserRole')
    //dt.AddColumn('Role', 'Role', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "SuperAdmin".fontcolor('Blue');
    //        } else if (data == 3) {
    //            return "SystemOwner".fontcolor('Blue');
    //        }
    //        else if (data == 4) {
    //            return "ITAdministrator".fontcolor('Blue');
    //        }
    //        else if (data == 5) {
    //            return "Author".fontcolor('Blue');
    //        }
    //        else if (data == 6) {
    //            return "Reviewer".fontcolor('Blue');
    //        }
    //        else if (data == 7) {
    //            return "EndUser".fontcolor('Blue');
    //        }
    //        return "";
    //    }
    //});
    dt.AddColumn('Contact Number', 'PhoneNumber');
    dt.AddColumn('Status', 'Status', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Approved</span>"
                //return "Active".fontcolor('Blue');
            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
                //  return "In-Active".fontcolor('Red');
            }
            return "";
        }
    });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    //  dt.AddColumn('', '');
}

//Symbols List
//Desertfire.SymbolLibraries = {};
Desertfire.SymbolLibraries.SymbolsList = function (parent) {
     //alert(1);
    Desertfire.SymbolLibraries.datatable = new DataTableWrapper($(parent), 'dbsymbolTable', false);
    var dt = Desertfire.SymbolLibraries.datatable;

    dt.DataSource = '/Settings/GetAllSymbolsList';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Meaning', 'SymbolMeaning');
    //dt.AddColumn('Industry', 'IndustryTypeId');
    dt.AddColumn('Industry', 'IndustryName');
    dt.AddColumn('Description', 'Description');

    dt.AddActionNewButton('Add New Symbol', function () {
        Desertfire.ChangeLocation("/Settings/AddEditSymbol/0");
    });

    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Settings/AddEditSymbol/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    dt.AddActionDeleteButton('Delete', function () {
        Desertfire.SymbolLibraries.DeleteSymbol(dt.GetSelectedAttribute('Id'));

        var hubs = dt.GetSelectedAttribute('Id');
        DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    //dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {

    //}, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.SymbolLibraries.DeleteSymbol = function (id) {
    var selectedids = id.toString();
    Comm.send_json("/Settings/DeleteSymbol", { symbolid: selectedids }, function (data) {
        Desertfire.SymbolLibraries.datatable.RetrieveData(true);
    });
}


//QuestionWheel Questions List
Desertfire.QuestionWheels = {};
Desertfire.QuestionWheels.QuestionsList = function (parent) {
    Desertfire.QuestionWheels.datatable = new DataTableWrapper($(parent), 'dbDatatable', false);
    var dt = Desertfire.QuestionWheels.datatable;

    dt.DataSource = '/Customer/GetAllCustomers';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
   
    dt.AddColumn('Customer Name', 'FullName');
    dt.AddColumn('Bank', 'bank');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('city', 'cityName');
    dt.AddColumn('Nominee', 'nominee');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-primary'>For KVC</span>"
            }
            return "";
        }
    });
    dt.AddActionEditButton('Edit Customer', function () {
        Desertfire.ChangeLocation("/Customer/EditCustomer/" + dt.GetSelectedAttribute('id'));
    });

    dt.AddActionNewButton('Add New Customer', function () {
        Desertfire.ChangeLocation("/Customer/AddEditCustomer/0");
    });
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Customer/ExportData/0");
    });

    //dt.AddActionNewButton('Add New Question', function () {
    //    Desertfire.ChangeLocation("/Settings/AddEditQuestion/0");
    //});

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Settings/AddEditQuestion/" + dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    //dt.AddActionDeleteButton('Delete', function () {
    //    Desertfire.QuestionWheels.DeleteQuestion(dt.GetSelectedAttribute('Id'));

    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.QuestionWheels.DeleteQuestion = function (id) {
    var selectedids = id.toString();
    Comm.send_json("/Settings/DeleteQuestion", { questionid: selectedids }, function (data) {
        Desertfire.QuestionWheels.datatable.RetrieveData(true);
    });
}


//Tranning-Videos List
Desertfire.TranningVideos = {};
Desertfire.TranningVideos.TranningVideoList = function (parent) 

{
    Desertfire.TranningVideos.datatable = new DataTableWrapper

($(parent), 'DbTranningVideos', false);
    var dt = Desertfire.TranningVideos.datatable;

    dt.DataSource = '/Settings/GetAllTranningVideos';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    bVisible: true, bSearchable: false, bSortable: false,
    mRender: function () {
        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    }
});

dt.AddColumn('Console', "RoleName");
dt.AddColumn('Title', 'VideoTitle');
dt.AddColumn('Description', 'Description');

dt.AddActionNewButton('Add New Video', function () {
    Desertfire.ChangeLocation("/Settings/AddEditTranningVideos/0");
});

dt.AddActionEditButton('Edit', function () {
    Desertfire.ChangeLocation("/Settings/AddEditTranningVideos/" + dt.GetSelectedAttribute('Id'));
}, { UseRightMenu: true });

dt.AddActionDeleteButton('Delete', function () {
    var hubs = dt.GetSelectedAttribute('Id');
    DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
}, { UseRightMenu: true });

//dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {
//     }, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });
Desertfire.datatable.setup(dt);
dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.TranningVideos.DeleteTranningVideos = function (id) 

{
    var selectedids = id.toString();
    Comm.send_json("/Settings/DeleteTranningVideos", { 

        tranningvideoid: selectedids }, function (data) {
            Desertfire.TranningVideos.datatable.RetrieveData(true);
        });
}

//Template list
Desertfire.Templates = {};
Desertfire.Templates.TemplateList = function (parent)
{
    Desertfire.Templates.datatable = new DataTableWrapper($(parent), 'DbTemplateList', false);
    var dt = Desertfire.Templates.datatable;
    dt.DataSource = '/Settings/GetAllTemplateList';
    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Template Code', 'TemplateCode');
    dt.AddColumn('Title', 'TemplateTitle');
    dt.AddColumn('Description', 'Description');

    dt.AddActionNewButton('Add New Template', function () {
        Desertfire.ChangeLocation("/Settings/AddEditTemplate/0");
    });

    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Settings/AddEditTemplate/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('Delete', function () {
        var hubs = dt.GetSelectedAttribute('Id');
        DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });

    //dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {
    //     }, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.Templates.DeleteTemplate = function (id) {
    var selectedids = id.toString();
    Comm.send_json("/Settings/DeleteTemplate", {
        templateid: selectedids
    }, function (data) {
        Desertfire.Templates.datatable.RetrieveData(true);
    });
}



Desertfire.CustomerRegistrations.UserData = function (parent) {

    Desertfire.CustomerRegistrations.UserData = new DataTableWrapper($(parent), 'AppUsersDatatable', false);
    var dt = Desertfire.CustomerRegistrations.UserData;
    dt.DataSource = '/Agent/GetUserCustomerRegistrations';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.CustomerRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');
    dt.AddColumn('Fathers Name', 'FathersName');
    dt.AddColumn('Phone', 'phone');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('Annual Income', 'annualIncome');
    //dt.AddColumn('Status', 'Status', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "<span class='label label-success'>Active</span>"
    //            //return "Active".fontcolor('Blue');
    //        } else if (data == 2) {
    //            return "<span class='label label-primary'>In-Active</span>"
    //            //  return "In-Active".fontcolor('Red');
    //        }
    //        else if (data == 3) {
    //            return "<span class='label label-warning'>Pending</span>"
    //        }
    //        else if (data == 4) {
    //            return "<span class='label label-danger'>Deleted</span>"
    //        }
    //        return "";
    //    }
    //});

    dt.AddActionDeleteButton('Approve Customer', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Customer', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers1(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.CustomerRegistrations.DeleteCustomers1 = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DisApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
    Desertfire.CustomerRegistrations.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/ApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
    //  dt.AddColumn('', '');
}

Desertfire.CustomerRegistrations.GetSingleUserTurnover = function (id) {
    Comm.send_json("/Agent/GetSingleCustomerRegistrations", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}


Desertfire.AgentRegistrations.InactiveUserData = function (parent) {

    Desertfire.AgentRegistrations.InactiveUserData = new DataTableWrapper($(parent), 'Datatable', false);
    var dt = Desertfire.AgentRegistrations.InactiveUserData;
    dt.DataSource = '/Agent/GetAgentRegistrations';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    dt.AddColumn('Profession', 'Profession');


    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Agent/DeleteAgent/" + dt.GetSelectedAttribute('id'));
    //}, { UseRightMenu: true });

    dt.AddActionDeleteButton('Approve Agent', function () {
        Desertfire.AgentRegistrations.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Agent', function () {
        Desertfire.AgentRegistrations.DeleteCustomers1(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    dt.AddActionCheckButton('Send for KYC', function () {
        Desertfire.AgentRegistrations.SendForKYC(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: false });



    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    //  dt.AddColumn('', '');

    Desertfire.AgentRegistrations.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/ApproveAgent", { id: selectedids }, function (data) {
            Desertfire.AgentRegistrations.InactiveUserData.RetrieveData(true);
        });
    }
    Desertfire.AgentRegistrations.DeleteCustomers1 = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/DisApproveAgent", { id: selectedids }, function (data) {
            Desertfire.AgentRegistrations.InactiveUserData.RetrieveData(true);
        });
    }
    Desertfire.AgentRegistrations.SendForKYC = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/SendAdvisorForKYC", { id: selectedids }, function (data) {
            Desertfire.AgentRegistrations.datatable.RetrieveData(true);
        });
    }
}
Desertfire.AgentRegistrations.GetSingleUserTurnover = function (id) {
    Comm.send_json("/Agent/GetSingleAgentRegistrations", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}


//@ Pending Customers HO

Desertfire.CustomerRegistrations.UserData = function (parent) {

    Desertfire.CustomerRegistrations.UserData = new DataTableWrapper($(parent), 'AppUsersDatatable', false);
    var dt = Desertfire.CustomerRegistrations.UserData;

    dt.DataSource = '/Agent/GetUserCustomerRegistrations';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            return '<a href="#" onclick="Desertfire.CustomerRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');
    dt.AddColumn('Fathers Name', 'FathersName');
    dt.AddColumn('Phone', 'phone');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('Annual Income', 'annualIncome');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"
            }
            else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-success'>KYC Pending</span>"
            }
            else if (data == 8) {
                return "<span class='label label-primary'>KYC Approved</span>"
            }
            else if (data == 9) {
                return "<span class='label label-primary'>KYC DisApproved</span>"
            }
            return "";
        }
    });

    dt.AddActionDeleteButton('Approve Customer', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Customer', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers1(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    //@
    dt.AddActionCheckButton('Send for KYC', function () {
        Desertfire.CustomerRegistrations.SendForKYC(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: false });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.CustomerRegistrations.DeleteCustomers1 = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DisApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }

    Desertfire.CustomerRegistrations.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/ApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }

    Desertfire.CustomerRegistrations.SendForKYC = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/SendCustomerForKYC", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
}


Desertfire.CustomerRegistrations.InactiveUserData = function (parent) {
    Desertfire.CustomerRegistrations.InactiveUserData = new DataTableWrapper($(parent), 'Datatable1', false);
    var dt = Desertfire.CustomerRegistrations.InactiveUserData;
    dt.DataSource = '/Agent/GetAllCusRegAcctoAgentApproved';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    dt.AddColumn('Profession', 'Profession');


    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Agent/DeleteAgent/" + dt.GetSelectedAttribute('id'));
    //}, { UseRightMenu: true });

    dt.AddActionDeleteButton('Approve Agent', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Agent', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers1(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });


    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    //  dt.AddColumn('', '');

    Desertfire.CustomerRegistrations.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/ApproveAgent", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
    Desertfire.CustomerRegistrations.DeleteCustomers1 = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/DisApproveAgent", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
}


Desertfire.CustomerRegistrations.ActiveUserData = function (parent) {

    Desertfire.CustomerRegistrations.ActiveUserData = new DataTableWrapper($(parent), 'Datatable2', false);
    var dt = Desertfire.CustomerRegistrations.ActiveUserData;
    dt.DataSource = '/Agent/GetAllCusRegAcctoAgentDisApproved';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    dt.AddColumn('Profession', 'Profession');


    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Agent/DeleteAgent/" + dt.GetSelectedAttribute('id'));
    //}, { UseRightMenu: true });

    dt.AddActionDeleteButton('Approve Agent', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Agent', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers1(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });


    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    //  dt.AddColumn('', '');

    Desertfire.CustomerRegistrations.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/ApproveAgent", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
    Desertfire.CustomerRegistrations.DeleteCustomers1 = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/DisApproveAgent", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
}


Desertfire.CustomerRegistrations.PendingUserData = function (parent) {
    Desertfire.CustomerRegistrations.PendingUserData = new DataTableWrapper($(parent), 'Datatable3', false);
    var dt = Desertfire.CustomerRegistrations.PendingUserData;
    dt.DataSource = '/Agent/GetAllCusRegAcctoAgentPending';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    dt.AddColumn('Profession', 'Profession');


    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Agent/DeleteAgent/" + dt.GetSelectedAttribute('id'));
    //}, { UseRightMenu: true });

    dt.AddActionDeleteButton('Approve Agent', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Agent', function () {
        Desertfire.CustomerRegistrations.DeleteCustomers1(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    
    Desertfire.CustomerRegistrations.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/ApproveAgent", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
    Desertfire.CustomerRegistrations.DeleteCustomers1 = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Agent/DisApproveAgent", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
        });
    }
}


//23/11/15
Desertfire.CustomerRegistrations.UserData1 = function (parent) {
    Desertfire.CustomerRegistrations.UserData1 = new DataTableWrapper($(parent), 'AppUsersDatatable1', false);
    var dt = Desertfire.CustomerRegistrations.UserData1;
    dt.DataSource = '/Agent/GetAllCustomer';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            return '<a href="#" onclick="Desertfire.CustomerRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last name', 'LastName');
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}


Desertfire.CustomerRegistrations.ApprovedUserData = function (parent) {
    Desertfire.CustomerRegistrations.ApprovedUserData = new DataTableWrapper($(parent), 'AppUsersDatatable2', false);
    var dt = Desertfire.CustomerRegistrations.ApprovedUserData;
    dt.DataSource = '/Agent/GetAllApprovedCustomer';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.CustomerRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddActionNewButton('Add New User', function () {
        Desertfire.ChangeLocation("/Customer/ExportDataForAgent/0");
    });

    //dt.AddColumn('First Name', 'FirstName');
    //dt.AddColumn('Last name', 'LastName');
    dt.AddColumn('Full Name', 'FullName');
    dt.AddColumn('Email Id', 'email');
    dt.AddColumn('Mobile Number', 'mobile');
    dt.AddColumn('Policy Package', 'PackageName');
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}


Desertfire.CustomerRegistrations.DisApprovedUserData = function (parent) {
    Desertfire.CustomerRegistrations.DisApprovedUserData = new DataTableWrapper($(parent), 'AppUsersDatatable3', false);
    var dt = Desertfire.CustomerRegistrations.DisApprovedUserData;
    dt.DataSource = '/Agent/GetAllDisApprovedCustomer';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.CustomerRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    //dt.AddColumn('First Name', 'FirstName');
    //dt.AddColumn('Last name', 'LastName');
    dt.AddColumn('Full Name', 'FullName');
    dt.AddColumn('Email Id', 'email');
    dt.AddColumn('Mobile Number', 'mobile');
    dt.AddColumn('Policy Package', 'PackageName');
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.CustomerRegistrations.PendingNewUserData = function (parent) {  
    Desertfire.CustomerRegistrations.PendingNewUserData = new DataTableWrapper($(parent), 'AppUsersDatatable4', false);
    var dt = Desertfire.CustomerRegistrations.PendingNewUserData;
    dt.DataSource = '/Agent/GetAllPendingCustomer';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    //dt.AddColumn('', 'id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {
    //        return '<a href="#" onclick="Desertfire.CustomerRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});
    dt.AddColumn('Full Name', 'FullName');
    dt.AddColumn('Email Id', 'email');
    dt.AddColumn('Mobile Number', 'mobile');
    dt.AddColumn('Policy Package', 'PackageName');
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}


//Desertfire.CustomerRegistration.datatable_setupId = function (parent) {    
//    Desertfire.CustomerRegistration.datatable = new DataTableWrapper($(parent), 'customerDatatable', false);
//    var dt = Desertfire.CustomerRegistration.datatable;

//    dt.DataSource = '/Customer/GetAllCustomersById/Id';
//    dt.RetrieveData(true);

//    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function () {
//            return '<input type="checkbox" class="select-checkbox checkboxes" />';
//        }
//    });
//    dt.AddColumn('', 'id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function (data) {
//            return '<a href="#" onclick="Desertfire.CustomerRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
//        }
//    });

//    dt.AddColumn('Customer Name', 'FullName');
//    dt.AddColumn('Bank', 'bank');
//    dt.AddColumn('Email', 'email');
//    dt.AddColumn('city', 'cityName');
//    dt.AddColumn('Nominee', 'nominee');
//    dt.AddColumn('Status', 'StatusId', {
//        mRender: function (data) {
//            if (data == 1) {
//                return "<span class='label label-success'>Active</span>"

//            } else if (data == 2) {
//                return "<span class='label label-primary'>In-Active</span>"
//            }
//            else if (data == 3) {
//                return "<span class='label label-warning'>Pending</span>"
//            }
//            else if (data == 4) {
//                return "<span class='label label-danger'>Deleted</span>"
//            }
//            else if (data == 5) {
//                return "<span class='label label-success'>Approved</span>"
//            }
//            else if (data == 6) {
//                return "<span class='label label-primary'>Dis-Approved</span>"
//            }
//            //else if (data == 6) {
//            //    return "<span class='label label-primary'>Dis-Approved</span>"
//            //}
//            return "";
//        }
//    });

//    dt.AddActionEditButton('Add New Policy', function () {
//        Desertfire.ChangeLocation("/Customer/AddNewPolicyExistingCus/" + dt.GetSelectedAttribute('id'));
//    });
    
//    Desertfire.datatable.setup(dt);
//    dt.selectableWindowsStyleWithCheckbox();
//    Desertfire.CustomerRegistration.DeleteCustomers = function (id) {
//        var selectedids = id.toString();
//        Comm.send_json("/Customer/DeleteCustomer", { customerid: selectedids }, function (data) {
//            Desertfire.CustomerRegistration.datatable.RetrieveData(true);
//        });
//    }
//}

//@ Get All Pending Policies For HO
Desertfire.PendingPolicies = {};
Desertfire.PendingPolicies.datatable_setup = function (parent) {
    Desertfire.PendingPolicies.datatable = new DataTableWrapper($(parent), 'policyDatatable', false);
    var dt = Desertfire.PendingPolicies.datatable;
    dt.DataSource = '/Customer/GetAllPendingPolicyForHO';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    //dt.AddColumn('', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {         
    //        return '<a href="#" onclick="Desertfire.AppUsers.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});
    dt.AddColumn('Customer Name', 'Customerfullname');
    dt.AddColumn('Customer Code', 'CustomerCode');
    dt.AddColumn('Package', 'PackageName');
    dt.AddColumn('Nominee', 'nominee');
    dt.AddColumn('Relationship With The Nominee', 'relationshipwithnominee');
    dt.AddColumn('Appointee Name', 'appointeesName');
    dt.AddColumn('Zone', 'Zonefullname');
      
    dt.AddActionDeleteButton('Approve Policy', function () {
        Desertfire.PendingPolicies.DisapprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Policy', function () {
        Desertfire.PendingPolicies.ApprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.PendingPolicies.DisapprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/ApprovePendingPolicy", { id: selectedids }, function (data) {
            Desertfire.PendingPolicies.datatable.RetrieveData(true);
        });
    }
    Desertfire.PendingPolicies.ApprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DisApprovePendingPolicy", { id: selectedids }, function (data) {
            Desertfire.PendingPolicies.datatable.RetrieveData(true);
        });
    }
}


//Try NEW
Desertfire.CustomerRegistrationsNewone = {};
Desertfire.CustomerRegistrationsNewone.datatable_setup = function (parent) {
 
    Desertfire.CustomerRegistrationsNewone.datatable = new DataTableWrapper($(parent), 'policyDatatable', false);
    var dt = Desertfire.CustomerRegistrationsNewone.datatable;
    dt.DataSource = '/Agent/GetUserCustomerRegistrations';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');
    dt.AddColumn('Fathers Name', 'FathersName');
    dt.AddColumn('Phone', 'phone');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('Annual Income', 'annualIncome');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"
            }
            else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-success'>KYC Pending</span>"
            }
            else if (data == 8) {
                return "<span class='label label-primary'>KYC Approved</span>"
            }
            else if (data == 9) {
                return "<span class='label label-primary'>KYC DisApproved</span>"
            }
            return "";
        }
    });

  


    dt.AddActionDeleteButton('Approve Customer', function () {
        Desertfire.CustomerRegistrationsNewone.DisapprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Customer', function () {
        Desertfire.CustomerRegistrationsNewone.ApprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    dt.AddActionCheckButton('Send for KYC', function () {
        Desertfire.CustomerRegistrationsNewone.SendForKYC(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: false });


    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.CustomerRegistrationsNewone.DisapprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/ApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrationsNewone.datatable.RetrieveData(true);
        });
    }
    Desertfire.CustomerRegistrationsNewone.ApprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DisApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrationsNewone.datatable.RetrieveData(true);
        });
    }
    Desertfire.CustomerRegistrationsNewone.SendForKYC = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/SendCustomerForKYC", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrationsNewone.datatable.RetrieveData(true);
        });
    }
}



Desertfire.TreatmentDetails.datatable_setupId = function (parent) {

    Desertfire.TreatmentDetails.datatable = new DataTableWrapper($(parent), 'customerDatatable', false);
    var dt = Desertfire.TreatmentDetails.datatable;

    dt.DataSource = '/Doctor/GetAllDetailByPolicyId/id';
    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });


    dt.AddColumn('Doctor Name', 'DoctorName');
    dt.AddColumn('Diseas Description', 'Diseasescription');
    dt.AddColumn('Treatment Details', 'TreatmentDetails');
    dt.AddColumn('city', 'CityName');
    dt.AddColumn('Zone Name', 'ZoneName');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }

            return "";
        }
    });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

}




Desertfire.CustomerRegistrationss.InactiveUserData1 = function (parent) {
  
    Desertfire.CustomerRegistrationss.InactiveUserData1 = new DataTableWrapper($(parent), 'Datatable', false);
    var dt = Desertfire.CustomerRegistrationss.InactiveUserData1;
    dt.DataSource = '/Customer/GetTotalcustomer';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Customer/ExportTotalcustomerformanager/0");
    });


    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-primary'>For KVC</span>"
            }
            return "";
        }
    });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}
Desertfire.CustomerRegistrationss.GetSingleUserTurnover = function (id) {
    Comm.send_json("/Agent/GetSingleAgentRegistrations", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

//18-01-2016

Desertfire.CustomerRegistrationsss.InactiveUserData1 = function (parent) {
  
    Desertfire.CustomerRegistrationsss.InactiveUserData1 = new DataTableWrapper($(parent), 'Datatable', false);
    var dt = Desertfire.CustomerRegistrationsss.InactiveUserData1;
    dt.DataSource = '/Customer/GetTotalcustomerAccZone';
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Customer/ExportTotalCustomerForRhead/0");
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    //dt.AddColumn('Profession', 'Profession');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }

            return "";
        }
    });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}
Desertfire.CustomerRegistrationsss.GetSingleUserTurnover = function (id) {
    Comm.send_json("/Agent/GetSingleAgentRegistrations", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}


//Desertfire.AgentRegistrationew.InactiveUserData1 = function (parent) {
    
//    Desertfire.AgentRegistrationew.InactiveUserData1 = new DataTableWrapper($(parent), 'Datatable', false);
//    var dt = Desertfire.AgentRegistrationew.InactiveUserData1;
//    dt.DataSource = '/Agent/GetTotalAgentAccZone';
//    dt.RetrieveData(true);
//    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function () {
//            return '<input type="checkbox" class="select-checkbox checkboxes" />';
//        }
//    });

//    dt.AddColumn('', 'id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function (data) {

//            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
//        }
//    });
//    dt.AddColumn('#', 'FirstName');
//    dt.AddColumn('Name', 'LastName');

//    dt.AddColumn('Contact Person', 'nominee');

//    dt.AddColumn('Subscription End Date', 'mobile');
//    dt.AddColumn('Subscription Type', 'Profession');
//    dt.AddColumn('Deals', 'CustomerCode');
//    Desertfire.datatable.setup(dt);
//    dt.selectableWindowsStyleWithCheckbox();
//}




//Desertfire.AgentRegistrationew.GetSingleUserTurnover = function (id) {
//    Comm.send_json("/Agent/GetSingleAgentRegistrations", { id: id }, function (data) {
//        if (data.Result) {
//            var turnoverData = {
//                Content: data.Object,
//                Selctor: ''
//            };
//            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
//        }
//    });
//}


//19/01/16
Desertfire.CustomerPolicyMasterss.InactiveUserData1 = function (parent) {
 
    Desertfire.CustomerPolicyMasterss.InactiveUserData1 = new DataTableWrapper($(parent), 'Datatable', false);
    var dt = Desertfire.CustomerPolicyMasterss.InactiveUserData1;
    dt.DataSource = '/Customer/GetTotalPolicyAccZone';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {

            return '<a href="#" onclick="Desertfire.AgentRegistrations.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    dt.AddColumn('Profession', 'Profession');
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}
Desertfire.CustomerPolicyMasterss.GetSingleUserTurnover = function (id) {
    Comm.send_json("/Agent/GetSingleAgentRegistrations", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}



Desertfire.AppUsersNew.UserData = function (parent) {
    Desertfire.AppUsersNew.datatable = new DataTableWrapper($(parent), 'AppUsersDatatable', false);
    var dt = Desertfire.AppUsersNew.datatable;
    dt.DataSource = '/Settings/GetUserDetails';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('Username', 'UserName');
    dt.AddColumn('Display Name', 'FullName');
    dt.AddColumn('Email', 'Email');
    dt.AddColumn('Role', 'UserRole')
 
    dt.AddColumn('Contact Number', 'PhoneNumber');
    dt.AddColumn('Status', 'Status', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"
              
            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
                //  return "In-Active".fontcolor('Red');
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-danger'>DisApproved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-warning'>KYCPending</span>"
            }
            else if (data == 8) {
                return "<span class='label label-primary'>KYCApproved</span>"
            }
            else if (data == 9) {
                return "<span class='label label-danger'>KYCDisApproved</span>"
            }
            return "";
        }
    });

    dt.AddActionNewButton('Add New User', function () {
        Desertfire.ChangeLocation("/Settings/AddEditAppUsers/0");
    });

   
   
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
       
}



//19
Desertfire.TotalPolicy.UserData = function (parent) {
    Desertfire.TotalPolicy.datatable = new DataTableWrapper($(parent), 'AppUsersDatatable', false);
    var dt = Desertfire.TotalPolicy.datatable;
    dt.DataSource = '/Customer/GetTotalPolicyAccCity';
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    dt.AddColumn('Package Name', 'PackageName');
    dt.AddColumn('Customer Name', 'Customerfullname');
    dt.AddColumn('Customer Code', 'CustomerCode');

    dt.AddColumn('Start Date', 'Strtdate');
    dt.AddColumn('Expiry Date', 'Endate');
    dt.AddColumn('Status', 'Status', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-primary'>For KVC</span>"
            }
            return "";
        }
    });
    //dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {
      
    //}, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });





    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();



}


Desertfire.TotalDoctor.UserData = function (parent) {
    Desertfire.TotalDoctor.datatable = new DataTableWrapper($(parent), 'AppUsersDatatable', false);
    var dt = Desertfire.TotalDoctor.datatable;
    dt.DataSource = '/Doctor/GetTotalDoctorAccZone';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('Doctor Name', 'FirstName');
    dt.AddColumn('Hospital Name', 'HospitalName');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('Mobile no', 'Mobileno');
    dt.AddColumn('Specialization', 'specialization');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-primary'>For KVC</span>"
            }
            return "";
        }
    });
    //dt.AddActionNewButton('Add New Doctor', function () {
    //    Desertfire.ChangeLocation("/Doctor/AddEditDoctor/0");
    //});
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Doctor/ExportShowDoctorReginalHead/0");
    });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.Commissionlevel.datatable_setup = function (parent) {
    Desertfire.Commissionlevel.datatable = new DataTableWrapper($(parent), 'commissionlevelDatatable', false);
    var dt = Desertfire.Commissionlevel.datatable;

    dt.DataSource = '/Master/GetAllCommissionLevel';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Commission Percentage', 'CommissionPercent');
  
    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Master/AddEditCommissionLevel/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });

 

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.Commissionlevel.DeleteCommissionlevel = function (id) {
    var selectedids = id.toString();
    Comm.send_json("/Master/DeleteCommissionlevel", { commissionid: selectedids }, function (data) {
        Desertfire.Commissionlevel.datatable.RetrieveData(true);
    });
}


//Add for doctor
Desertfire.DoctorRegistrationsNew = {};
Desertfire.DoctorRegistrationsNew.datatable_setup = function (parent) {
  
    Desertfire.DoctorRegistrationsNew.datatable = new DataTableWrapper($(parent), 'policyDatatable', false);
    var dt = Desertfire.DoctorRegistrationsNew.datatable;
    dt.DataSource = '/Agent/GetUserCustomerRegistrations';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');
    dt.AddColumn('Fathers Name', 'FathersName');
    dt.AddColumn('Phone', 'phone');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('Annual Income', 'annualIncome');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"
            }
            else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Approved</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Dis-Approved</span>"
            }
            else if (data == 7) {
                return "<span class='label label-success'>KYC Pending</span>"
            }
            else if (data == 8) {
                return "<span class='label label-primary'>KYC Approved</span>"
            }
            else if (data == 9) {
                return "<span class='label label-primary'>KYC DisApproved</span>"
            }
            return "";
        }
    });




    dt.AddActionDeleteButton('Approve Customer', function () {
        Desertfire.DoctorRegistrationsNew.DisapprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Customer', function () {
        Desertfire.DoctorRegistrationsNew.ApprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    dt.AddActionCheckButton('Send for KYC', function () {
        Desertfire.DoctorRegistrationsNew.SendForKYC(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: false });


    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.DoctorRegistrationsNew.DisapprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/ApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.DoctorRegistrationsNew.datatable.RetrieveData(true);
        });
    }
    Desertfire.DoctorRegistrationsNew.ApprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DisApproveCustomer", { id: selectedids }, function (data) {
            Desertfire.DoctorRegistrationsNew.datatable.RetrieveData(true);
        });
    }
    Desertfire.DoctorRegistrationsNew.SendForKYC = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/SendCustomerForKYC", { id: selectedids }, function (data) {
            Desertfire.DoctorRegistrationsNew.datatable.RetrieveData(true);
        });
    }
}


Desertfire.Lead = {};
Desertfire.Lead.datatable_setup = function (parent) {
    Desertfire.Lead.datatable = new DataTableWrapper($(parent), 'LeadDisplayDatatable', false);
    var dt = Desertfire.Lead.datatable;
    dt.DataSource = '/Master/GetAllLead';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('LeadName', 'LeadName');
    dt.AddColumn('MobileNo', 'MobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('Email', 'Email');
    //dt.AddColumn('Comment', 'Comment');
    dt.AddColumn('Status', 'StatusId', {
        mRender: function (data) {
            if (data == 1) {
                return "<span class='label label-success'>Active</span>"

            } else if (data == 2) {
                return "<span class='label label-primary'>In-Active</span>"
            }
            else if (data == 3) {
                return "<span class='label label-warning'>Pending</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 5) {
                return "<span class='label label-success'>Verified</span>"
            }
            else if (data == 6) {
                return "<span class='label label-primary'>Unverified</span>"
            }
            return "";
        }
    });

    dt.AddActionNewButton('Add New User', function () {
        Desertfire.ChangeLocation("/Settings/AddEditAppUsers/0");
    });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

}
