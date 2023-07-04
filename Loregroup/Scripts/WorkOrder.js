Desertfire.WorkOrders = {};
Desertfire.GetAllCustomersnew = {};
Desertfire.WorkOrders.Relation = 4;
Desertfire.WorkOrders.WorkOrderUpdates = {};
Desertfire.WorkOrders.WorkOrderUpdates.Relation = 5;
Desertfire.WorkOrders.datatable_setup = function (parent) {
    Desertfire.WorkOrders.datatable = new DataTableWrapper($(parent), 'workOrdersDatatable', false);
    var dt = Desertfire.WorkOrders.datatable;
    
    dt.DataSource = '/WorkOrder/GetAllWorkOrders';
    
    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    
    dt.AddColumn('Title', 'Title');
    dt.AddColumn('Business', 'BusinessViewModel', {
        mRender: function (data) {
        return data.Name;
    }});

    dt.AddColumn('Product', 'ProductViewModel', {
        mRender: function(data) {
            return data.Name;
        }
    });
    dt.AddColumn('Work Order', 'WorkOrderType');
    dt.AddColumn('Analysis Technique', 'AnalysisTechnique');
    dt.AddColumn('Work Status', 'WorkStatus');
    dt.AddColumn('Assigned To', 'AssignedToId');

    dt.AddActionButton('Comments', 'fa fa-fw fa-comment-o', 'btn-primary', function () {
        Desertfire.ChangeLocation("/WorkOrder/WorkOrderComments/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/WorkOrder/AddEditWorkOrder/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    dt.AddActionDeleteButton('Delete', function () {
        var hubs = dt.GetSelectedAttribute('Id');
        DeleteStaffByButton(dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    dt.AddActionDownloadCsvButton('', 'icon-arrow-down', 'btn-success', function () {

    }, { UseRightMenu: true, IsDisabled: true, Text: "Export to .csv", DisableFn: function () { this.IsDisabled = true } });

    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
}

Desertfire.WorkOrders.UploadFiles = function() {
    var uploadFiles = new Upload(Desertfire.WorkOrders);
}
Desertfire.WorkOrders.UploadCallback = function (data) {
    //console.log(data);
    if (data.result.Result) {
        var comparitiveId = $(".hdnFiles").length;
        $("#uploadFiles").parent(".form-group").append('<input class="hdnFiles" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="FileDetailViewModels_' + comparitiveId + '__Id" name="FileDetailViewModels[' + comparitiveId + '].Id" type="hidden" value="' + data.result.ImageId + '">');
        $("#uploadFiles").parent(".form-group").append('<a class="btn btn-sm btn-info pull-right downloadFile" style="margin-right: 5px;" href="' + Desertfire.RootUrl + "Contents/File/" + data.result.ImageId + '" data-fileId="' + data.result.ImageId + '"><i class="fa fa-download"></i> ' + data.result.File + '</a>');
    }
    //console.log(data.result);
}


Desertfire.WorkOrders.WorkOrderUpdates.UploadFiles = function () {
    var uploadFiles = new Upload(Desertfire.WorkOrders.WorkOrderUpdates);
}
Desertfire.WorkOrders.WorkOrderUpdates.UploadCallback = function (data) {
    //console.log(data);
    if (data.result.Result) {
        var comparitiveId = $(".hdnUpdateFiles").length;
        $("#updateUploadFiles").parent(".form-group").append('<input class="hdnUpdateFiles" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="FileDetailViewModels_' + comparitiveId + '__Id" name="FileDetailViewModels[' + comparitiveId + '].Id" type="hidden" value="' + data.result.ImageId + '">');
        $("#updateUploadFiles").parent(".form-group").append('<a class="btn btn-sm btn-info pull-right downloadFile" style="margin-right: 5px;" href="' + Desertfire.RootUrl + "Contents/File/" + data.result.ImageId + '" data-fileId="' + data.result.ImageId + '"><i class="fa fa-download"></i> ' + data.result.File + '</a>');
        //$("#updateUploadFiles").parent(".form-group").append('<button class="btn btn-primary pull-right downloadFile" style="margin-right: 5px;" data-fileId="' + data.result.ImageId + '"><i class="fa fa-download"></i> ' + data.result.File + '</button>');
    }
    //console.log(data.result);
}


Desertfire.GetAllCustomersnew.datatable_setup = function (parent) {
    Desertfire.GetAllCustomersnew.datatable = new DataTableWrapper($(parent), 'GetAllCustomersDataTable', false);
    var dt = Desertfire.GetAllCustomersnew.datatable;

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
