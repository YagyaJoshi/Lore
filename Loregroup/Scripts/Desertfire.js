Desertfire.datatable = {};
Desertfire.overlay = {};
Desertfire.Alert = {};
Desertfire.Widget = {};
Desertfire.Company = {};
Desertfire.Question = {};
Desertfire.QuestionMaster = {};
Desertfire.TestRecord = {};
Desertfire.AgentRegistration = {};
Desertfire.Minal = {};
Desertfire.CustomerRegistration = {};
Desertfire.PolicyService = {};
Desertfire.Package = {};
Desertfire.CustomerPolicyMaster = {};
Desertfire.DoctorRegistration = {};
Desertfire.AgentRegistrationss = {};
Desertfire.CustomerPolicyMasters = {};
Desertfire.AgentRegistrationew = {};
Desertfire.AppUsers = {};
Desertfire.GetAllCustomers = {};
Desertfire.State = {};
Desertfire.City = {};
Desertfire.Branch = {};
Desertfire.Store = {};
Desertfire.showMall = {};
Desertfire.User = {};
Desertfire.Abc = {};
Desertfire.LeadUpload = {};
Desertfire.LeadAlloted = {};
Desertfire.District = {};
Desertfire.Company = {};
Desertfire.LeadStatus = {};
Desertfire.Lead = {};
Desertfire.Block = {};


//For Block
Desertfire.Block.datatable_setup = function (parent) {
   
    Desertfire.Block.datatable = new DataTableWrapper($(parent), 'BlockDisplayDataTable', false);
    var dt = Desertfire.Block.datatable;

    dt.DataSource = '/Master/GetAllBlock';// '/Master/GetAllDistrict';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('BlockName', 'BlockName');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });


    dt.AddActionEditButton('Edit Block', function () {
        Desertfire.ChangeLocation("/Master/EditBlock/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add New Block', function () {
        Desertfire.ChangeLocation("/Master/Block/0");
    });
    dt.AddActionDeleteButton('Delete Block', function () {
        Desertfire.Block.DeleteCities(dt.GetSelectedAttribute('Id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Block.DeleteCities = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteBlock", { id: selectedids }, function (data) {
            Desertfire.Block.datatable.RetrieveData(true);
        });
    }
}
//For District
Desertfire.District.datatable_setup = function (parent) {
    
    Desertfire.District.datatable = new DataTableWrapper($(parent), 'CityDisplayDataTable', false);
    var dt = Desertfire.District.datatable;

    dt.DataSource = '/Master/GetAllDistrict';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('DistricName', 'DistrictName');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });


    dt.AddActionEditButton('Edit District', function () {
        Desertfire.ChangeLocation("/Master/EditDistrict/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add New District', function () {
        Desertfire.ChangeLocation("/Master/District/0");
    });
    dt.AddActionDeleteButton('Delete District', function () {
        Desertfire.District.DeleteCities(dt.GetSelectedAttribute('Id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.District.DeleteCities = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteDistrict", { id: selectedids }, function (data) {
            Desertfire.District.datatable.RetrieveData(true);
        });
    }
}
//For Company
Desertfire.Company.datatable_setup = function (parent) {
   
    Desertfire.Company.datatable = new DataTableWrapper($(parent), 'CityDisplayDataTable', false);
    var dt = Desertfire.Company.datatable;

    dt.DataSource = '/Master/GetAllCompany';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('CompanyName', 'CompanyName');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });


    dt.AddActionEditButton('Edit Company', function () {
        Desertfire.ChangeLocation("/Master/EditCompany/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add New Company', function () {
        Desertfire.ChangeLocation("/Master/Company/0");
    });
    dt.AddActionDeleteButton('Delete Company', function () {
        Desertfire.Company.DeleteCities(dt.GetSelectedAttribute('Id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Company.DeleteCities = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteCompany", { id: selectedids }, function (data) {
            Desertfire.Company.datatable.RetrieveData(true);
        });
    }
}
//For State
Desertfire.State.datatable_setup = function (parent) {
   
    Desertfire.State.datatable = new DataTableWrapper($(parent), 'StateDisplayDataTable', false);
    var dt = Desertfire.State.datatable;

    dt.DataSource = '/Master/GetAllState';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('State Name', 'Statename');
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
            return "";
        }
    });

    dt.AddActionEditButton('Edit State', function () {
        Desertfire.ChangeLocation("/Master/EditState/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add New State', function () {
        Desertfire.ChangeLocation("/Master/State/0");
    });

  

    dt.AddActionDeleteButton('Delete State', function () {
        Desertfire.State.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.State.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteState", { id: selectedids }, function (data) {
            Desertfire.State.datatable.RetrieveData(true);
        });
    }

}
//For City
//Desertfire.City.datatable_setup = function (parent) {

//    Desertfire.City.datatable = new DataTableWrapper($(parent), 'CityDisplayDataTable', false);
//    var dt = Desertfire.City.datatable;

//    dt.DataSource = '/Master/GetAllCity';
//    dt.RetrieveData(true);
//    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function () {
//            return '<input type="checkbox" class="select-checkbox checkboxes" />';
//        }
//    });
//    dt.AddColumn('State Name', 'Statename');
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
//            return "";
//        }
//    });

//    dt.AddActionEditButton('Edit State', function () {
//        Desertfire.ChangeLocation("/Master/EditState/" + dt.GetSelectedAttribute('Id'));
//    });
//    dt.AddActionNewButton('Add New State', function () {
//        Desertfire.ChangeLocation("/Master/State/0");
//    });



//    dt.AddActionDeleteButton('Delete State', function () {
//        Desertfire.State.DeleteQuestions(dt.GetSelectedAttribute('Id'));
//        var hubs = dt.GetSelectedAttribute('Id');
//    }, { UseRightMenu: true });

//    Desertfire.datatable.setup(dt);
//    dt.selectableWindowsStyleWithCheckbox();
//    Desertfire.State.DeleteQuestions = function (id) {
//        var selectedids = id.toString();
//        Comm.send_json("/Master/DeleteState", { id: selectedids }, function (data) {
//            Desertfire.State.datatable.RetrieveData(true);
//        });
//    }

//}


Desertfire.City.datatable_setup = function (parent) {
   
    Desertfire.City.datatable = new DataTableWrapper($(parent), 'CityDisplayDataTable', false);
    var dt = Desertfire.City.datatable;

    dt.DataSource = '/Master/GetAllCity';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('City', 'Cityname');
    dt.AddColumn('District', 'DistrictName');
    dt.AddColumn('State', 'StateName');
    dt.AddColumn('Status', 'Education');

    //dt.AddColumn('Status', 'StatusId', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "<span class='label label-success'>Active</span>"
    //            //return "Active".fontcolor('Blue');
    //        } else if (data == 2) {
    //            return "<span class='label label-primary'>In-Active</span>"
    //             //return "In-Active".fontcolor('Red');
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


    dt.AddActionEditButton('Edit City', function () {
        Desertfire.ChangeLocation("/Master/EditCity/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add New City', function () {
        Desertfire.ChangeLocation("/Master/City/0");
    });
    dt.AddActionDeleteButton('Delete City', function () {
        Desertfire.City.DeleteCities(dt.GetSelectedAttribute('Id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.City.DeleteCities = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteCity", { id: selectedids }, function (data) {
            Desertfire.City.datatable.RetrieveData(true);
        });
    }
}



Desertfire.Branch.datatable_setup = function (parent) {
    //For Branch
    Desertfire.Branch.datatable = new DataTableWrapper($(parent), 'CityDisplayDataTable', false);
    var dt = Desertfire.Branch.datatable;

    dt.DataSource = '/Master/GetAllBranch';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Branch', 'BranchName');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });


    dt.AddActionEditButton('Edit Branch', function () {
        Desertfire.ChangeLocation("/Master/EditBranch/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add New Branch', function () {
        Desertfire.ChangeLocation("/Master/Branch/0");
    });
    dt.AddActionDeleteButton('Delete Branch', function () {
        Desertfire.Branch.DeleteBraches(dt.GetSelectedAttribute('Id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Branch.DeleteBraches = function (Id) {
        var selectedids = Id.toString();
        Comm.send_json("/Master/DeleteBranch", { Id: selectedids }, function (data) {
            Desertfire.Branch.datatable.RetrieveData(true);
        });
    }
}
Desertfire.Store.datatable_setup = function (parent) {
    //For Store
    Desertfire.Store.datatable = new DataTableWrapper($(parent), 'StoreDisplayDataTable', false);
    var dt = Desertfire.Store.datatable;

    dt.DataSource = '/Master/GetAllStore';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Store Type', 'StoreName');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });

 
    dt.AddActionEditButton('Edit Store Type', function () {
        Desertfire.ChangeLocation("/Master/EditStore/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add Store Type', function () {
        Desertfire.ChangeLocation("/Master/Store/0");
    });
    dt.AddActionDeleteButton('Delete Store Type', function () {
        Desertfire.Store.DeleteBraches(dt.GetSelectedAttribute('Id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Store.DeleteBraches = function (Id) {
        var selectedids = Id.toString();
        Comm.send_json("/Master/DeleteStore", { Id: selectedids }, function (data) {
            Desertfire.Store.datatable.RetrieveData(true);
        });
    }
}
Desertfire.Package.datatable_setup = function (parent) {

    Desertfire.Package.datatable = new DataTableWrapper($(parent), 'PackageDisplayDataTable', false);
    var dt = Desertfire.Package.datatable;

    dt.DataSource = '/Master/GetAllPackage';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Package Name', 'PackageName');
    dt.AddColumn('Days', 'Days');

    dt.AddColumn('Amount', 'Amount');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });

    dt.AddActionEditButton('Edit Package', function () {
        Desertfire.ChangeLocation("/Master/EditPackage/" + dt.GetSelectedAttribute('id'));
    });
    dt.AddActionNewButton('Add New Package', function () {
        Desertfire.ChangeLocation("/Master/Package/0");
    });
    dt.AddActionDeleteButton('Delete Package', function () {
        Desertfire.Package.DeleteBraches(dt.GetSelectedAttribute('id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Package.DeleteBraches = function (Id) {
        var selectedids = Id.toString();
        Comm.send_json("/Master/DeletePackage", { Id: selectedids }, function (data) {
            Desertfire.Package.datatable.RetrieveData(true);
        });
    }

}
Desertfire.Package.GetSingleQuestionTurnover = function (id) {

    Comm.send_json("/Question/GetSingleQuestiondetail", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}
Desertfire.CustomerRegistration.datatable_setup = function (parent) {
    Desertfire.CustomerRegistration.datatable = new DataTableWrapper($(parent), 'customerDatatable', false);

    var dt = Desertfire.CustomerRegistration.datatable;

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
    //dt.AddActionNewButton('pdf', function () {
    //    Desertfire.ChangeLocation("/Zone/PrintIndex/" + dt.GetSelectedAttribute('id'));
    //});
    dt.AddActionDeleteButton('Delete Customer', function () {
        Desertfire.CustomerRegistration.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    
    dt.AddActionEditButton("Print Detail", function () {
        //window.location = Loregroup + 'Report/RedirectToSalesReports?FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val() + '&RestaurantId=' + $("#RestaurantId").val() + '&OrderType=' + $("#txtOrderType").val() + '&CounterName=' + $("#txtCounterName").val() + '&CashierId=' + $("#txtCashier").val();

        Desertfire.ChangeLocation("/Report/RedirectToSalesReports/" + dt.GetSelectedAttribute('id'));
    });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.CustomerRegistration.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DeleteCustomer", { customerid: selectedids }, function (data) {
            Desertfire.CustomerRegistration.datatable.RetrieveData(true);
        });
    }
}













Desertfire.PolicyService.datatable_setup = function (parent) {

    Desertfire.PolicyService.datatable = new DataTableWrapper($(parent), 'QuestionDisplayDataTable', false);
    var dt = Desertfire.PolicyService.datatable;

    dt.DataSource = '/Master/GetAllPolicy';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('Service Name', 'ServiceName');
    dt.AddColumn('Description', 'description');
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
            return "";
        }
    });
    dt.AddActionEditButton('Edit Policy Services', function () {
        Desertfire.ChangeLocation("/Master/EditPolicyServices/" + dt.GetSelectedAttribute('id'));
    });
    dt.AddActionNewButton('Add New Policy Service', function () {
        Desertfire.ChangeLocation("/Master/PolicyService/");
    });
    dt.AddActionDeleteButton('Select Policy', function () {
        Desertfire.PolicyService.DeleteQuestions(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.PolicyService.DeleteQuestions = function (id) {
        //alert('');
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteAgent", { id: selectedids }, function (data) {
            Desertfire.PolicyService.datatable.RetrieveData(true);

        });
    }

}

Desertfire.PolicyService.GetSingleQuestionTurnover = function (id) {

    Comm.send_json("/Question/GetSingleQuestiondetail", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

//End Script For PolicyServices

//Start Script For PolicyServices show in Package Master
Desertfire.PolicyService.datatable_setup1 = function (parent) {
    //alert('');
    Desertfire.PolicyService.datatable = new DataTableWrapper($(parent), 'QuestionDisplayDataTable', false);
    var dt = Desertfire.PolicyService.datatable;

    dt.DataSource = '/Master/GetAllPolicy';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Service Name', 'ServiceName');
    dt.AddColumn('Description', 'description');
    
    dt.AddActionNewButton('Select Policy', function () {
        Desertfire.PolicyService.DeleteQuestions(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.PolicyService.DeleteQuestions = function (id) {
        //alert('');
        var selectedids = id.toString();
        Comm.send_json("/Master/SelectPolicy", { id: selectedids }, function (data) {
            Desertfire.ChangeLocation(data.Url);
            Desertfire.PolicyService.datatable.RetrieveData(true);            
        });        
    }
}

Desertfire.PolicyService.GetSingleQuestionTurnover = function (id) {

    Comm.send_json("/Question/GetSingleQuestiondetail", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

//End Script For PolicyServices show in Package Master



Desertfire.ReplaceString = "!!";

//--------------------------------------------------------------start license Module

//---------------------------------start license services
Desertfire.LicenseServicesForTrial = {};
Desertfire.LicenseServicesForCommercial = {};
Desertfire.LicenseServicesForEnterprise = {};
Desertfire.LicenseServicesForCorporate = {};

//--------------------------------subscriptions here
Desertfire.LicenseSubscriptionForCommercial = {};
Desertfire.LicenseSubscriptionForEnterprise = {};
Desertfire.LicenseSubscriptionForCorporate = {};

//-------------------------------------------------------------------------------number of AppUsers LicenseRole here

Desertfire.LicenseRoleForTrial = {};
Desertfire.LicenseRoleForCommercial = {};
Desertfire.LicenseRoleForEnterprise = {};
Desertfire.LicenseRoleForCorporate = {};

//--------------------------------Trial number of AppUsers LicenseRole here
Desertfire.LicenseRoleForTrial.datatable_setup = function (parent) {
    Desertfire.LicenseRoleForTrial.datatable = new DataTableWrapper($(parent), 'LicenseRoleForTrialTableDisplayDataTable', false);
    var dt = Desertfire.LicenseRoleForTrial.datatable;

    dt.DataSource = '/Agent/GetTotalAgentAccZone';

    dt.RetrieveData(true);

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'RoleStatus', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {
    //        // return '<input type="checkbox" class="select-checkbox checkboxes" />';

    //        if (data == true) {
    //            //alert(1);
    //            return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
    //        } else if (data == false) {
    //            //alert(2);
    //            return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
    //        }

    //        return "";
    //    }
    //});

    


    dt.AddColumn('First Name', 'FirstName');
    dt.AddColumn('Last Name', 'LastName');

    dt.AddColumn('Nominee', 'nominee');

    dt.AddColumn('Mobile', 'mobile');
    dt.AddColumn('Profession', 'Profession');
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
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Agent/ExportDataForRH/0");
    });
    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    //    dt.AddColumn('License Services Name', 'Name');

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseRoleForTrial.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

//--------------------------------end -Trial number of AppUsers LicenseRole here

//--------------------------------Commercial  number of AppUsers LicenseRole here
Desertfire.LicenseRoleForCommercial.datatable_setup = function (parent) {
  
    Desertfire.LicenseRoleForCommercial.datatable = new DataTableWrapper($(parent), 'LicenseRoleForCommercialTableDisplayDataTable', false);
    var dt = Desertfire.LicenseRoleForCommercial.datatable;

    dt.DataSource = '/Customer/GetTotalPolicyAccZone';

    dt.RetrieveData(true);

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'RoleStatus', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {
    //        // return '<input type="checkbox" class="select-checkbox checkboxes" />';

    //        if (data == true) {
    //            //alert(1);
    //            return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
    //        } else if (data == false) {
    //            //alert(2);
    //            return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
    //        }

    //        return "";
    //    }
    //});

    //dt.AddColumn('User Type', 'Role');
  
    //dt.AddColumn('No. of AppUsers', 'NumeberofUses');
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


    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Settings/SaveUserDescription/" + dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });


    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    //    dt.AddColumn('License Services Name', 'Name');

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseRoleForCommercial.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });

    //dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {
    //}, { UseRightMenu: true, IsDisabled:false, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = false } });


    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

//--------------------------------end -Commercial number of AppUsers LicenseRole here

//--------------------------------Enterprise number of AppUsers LicenseRole here
Desertfire.LicenseRoleForEnterprise.datatable_setup = function (parent) {
    Desertfire.LicenseRoleForEnterprise.datatable = new DataTableWrapper($(parent), 'LicenseRoleForEnterpriseTableDisplayDataTable', false);
    var dt = Desertfire.LicenseRoleForEnterprise.datatable;

    dt.DataSource = '/Doctor/GetTotalDoctorAccCity';

    dt.RetrieveData(true);

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'RoleStatus', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {
    //        // return '<input type="checkbox" class="select-checkbox checkboxes" />';

    //        if (data == true) {
    //            //alert(1);
    //            return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
    //        } else if (data == false) {
    //            //alert(2);
    //            return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
    //        }

    //        return "";
    //    }
    //});

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
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Doctor/ExportTotalDoctorformanager/0");
    });

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Settings/SaveUserDescription/" + dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });


    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    //    dt.AddColumn('License Services Name', 'Name');

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseRoleForEnterprise.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

//--------------------------------end -Enterprise number of AppUsers LicenseRole here

//--------------------------------Corporate number of AppUsers LicenseRole here
Desertfire.LicenseRoleForCorporate.datatable_setup = function (parent) {
    Desertfire.LicenseRoleForCorporate.datatable = new DataTableWrapper($(parent), 'LicenseRoleForCorporateTableDisplayDataTable', false);
    var dt = Desertfire.LicenseRoleForCorporate.datatable;

    dt.DataSource = '/Doctor/GetTotalDoctorAccCityforcustmr';

    dt.RetrieveData(true);
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

   
    //, {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "SuperAdmin".fontcolor('Blue');
    //            //                return "SuperAdmin".fontcolor('Blue');
    //        } else if (data == 3) {
    //            return "SystemOwner";
    //        }
    //        else if (data == 4) {
    //            return "ITAdministrator";
    //        }
    //        else if (data == 5) {
    //            return "Author";
    //        }
    //        else if (data == 6) {
    //            return "Reviewer";
    //        }
    //        else if (data == 7) {
    //            return "EndUser";
    //        }
    //        return "";
    //    }
    //});
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Doctor/ExportShowAllDoctorForCustomer/0");
    });

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Settings/SaveUserDescription/" + dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });


    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    //    dt.AddColumn('License Services Name', 'Name');

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseRoleForCorporate.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

//--------------------------------end -Corporate number of AppUsers LicenseRole here


//------------------------------------------------------------------------------------------------subscriptions here

//--------------------------------Commercial subscriptions here
Desertfire.LicenseSubscriptionForCommercial.datatable_setup = function (parent) {
    Desertfire.LicenseSubscriptionForCommercial.datatable = new DataTableWrapper($(parent), 'LicenseSubscriptionForCommercialTableDisplayDataTable', false);
    var dt = Desertfire.LicenseSubscriptionForCommercial.datatable;

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

    //dt.AddActionDeleteButton('Delete', function () {
    //    Desertfire.Roles.DeleteRole(dt.GetSelectedAttribute('Id'));

    //    var hubs = dt.GetSelectedAttribute('Id');
    //    DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });

    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    //    dt.AddColumn('License Services Name', 'Name');

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseSubscriptionForCommercial.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.LicenseSubscriptionForCommercial.DeleteCommissionlevel = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteCommissionlevel", { commissionid: selectedids }, function (data) {
         Desertfire.LicenseSubscriptionForCommercial.datatable.RetrieveData(true);
        });
    }

}

//--------------------------------Commercial End subscriptions here

//--------------------------------Enterprise subscriptions here
Desertfire.LicenseSubscriptionForEnterprise.datatable_setup = function (parent) {
    Desertfire.LicenseSubscriptionForEnterprise.datatable = new DataTableWrapper($(parent), 'LicenseSubscriptionForEnterpriseTableDisplayDataTable', false);
    var dt = Desertfire.LicenseSubscriptionForEnterprise.datatable;

    dt.DataSource = '/Doctor/GetPendingDoctor';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';

            //if (data == true) {
            //    //alert(1);
            //    return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
            //} else if (data == false) {
            //    //alert(2);
            //    return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
            //}

            //return "";
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

    //dt.AddActionNewButton('Add New Subscription Schemes', function () {
    //    Desertfire.ChangeLocation("/Settings/SaveSubscription/0");
    //});

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Settings/SaveSubscription/" + dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    dt.AddActionDeleteButton('Approve Doctor', function () {
        Desertfire.LicenseSubscriptionForEnterprise.DisapprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Doctor', function () {
        Desertfire.LicenseSubscriptionForEnterprise.ApprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });


    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    //    dt.AddColumn('License Services Name', 'Name');

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseSubscriptionForEnterprise.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.LicenseSubscriptionForEnterprise.DisapprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Doctor/ApproveDoctor", { id: selectedids }, function (data) {
            Desertfire.LicenseSubscriptionForEnterprise.datatable.RetrieveData(true);
        });
    }
    Desertfire.LicenseSubscriptionForEnterprise.ApprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Doctor/DisApproveDoctor", { id: selectedids }, function (data) {
            Desertfire.LicenseSubscriptionForEnterprise.datatable.RetrieveData(true);
        });
    }
}
//--------------------------------Enterprise End subscriptions here

//-------------------------------Corporate  subscriptions here
Desertfire.GetAllCustomers.datatable_setup = function (parent) {
    Desertfire.GetAllCustomers.datatable = new DataTableWrapper($(parent), 'GetAllCustomersDataTable', false);
    var dt = Desertfire.GetAllCustomers.datatable;

    dt.DataSource = '/Customer/GetAllCustomers';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';

            //if (data == true) {
            //    //alert(1);
            //    return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
            //} else if (data == false) {
            //    //alert(2);
            //    return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
            //}

            //return "";
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
    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    //    dt.AddColumn('License Services Name', 'Name');

    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseSubscriptionForCorporate.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

//-------------------------------Corporate  End subscriptions here
//--------------------------------ens subscriptions


//-------------------------for Trial license services
Desertfire.LicenseServicesForTrial.datatable_setup = function (parent) {
    Desertfire.LicenseServicesForTrial.datatable = new DataTableWrapper($(parent), 'LicenseDisplayDataTable', false);
    var dt = Desertfire.LicenseServicesForTrial.datatable;

    dt.DataSource = '/Customer/GetAllCustomersForKyc';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'ServiceStatus', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            // return '<input type="checkbox" class="select-checkbox checkboxes" />';

            if (data == true) {
                //alert(1);
                return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
            } else if (data == false) {
                //alert(2);
                return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
            }

            return "";
        }
    });
    dt.AddColumn('Customer Name', 'FullName');
    dt.AddColumn('Customer Code', 'CustomerCode');
    dt.AddColumn('Nominee', 'nominee');
    dt.AddColumn('Relationship With The Nominee', 'relationshipwithnominee');
    dt.AddColumn('Appointee Name', 'appointeesName');

    dt.AddColumn('Bank', 'bank');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('city', 'cityName');
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

    dt.AddActionEditButton('Kyc Customer Detail', function () {
        Desertfire.ChangeLocation("/Customer/ShowCustomerDetail/" + dt.GetSelectedAttribute('id'));
    });


    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.LicenseServicesForTrial.EditLicenseServiceControll = function (id) {

    var selectedids = id.toString();
    //alert(selectedids);

    Comm.send_json("/Settings/EditLicenseServiceControll", { serviceid: selectedids }, function (data) {
        Desertfire.LicenseServicesForTrial.datatable.RetrieveData(true);
        alert('License Service will display on dashboard');
    });
}

//-------------------------end trial

//-------------------------for Commercial license services
Desertfire.LicenseServicesForCommercial.datatable_setup = function (parent) {
    Desertfire.LicenseServicesForCommercial.datatable = new DataTableWrapper($(parent), 'ForCommercialLicenseDisplayDataTable', false);
    var dt = Desertfire.LicenseServicesForCommercial.datatable;

    dt.DataSource = '/Settings/GetAllServicesForCommercial';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'ServiceStatus', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            // return '<input type="checkbox" class="select-checkbox checkboxes" />';

            if (data == true) {
                //alert(1);
                return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
            } else if (data == false) {
                //alert(2);
                return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
            }

            return "";
        }
    });
    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    dt.AddColumn('License Services Name', 'Name');

    dt.AddActionDeleteButton('Add to Available Services', function () {
        Desertfire.LicenseServicesForCommercial.EditLicenseServiceControll(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });



    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseServicesForCommercial.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}


Desertfire.LicenseServicesForCommercial.EditLicenseServiceControll = function (id) {
    //alert(id);
    var selectedids = id.toString();
    //alert(selectedids);
    Comm.send_json("/Settings/EditLicenseServiceControll", { serviceid: selectedids }, function (data) {
        Desertfire.LicenseServicesForCommercial.datatable.RetrieveData(true);
        alert('License Service will display on dashboard');
    });
}

//-------------------------end Commercial

//--------------------------for Enterprise license services
Desertfire.LicenseServicesForEnterprise.datatable_setup = function (parent) {
    Desertfire.LicenseServicesForEnterprise.datatable = new DataTableWrapper($(parent), 'ForEnterpriseLicenseDisplayDataTable', false);
    var dt = Desertfire.LicenseServicesForEnterprise.datatable;

    dt.DataSource = '/Settings/GetAllServicesForEnterprise';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'ServiceStatus', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            // return '<input type="checkbox" class="select-checkbox checkboxes" />';

            if (data == true) {
                //alert(1);
                return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
            } else if (data == false) {
                //alert(2);
                return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
            }

            return "";
        }
    });
    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    dt.AddColumn('License Services Name', 'Name');


    dt.AddActionDeleteButton('Add to Available Services', function () {
        Desertfire.LicenseServicesForEnterprise.EditLicenseServiceControll(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });



    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseServicesForEnterprise.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}


Desertfire.LicenseServicesForEnterprise.EditLicenseServiceControll = function (id) {
    alert(id);
    var selectedids = id.toString();
    alert(selectedids);
    Comm.send_json("/Settings/EditLicenseServiceControll", { serviceid: selectedids }, function (data) {
        Desertfire.LicenseServicesForEnterprise.datatable.RetrieveData(true);
        alert('License Service will display on dashboard');
    });
}

//--------------------------end Enterprise

//-------------------------for Corporate license services
Desertfire.LicenseServicesForCorporate.datatable_setup = function (parent) {
    Desertfire.LicenseServicesForCorporate.datatable = new DataTableWrapper($(parent), 'ForCorporateLicenseDisplayDataTable', false);
    var dt = Desertfire.LicenseServicesForCorporate.datatable;

    dt.DataSource = '/Settings/GetAllServicesForCorporate';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'ServiceStatus', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            // return '<input type="checkbox" class="select-checkbox checkboxes" />';

            if (data == true) {
                //alert(1);
                return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
            } else if (data == false) {
                //alert(2);
                return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
            }

            return "";
        }
    });
    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    dt.AddColumn('License Services Name', 'Name');


    dt.AddActionDeleteButton('Add to Available Services', function () {
        Desertfire.LicenseServicesForCorporate.EditLicenseServiceControll(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });



    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionDeleteButton('Add to Dashboard', function () {
    //    Desertfire.LicenseServicesForCorporate.EditWidgetControll(dt.GetSelectedAttribute('Id'));
    //    //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
    //    //var hubs = dt.GetAllAttribute('Id');
    //    var hubs = dt.GetSelectedAttribute('Id');
    //    //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    //}, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}




Desertfire.LicenseServicesForCorporate.EditLicenseServiceControll = function (id) {
    alert(id);
    var selectedids = id.toString();
    alert(selectedids);
    Comm.send_json("/Settings/EditLicenseServiceControll", { serviceid: selectedids }, function (data) {
        Desertfire.LicenseServicesForCorporate.datatable.RetrieveData(true);
        alert('License Service will display on dashboard');
    });
}

//-------------------------end Corporate

//--------------------------------------------------------------end license services

Desertfire.Widget.datatable_setup = function (parent) {

    Desertfire.Widget.datatable = new DataTableWrapper($(parent), 'WidgetDisplayDataTable', false);
    var dt = Desertfire.Widget.datatable;

    dt.DataSource = '/Widgets/GetAllWidgets';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function (data) {
            // return '<input type="checkbox" class="select-checkbox checkboxes" />';

            if (data == true) {
                //alert(1);
                return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
            } else if (data == false) {
                //alert(2);
                return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
            }

            return "";
        }
    });
    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
    dt.AddColumn('Widget Name', 'WidgetName');


    //dt.AddActionEditButton('Edit', function () {
    //    Desertfire.ChangeLocation("/Widgets/EditWidgetControll/" + dt.GetSelectedAttribute('Id'));
    //});
    dt.AddActionDeleteButton('Add to Dashboard', function () {
        Desertfire.Widget.EditWidgetControll(dt.GetSelectedAttribute('Id'));
        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
        //var hubs = dt.GetAllAttribute('Id');
        var hubs = dt.GetSelectedAttribute('Id');
        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

}

Desertfire.Widget.EditWidgetControll = function (id) {
    var selectedids = id.toString();
    Comm.send_json("/Widgets/EditWidgetControll", { widgetid: selectedids }, function (data) {
        Desertfire.Widget.datatable.RetrieveData(true);
        alert('Widget will display on dashboard');
    });
}

Desertfire.startLoader = function () {
    $("#pageLoader").removeClass("hide");
}
Desertfire.stopLoader = function () {
    $("#pageLoader").addClass("hide");
}

Desertfire.ChangeLocation = function (location) {
    Desertfire.startLoader();
    window.location = $("#hdnUrl").val() + location;
}

Desertfire.datatable.setup = function (dtcontainer, noData) {
    dtcontainer.MakeDataTable(noData);
}

Desertfire.Alert.setup = function (dtcontainer, noData) {
    dtcontainer.MakeAlert(noData);
}

Desertfire.overlay.setup = function (olcontainer, noData) {
    olcontainer.MakeOverlay(noData);
}

Desertfire.datatable.from_table = function (table, search) {
    var tablesettings = {
        "iDisplayLength": 10,
        "bDeferRender": true,
        "bProcessing": false,
        "fnCreatedRow": function (nRow, aData) {
            $(nRow).attr("name", aData.MediaId).addClass("table-row-height");
        }
    }
    if ($.fn.DataTable.fnIsDataTable(table)) {
        table.dataTable().fnAdjustColumnSizing();
        return;
    }
    //create the form input element for recording selected rows, if applicable
    var oDTable = table.dataTable(DataTable.settings(table, tablesettings));
    if (search) {
        Desertfire.setup_search(search);
    }
}

Desertfire.Replace = function (mainStr, searchStr, replaceStr) {
    searchStr = Desertfire.ReplaceString + searchStr + Desertfire.ReplaceString;
    while (mainStr.indexOf(searchStr) > -1) {
        mainStr = mainStr.replace(searchStr, replaceStr);
    }
    return mainStr;
}

Desertfire.setup_search = function (selector) {
    $('.data-search', selector).off('keyup').keyup(function (e) {
        var datatables = $($(this).data("selector")).dataTable();
        if (datatables.length > 0) {
            datatables.fnFilter(this.value);
        }
    }).keyup();
}

Desertfire.multi_select_get_selected = function (select) {
    var values = [];
    $('.active a', select).each(function () {
        values.push($(this).data('value'));
    });
    return values;
}

Desertfire.get_selected = function (dropdown) {
    var list = { Names: [], Values: [] };
    $('li.active > a', dropdown).each(function () {
        list.Names.push($(this).data("name"));
        list.Values.push($(this).data("value"));
    });
    return list;
}

Desertfire.field_value = function (field, dataRow) {
    var objs = field.split(".");
    var col = dataRow[objs[0]];
    for (var k = 1; k < objs.length; k++) {
        col = col[objs[k]];
    }
    return col;
}

function resetSelectableTable(sTableId) {
    var oTable = $("body #" + sTableId).filter(":visible");

    oTable.dataTable().fnDestroy();
    oTable.find(".clicked").removeClass("clicked");
    setup_datatables(oTable.parent());
}

function selectableTableSelect(oTable) {
    var thisInputName = oTable.data("inputname");
    var arr = table_get_selected_by_attribute(oTable, "data-value"); //arrrrrr!
    $("input[name='" + thisInputName + "']").attr("value", arr.join(",")).change();
}

Desertfire.multi_select_table = function (table) {
    table = $(table);
    table.off('click').click(function (e) {
        var target = $(e.target);
        if (target.parents('thead').length > 0) { return; }
        var row = target.parents('tr').toggleClass('selected');
        var input = row.find('input[type=checkbox]');
        var isSelected = row.hasClass('selected');
        row.data('active', isSelected);
        input[0].checked = isSelected;
        e.stopPropagation();
        return;
    });
}

Desertfire.multi_select_dropdown = function (dd) {
    var select = $(dd);
    select.off('click').click(function (e) {
        Codigo.multi_select_click(e, select);
        e.stopPropagation();
        return;
    });
}

Desertfire.multi_select_click = function (event, ele, cookiePrefix) {
    event.stopPropagation();
    var target = $(event.target);
    var li = target.closest('li');
    var value = li.find('a').data('value');
    var cookievalue = $.trim(cookiePrefix + '_' + value);
    if (li.hasClass("action")) {
        $('a.dropdown-toggle', ele).click();
        return;
    }
    if (target.parents('.dropdown-menu-buttons').length > 0) {
        var clicked = target.closest('span,a');
        if (clicked.attr("onclick") == '') {
            clicked.click();
        }
        $('a.dropdown-toggle', ele).click();
        return;
    }
    else if (li.hasClass("expand")) {
        Codigo.expand_dropdown_sub(li);
        return null;
    }
    else if (target.parents('.expander').length > 0) {
        return;
    }
    if (li.hasClass('single')) {
        if (li.hasClass('active')) { return; }
        $('.active', ele).each(function () {
            var prevValue = $(this).find('a').data('value');
            removeCookie($.trim(cookiePrefix + '_' + prevValue));
        })
        $('.active', ele).removeClass('active');
        $('input:checked', ele).removeAttr('checked');
        var input = $('input', li).attr('checked', 'checked');
        li.addClass('active');
        removeCookie(cookievalue);
        addCookie(cookievalue, 1);
    } else {
        $('li.single', ele).removeClass('active');
        li.toggleClass('active');
        removeCookie(cookievalue);
        if (li.hasClass('active')) {
            var input = $('input', li).attr('checked', 'checked');
            addCookie(cookievalue, 1);
        }
        else {
            var input = $('input', li).removeAttr('checked');
        }
    }
    return value;
}

$.fn.prepareForm = function () {
    $(this).find('form').bind('submit', window.Desertfire.formSubmit);
    $.validator.unobtrusive.parse($(this).find('form'));
    $(this).find('form').on("keyup", "input.valid", function () {
        if ($(this).parents('.form-group').hasClass('has-error')) {
            $(this).next('.field-validation-valid').each(function () {
                if (!($(this).find("span").length > 0)) {
                    $(this).parents('.form-group').removeClass('has-error');
                    $(this).parents('.form-group').find("i[error-for]").remove();
                }
            });
        }
    });
};

Desertfire.formSubmit = function () {
    // alert(1);
    $(".validationMessageList").html("");
    $('span.field-validation-valid, span.field-validation-error').each(function () {
        $(this).addClass('help-inline');
    });

    $('.validation-summary-errors').each(function () {
        $(this).addClass('alert');
        $(this).addClass('alert-error');
        $(this).addClass('alert-block');
    });

    $('form').submit(function () {
        $(".validationMessageList").html("");
        if ($(this).valid()) {
            $(this).find('div.form-group').each(function () {
                if ($(this).find('span.field-validation-error').length == 0) {
                    $(this).removeClass('has-error');
                }
            });
        }
        else {
            $(".alert").removeClass("hide");
            $(this).find('div.form-group').each(function () {
                if ($(this).find('span.field-validation-error').length > 0) {
                    $(this).addClass('has-error');
                    $(this).find('span.field-validation-error').each(function () {
                        $(".validationMessageList").append("<div class='row'><div class='col-md-12'>" + $(this).html() + "</div></div>");
                    });
                }
            });
            $('.validation-summary-errors').each(function () {
                if ($(this).hasClass('alert-error') == false) {
                    $(this).addClass('alert');
                    $(this).addClass('alert-error');
                    $(this).addClass('alert-block');
                }
            });
        }
    });

    $('form').each(function () {
        $(this).find('div.form-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(".alert").removeClass("hide");
                $(this).addClass('has-error');

                $(this).find('span.field-validation-error').each(function () {
                    $(".validationMessageList").append("<div class='row'><div class='col-md-12'>" + $(this).html() + "</div></div>");
                });
            }
        });
    });

    $("input[type='password'], input[type='text']").blur(function () {
        if ($(this).hasClass('input-validation-error') == true || $(this).closest(".form-group").find('span.field-validation-error').length > 0) {
            $(this).addClass('has-error');
            $(this).closest(".form-group").addClass("has-error");
        } else {
            $(this).removeClass('has-error');
            $(this).closest(".form-group").removeClass("has-error");
        }
    });

    var page = function () {
        //Update that validator
        $.validator.setDefaults({
            highlight: function (element) {
                $(element).closest(".form-group").addClass("has-error");
            },
            unhighlight: function (element) {
                $(element).closest(".form-group").removeClass("has-error");
            }
        });
    }();
};

Desertfire.formSubmitBegin = function (formId) {
    Desertfire.startLoader();
};

Desertfire.formSubmitComplete = function (formId, data, statusCode, onSuccess) {
    Desertfire.stopLoader();
    //alert(10);
    $('#' + formId + ' input[type=submit]').attr('disabled', false);
    $('#' + formId + ' input[type=submit]').removeClass('loading');

    if (onSuccess != undefined) {
        if (statusCode == 200) {
            if (data.Result) {
                onSuccess();
                $("#successMessage").removeClass("hide");
            }
            else {
                $(".alert").not("#successMessage").removeClass("hide");
                $("#successMessage").addClass("hide");
                //Metronic.Error(data.Message, false);
            }
        }
        else {
            $("#successMessage").addClass("hide");
            $(".alert").removeClass("hide");
            //Metronic.Error();
        }
    }
};

Desertfire.SetSidebarMenu = function (menu, subMenu) {
    $("ul.page-sidebar-menu li").each(function () {
        $(this).removeClass("start");
        $(this).removeClass("active");
        $(this).removeClass("open");
        $(this).find("li[data-subTitle]").removeClass("active");
    });

    $("ul.page-sidebar-menu").find("li[data-menutitle=" + menu + "]").addClass("start").addClass("active").addClass("open");
    $("ul.page-sidebar-menu").find("li[data-menutitle=" + menu + "]").find("li[data-subTitle=" + subMenu + "]").addClass("active");

}

jQuery.fn.expandedDropdown = function (settings) {
    settings = jQuery.extend({

    }, settings);
    return this.each(function () {
        $(this).dropdown();
        var oUl = $(this).parent().find(".dropdown-menu");
        var w = 0;
        oUl.find("li").each(function () {
            /*if ($(this).find("a").hasClass("expandable")) {
                var oLi = $(this);
                $(this).find(".expander").unbind("click").click(function () {
                    if (oLi.find(".dropdown-sub").height() < 10) {
                        oLi.find(".dropdown-sub").css("height", "auto");
                        $(this).css("transform", "rotateZ(0deg)");
                    } else {
                        oLi.find(".dropdown-sub").css("height", "0px");
                        $(this).css("transform", "rotateZ(-90deg)");
                    }
                    return false;
                });
            }*/
        });

        $(this).click(function () {
            $(this).parent().find(".dropdown-menu li").each(function () {
                if ($(this).find(".dropdown-menu-buttons").length > 0) {
                    $(this).find("a").css("padding-right", $(this).find(".dropdown-menu-buttons").eq(0).outerWidth() + 8);
                    $(this).find(".dropdown-menu-buttons > *").click(function () {
                        //return false; //THIS IS BAD AND KEEPS MENUS OPEN!!
                    });
                }
            });
        });
    });
}

function convertToDate(s) {
    if (s.search("Date") > -1) { //data from server: "/Date(xxxxx)/"
        var s2 = "new " + s.replace(/\//g, "");
        var d = eval(s2);
    } else { // in format 'MM/DD/YYYY'
        var sp = s.split("/");
        var d = new Date(sp[2], sp[0] - 1, sp[1], 0, 0, 0, 0);
    }
    return d;
}

Desertfire.setup_turnovers = function (selector) {
    Desertfire.BodyEvents.add_click(function (event) {
        var target = $(event.target);
        if (target.hasClass('turnover-bearer') || target.parents('.turnover-bearer').length > 0) {
            var bearer = target.hasClass('turnover-bearer') ? target : target.parents('.turnover-bearer');
            if (bearer.hasClass('active')) { return; }
            if (bearer.attr("id") == null) {
                bearer.attr('id', 'bearer_' + new Date().getTime());
            }
            var url = bearer.data("turnoverurl");
            if (typeof url != "string") {
                var turnover = $('.turnover', bearer).clone().attr("id", bearer.data("turnoverid"));
                Desertfire.show_turnover(turnover, bearer);
            }
            else {
                Comm.send_json(url, {}, function (data) { Desertfire.attach_turnover(data, bearer); });
            }
        }
    });
    return;
}
Desertfire.attach_turnover = function (data, bearer) {
    //master_callback(data);
    Desertfire.stopLoader();
    if (data.Selector != "#alerts") {//turnover succeeded and is attached to the body element
        var turnover = $(data.Content);
        var turnoverClass = bearer.parents('.modal').length == 0 ? '' : '.modal-turnover';
        Desertfire.close_turnovers(turnoverClass);
        Desertfire.show_turnover(turnover, bearer);
        return turnover;
    }
}
Desertfire.show_turnover = function (turnover, bearer) {
    if (bearer.parents('.modal').length > 0) {
        turnover.css('z-index', bearer.parents('.modal').css('z-index') + 10).addClass('modal-turnover');
        $('.turnover-toolbar', turnover).remove();
    }
    $('.tooltip').remove();

    $('body').append(turnover);
    turnover.data("bearer", bearer);
    turnover.find('.turnover-content').css('max-height', window.innerHeight * .75 + "px");
    turnover.show().removeClass("hide").removeClass("out").addClass("in");
    var close = $('.close', turnover);
    if (close.length == 0) {
        close = $('<a class="close">x</a>');
        turnover.find('.turnover-title').prepend(close);
    }
    close.click(function () {
        Desertfire.close_turnover(turnover);
    });
    bearer.addClass("active");
    turnover.click(function (e) { $(window).click(); e.stopPropagation(); });
    var callback = window[turnover.data("callback")];
    if (typeof (callback) == "function") {
        callback(turnover, bearer.attr("id"));
    }
    Desertfire.turnover_center_top(turnover);
    $('.btn:not(.tooltip-ready)', turnover).tooltip();
    var closeFn = function (e) {
        if ($(e.target) != undefined && $(e.target).parents(".popover").length > 0) {
            // do something else if we wanted to
        }
        else {
            Desertfire.close_turnover(turnover);
        }
    }
    turnover.data("close", closeFn);

    //$('body').bind('click', turnover.data("close")).append('<div class="modal-backdrop"></div>');

    $('.turnover-pager', turnover).click(function (e) {
        if ($(this).hasClass('active')) { return; }
        var page = $(this).data("turnoverpagetoggle");
        turnover.find(".turnover-inner:visible").removeClass('fadetoggle').hide();
        turnover.find(".turnover-inner[data-turnoverpage='" + page + "']").removeClass('fadetoggle').show();
        Desertfire.turnover_center_top(turnover);
    });

    // if we want to show alerts inside turnovers #3057
    if (turnover.find("div#alerts").length > 0) {
        $("body>div.container").find("div#alerts").attr("id", "alerts-doNotUseThis");
    }

    $(turnover).find(".popoverContent").popover({ trigger: "manual", html: true, animation: false, placement: 'top' })
                      .on("mouseenter", function (e) {
                          var _this = this;
                          $(".popover").each(function () {
                              $(this).remove();
                          });

                          $(_this).popover("show");

                          $(".popover.in").each(function () {
                              if (!$(this).hasClass("turnover-popover")) {
                                  $(this).addClass("turnover-popover");
                              }
                              var offset = $(this).offset();
                              var topPosition = offset.top;
                              // specifically for shows turnover.
                              if ($(_this).parents(".zone-box").length > 0) {
                                  var zoneBoxTopPosition = $(_this).parents(".zone-box").offset().top;
                                  var popOverHeight = $(this).height();
                                  $(this).css('top', (topPosition < 0 ? (zoneBoxTopPosition - popOverHeight) : topPosition) + 'px');
                              } else {
                                  $(this).css('top', (topPosition < 0 ? 0 : topPosition) + 'px');
                              }
                          });

                          $(".popover").on("mouseleave", function () {
                              $(_this).popover('hide');
                          });
                      }).on("mouseleave", function () {
                          var _this = this;
                          setTimeout(function () {
                              if (!$(".popover:hover").length) {
                                  $(_this).popover("hide");
                              }
                          }, 100);
                      });

}
Desertfire.turnover_change_bearer = function (turnover, newBearer) {
    if (turnover.length > 0) {
        turnover.data('bearer', newBearer);
        Desertfire.turnover_position(turnover, newBearer.addClass('active'));
    }
    return turnover;
}
Desertfire.close_turnovers = function (turnoverClass) {
    turnoverClass = turnoverClass == null ? '' : turnoverClass;
    $(".turnover.in" + turnoverClass).each(function () {
        Desertfire.close_turnover($(this));
    });
}
Desertfire.close_turnover = function (turnover) {
    var bearer = turnover.data('bearer');
    if (bearer) { bearer.removeClass('active'); }
    $('form input, form select', turnover).blur();
    turnover.removeClass("in").addClass("out");
    setTimeout(function () {
        var onclose = turnover.data('onclose');
        if (onclose) {
            eval(onclose);
        }
        if (turnover.hasClass('turnover-permanent')) {
            turnover.hide();
        } else {
            turnover.remove();
        }
        $('.modal-backdrop').remove();
    }, 200);
    $('.tooltip').remove();
    $('.modal-backdrop').remove();
    $('body').unbind('click', turnover.data('close'));
    // if we want to show alerts inside turnovers #3057
    $("body>div.container").find("div#alerts-doNotUseThis").attr("id", "alerts");
}
Desertfire.turnover_center_top = function (oTurnover, distance) {
    distance = (distance == null ? 50 : distance) + window.scrollX;// it was (distance == null ? 50 : distance) + window.scrollY which makes the turnover to appear after Y position and causes it to be hidden from the screen if the window is scrolled to bottom. #2605
    var turnoverWidth = oTurnover.outerWidth();
    var windowWidth = $(window).width();
    var offset = {
        top: distance,
        left: windowWidth / 2 - turnoverWidth / 2
    }
    oTurnover.offset(offset);
}
Desertfire.turnover_center_screen = function (oTurnover) {
    var turnoverWidth = oTurnover.outerWidth();
    var turnoverHeight = oTurnover.outerHeight();
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();
    var offset = {
        top: windowHeight / 2 - turnoverHeight / 2,
        left: windowWidth / 2 - turnoverWidth / 2
    }
    oTurnover.offset(offset);
    //window.sc
}
Desertfire.turnover_position = function (turnover, bearer) {
    bearer = $(bearer);
    var offset = bearer.offset();
    var arrowOffset = { top: offset.top };
    var arrowWidth = 7;
    offset.left += bearer.width() + arrowWidth;
    offset.top -= turnover.height() / 2 - bearer.height() / 2; //centers the tooltip
    if (offset.top < 40) {
        offset.top = 40;
    }
    if ((offset.left + turnover.width()) > $(window).width() || turnover.hasClass('left')) {
        offset.left = bearer.offset().left - turnover.width() - arrowWidth;
        turnover.addClass("left");
    }
    else {
        turnover.addClass('right');
    }
    arrowOffset.top -= (offset.top - bearer.height() / 2);
    $('.turnover-arrow', turnover).css(arrowOffset);
    try {
        turnover.css(offset); //.offset doesn't work for some reason
    }
    catch (ex) {
        //turnover.css(offset); //.offset doesn't work for some reason
    }
    return turnover;
}


Desertfire.ItemExistsInArray = function (arr, d) {
    if (Desertfire.ObjArrayIndexOf(arr, d) >= 0) {
        return true;
    }
    return false;
}
// this should be generalized.
Desertfire.ObjArrayIndexOf = function (arr, o) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i].id == o.id && arr[i].value == o.value) {
            return i;
        }
    }
    return -1;
}
// type ahead tags input only
Desertfire.substringMatcher = function (strs) {
    return function findMatches(q, cb) {
        var matches, substrRegex;

        // an array that will be populated with substring matches
        matches = [];

        // regex used to determine if a string contains the substring `q`
        substrRegex = new RegExp(q, 'i');

        // iterate through the pool of strings and for any string that
        // contains the substring `q`, add it to the `matches` array
        $.each(strs, function (i, str) {
            if (substrRegex.test(str.value)) {
                // the typeahead jQuery plugin expects suggestions to a
                // JavaScript object, refer to typeahead docs for more info
                matches.push({ id: str.id, value: str.value });
            }
        });

        cb(matches);
    };
};
$(document).ready(function () {
    //if (jQuery().datepicker) {
    //    //$('.date-picker').datepicker({
    //    //    //rtl: Metronic.isRTL(),
    //    //    orientation: "left",
    //    //    autoclose: true,
    //    //    format: "dd/M/yyyy",
    //    //    pickTime: false
    //    //});
    //    ////$('.date-time-picker').datetimepicker({
    //    ////    //rtl: Metronic.isRTL(),
    //    ////    orientation: "left",
    //    ////    autoclose: true,
    //    ////    format: "dd/M/yyyy HH:ii P",
    //    ////    showMeridian: true
    //    ////});
    //}
    if (jQuery().timepicker) {
        $('.timepicker-default').timepicker({
            autoclose: true,
            showSeconds: true,
            minuteStep: 1
        });

        $('.timepicker-no-seconds').timepicker({
            autoclose: true,
            minuteStep: 5
        });

        $('.timepicker-24').timepicker({
            autoclose: true,
            minuteStep: 5,
            showSeconds: false,
            showMeridian: false
        });

        // handle input group button click
        $('.timepicker').parent('.input-group').on('click', '.input-group-btn', function (e) {
            e.preventDefault();
            $(this).parent('.input-group').find('.timepicker').timepicker('showWidget');
        });
    }

    $(document).prepareForm();
    Desertfire.Navigation.SetupNav();
});

//Company Information
//Desertfire.Company.datatable_setup = function (parent) {

//    Desertfire.Company.datatable = new DataTableWrapper($(parent), 'CompanyDisplayDataTable', false);
//    var dt = Desertfire.Company.datatable;
//    //var dt1=
//    dt.DataSource = '/CompanyManagement/GetAllCompanies';

//    dt.RetrieveData(true);

//    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function () {
//            return '<input type="checkbox" class="select-checkbox checkboxes" />';
//        }
//    });

//    //if (data == true) {
//    //    //alert(1);
//    //    return '<input type="checkbox" class="select-checkbox checkboxes" checked="checked" />';
//    //} else if (data == false) {
//    //    //alert(2);
//    //    return '<input type="checkbox" class="select-checkbox checkboxes" aria-checked="false" />';
//    //}


//    //dt.AddColumn('<input type="checkbox" class="select-checkbox checkboxes" />', 'DisplayonDashboard');

//    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'DisplayonDashboard', {
//    //    bVisible: true, bSearchable: false, bSortable: false,
//    //    mRender: function () {
//    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
//    //    }
//    //});

//    //    , {
//    //    mRender: function (data) {
//    //        Desertfire.ChangeLocation("/Settings/GetCity/" + dt.GetSelectedAttribute(data));
//    //    }
//    //});
//    dt.AddColumn('Company Name', 'CompanyName');
//    dt.AddColumn('Country', 'CountryName');
//    dt.AddColumn('State', 'StateName');
//    dt.AddColumn('City', 'CityName');

//    dt.AddColumn('License Key', 'LicenseKey');
//    dt.AddColumn('License Type', 'LicenseId',
//        {
//            mRender: function (data) {

//                if (data == 1) {

//                    return "Trail Version"
//                }
//                else if (data == 2) {
//                    return "Commercial Version"
//                }
//                else if (data == 3) {
//                    return "Enterprise Version"
//                }
//                else if (data == 4) {
//                    return "Corporate Version"
//                }
//                return "";
//            }
//        });
//    dt.AddColumn('Database', 'DatabaseName');
//    dt.AddColumn('Company Status', 'StatusId', {
//        mRender: function (data) {
//            if (data == 1) {
//                return "<span class='label label-success'>Active</span>"
//                //return "Active".fontcolor('Blue');
//            } else if (data == 2) {
//                return "<span class='label label-primary'>In-Active</span>"
//                //  return "In-Active".fontcolor('Red');
//            }
//            else if (data == 3) {
//                return "<span class='label label-warning'>Pending</span>"
//            }
//            else if (data == 4) {
//                return "<span class='label label-danger'>Deleted</span>"
//            }
//            return "";
//        }
//    });
//    dt.AddActionEditButton('Edit', function () {
//        Desertfire.ChangeLocation("/CompanyManagement/AddEditCompany/" + dt.GetSelectedAttribute('Id'));
//    });
//    dt.AddActionDeleteButton('Delete Company', function () {
//        Desertfire.Company.DeleteCompanies(dt.GetSelectedAttribute('Id'));
//        //Desertfire.Widget.EditWidgetControll(dt.GetAllAttribute('Id'));//   GetSelectedAttribute('Id'));
//        //var hubs = dt.GetAllAttribute('Id');
//        var hubs = dt.GetSelectedAttribute('Id');
//        //DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
//    }, { UseRightMenu: true });
//    Desertfire.datatable.setup(dt);
//    dt.selectableWindowsStyleWithCheckbox();
//}

//Desertfire.Company.DeleteCompanies = function (id) {

//    var selectedids = id.toString();
//    Comm.send_json("/CompanyManagement/DeleteCompany", { Companyid: selectedids }, function (data) {
//        Desertfire.Company.datatable.RetrieveData(true);
//        //alert('Widget will display on dashboard');
//    });
//}

//Account Settings
Desertfire.AccountSetting = {};
Desertfire.AccountSetting.datatable_setup = function (parent) {

    Desertfire.AccountSetting.datatable = new DataTableWrapper($(parent), 'AccountSettingDisplayDataTable', false);
    var dt = Desertfire.AccountSetting.datatable;
    //var dt1=
    dt.DataSource = '/Settings/GetAllMYOBSetting';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('API Key', 'APIKey');
    dt.AddColumn('User Name', 'UserName');
    dt.AddColumn('Signature', 'FileLibraryId');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });
    dt.AddActionEditButton('Edit MYOBSetting', function () {
        Desertfire.ChangeLocation("/Settings/AddEditMYOBSetting/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionDeleteButton('Delete MYOB Setting', function () {
        Desertfire.AccountSetting.MYOBSetting(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

}
Desertfire.AccountSetting.MYOBSetting = function (id) {

    var selectedids = id.toString();
    Comm.send_json("/Settings/DeleteMYOBSetting", { OBid: selectedids }, function (data) {
        Desertfire.AccountSetting.datatable.RetrieveData(true);
        //alert('Widget will display on dashboard');
    });
}


Desertfire.ActSetting = {};
Desertfire.ActSetting.datatable_setup = function (parent) {

    Desertfire.ActSetting.datatable = new DataTableWrapper($(parent), 'ActSettingDisplayDataTable', false);
    var dt = Desertfire.ActSetting.datatable;
    //var dt1=
    dt.DataSource = '/Settings/GetAllActSetting';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('API Key', 'APIKey');
    dt.AddColumn('User Name', 'UserName');
    dt.AddColumn('Signature', 'FileLibraryId');
    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });
    dt.AddActionEditButton('Edit Act Setting', function () {
        Desertfire.ChangeLocation("/Settings/AddEditActSetting/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionDeleteButton('Delete Act Setting', function () {
        Desertfire.AccountSetting.ActSetting(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.AccountSetting.ActSetting = function (id) {

    var selectedids = id.toString();
    Comm.send_json("/Settings/DeleteActSetting", { ACTid: selectedids }, function (data) {
        Desertfire.ActSetting.datatable.RetrieveData(true);
        //alert('Widget will display on dashboard');
    });
}

Desertfire.PaymentMode = {};
Desertfire.PaymentMode.datatable_setup = function (parent) {

    Desertfire.PaymentMode.datatable = new DataTableWrapper($(parent), 'dbPaymentModeSettings', false);
    var dt = Desertfire.PaymentMode.datatable;
    //var dt1=
    dt.DataSource = '/Settings/GetAllPaymentMode';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('Payment Mode', 'PaymentMode');

    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });
    dt.AddActionEditButton('Edit Payment Mode', function () {
        Desertfire.ChangeLocation("/Settings/AddEditPaymentMode/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionDeleteButton('Delete Payment Mode', function () {
        Desertfire.AccountSetting.PaymentMode(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

}
Desertfire.AccountSetting.PaymentMode = function (id) {

    var selectedids = id.toString();
    Comm.send_json("/Settings/DeletePaymentMode", { paymentModeid: selectedids }, function (data) {
        Desertfire.PaymentMode.datatable.RetrieveData(true);
        //alert('Widget will display on dashboard');
    });
}




Desertfire.PaymentCardType = {};
Desertfire.PaymentCardType.datatable_setup = function (parent) {

    Desertfire.PaymentCardType.datatable = new DataTableWrapper($(parent), 'dbPaymentCardTypeSettings', false);
    var dt = Desertfire.PaymentCardType.datatable;
    //var dt1=
    dt.DataSource = '/Settings/GetAllPaymentCardType';

    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('Accepted Payment Card Type', 'PaymentCardType');

    dt.AddColumn('Status', 'StatusId', {
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
            return "";
        }
    });
    dt.AddActionEditButton('Edit Payment Card Type', function () {
        Desertfire.ChangeLocation("/Settings/AddEditPaymentCardType/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionDeleteButton('Delete Payment Card Type', function () {
        Desertfire.PaymentCardType.PaymentCardType(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

}
Desertfire.PaymentCardType.PaymentCardType = function (id) {

    var selectedids = id.toString();
    Comm.send_json("/Settings/DeletePaymentCard", { paymentCardid: selectedids }, function (data) {
        Desertfire.PaymentCardType.datatable.RetrieveData(true);
        //alert('Widget will display on dashboard');
    });
}
//Desertfire.Widget.EditWidgetControll = function (id) {
//    var selectedids = id.toString();
//    Comm.send_json("/Widgets/EditWidgetControll", { widgetid: selectedids }, function (data) {
//        Desertfire.Widget.datatable.RetrieveData(true);
//        alert('Widget will display on dashboard');
//    });
//}




//Question Master 
Desertfire.Question.datatable_setup = function (parent) {

    Desertfire.Question.datatable = new DataTableWrapper($(parent), 'QuestionDisplayDataTable', false);
    var dt = Desertfire.Question.datatable;

    dt.DataSource = '/Question/GetAllQuestion';
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
            return '<a href="#" onclick="Desertfire.Question.GetSingleQuestionTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });



    dt.AddColumn('Procedure', 'ProcedureId');
    dt.AddColumn('Auther', 'AutherId');
    dt.AddColumn('Question', 'Question');
    dt.AddColumn('Answer', 'Answer');
    dt.AddColumn('Object1', 'Obj1');
    dt.AddColumn('Object2', 'Obj2');
    dt.AddColumn('Object3', 'Obj3');
    dt.AddColumn('Object4', 'Obj4');
    dt.AddColumn('Question Type', 'QuestionType');
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
            return "";
        }
    });

    dt.AddActionEditButton('Edit Company', function () {
        Desertfire.ChangeLocation("/Question/EditQuestion/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionDeleteButton('Delete Company', function () {
        Desertfire.Question.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Question.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Question/DeleteQuestion", { Questionid: selectedids }, function (data) {
            Desertfire.Question.datatable.RetrieveData(true);

        });
    }

}

Desertfire.Question.GetSingleQuestionTurnover = function (id) {

    Comm.send_json("/Question/GetSingleQuestiondetail", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

// Try
Desertfire.QuestionMaster.datatable_setup = function (parent) {
    Desertfire.QuestionMaster.datatable = new DataTableWrapper($(parent), 'QuestionDisplayDataTable', false);
    var dt = Desertfire.QuestionMaster.datatable;
    dt.DataSource = '/Test/GetAllQuestionProcedureWise';
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
    //        return '<a href="#" onclick="Desertfire.QuestionMaster.GetSingleQuestionTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});

    dt.AddColumn('Question', 'Question');
    //dt.AddColumn('Auther', 'AutherId');
    //dt.AddColumn('Question', 'Question');

    //dt.AddColumn('Status', 'StatusId', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "<span class='label label-success'>Active</span>"

    //        } else if (data == 2) {
    //            return "<span class='label label-primary'>In-Active</span>"

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

    dt.AddActionEditButton('Edit Question', function () {
        Desertfire.ChangeLocation("/Test/EditQuestion/" + dt.GetSelectedAttribute('Id'));
    });

    //dt.AddActionDeleteButton('Delete Company', function () {
    //    Desertfire.QuestionMaster.DeleteQuestions(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.QuestionMaster.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Question/DeleteQuestion", { Questionid: selectedids }, function (data) {
            Desertfire.QuestionMaster.datatable.RetrieveData(true);

        });
    }

}

Desertfire.QuestionMaster.GetSingleQuestionTurnover = function (id) {

    Comm.send_json("/Question/GetSingleQuestiondetail", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}


// NEW
Desertfire.TestRecord.datatable_setup = function (parent) {
    Desertfire.TestRecord.datatable = new DataTableWrapper($(parent), 'QuestionDisplayDataTable', false);
    var dt = Desertfire.TestRecord.datatable;

    dt.DataSource = '/Reports/GetAllReports';
    dt.RetrieveData(true);

    //dt.AddColumn('', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function (data) {
    //        return '<a href="#" onclick="Desertfire.QuestionMaster.GetSingleQuestionTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});



    dt.AddColumn('Procedure', 'Procedure');
    dt.AddColumn('User', 'User');
    dt.AddColumn('Result', 'Result');
    dt.AddColumn('Date', 'Date');

    //dt.AddColumn('Status', 'StatusId', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "<span class='label label-success'>Active</span>"

    //        } else if (data == 2) {
    //            return "<span class='label label-primary'>In-Active</span>"

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

    dt.AddActionEditButton('Edit Question', function () {
        Desertfire.ChangeLocation("/Test/EditQuestion/" + dt.GetSelectedAttribute('Id'));
    });

    dt.AddActionNewButton("Generate Report", function () {
        window.location = Ras.RootUrl + 'Report/ReportOneNew';
    }, { UseRightMenu: true, IsDisabled: true, DisableFn: function () { this.IsDisabled = true } });
    Ras.datatable.setup(dt);
    //dt.AddActionDeleteButton('Delete Company', function () {
    //    Desertfire.QuestionMaster.DeleteQuestions(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.TestRecord.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Question/DeleteQuestion", { Questionid: selectedids }, function (data) {
            Desertfire.TestRecord.datatable.RetrieveData(true);

        });
    }

}

Desertfire.TestRecord.GetSingleQuestionTurnover = function (id) {

    Comm.send_json("/Question/GetSingleQuestiondetail", { id: id }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });

}
//
Desertfire.AgentRegistration.datatable_setup = function (parent) {
    Desertfire.AgentRegistration.datatable = new DataTableWrapper($(parent), 'agentDatatable', false);

    var dt = Desertfire.AgentRegistration.datatable;

    dt.DataSource = '/Agent/GetAllAgent';
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
 
    dt.RetrieveData(true);

    dt.AddColumn('Advisor Name', 'fullname');
    dt.AddColumn('Father Name', 'fathersHusbandName');
    dt.AddColumn('email', 'email');
    //dt.AddColumn('Advisor Status', 'Advisorstatus');
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
            else if (data == 5) {
                return "<span class='label label-warning'>Approved</span>"
            }
            else if (data == 4) {
                return "<span class='label label-danger'>Deleted</span>"
            }
            else if (data == 8) {
                return "<span class='label label-danger'>Kyc Approved</span>"
            }
            return "";
        }
    });
    dt.AddActionEditButton('Edit Advisor', function () {
        Desertfire.ChangeLocation("/Agent/EditAgent/" + dt.GetSelectedAttribute('id'));
    });

    dt.AddActionNewButton('Add New Advisor', function () {
        Desertfire.ChangeLocation("/Agent/AddEditAgent/0");
    });
    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Agent/ExportData/0");
    });
    dt.AddActionEditButton("Print Detail", function () {
        //window.location = Loregroup + 'Report/RedirectToSalesReports?FromDate=' + $("#txtFromDate").val() + '&ToDate=' + $("#txtToDate").val() + '&RestaurantId=' + $("#RestaurantId").val() + '&OrderType=' + $("#txtOrderType").val() + '&CounterName=' + $("#txtCounterName").val() + '&CashierId=' + $("#txtCashier").val();

        Desertfire.ChangeLocation("/Report/RedirectToSalesReports/" + dt.GetSelectedAttribute('id'));
    });

    dt.AddActionDeleteButton('Delete Advisor', function () {
        Desertfire.AgentRegistration.DeleteQuestions(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.AgentRegistration.DeleteQuestions = function (id) {
        var selectedids = id.toString();

        Comm.send_json("/Agent/DeleteAdvisor", { id: selectedids }, function (data) {
            Desertfire.AgentRegistration.datatable.RetrieveData(true);

        });
    }

}

// Show all policy for customer

Desertfire.CustomerPolicyMaster.datatable_setup = function (parent) {
    Desertfire.CustomerPolicyMaster.datatable = new DataTableWrapper($(parent), 'customerDatatable', false);

    var dt = Desertfire.CustomerPolicyMaster.datatable;

    dt.DataSource = '/Customer/GetPolicyForCustomer';
    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
     dt.AddActionNewButton('Export Data', function () {
         Desertfire.ChangeLocation("/Customer/ExportGetAllPolicyForCustomer/0");
    });
    dt.AddColumn('Nominee', 'nominee');
    dt.AddColumn('Relationship With The Nominee', 'relationshipwithnominee');
    dt.AddColumn('Appointee Address', 'appointeeaddress');
    dt.AddColumn('Appointee City', 'appointeeCity');
    dt.AddColumn('Package Name', 'PackageName');
    dt.AddColumn('Policy Start', 'Strtdate');
    dt.AddColumn('Policy Expiry Date', 'Endate');
    dt.AddColumn('Policy Status', 'Policystatusid', {
        mRender: function (data) {
            if (data == 0) {
                return "<span class='label label-success'>Running</span>"

            } else if (data == 1) {
                return "<span class='label label-primary'>Expiry</span>"

            }
            return "";
        }
    });
    dt.AddActionEditButton('Show Policy Detail', function () {
        Desertfire.ChangeLocation("/Customer/EditPolicyDetail/" + dt.GetSelectedAttribute('id'));
    });
    dt.AddActionEditButton('Send Renewal Policy Request', function () {
        Desertfire.ChangeLocation("/Customer/RenewalPolicyRequest/" + dt.GetSelectedAttribute('id'));
    });
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.CustomerPolicyMaster.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DeleteCustomer", { customerid: selectedids }, function (data) {
            Desertfire.CustomerPolicyMaster.datatable.RetrieveData(true);
        });
    }
}
//
//Desertfire.CustomerRegistration = {};
//Desertfire.CustomerRegistration.datatable_setupKYC = function (parent) {
//    alert("")
//    //Added By 
//    Desertfire.CustomerRegistration.datatable = new DataTableWrapper($(parent), 'customerDatatable10', false);

//    var dt = Desertfire.CustomerRegistration.datatable;

//    dt.DataSource = '/Customer/GetAllCustomersForKyc';
//    dt.RetrieveData(true);

//    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function () {
//            return '<input type="checkbox" class="select-checkbox checkboxes" />';
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
//            else if (data == 7) {
//                return "<span class='label label-primary'>For KVC</span>"
//            }
//            return "";
//        }
//    });

//    dt.AddActionEditButton('Edit Customer', function () {
//        Desertfire.ChangeLocation("/Customer/EditCustomer/" + dt.GetSelectedAttribute('id'));
//    });

//    dt.AddActionNewButton('Add New Customer', function () {
//        Desertfire.ChangeLocation("/Customer/AddEditCustomer/0");
//    });

//    //dt.AddActionDeleteButton('Delete Customer', function () {
//    //    Desertfire.CustomerRegistration.DeleteCustomers(dt.GetSelectedAttribute('id'));
//    //    var hubs = dt.GetSelectedAttribute('id');
//    //}, { UseRightMenu: true });

//    dt.AddActionDeleteButton('Approve Customer', function () {
//        Desertfire.CustomerRegistrations.DeleteCustomers(dt.GetSelectedAttribute('id'));
//        var hubs = dt.GetSelectedAttribute('id');
//    }, { UseRightMenu: true });

//    dt.AddActionDeleteButton('DisApprove Customer', function () {
//        Desertfire.CustomerRegistrations.DeleteCustomers1(dt.GetSelectedAttribute('id'));
//        var hubs = dt.GetSelectedAttribute('id');
//    }, { UseRightMenu: true });
//    Desertfire.datatable.setup(dt);
//    dt.selectableWindowsStyleWithCheckbox();
//    Desertfire.CustomerRegistrations.DeleteCustomers = function (id) {
//        var selectedids = id.toString();
//        Comm.send_json("/Customer/DisApproveCustomer", { id: selectedids }, function (data) {
//            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
//        });
//    }
//    Desertfire.CustomerRegistrations.DeleteCustomers = function (id) {
//        var selectedids = id.toString();
//        Comm.send_json("/Customer/ApproveCustomer", { id: selectedids }, function (data) {
//            Desertfire.CustomerRegistrations.datatable.RetrieveData(true);
//        });
//    }
//    //Desertfire.CustomerRegistration.DeleteCustomers = function (id) {
//    //    var selectedids = id.toString();
//    //    Comm.send_json("/Customer/DeleteCustomer", { customerid: selectedids }, function (data) {
//    //        Desertfire.CustomerRegistration.datatable.RetrieveData(true);
//    //    });
//    //}


//}


Desertfire.CustomerRegistrationsNew = {};
Desertfire.CustomerRegistrationsNew.datatable_setup = function (parent) {
    Desertfire.CustomerRegistrationsNew.datatable = new DataTableWrapper($(parent), 'policyDatatable', false);
    var dt = Desertfire.CustomerRegistrationsNew.datatable;
    dt.DataSource = '/Customer/GetAllCustomersForKyc';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Customer Name', 'FullName');
    dt.AddColumn('Customer Code', 'CustomerCode');
    dt.AddColumn('Nominee', 'nominee');
    dt.AddColumn('Relationship With The Nominee', 'relationshipwithnominee');
    dt.AddColumn('Appointee Name', 'appointeesName');

    dt.AddColumn('Bank', 'bank');
    dt.AddColumn('Email', 'email');
    dt.AddColumn('city', 'cityName');
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

    dt.AddActionEditButton('Kyc Customer Detail', function () {
        Desertfire.ChangeLocation("/Customer/ShowCustomerDetail/" + dt.GetSelectedAttribute('id'));
    });

    dt.AddActionDeleteButton('Approve Customer', function () {
        Desertfire.CustomerRegistrationsNew.DisapprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    dt.AddActionDeleteButton('DisApprove Customer', function () {
        Desertfire.CustomerRegistrationsNew.ApprovePolicy(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.CustomerRegistrationsNew.DisapprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/ApproveCustomerKYC", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrationsNew.datatable.RetrieveData(true);
        });
    }
    Desertfire.CustomerRegistrationsNew.ApprovePolicy = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Customer/DisApproveCustomerKYC", { id: selectedids }, function (data) {
            Desertfire.CustomerRegistrationsNew.datatable.RetrieveData(true);
        });
    }
}


//@S Notifications
Desertfire.NotificationsList = {};
Desertfire.NotificationsList.datatable_setup = function (parent) {
    Desertfire.NotificationsList.datatable = new DataTableWrapper($(parent), 'NotificationDatatable', false);
    var dt = Desertfire.NotificationsList.datatable;
    dt.DataSource = '/Agent/GetAllNotifications';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Message', 'NotificationMessage');
    dt.AddColumn('Date', 'notifyDate');
    //dt.AddColumn('Status', 'StatusId', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "<span class='label label-success'>Active</span>"
    //        } else if (data == 2) {
    //            return "<span class='label label-primary'>In-Active</span>"
    //        }
    //        return "";
    //    }
    //});

    //dt.AddActionDeleteButton('DisApprove Customer', function () {
    //    Desertfire.NotificationsList.ApprovePolicy(dt.GetSelectedAttribute('id'));
    //    var hubs = dt.GetSelectedAttribute('id');
    //}, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    //Desertfire.NotificationsList.ApprovePolicy = function (id) {
    //    var selectedids = id.toString();
    //    Comm.send_json("/Agent/DisApproveCustomerKYC", { id: selectedids }, function (data) {
    //        Desertfire.NotificationsList.datatable.RetrieveData(true);
    //    });
    //}
}


Desertfire.DoctorRegistration.datatable_setup = function (parent) {
    Desertfire.DoctorRegistration.datatable = new DataTableWrapper($(parent), 'DoctorDatatable', false);

    var dt = Desertfire.DoctorRegistration.datatable;

    dt.DataSource = '/Doctor/GetAllDoctor';
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

    dt.AddActionEditButton('Edit Doctor', function () {
        Desertfire.ChangeLocation("/Doctor/EditDoctor/" + dt.GetSelectedAttribute('id'));
    });

    dt.AddActionNewButton('Add New Doctor', function () {
        Desertfire.ChangeLocation("/Doctor/AddEditDoctor/0");
    });

    dt.AddActionNewButton('Export Data', function () {
        Desertfire.ChangeLocation("/Doctor/ExportSuperadmin/0");
    });
    dt.AddActionDeleteButton('Delete Doctor', function () {
        Desertfire.DoctorRegistration.DeleteCustomers(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.DoctorRegistration.DeleteCustomers = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Doctor/DeleteDoctor", { id: selectedids }, function (data) {
            Desertfire.DoctorRegistration.datatable.RetrieveData(true);
        });
    }
}





Desertfire.AgentRegistrationew.InactiveUserData1 = function (parent) {
    //alert("hELLO");
    Desertfire.AgentRegistrationew.InactiveUserData1 = new DataTableWrapper($(parent), 'questionTable', false);
    var dt = Desertfire.AgentRegistrationew.InactiveUserData1;


    dt.DataSource = '/Customer/TotalPolicyForRhead';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('#', 'appointeeAddress');
    dt.AddColumn('Name', 'CustomerCode');
    dt.AddColumn('Contact Person', 'CustomerCode');
    dt.AddColumn('Subscription End Date', 'CustomerCode');
    dt.AddColumn('Subscription Type', 'CustomerCode');
    dt.AddColumn('Deals', 'CustomerCode');


    dt.AddColumn('Subscription End Date', 'RelationWithNominee', {
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
            return "";
        }
    });

    dt.AddActionEditButton('Edit Zone', function () {
        Desertfire.ChangeLocation("/Master/EditZone/" + dt.GetSelectedAttribute('id'));
    });

    dt.AddActionDeleteButton('Delete Zone', function () {
        Desertfire.AgentRegistrationew.DeleteQuestions(dt.GetSelectedAttribute('id'));
        var hubs = dt.GetSelectedAttribute('id');
    }, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.AgentRegistrationew.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteZone", { id: selectedids }, function (data) {
            Desertfire.AgentRegistrationew.datatable.RetrieveData(true);
        });
    }

}

//it is use in user add edit or delete VP 
Desertfire.User.datatable_setup = function (parent) {
    //alert('');
    Desertfire.User.datatable = new DataTableWrapper($(parent), 'UserDisplayDataTable', false);
    var dt = Desertfire.User.datatable;
    dt.DataSource = '/Master/GetAllUserCheckingApp';
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
    //        return '<a href="#" onclick="Desertfire.User.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});
    dt.AddColumn('FirstName', 'FirstName');
    dt.AddColumn('UserName', 'UserName');
    dt.AddColumn('Mobile', 'Mobile');
    dt.AddColumn('Role Name', 'RoleName');
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

    dt.AddActionDeleteButton('Delete User', function () {
        Desertfire.User.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });
    //dt.AddActionEditButton('Add New User', function () {
    //    Desertfire.ChangeLocation("/CheckingAppAdmin/UserRegistration/" + dt.GetSelectedAttribute('Id'));
    //});
    dt.AddActionEditButton('Edit User', function () {
        Desertfire.ChangeLocation("/Master/EditUser/" + dt.GetSelectedAttribute('Id'));
    });
    dt.AddActionNewButton('Add New User', function () {
        Desertfire.ChangeLocation("/Master/UserRegistration/0");
            });

   
    //dt.AddActionApproveButton('Verified User', function () {
    //    Desertfire.User.ApproveMall(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { UseRightMenu: true });

    //dt.AddActionDisApproveButton('DisApprove User', function () {
    //    Desertfire.User.DisapproveMall(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { UseRightMenu: true });


    //dt.AddActionDisApproveButton('Send Mail', function () {
    //    Desertfire.User.SendmailTurnover(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { UseRightMenu: true });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.User.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DeleteUser", { id: selectedids }, function (data) {
            Desertfire.User.datatable.RetrieveData(true);
        });
    }
    Desertfire.User.ApproveMall = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/ApproveUser", { id: selectedids }, function (data) {
            Desertfire.User.datatable.RetrieveData(true);
        });
    }

    Desertfire.User.DisapproveMall = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/DisApproveUser", { id: selectedids }, function (data) {
            Desertfire.User.datatable.RetrieveData(true);
        });
    }

}







Desertfire.LeadUpload.datatable_setup = function (parent) {

    Desertfire.LeadUpload.datatable = new DataTableWrapper($(parent), 'LeadUploadDisplayDataTable', false);
    var dt = Desertfire.LeadUpload.datatable;
    dt.DataSource = '/CheckingAppAdmin/GetAllLeadAllotmenet';
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
    //        return '<a href="#" onclick="Desertfire.LeadUpload.GetSingleLeadUploadTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('MobileNo', 'MobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('EMail', 'EMail');
    //dt.AddColumn('Comment', 'Comment');
    //dt.AddColumn('User Name', 'UserName');
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

 

    //dt.AddActionNewButton('Select Lead', function () {
    //        Desertfire.ChangeLocation("/CheckingAppAdmin/UpdateUserInLead/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionNewButton('Select Lead', function () {
    //    Desertfire.ChangeLocation("/CheckingAppAdmin/UpdateUserInLead/" + dt.GetSelectedAttribute('Id: selectedids'));
    //});

    //dt.AddActionNewButton('Add New LeadUpload', function () {
    //    Desertfire.ChangeLocation("/CheckingAppAdmin/LeadUploadRegistration/0");
    //});



    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.LeadUpload.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/CheckingAppAdmin/UpdateUserInLead", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select leads First")
            }
            Desertfire.LeadUpload.datatable.RetrieveData(true);
        });
    }

    dt.AddActionDeleteButton('Click here for assign leads', function () {
        Desertfire.LeadUpload.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { LeadightMenu: true });

    //Desertfire.LeadUpload.DeleteQuestions = function (id) {
    //    var selectedids = id.toString();
    //    Comm.send_json("/Master/DeleteLeadUpload", { id: selectedids }, function (data) {
    //        Desertfire.LeadUpload.datatable.RetrieveData(true);
    //    });
    //}

    //dt.AddActionDeleteButton('Delete LeadUpload', function () {
    //    Desertfire.LeadUpload.DeleteQuestions(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { LeadUploadightMenu: true });



}


Desertfire.LeadAlloted.datatable_setup = function (parent) {

    Desertfire.LeadAlloted.datatable = new DataTableWrapper($(parent), 'LeadAllotedDisplayDataTable', false);
    var dt = Desertfire.LeadAlloted.datatable;
    //dt.DataSource = '/CheckingAppAdmin/GetAllLead';
    dt.DataSource = '/CheckingAppAdmin/GetAllLeadAllotmenet';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
   
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('MobileNo', 'MobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('EMail', 'EMail');
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
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.LeadAlloted.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/CheckingAppAdmin/DeleteLead", { id: selectedids }, function (data) {
            Desertfire.LeadAlloted.datatable.RetrieveData(true);
        });
    }

    dt.AddActionDeleteButton('Delete Lead', function () {
        Desertfire.LeadAlloted.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { LeadightMenu: true });


}



Desertfire.Abc.datatable_setup = function (parent) {

    Desertfire.Abc.datatable = new DataTableWrapper($(parent), 'LeadTableDataTable', false);
    var dt = Desertfire.Abc.datatable;
    dt.DataSource = '/CheckingAppAdmin/GetAllLead';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
   
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('MobileNo', 'MobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('EMail', 'EMail');
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
    dt.AddActionDeleteButton('Delete Lead', function () {
        Desertfire.Abc.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });

    dt.AddActionEditButton('Edit Lead', function () {
        Desertfire.ChangeLocation("/CheckingAppAdmin/EditLead/" + dt.GetSelectedAttribute('Id'));
    });

    dt.AddActionNewButton('Add New Lead', function () {
        Desertfire.ChangeLocation("/CheckingAppAdmin/LeadRegistration/0");
    });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Abc.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/CheckingAppAdmin/DeleteLead", { id: selectedids }, function (data) {
            Desertfire.Abc.datatable.RetrieveData(true);
        });
    }

}






Desertfire.LeadStatus.datatable_setup = function (parent) {

    Desertfire.LeadStatus.datatable = new DataTableWrapper($(parent), 'LeadTableDataTable', false);
    var dt = Desertfire.LeadStatus.datatable;
    dt.DataSource = '/CheckingAppAdmin/GetAllLeadByStatus';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('LeadName', 'LeadName');
    dt.AddColumn('LeadMobileNo', 'LeadMobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('Email', 'Email');
    //dt.AddColumn('Comment', 'Comment');
    //dt.AddColumn('Status', 'Status', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "<span class='label label-success'>Active</span>"

    //        } else if (data == 2) {
    //            return "<span class='label label-primary'>In-Active</span>"
    //        }
    //        else if (data == 3) {
    //            return "<span class='label label-warning'>Pending</span>"
    //        }
    //        else if (data == 4) {
    //            return "<span class='label label-danger'>Deleted</span>"
    //        }
    //        else if (data == 5) {
    //            return "<span class='label label-success'>Verified</span>"
    //        }
    //        else if (data == 6) {
    //            return "<span class='label label-primary'>Unverified</span>"
    //        }
    //        return "";
    //    }
    //});
    dt.AddActionDeleteButton('Delete Lead', function () {
        Desertfire.LeadStatus.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { UseRightMenu: true });

    dt.AddActionEditButton('Edit Lead', function () {
        Desertfire.ChangeLocation("/CheckingAppAdmin/EditLead/" + dt.GetSelectedAttribute('Id'));
    });

    dt.AddActionNewButton('Add New Lead', function () {
        Desertfire.ChangeLocation("/CheckingAppAdmin/LeadRegistration/0");
    });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.LeadStatus.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/CheckingAppAdmin/DeleteLead", { id: selectedids }, function (data) {
            Desertfire.LeadStatus.datatable.RetrieveData(true);
        });
    }

}



//upload code ...vp

Desertfire.Lead.datatable_setup = function (parent) {

    Desertfire.Lead.datatable = new DataTableWrapper($(parent), 'LeadAllotedDisplayDataTable', false);
    var dt = Desertfire.Lead.datatable;
    //dt.DataSource = '/CheckingAppAdmin/GetAllLead';
    //dt.DataSource = '/CheckingAppAdmin/GetAllCustomersNew';
    dt.DataSource = '/UploadFile/GetAllCustomersNew';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Name', 'Name');
    dt.AddColumn('MobileNo', 'MobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('EMail', 'EMail');
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
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.Lead.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/UploadFile/DeleteCustomer", { id: selectedids }, function (data) {
            Desertfire.Lead.datatable.RetrieveData(true);
        });
    }

    dt.AddActionDeleteButton('Delete Customer', function () {
        Desertfire.Lead.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { LeadightMenu: true });


}
Desertfire.CustomerChkDone = {};
Desertfire.CustomerChkDone.datatable_setup = function (parent) {

    Desertfire.CustomerChkDone.datatable = new DataTableWrapper($(parent), 'CustomerChkDoneDisplayDataTable', false);
    var dt = Desertfire.CustomerChkDone.datatable;
    //dt.DataSource = '/CheckingAppAdmin/GetAllLead';
    //dt.DataSource = '/CheckingAppAdmin/GetAllCustomersNew';
    dt.DataSource = '/UploadFile/GetAllCustomerschkingDone';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Name', 'Name');
    dt.AddColumn('MobileNo', 'MobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('EMail', 'EMail');
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
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.CustomerChkDone.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/UploadFile/DeleteCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerChkDone.datatable.RetrieveData(true);
        });
    }

    dt.AddActionDeleteButton('Delete Customer', function () {
        Desertfire.CustomerChkDone.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { LeadightMenu: true });


}
Desertfire.CustomerChkDue = {};
Desertfire.CustomerChkDue.datatable_setup = function (parent) {

    Desertfire.CustomerChkDue.datatable = new DataTableWrapper($(parent), 'CustomerChkDueDisplayDataTable', false);
    var dt = Desertfire.CustomerChkDue.datatable;
    //dt.DataSource = '/CheckingAppAdmin/GetAllLead';
    //dt.DataSource = '/CheckingAppAdmin/GetAllCustomersNew';
    dt.DataSource = '/UploadFile/GetAllCustomersCheckingDue';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });

    dt.AddColumn('Name', 'Name');
    dt.AddColumn('MobileNo', 'MobileNo');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('EMail', 'EMail');
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
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
    Desertfire.CustomerChkDue.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/UploadFile/DeleteCustomer", { id: selectedids }, function (data) {
            Desertfire.CustomerChkDue.datatable.RetrieveData(true);
        });
    }

    dt.AddActionDeleteButton('Delete Customer', function () {
        Desertfire.CustomerChkDue.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { LeadightMenu: true });


}
Desertfire.CustomerAllotement = {};
Desertfire.CustomerAllotement.datatable_setup = function (parent) {

    Desertfire.CustomerAllotement.datatable = new DataTableWrapper($(parent), 'CustomerAllotementDisplayDataTable', false);
    var dt = Desertfire.CustomerAllotement.datatable;
    dt.DataSource = '/Master/GetAllCustomer'; //'/CheckingAppAdmin/GetAllLeadAllotmenet';
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
    //        return '<a href="#" onclick="Desertfire.LeadUpload.GetSingleLeadUploadTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
    //    }
    //});
    dt.AddColumn('ConsumerId', 'ConsumerId');
    dt.AddColumn('ConsumerNumber', 'ConsumerNumber');
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('AreaName', 'AreaName');
    dt.AddColumn('ConsumerStatus', 'ConsumerStatus'); 
    dt.AddColumn('MobileNo', 'MobileNo');
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



    //dt.AddActionNewButton('Select Lead', function () {
    //        Desertfire.ChangeLocation("/CheckingAppAdmin/UpdateUserInLead/" + dt.GetSelectedAttribute('Id'));
    //});
    //dt.AddActionNewButton('Select Lead', function () {
    //    Desertfire.ChangeLocation("/CheckingAppAdmin/UpdateUserInLead/" + dt.GetSelectedAttribute('Id: selectedids'));
    //});

    //dt.AddActionNewButton('Add New LeadUpload', function () {
    //    Desertfire.ChangeLocation("/CheckingAppAdmin/LeadUploadRegistration/0");
    //});



    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.CustomerAllotement.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/UpdateAlltedCustomer", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select customer First")
            }
            Desertfire.CustomerAllotement.datatable.RetrieveData(true);
        });
    }

    dt.AddActionDeleteButton('Click here for assign Customer', function () {
        Desertfire.CustomerAllotement.DeleteQuestions(dt.GetSelectedAttribute('Id'));
        var hubs = dt.GetSelectedAttribute('Id');
    }, { LeadightMenu: true });

    //Desertfire.LeadUpload.DeleteQuestions = function (id) {
    //    var selectedids = id.toString();
    //    Comm.send_json("/Master/DeleteLeadUpload", { id: selectedids }, function (data) {
    //        Desertfire.LeadUpload.datatable.RetrieveData(true);
    //    });
    //}

    //dt.AddActionDeleteButton('Delete LeadUpload', function () {
    //    Desertfire.LeadUpload.DeleteQuestions(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { LeadUploadightMenu: true });



}

// script for showing report
Desertfire.TotalCustomer = {};
Desertfire.TotalCustomer.datatable_setup = function (parent) {

    Desertfire.TotalCustomer.datatable = new DataTableWrapper($(parent), 'TotalCustomerDisplayDataTable', false);
    var dt = Desertfire.TotalCustomer.datatable;
    dt.DataSource = '/UploadFile/GetTotalCustomerRecords'; 
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});
   
    dt.AddColumn('ConsumerId', 'ConsumerId');
    dt.AddColumn('ConsumerNumber', 'ConsumerNumber');
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('City', 'City');
    dt.AddColumn('AreaName', 'AreaName');
   
    dt.AddColumn('EMail', 'EMail');
    dt.AddColumn('ServiceDue', 'ServiceDue');
    dt.AddColumn('CheckingStatus', 'CheckingStatus');
    

    //dt.AddColumn('Status', 'StatusId', {
    //    mRender: function (data) {
    //        if (data == 1) {
    //            return "<span class='label label-success'>Active</span>"

    //        } else if (data == 2) {
    //            return "<span class='label label-primary'>In-Active</span>"
    //        }
    //        else if (data == 3) {
    //            return "<span class='label label-warning'>Pending</span>"
    //        }
    //        else if (data == 4) {
    //            return "<span class='label label-danger'>Deleted</span>"
    //        }
    //        else if (data == 5) {
    //            return "<span class='label label-success'>Verified</span>"
    //        }
    //        else if (data == 6) {
    //            return "<span class='label label-primary'>Unverified</span>"
    //        }
    //        return "";
    //    }
   // });



    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.TotalCustomer.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/UpdateAlltedCustomer", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select customer First")
            }
            Desertfire.TotalCustomer.datatable.RetrieveData(true);
        });
    }

    //dt.AddActionDownloadCsvButton('Export into Csv file', 'icon-arrow-down', 'btn-success', function () {
    //}, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });
    //dt.AddActionDeleteButton('Click here for assign Customer', function () {
    //    Desertfire.TotalCustomer.DeleteQuestions(dt.GetSelectedAttribute('Id'));
    //    var hubs = dt.GetSelectedAttribute('Id');
    //}, { LeadightMenu: true });

   



}

Desertfire.TotalDueCustomer = {};
Desertfire.TotalDueCustomer.datatable_setup = function (parent) {

    Desertfire.TotalDueCustomer.datatable = new DataTableWrapper($(parent), 'TotalCustomerDisplayDataTable', false);
    var dt = Desertfire.TotalDueCustomer.datatable;
    dt.DataSource = '/UploadFile/GetTotalDueCustomerRecords';
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});

    dt.AddColumn('ConsumerId', 'ConsumerId');
    dt.AddColumn('ConsumerNumber', 'ConsumerNumber');
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('City', 'City');
    dt.AddColumn('AreaName', 'AreaName');

    dt.AddColumn('EMail', 'EMail');
    dt.AddColumn('ServiceDue', 'ServiceDue');
    dt.AddColumn('CheckingStatus', 'CheckingStatus');

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.TotalCustomer.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/UpdateAlltedCustomer", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select customer First")
            }
            Desertfire.TotalCustomer.datatable.RetrieveData(true);
        });
    }


}

Desertfire.TotalDoneCustomer = {};
Desertfire.TotalDoneCustomer.datatable_setup = function (parent) {

    Desertfire.TotalDoneCustomer.datatable = new DataTableWrapper($(parent), 'TotalCustomerDisplayDataTable', false);
    var dt = Desertfire.TotalDoneCustomer.datatable;
    dt.DataSource = '/UploadFile/GetTotalDoneCustomerRecords';
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});

    dt.AddColumn('ConsumerId', 'ConsumerId');
    dt.AddColumn('ConsumerNumber', 'ConsumerNumber');
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Address', 'Address');
    dt.AddColumn('City', 'City');
    dt.AddColumn('AreaName', 'AreaName');

    dt.AddColumn('EMail', 'EMail');
    dt.AddColumn('ServiceDue', 'ServiceDue');
    dt.AddColumn('CheckingStatus', 'CheckingStatus');

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.TotalCustomer.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/UpdateAlltedCustomer", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select customer First")
            }
            Desertfire.TotalCustomer.datatable.RetrieveData(true);
        });
    }


}

Desertfire.TotalService = {};
Desertfire.TotalService.datatable_setup = function (parent) {

    Desertfire.TotalService.datatable = new DataTableWrapper($(parent), 'TotalCustomerDisplayDataTable', false);
    var dt = Desertfire.TotalService.datatable;
    dt.DataSource = '/UploadFile/GetTotalService';
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});

    dt.AddColumn('ConsumerId', 'ConsumerId');
    dt.AddColumn('ConsumerNumber', 'ConsumerNumber');
    dt.AddColumn('PaymentRecived', 'Name');
    dt.AddColumn('PaymentMode', 'Address');
    dt.AddColumn('PendingReason', 'City');
    dt.AddColumn('ServiceStatus', 'AreaName');

    dt.AddColumn('TotalCharge', 'EMail');
    //dt.AddColumn('ServiceDue', 'ServiceDue');
    //dt.AddColumn('CheckingStatus', 'CheckingStatus');

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.TotalService.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/UpdateAlltedCustomer", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select customer First")
            }
            Desertfire.TotalService.datatable.RetrieveData(true);
        });
    }


}
Desertfire.TotalDoneService = {};
Desertfire.TotalDoneService.datatable_setup = function (parent) {

    Desertfire.TotalDoneService.datatable = new DataTableWrapper($(parent), 'TotalCustomerDisplayDataTable', false);
    var dt = Desertfire.TotalDoneService.datatable;
    dt.DataSource = '/UploadFile/GetTotalDoneService';
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});

    dt.AddColumn('ConsumerId', 'ConsumerId');
    dt.AddColumn('ConsumerNumber', 'ConsumerNumber');
    dt.AddColumn('PaymentRecived', 'Name');
    dt.AddColumn('PaymentMode', 'Address');
    dt.AddColumn('PendingReason', 'City');
    dt.AddColumn('ServiceStatus', 'AreaName');

    dt.AddColumn('TotalCharge', 'EMail');
    //dt.AddColumn('ServiceDue', 'ServiceDue');
    //dt.AddColumn('CheckingStatus', 'CheckingStatus');

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.TotalDoneService.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/UpdateAlltedCustomer", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select customer First")
            }
            Desertfire.TotalDoneService.datatable.RetrieveData(true);
        });
    }


}
Desertfire.TotalDueService = {};
Desertfire.TotalDueService.datatable_setup = function (parent) {

    Desertfire.TotalDueService.datatable = new DataTableWrapper($(parent), 'TotalCustomerDisplayDataTable', false);
    var dt = Desertfire.TotalDueService.datatable;
    dt.DataSource = '/UploadFile/GetTotalDueService';
    dt.RetrieveData(true);
    //dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
    //    bVisible: true, bSearchable: false, bSortable: false,
    //    mRender: function () {
    //        return '<input type="checkbox" class="select-checkbox checkboxes" />';
    //    }
    //});

    dt.AddColumn('ConsumerId', 'ConsumerId');
    dt.AddColumn('ConsumerNumber', 'ConsumerNumber');
    dt.AddColumn('PaymentRecived', 'Name');
    dt.AddColumn('PaymentMode', 'Address');
    dt.AddColumn('PendingReason', 'City');
    dt.AddColumn('ServiceStatus', 'AreaName');

    dt.AddColumn('TotalCharge', 'EMail');
    //dt.AddColumn('ServiceDue', 'ServiceDue');
    //dt.AddColumn('CheckingStatus', 'CheckingStatus');

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();

    Desertfire.TotalDueService.DeleteQuestions = function (id) {
        var selectedids = id.toString();
        Comm.send_json("/Master/UpdateAlltedCustomer", { id: selectedids }, function (data) {

            if (selectedids == "") {
                alert("Please select customer First")
            }
            Desertfire.TotalDueService.datatable.RetrieveData(true);
        });
    }


}
//Desertfire.Rates.datatable_setup={};
//Desertfire.Rates.datatable_setup = function (parent) {
//    //alert('');
//    Desertfire.Rates.datatable = new DataTableWrapper($(parent), 'RatesDisplayDataTable', false);
//    var dt = Desertfire.Rates.datatable;
//    dt.DataSource = '/ServiceRates/GetAllRates';//'/Master/GetAllUserCheckingApp';
//    dt.RetrieveData(true);
//    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
//        bVisible: true, bSearchable: false, bSortable: false,
//        mRender: function () {
//            return '<input type="checkbox" class="select-checkbox checkboxes" />';
//        }
//    });
//    //dt.AddColumn('', 'Id', {
//    //    bVisible: true, bSearchable: false, bSortable: false,
//    //    mRender: function (data) {
//    //        return '<a href="#" onclick="Desertfire.User.GetSingleUserTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
//    //    }
//    //});
//    dt.AddColumn('ServiceCharge', 'ServiceCharge');
//    dt.AddColumn('OneBurner', 'OneBurner');
//    dt.AddColumn('TwoBurner', 'TwoBurner');
//    dt.AddColumn('ThreeBurner', 'ThreeBurner');
//    dt.AddColumn('FourBurner', 'FourBurner');
//    dt.AddColumn('O_Ring', 'O_Ring');
//    dt.AddColumn('PresuerRegulator', 'PresuerRegulator');
//    dt.AddColumn('Suraksha_RubberTube', 'Suraksha_RubberTube');
//    dt.AddColumn('Suraksha_RubberClip', 'Suraksha_RubberClip');
//    dt.AddColumn('LPG_Valve', 'LPG_Valve');
//    dt.AddColumn('MixingTube', 'MixingTube');
//    dt.AddColumn('Assambali_Rood', 'Assambali_Rood');
//    dt.AddColumn('Burner', 'Burner');
//    dt.AddColumn('Tell_Support', 'Tell_Support');
//    dt.AddColumn('NOB', 'NOB');
//    dt.AddColumn('LEG', 'LEG');
//    //dt.AddColumn('Status', 'StatusId', {
//    //    mRender: function (data) {
//    //        if (data == 1) {
//    //            return "<span class='label label-success'>Active</span>"

//    //        } else if (data == 2) {
//    //            return "<span class='label label-primary'>In-Active</span>"
//    //        }
//    //        else if (data == 3) {
//    //            return "<span class='label label-warning'>Pending</span>"
//    //        }
//    //        else if (data == 4) {
//    //            return "<span class='label label-danger'>Deleted</span>"
//    //        }
//    //        else if (data == 5) {
//    //            return "<span class='label label-success'>Verified</span>"
//    //        }
//    //        else if (data == 6) {
//    //            return "<span class='label label-primary'>Unverified</span>"
//    //        }
//    //        return "";
//    //    }
//    //});

//    dt.AddActionDeleteButton('Delete User', function () {
//        Desertfire.Rates.DeleteQuestions(dt.GetSelectedAttribute('Id'));
//        var hubs = dt.GetSelectedAttribute('Id');
//    }, { UseRightMenu: true });
//    dt.AddActionEditButton('Add New User', function () {
//        Desertfire.ChangeLocation("/CheckingAppAdmin/UserRegistration/" + dt.GetSelectedAttribute('Id'));
//    });
//    dt.AddActionEditButton('Edit User', function () {
//        Desertfire.ChangeLocation("/Master/EditUser/" + dt.GetSelectedAttribute('Id'));
//    });
//    //dt.AddActionNewButton('Add New User', function () {
//    //    Desertfire.ChangeLocation("/Master/UserRegistration/0");
//    //});


//    //dt.AddActionApproveButton('Verified User', function () {
//    //    Desertfire.User.ApproveMall(dt.GetSelectedAttribute('Id'));
//    //    var hubs = dt.GetSelectedAttribute('Id');
//    //}, { UseRightMenu: true });

//    //dt.AddActionDisApproveButton('DisApprove User', function () {
//    //    Desertfire.User.DisapproveMall(dt.GetSelectedAttribute('Id'));
//    //    var hubs = dt.GetSelectedAttribute('Id');
//    //}, { UseRightMenu: true });


//    //dt.AddActionDisApproveButton('Send Mail', function () {
//    //    Desertfire.User.SendmailTurnover(dt.GetSelectedAttribute('Id'));
//    //    var hubs = dt.GetSelectedAttribute('Id');
//    //}, { UseRightMenu: true });

//    Desertfire.datatable.setup(dt);
//    dt.selectableWindowsStyleWithCheckbox();
//    Desertfire.Rates.DeleteQuestions = function (id) {
//        var selectedids = id.toString();
//        Comm.send_json("/Master/DeleteUser", { id: selectedids }, function (data) {
//            Desertfire.Rates.datatable.RetrieveData(true);
//        });
//    }
//    Desertfire.Rates.ApproveMall = function (id) {
//        var selectedids = id.toString();
//        Comm.send_json("/Master/ApproveUser", { id: selectedids }, function (data) {
//            Desertfire.Rates.datatable.RetrieveData(true);
//        });
//    }

//    Desertfire.Rates.DisapproveMall = function (id) {
//        var selectedids = id.toString();
//        Comm.send_json("/Master/DisApproveUser", { id: selectedids }, function (data) {
//            Desertfire.Rates.datatable.RetrieveData(true);
//        });
//    }

//}









