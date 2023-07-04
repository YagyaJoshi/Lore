Desertfire.Business = {};
Desertfire.Business.Products = {};
Desertfire.Business.ProductParts = {};
Desertfire.CustomerRegistrationem = {};

// business
Desertfire.Business.datatable_setup = function (parent) {
    Desertfire.Business.datatable = new DataTableWrapper($(parent), 'businessDatatable', false);
    var dt = Desertfire.Business.datatable;
    
    dt.DataSource = '/Business/GetAllBusinesses';
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
            return '<a href="#" onclick="Desertfire.Business.GetSingleBusinessTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Description', 'Description');
    dt.AddColumn('Verified', 'IsVerified');
    dt.AddColumn('Description', 'Description');

    dt.AddActionNewButton('Add New Business', function () {
        Desertfire.ChangeLocation("/Business/AddEditBusiness/0");
    });
    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Business/AddEditBusiness/" + dt.GetSelectedAttribute('Id'));
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
Desertfire.Business.SetupProductsSelectionContainer = function () {
    var productsList = [];

    Comm.send_json("/Business/GetAllProducts", { size: 0, page: 0 }, function (data) {
        for (var i = 0; i < data.List.length; i++) {
            productsList.push({ id: data.List[i].Id, value: data.List[i].Name });
        }
        var alreadyAddedProducts = [];
        $(".productId").each(function () {
            var el = $(this);
            var idField = el.val();
            var nameField = $(".productName[id=" + el.attr("id").replace("Id", "Name") + "]").val();
            alreadyAddedProducts.push({ id: idField, value: nameField });
        });

        var tagApi = $("#productsTagsInput").tagsManager({
            prefilled: alreadyAddedProducts,
            validationData: productsList,
            onRemoveTag: function (removedId) {
                var tagRemovedId = $(".productId[value=" + removedId + "]").attr("id");
                var tagRemovedName = $(".productName#" + tagRemovedId.replace("Id", "Name")).attr("id");

                $("#" + tagRemovedId).remove();
                $("#" + tagRemovedName).remove();
            }
        });

        $('#productsTagsInput').typeahead({
            hint: false,
            highlight: true,
            minLength: 1
        },
            {
                name: 'productsList',
                displayKey: 'value',
                source: Desertfire.substringMatcher(productsList)
            })
            .on('typeahead:selected', function (e, d) {
                var totalAddedTags = $(".productId").length;
                var comparitiveId = totalAddedTags;

                $("#productsTagsInput").parents(".form-group").append('<input class="productId" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="ProductViewModels_' + comparitiveId + '__Id" name="ProductViewModels[' + comparitiveId + '].Id" type="hidden" value="' + d.id + '">');
                $("#productsTagsInput").parents(".form-group").append('<input class="productName" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="ProductViewModels_' + comparitiveId + '__Name" name="ProductViewModels[' + comparitiveId + '].Name" type="hidden" value="' + d.value + '">');
                tagApi.tagsManager("pushTag", d);
            });
    });
}
Desertfire.Business.GetSingleBusinessTurnover = function (businessId) {
    Comm.send_json("/Business/GetSingleBusinessDetail", { id: businessId }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

// products
Desertfire.Business.Products.datatable_setup = function (parent) {
    Desertfire.Business.Products.datatable = new DataTableWrapper($(parent), 'productsDatatable', false);
    var dt = Desertfire.Business.Products.datatable;

    dt.DataSource = '/Business/GetAllProducts';
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
            return '<a href="#" onclick="Desertfire.Business.Products.GetSingleProductTurnover(' + data + ');"><i class="fa fa-align-left"></i></a>';
        }
    });
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Material Type', 'MaterialType');
    dt.AddColumn('Status', 'Status');

    dt.AddActionNewButton('Add New Product', function () {
        Desertfire.ChangeLocation("/Business/AddEditProduct/0");
    });

    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Business/AddEditProduct/" + dt.GetSelectedAttribute('Id'));
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
Desertfire.Business.Products.SetupPartsSelectionContainer = function () {
    var productPartsList = [];

    Comm.send_json("/Business/GetAllProductParts", { size: 0, page: 0 }, function (data) {
        for (var i = 0; i < data.List.length; i++) {
            productPartsList.push({ id: data.List[i].Id, value: data.List[i].Name });
        }
        var alreadyAddedParts = [];
        $(".partId").each(function () {
            var el = $(this);
            var idField = el.val();
            var nameField = $(".partName[id=" + el.attr("id").replace("Id", "Name") + "]").val();
            alreadyAddedParts.push({ id: idField, value: nameField });
        });

        var tagApi = $("#productPartsTagsInput").tagsManager({
            prefilled: alreadyAddedParts,
            validationData: productPartsList,
            onRemoveTag: function (removedId) {
                var tagRemovedId = $(".partId[value=" + removedId + "]").attr("id");
                var tagRemovedName = $(".partName#" + tagRemovedId.replace("Id", "Name")).attr("id");

                $("#" + tagRemovedId).remove();
                $("#" + tagRemovedName).remove();
            }
        });

        $('#productPartsTagsInput').typeahead({
            hint: false,
            highlight: true,
            minLength: 1
        },
            {
                name: 'productPartsList',
                displayKey: 'value',
                source: Desertfire.substringMatcher(productPartsList)
            })
            .on('typeahead:selected', function (e, d) {
                var totalAddedTags = $(".partId").length;
                var comparitiveId = totalAddedTags;

                $("#productPartsTagsInput").parents(".form-group").append('<input class="partId" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="ProductPartViewModels_' + comparitiveId + '__Id" name="ProductPartViewModels[' + comparitiveId + '].Id" type="hidden" value="' + d.id + '">');
                $("#productPartsTagsInput").parents(".form-group").append('<input class="partName" data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="ProductPartViewModels_' + comparitiveId + '__Name" name="ProductPartViewModels[' + comparitiveId + '].Name" type="hidden" value="' + d.value + '">');
                tagApi.tagsManager("pushTag", d);
            });
    });
}
Desertfire.Business.Products.GetSingleProductTurnover = function (productId) {
    Comm.send_json("/Business/GetSingleProductDetail", { id: productId }, function (data) {
        if (data.Result) {
            var turnoverData = {
                Content: data.Object,
                Selctor: ''
            };
            Desertfire.attach_turnover(turnoverData, $('<div></div>'));
        }
    });
}

// product parts
Desertfire.Business.ProductParts.datatable_setup = function (parent) {
    Desertfire.Business.ProductParts.datatable = new DataTableWrapper($(parent), 'productPartsDatatable', false);
    var dt = Desertfire.Business.ProductParts.datatable;

    dt.DataSource = '/Business/GetAllProductParts';
    dt.RetrieveData(true);
    dt.AddColumn('<input type="checkbox" class="group-checkable" id="SelectAll"/>', 'Id', {
        bVisible: true, bSearchable: false, bSortable: false,
        mRender: function () {
            return '<input type="checkbox" class="select-checkbox checkboxes" />';
        }
    });
    dt.AddColumn('Name', 'Name');
    dt.AddColumn('Status', 'Status');

    dt.AddActionNewButton('Add New Part', function () {
        Desertfire.ChangeLocation("/Business/AddEditProductPart/0");
    });

    dt.AddActionEditButton('Edit', function () {
        Desertfire.ChangeLocation("/Business/AddEditProductPart/" + dt.GetSelectedAttribute('Id'));
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


Desertfire.CustomerRegistrationem.datatable_setup = function (parent) {
    Desertfire.CustomerRegistrationem.datatable = new DataTableWrapper($(parent), 'customerDatatable', false);

    var dt = Desertfire.CustomerRegistrationem.datatable;

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
  
    Desertfire.datatable.setup(dt);
    dt.selectableWindowsStyleWithCheckbox();
   
}