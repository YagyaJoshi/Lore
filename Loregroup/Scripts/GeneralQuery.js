Desertfire.GeneralQuery = {};
Desertfire.GeneralQuery.Relation = 6;
Desertfire.GeneralQuery.GeneralQueryUpdates = {};
Desertfire.GeneralQuery.GeneralQueryUpdates.Relation = 7;
Desertfire.GeneralQuery.datatable_setup = function (parent) {
    Desertfire.WorkOrders.datatable = new DataTableWrapper($(parent), 'generalQueriesDatatable', false);
    var dt = Desertfire.WorkOrders.datatable;
    
    dt.DataSource = '/GeneralQueries/GetAllGeneralQueries';
    
    dt.RetrieveData(true);

    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Business', 'BusinessViewModel', {
        mRender: function (data) {
        return data.Name;
    }});

    dt.AddColumn('Email', 'Email');
    dt.AddColumn('GeneralQueryType', 'GeneralQueryType');

    dt.AddActionButton('Comments', 'fa fa-fw fa-comment-o', 'btn-primary', function () {
        Desertfire.ChangeLocation("/GeneralQueries/GeneralQueryComments/" + dt.GetSelectedAttribute('Id'));
    }, { UseRightMenu: true });
    dt.AddActionNewButton('Add New GeneralQuery', function () {
        Desertfire.ChangeLocation("/GeneralQueries/AddEditGeneralQuery/0");
    });
    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/GeneralQueries/AddEditGeneralQuery/" + dt.GetSelectedAttribute('Id'));
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

//Desertfire.WorkOrders.UploadFiles = function() {
//    var uploadFiles = new Upload(Desertfire.WorkOrders);
//}
//Desertfire.WorkOrders.UploadCallback = function (data) {
//    console.log(data);
//    if (data.result.Result) {
//        var comparitiveId = $(".hdnFiles").length;
//        $("#uploadFiles").parent(".form-group").append('<input class="hdnFiles" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="FileDetailViewModels_' + comparitiveId + '__Id" name="FileDetailViewModels[' + comparitiveId + '].Id" type="hidden" value="' + data.result.ImageId + '">');
//        $("#uploadFiles").parent(".form-group").append('<button class="btn btn-primary pull-right downloadFile" style="margin-right: 5px;" data-fileId="' + data.result.ImageId + '"><i class="fa fa-download"></i> ' + data.result.File + '</button>');
//    }
//    //console.log(data.result);
//}

Desertfire.GeneralQuery.GeneralQueryUpdates.UploadFiles = function () {
    var uploadFiles = new Upload(Desertfire.GeneralQuery.GeneralQueryUpdates);
}
Desertfire.GeneralQuery.GeneralQueryUpdates.UploadCallback = function (data) {
    console.log(data);
    if (data.result.Result) {
        var comparitiveId = $(".hdnUpdateFiles").length;
        $("#updateUploadFiles").parent(".form-group").append('<input class="hdnUpdateFiles" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="FileDetailViewModels_' + comparitiveId + '__Id" name="FileDetailViewModels[' + comparitiveId + '].Id" type="hidden" value="' + data.result.ImageId + '">');
        $("#updateUploadFiles").parent(".form-group").append('<a class="btn btn-sm btn-info pull-right downloadFile" style="margin-right: 5px;" href="' + Desertfire.RootUrl + "Contents/File/" + data.result.ImageId + '" data-fileId="' + data.result.ImageId + '"><i class="fa fa-download"></i> ' + data.result.File + '</a>');
    }
    //console.log(data.result);
}