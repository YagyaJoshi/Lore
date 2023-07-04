var DataTable = {}
DataTable.settings = function (table, settings) {
    if (settings == null) {
        settings = {};
    }

    var sDom;
    var iPagesize = DataTable.ViewSize;
    var rowcount = settings.aaData ? settings.aaData.length : table.find("tbody tr").length
    sDom = '<"top">rt<"row" <"col-xs-6" <"pull-left rowLength"> i><"col-xs-6" <p>>><"clearfix">';
    settings.bAutoWidth = true; //DataTables will ruin your table if you don't do this
    settings.bPaginate = settings.iDisplayLength == 0 || settings.iDisplayLength > 20 ? false : true
    settings.sScrollY = table.data("scrolly") != null ? table.data("scrolly") : ""
    settings.bInfo = table.data("info") != null ? table.data("info") : true
    settings.sDom = sDom;
    //settings.sPaginationType = "full_numbers"
    settings.lengthMenu = [[10, 25, 50, -1], [10, 25, 50, "All"]];

    if (!settings.aoColumnDefs) {
        settings.aoColumnDefs = [
            { "bSortable": false, "aTargets": ["unsortable"], "bSearchable": false },
            {
                "aTargets": ["actionMenu"], "bSortable": false, "bSearchable": false,
                "fnCreatedCell": fnTableActionMenu
            }
        ];
        table.find("thead th").each(function (i) {
            if ($(this).data("sortcol")) {
                var coldef = {
                    "aTargets": [i], "iDataSort": $(this).data("sortcol")
                };
                settings.aoColumnDefs.push(coldef);
            }
        });
        if (table.hasClass("selectable") || table.find("tbody tr:eq(0) > td:eq(0) > .table-actions [type=checkbox]").length > 0) {
            settings.aoColumnDefs.push({
                "aTargets": [0], "bSortable": false,
                "fnCreatedCell": fnTableCheckbox
            })
        }
    }
    var sort = [];
    table.find(".sort_default").each(function () {
        var thissort = [$(this).index()];
        if ($(this).hasClass("desc")) {
            thissort.push("desc");
        } else {
            thissort.push("asc");
        }
        sort.push(thissort);
    });
    if (sort.length == 0) {
        sort.push([0, 'asc']);
    }
    settings.aaSorting = sort;

    settings.oLanguage = {
        "oPaginate": {
            "sNext": "Next",
            "sLast": "&gtl",
            "sFirst": "l&lt;",
            "sPrevious": "Previous"
        }
    }
    settings.fnInitComplete = function (oSettings, json) {
        //Could be useful for getting extra data!
    }
    return settings;
}
var oDTable;
DataTable.Success = { Content: '<div class="alert-success"></div>' };
DataTable.SelectedClass = 'active';
DataTable.GetAmount = 40;
DataTable.ViewSize = 10;
DataTable.NameColumnWidth = 400;
////var iCheckOpts = {
////    checkboxClass: 'icheckbox_minimal',
////    radioClass: 'iradio_minimal'
////}
var DataTableWrapper = function (parent, id) {
    this.Columns = [];
    this.DataSource = null;
    this.Parent = $(parent);
    this.Id = id;
    this.Actions = [];
    this.Table = null;
    this.DataSourceFn = null;
    this.OnDataLoad = null;
    this.HasData = false;
    this.Ready = false;
    this.Paged = true;
    this.PageLength = 10;
    var dtTemplate = $('#datatable_template').html();
    dtTemplate = Desertfire.Replace(dtTemplate, "TableName", this.Id);

    if ($('#' + this.Id).length == 0) {
        this.Parent.html(dtTemplate);
    }
    $('#' + this.Id).data('container', this);
    this.Table = $('#' + this.Id);
    var prevClick = 0;
    var self = this;
    var keyListener = this.KeyPressListener.bind(this);
    $('body').on('keypress', keyListener);
    $('tbody', this.Table).dblclick(function (event) {
        event.stopPropagation();
    });
    var table = this.Table;
    var lastChecked;
    $('tbody', this.Table).click(function (event) {
        if (event.timeStamp - prevClick > 200) {
            prevClick = event.timeStamp;
            $(event.target.parentNode).toggleClass(DataTable.SelectedClass);
            self.EnableDisableButtons();
        }
    });
    self.ClearWrapper = function () {
        $('body').off('keypress', keyListener);
        this.Table = null;
        this.Parent.html('');
    }
}
DataTableWrapper.prototype.selectableWithCheckbox = function () {
    var self = this;
    var prevClick = 0;
    $('tbody', this.Table).off("click");
    $('tbody', this.Table).click(function (event) {
        if (event.timeStamp - prevClick > 200) {
            prevClick = event.timeStamp;
            if (event.target.nodeName != "TBODY") {
                $(event.target.parentNode).toggleClass(DataTable.SelectedClass);
                var checkbox = $(event.target.parentNode).find(".select-checkbox");
                checkbox.attr("checked", !checkbox.attr("checked"));
                self.EnableDisableButtons();
            }
        }
    });
    $("#SelectAll").on("ifToggled", function (e) {
        var isChecked = $("#SelectAll").is(":checked");
        if (isChecked) {
            $('tr', this.Table).addClass('active');
            $('tr', this.Table).find(".select-checkbox").prop('checked', true);
        } else {
            $('tr',this.Table).removeClass('active');
            $('tr', this.Table).find(".select-checkbox").prop('checked', false);
        }
        //$('tbody', this.Table).find(".checkboxes").uniform();
        ////$('tbody', this.Table).find('.checkboxes').iCheck(iCheckOpts);
        self.EnableDisableButtons();
    });
}
DataTableWrapper.prototype.selectableWindowsStyleWithCheckbox = function () {
    var self = this;
    var table = this.Table;
    //Desertfire.datatable = self;
    var lastChecked;
    $('tbody', this.Table).off("click");
    $('tbody', this.Table).on("click", "tr", function (event) {
        if (!lastChecked) {
            lastChecked = this;
        }

        if (event.shiftKey) {
            if ($(lastChecked).hasClass('active')) {
                var start = $('tbody tr', table).index(this);
                var end = $('tbody tr', table).index(lastChecked);
                if (end > -1) {
                    for (i = Math.min(start, end) ; i <= Math.max(start, end) ; i++) {
                        if (!$('tbody tr', table).eq(i).hasClass('active')) {
                            $('tbody tr', table).eq(i).addClass("active");
                            $('tbody tr', table).eq(i).find(".select-checkbox").prop('checked', true);
                        }
                    }
                } else {
                    $(this).addClass("active");
                }

            } else {
                $(this).addClass("active");
            }
            // Clear browser text selection mask
            if (window.getSelection) {
                if (window.getSelection().empty) { // Chrome
                    window.getSelection().empty();
                } else if (window.getSelection().removeAllRanges) { // Firefox
                    window.getSelection().removeAllRanges();
                }
            } else if (document.selection) { // IE?
                document.selection.empty();
            }
        } else if ((event.metaKey || event.ctrlKey)) {
            $(this).toggleClass('active');
        } else {
            $(this).toggleClass('active');
        }
        $(this).find('.select-checkbox').prop("checked", $(this).hasClass(DataTable.SelectedClass));
        //$(this).find(".checkboxes").eq(0).uniform();
        ////$('.checkboxes').iCheck(iCheckOpts);
        lastChecked = this;
        self.EnableDisableButtons();
        updateSelectAll(table);
    });
    $("#SelectAll").on("ifToggled", function (e) {
        var isChecked = $("#SelectAll").is(":checked");
        if (isChecked) {
            $('tr', this.Table).addClass('active');
            $('tr', this.Table).find(".select-checkbox").prop('checked', true);
        } else {
            $('tr', this.Table).removeClass('active');
            $('tr', this.Table).find(".select-checkbox").prop('checked', false);
        }
        //$('tbody', this.Table).find(".checkboxes").uniform();
        ////$('tbody', this.Table).find('.checkboxes').iCheck(iCheckOpts);
        self.EnableDisableButtons();
    });

    $(table).on('draw.dt', function () {
        updateSelectAll(table);
        $tdCheckBox = $('.select-checkbox, #SelectAll').parent();
        $($tdCheckBox).css('text-align', 'center');
    });
}
function updateSelectAll(table) {
    var areAllSelected = true;
    var isFirst = true;
    $('tr', table).each(function () {
        if (!isFirst) {
            if (!$(this).hasClass('active'))
                areAllSelected = false;
        } else
            isFirst = false;
    });
    if (areAllSelected) {
        $("#SelectAll").prop('checked', true);
    } else {
        $("#SelectAll").prop('checked', false);
    }
    //table.find(".checkboxes").eq(0).uniform();
    ////table.find('.checkboxes').iCheck(iCheckOpts);

}
DataTableWrapper.prototype.selectableWindowsStyle = function () {
    var self = this;
    var table = this.Table;
    var lastChecked;
    $('tbody', this.Table).off("click");
    $('tbody', this.Table).on("click", "tr", function (event) {
        if (!lastChecked) {
            lastChecked = this;
        }

        if (event.shiftKey) {
            var start = $('tbody tr', table).index(this);
            var end = $('tbody tr', table).index(lastChecked);

            for (i = Math.min(start, end) ; i <= Math.max(start, end) ; i++) {
                if (!$('tbody tr', table).eq(i).hasClass('active')) {
                    $('tbody tr', table).eq(i).addClass("active");
                }
            }

            // Clear browser text selection mask
            if (window.getSelection) {
                if (window.getSelection().empty) {  // Chrome
                    window.getSelection().empty();
                } else if (window.getSelection().removeAllRanges) {  // Firefox
                    window.getSelection().removeAllRanges();
                }
            } else if (document.selection) {  // IE?
                document.selection.empty();
            }
        } else if ((event.metaKey || event.ctrlKey)) {
            $(this).toggleClass('active');
        } else {
            self.Deselect();
            $(this).toggleClass('active');
        }

        lastChecked = this;
        self.EnableDisableButtons();
    });
}
DataTableWrapper.prototype.KeyPressListener = function (e) {
    var self = this;
    var keycode = e.keyCode;
    var ctrlKey = e.ctrlKey;
    var altKey = e.altKey;
    var shiftKey = e.shiftKey;
    switch (keycode) {
        case 1:
            if (ctrlKey || altKey) { self.HighlightVisible(); }
            break;
    }
}
DataTableWrapper.prototype.EnableDisableButtons = function () {
    var self = this;
    var numSelected = this.GetSelected().length;
    if (numSelected == 1) {
        self.Actions.forEach(function (action, index) {
            if (action.SingleSelect || action.UseRightMenu || action.AlwaysOn) {
                action.Enable();
            }
        });
    } else if (numSelected == 0) {
        self.Actions.forEach(function (action, index) {
            if (action.UseRightMenu || action.SingleSelect) {
                action.Disable();
            }
            if (action.AlwaysOn) {
                action.Enable();
            }
        });
    } else {
        self.Actions.forEach(function (action, index) {
            if (action.SingleSelect) {
                action.Disable();
            }
            else if (action.UseRightMenu || action.AlwaysOn) {
                action.Enable();
            }
        });
    }
}
DataTableWrapper.prototype.HighlightVisible = function () {
    $('tr:not(.active)', this.Table).click();
    this.EnableDisableButtons();
}
DataTableWrapper.prototype.GetTable = function () {
    return this.Table;
}
DataTableWrapper.prototype.PrintTable = function () {
    var table = $('#' + this.Id);
    var header = $('thead tr', table);
    this.Columns.forEach(function (col) { header.append(col.Print()); });
    var actionMenu = $('#' + this.Id + 'RighRighttActions');
    this.Table = table;
    return this;
}
DataTableWrapper.prototype.MakeDataTable = function (noData) {
    noData = noData || false;
    this.PrintTable();
    var table = $('#' + this.Id);
    var self = this;
    var tablesettings = {
        "iDisplayLength": this.PageLength,
        "fnDrawCallback": this.DrawCallBack,
        "fnPreDrawCallback": this.PreDrawCallBack,
        //"fnRowCallback": this.DrawCallBack,
        "aoColumnDefs": this.Columns,
        "bDeferRender": false,
        "bProcessing": false,
        "fnCreatedRow": function (nRow, aData) {
            $(nRow).addClass("odd gradeX");
            //$(nRow).find('.checkboxes').uniform();
            ////$(nRow).find('.checkboxes').iCheck(iCheckOpts);
        }
    }
    if ($.fn.DataTable.fnIsDataTable(table)) {
        table.dataTable().fnAdjustColumnSizing();
        return;
    }
    //create the form input element for recording selected rows, if applicable
    oDTable = table.dataTable(DataTable.settings(table, tablesettings));
    if (table.data("grouping") != null) {
        table.rowGrouping({
            iGroupingColumnIndex: table.data("grouping"),
            bExpandableGrouping: true
        });
    }

    $.extend($.fn.dataTableExt.oStdClasses, {
        "sSortAsc": "header headerSortDown",
        "sSortDesc": "header headerSortUp",
        "sSortable": "header"
    });
    $('thead th:not(.unsortable)', table).click(function (e) {
        var target = $(e.currentTarget);
        if (target.hasClass('unsortable')) {
            return;
        }
        $('.sorting_asc', table).removeClass("sorting_asc");
        target.addClass("sorting_asc");
    });
    Desertfire.setup_search(this.Parent);
    self.Ready = true;
    self.EnableDisableButtons();
    if (!noData) {
        this.RetrieveData();
    }
    oDTable.bind('filter', function () { self.EnableDisableButtons() });
    var rowLengthDiv = $('.rowLength', $(self.Parent));
    rowLengthDiv.addClass('span2 form-inline dataTables_info');
    rowLengthDiv.html('page size: &nbsp;<select id="rowLengthSelect" class="span1 rowLengthSelect"><option value="10">10</option><option value="15">15</option><option value="25">25</option><option value="50">50</option><option value="100">100</option><option value="-1">All</option></select>');

    $('.rowLengthSelect', $(self.Parent)).off("change").on('change', function () {
        oDTable.fnSettings()._iDisplayLength = parseInt(this.value);
        oDTable.fnDraw();
        $.cookie("rowLength", this.value);
    });

    var cookieValue = $.cookie("rowLength");
    if (cookieValue != null && cookieValue != "") {
        $(".rowLengthSelect option").filter(function () {
            return this.value == cookieValue;
        }).prop('active', true);
        $('.rowLengthSelect').trigger('change');
        // oDTable.fnSettings()._iDisplayLength = cookieValue;
    }
    //oDTable.fnDraw();
}
var $wrapper;
function SetTableTools(table) {
    try {
        // setup table tools csv download button
        var objTableTools = new $.fn.dataTable.TableTools(table, {
            sSwfPath: $("#hdnUrl").val() + "/Scripts/swf/copy_csv_xls.swf", aButtons: [{
                "sExtends": "csv",
                "sButtonText": ""
                
            }]
        });

        // make a dummy wrapper div to wrap the content of flash download button.
        $wrapper = $('<div class="inline-block" style="display:none;"></div>');
        // add the tabletools csv into wrapper.
        $(objTableTools.fnContainer()).appendTo($wrapper);
        // add wrapper to body so that it gets flash button from tabletools.
        $wrapper = $($wrapper).appendTo('body');
        // give some time so that the download button of datatable can gets generated.
        setTimeout(function () {
            // extact the download buttonm from wrapper and append it into download button.
            $wrapper = $($wrapper.find("embed")).prependTo($(".btn-tabletools-download-csv"));
            // change the class of the button container to hold the flash.
            $wrapper.parent().parent().addClass("inline-block-flash-tabletool")
        }, 500);
    } catch (e) {

    }
   

}
DataTableWrapper.prototype.RetrieveData = function (force) {
    force = force || false;
    var self = this;
    if (!self.Ready) { return; }
    if (self.DataSource == null) { return; }
    var loadFn = self.OnDataLoad == null ? function () { } : self.OnDataLoad.bind(self);
    var dataTable = self.Table.dataTable();
    var sendData = {};
    if (self.DataSourceFn) {
        sendData = self.DataSourceFn();
    }
    var stringifiedSendData = JSON.stringify(sendData);
    if (stringifiedSendData == self.CurrentSendData && !force) { return; }
    dataTable.fnClearTable();
    self.CurrentSendData = stringifiedSendData;
    var fillDataFn = function (page) {
        sendData.NumItems = DataTable.GetAmount;
        sendData.ItemId = page;
        Comm.send_json(self.DataSource + (self.DataSource.indexOf('?') > 0 ? "&" : "?") + "size=" + DataTable.GetAmount + "&page=" + page, sendData, function (response) {
            var data = response.List;//I tried making a DataList<T> object server side but it was basically CommandContainer but with Data insead of Commands - Jason Dees
            //Comm.master_callback(response);
            if (self.Table != null) {
                if (!$.contains(document, self.Table[0])) { return; }
                self.AddData(data);
                if (data.length != DataTable.GetAmount || !self.Paged) {
                    Desertfire.stopLoader();
                    loadFn();
                    return;
                }
                page++;
                fillDataFn(page);
            }
        });
    }
    fillDataFn(0);
}
DataTableWrapper.prototype.RetrieveCache = function (force) {
    var self = this;
    if (!self.Ready) { return; }
}
DataTableWrapper.prototype.CurrentPage = function () {
    var dt = this.Table.dataTable();
    var settings = dt.fnSettings();
    var start = settings._iDisplayStart;
    var length = settings._iDisplayLength;
    return start / length;
}
DataTableWrapper.prototype.Data = function () {
    return this.Table.dataTable()._('tr');
}
DataTableWrapper.prototype.DataObjects = function () {
    return this.Table.dataTable()._('tr');
}
DataTableWrapper.prototype.fnGetData = function (arguments) {
    return this.Table.dataTable().fnGetData(arguments);
}
DataTableWrapper.prototype.DataByAttr = function (attr, val) {
    var dataTable = this.Table.dataTable();
    var tabledata = dataTable.fnSettings().aoData;
    for (var i = 0; i < tabledata.length; i++) {
        var data = tabledata[i]._aData;
        if (data[attr] == val) {
            return data;
        }
    }
}
DataTableWrapper.prototype.AddData = function (data) {
    var page = this.CurrentPage();
    var dt = this.Table.dataTable();
    dt.fnAddData(data);
    dt.fnPageChange(page);
}
DataTableWrapper.prototype.ClearData = function () {
    this.Table.dataTable().fnClearTable();
    return this;
}
DataTableWrapper.prototype.DeleteSelectedData = function () {
    var dataTable = this.Table.dataTable();
    var data = dataTable.fnSettings().aoData;
    var selectedData = [];
    for (var i = 0; i < data.length; i++) {
        var row = $(data[i].nTr);
        if (row.hasClass(DataTable.SelectedClass)) {
            selectedData.push(data[i]._aData);
            dataTable.fnDeleteRow(i);
            i--;
        }
    }
    return selectedData;
}
DataTableWrapper.prototype.DeleteDataByAttribute = function (attr, data) {
    var dataTable = this.Table.dataTable();
    var tabledata = dataTable.fnSettings().aoData;
    for (var i = 0; i < tabledata.length; i++) {
        var d = tabledata[i]._aData[attr];
        if (data.indexOf(d) > -1) {
            dataTable.fnDeleteRow(i);
            i--;
        }
    }
}
DataTableWrapper.prototype.Update = function (data, index) {
    var page = this.CurrentPage();
    var dt = this.Table.dataTable();
    dt.fnUpdate(data, index);
    dt.fnPageChange(page);
}
DataTableWrapper.prototype.UpdateByAttr = function (data, attr, val) {
    var dataTable = this.Table.dataTable();
    var tabledata = dataTable.fnSettings().aoData;
    for (var i = 0; i < tabledata.length; i++) {
        var d = tabledata[i]._aData[attr];
        if (d == val) {
            this.Update(data, i);
            return;
        }
    }
}
DataTableWrapper.prototype.UpdateAttrByAttr = function (attr, newVal, compAttr, compVal) {
    var dataTable = this.Table.dataTable();
    var tabledata = dataTable.fnSettings().aoData;
    for (var i = 0; i < tabledata.length; i++) {
        var data = tabledata[i]._aData;
        var d = data[compAttr];
        if (d == compVal) {
            data[attr] = newVal;
            this.Update(data, i);
            return;
        }
    }
}
DataTableWrapper.prototype.UpdateAttr = function (attr, index, newVal) {
    var dataTable = this.Table.dataTable();
    var data = dataTable.fnSettings().aoData[index]._aData;
    data[attr] = newVal;
    this.Update(data, index);
}
DataTableWrapper.prototype.UpdateArrayAttr = function (attr, newVal, dataArray, compAttr) {
    var dataTable = this.Table.dataTable();
    var tabledata = dataTable.fnSettings().aoData;
    for (var i = 0; i < tabledata.length; i++) {
        var data = tabledata[i]._aData;
        if (dataArray.indexOf(data[compAttr]) > -1) {
            data[attr] = newVal;
            this.Update(data, i);
        }
    }
}
DataTableWrapper.prototype.TrashLink = function (text, action) {
    var link = $('<a class="btn-link">' + text + '</a>');
    link.click(action);
    $('.trash-link', this.Parent).append('&nbsp;').append(link);
}
DataTableWrapper.prototype.GetFirstSelected = function (attr) {
    var selected = this.GetSelected();
    if (selected.length == 0) {
        return null;
    }
    if (attr == null) {
        return selected[0];
    }
    else {
        return selected[0][attr];
    }
}
DataTableWrapper.prototype.GetSelected = function (attr) {//I'll have to keep an eye on this for very large sets of items
    if (attr != null) {
        return this.GetSelectedAttribute(attr);
    }
    return this.Table.dataTable()._('tr.active', { filter: 'applied' });
}
DataTableWrapper.prototype.GetSelectedAttribute = function (attr) {
    var selectedData = [];
    var data = this.GetSelected();
    for (var i = 0; i < data.length; i++) {
        selectedData.push(data[i][attr]);
    }
    return selectedData;
}
DataTableWrapper.prototype.GetSelectionSelected = function (index) {
    var selectedData = [];
    if (!index) {
        this.Actions.forEach(function (action, index) {
            if (action.IsDropDown) {
                selectedData.push({ action: action, selected: action.GetSelected() });
            }
        });
    }
    else {
        if (this.Actions[index].IsDropDown) {
            selectedData.push({ action: this.Action, selected: Actions[index].GetSelected() });
        }
    }
    return selectedData;
}
DataTableWrapper.prototype.Deselect = function () {
    return this.Table.dataTable().$('tr.active').removeClass('active');
}
DataTableWrapper.prototype.HideSearch = function () {
    $('.search-container', this.Parent).hide();
}
DataTableWrapper.prototype.ShowSearch = function () {
    $('.search-container', this.Parent).show();
}
DataTableWrapper.prototype.AddColumn = function (title, mData, opts) {
    var index = this.Columns.length;
    this.Columns.push(new DataTableColumn(title, mData, index, opts));
}
DataTableWrapper.prototype.AddColumnGoMediaIcon = function () {
    this.AddColumn('<img src="/img/go-icon.png" class="inline-block has-tooltip tooltip-ready" title="" height="15">', 'GoMediaRank', {
        mRender: function (data) {
            if (data > 0) {
                var icon = data == 3 ? "/img/go-icon-bw.png" : "/img/go-icon.png";
                var title = data == 3 ? 'GoMedia 3' : 'GoMedia 4';
                return '<img src="' + icon + '" class="inline-block has-tooltip" title="' + title + '" height="15" />';
            } else {
                return "";
            }
        }, iDataSort: 1, Width: '20px', asSorting: ["desc", "asc"]
    });
}
DataTableWrapper.prototype.AddAction = function (action) {
    this.Actions.push(action);
    var actionRightMenu = $('#' + this.Id + 'RightActions');
    var actionLeftMenu = $('#' + this.Id + 'LeftActions');
    var btn;
    if (action.UseRightMenu) {
        actionRightMenu.append(action.Print())
        actionRightMenu.find(".has-tooltip").last().tooltip();
    }
    else {
        actionLeftMenu.append(action.Print())
        actionLeftMenu.find(".has-tooltip").last().tooltip();
    }
    return action;
}
DataTableWrapper.prototype.AddActionButton = function (tooltip, icon, color, action, opts) {
    if (typeof action == "function") { action = action; }
    return this.AddAction(new DataTableActionButton(tooltip, icon, color, action, opts));
}
DataTableWrapper.prototype.AddActionNewButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.NoTooltip = true;
    return this.AddActionButton(tooltip, 'icon-plus-sign icon-white', 'btn btn-primary', action, opts);
}
DataTableWrapper.prototype.AddActionEditButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = true;
    return this.AddActionButton(tooltip, 'fa fa-edit', 'btn-primary', action, opts);
}
DataTableWrapper.prototype.AddActionMediaEditButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = true;
    return this.AddActionButton(tooltip, 'icon-edit', 'btn-codigo', action, opts);
}
DataTableWrapper.prototype.AddActionDeleteButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = false;
    this.AddActionButton(tooltip, 'fa fa-fire', 'btn-danger', action, opts);
}
DataTableWrapper.prototype.AddActionApproveButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = false;
    this.AddActionButton(tooltip, 'fa fa-fire', 'btn-info', action, opts);
}
DataTableWrapper.prototype.AddActionDisApproveButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = false;
    this.AddActionButton(tooltip, 'fa fa-fire', 'btn-warning', action, opts);
}
//@
DataTableWrapper.prototype.AddActionCheckButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = false;
    opts.NoTooltip = true;
    this.AddActionButton(tooltip, 'fa fa-search-plus', 'btn-success', action, opts);
}

DataTableWrapper.prototype.AddActionTrashButton = function (tooltip, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = true;
    this.AddActionButton(tooltip, 'icon-trash', 'btn-danger', action, opts);
}
DataTableWrapper.prototype.AddActionSpace = function (width, content, opts) {
    return this.AddAction(new DataTableActionSpace(width, content, opts));
}
DataTableWrapper.prototype.AddActionText = function (id, defaultValue, opts) {
    return this.AddAction(new DataTableActionText(id, defaultValue, opts));
}
DataTableWrapper.prototype.AddActionDownloadCsvButton = function (tooltip, icon, color, action, opts) {
    opts = opts == null ? {} : opts;
    opts.SingleSelect = false;
    this.bAddActionDownloadCsvButton = true;
    var actionButton = this.AddActionButton(tooltip, icon, color + ' btn-tabletools-download-csv', action, opts);

    setTimeout(function () {
        var dt = oDTable;
        SetTableTools(dt);
    }, 500);
    return actionButton;
}
var DataTableColumn = function (title, mData, index, opts) {
    //This class takes any property that can go into a datatable column (such as mRender and fnCellCreated and bSortable)
    //and other custom properties.
    this.aTargets = [index];
    this.mData = mData;
    this.bSortable = true;
    this.bSearchable = true;
    this.mRender = null;
    this.fnCreatedCell = null;
    this.title = title;
    if (opts) {
        for (var prop in opts) {
            this[prop] = opts[prop];
        }
    }
}
DataTableColumn.prototype.Print = function () {
    var cell = $('<th>' + this.title + '</th>');
    if (this.Width != null) {
        cell.css('width', this.Width);
        cell.css('max-width', this.Width);
    }
    if (this.Hidden) {
        cell.addClass('hide');
    }
    if (!this.bSortable) { cell.addClass('unsortable'); }
    if (this.IsDefault) { cell.addClass('sort_default'); }
    if (this.SortDirection) { cell.addClass(this.SortDirection); }
    return cell;
}

var DataTableActionButton = function (tooltip, icon, color, action, opts) {
    var self = this;
    this.Tooltip = tooltip;
    this.Width = null;
    this.Icon = icon;
    this.Action = typeof action == "function" ? action.bind(this) : action;
    this.IsLink = typeof action != "function";
    this.Color = color;
    this.IsHidden = false;
    this.Container = null;
    this.IsDisabled = false;
    this.Text = null;
    this.DisableFn = function () { return !this.IsDisabled; }
    if (opts) {
        for (var prop in opts) {
            this[prop] = opts[prop];
        }
    }
    this.DisableFn.bind(self);
}
DataTableActionButton.prototype.Print = function () {
    var btn = $('#datatable_button_template').html();
    btn = Desertfire.Replace(btn, "Icon", this.Icon);
    btn = $(btn);
    var anchor = $('a', btn);
    if (this.NoTooltip) {
        anchor.append(this.Tooltip).attr('title', '');
    }
    else {
        anchor.addClass('has-tooltip').append('&nbsp;').prepend('&nbsp;').attr('title', this.Tooltip);
    }
    anchor.addClass(this.Color);
    if (this.Width != null) {
        btn.css('width', this.Width);
    }
    if (this.Class) {
        anchor.addClass(this.Class);
    }
    var self = this;
    if (this.IsLink) {
        anchor.attr("href", this.Action);
    }
    else {
        anchor.click(function () { if (!anchor.hasClass('disabled')) { self.Action() } });
    }
    //btn.properTooltip({ btn: "a[rel=tooltip], .has-tooltip" });
    if (this.Text != null) {
        btn.find("a").append(this.Text);
    }
    this.Container = btn;
    return btn;
}
DataTableActionButton.prototype.Disable = function () {
    if (this.DisableFn()) {
        $('a', this.Container).addClass("disabled");
        this.IsDisabled = true;
    }
}
DataTableActionButton.prototype.Enable = function () {
    if (!this.DisableFn()) {
        $('a', this.Container).removeClass("disabled");
        this.IsDisabled = false;
    }
}
DataTableActionButton.prototype.Href = function (newHref) {
    $('a', this.Container).attr('href', newHref);
}

var DataTableActionSpace = function (width, content, opts) {
    this.Width = width;
    this.Content = content;
    if (opts) {
        for (var prop in opts) {
            this[prop] = opts[prop];
        }
    }
}
DataTableActionSpace.prototype.Print = function () {
    var space = $('#datatable_space_template').html();
    space = $(space);
    space.css('width', this.Width);
    if (this.Content) {
        space.html(this.Content);
    }
    return space;
}
DataTableActionSpace.prototype.Disable = function () { }
DataTableActionSpace.prototype.Enable = function () { }

var DataTableActionText = function (id, defaultValue, opts) {
    this.Id = id;
    this.Default = defaultValue;
    this.IsDateField = false;
    this.Width = null;
    if (opts) {
        for (var prop in opts) {
            this[prop] = opts[prop];
        }
    }
}
DataTableActionText.prototype.Print = function () {
    var self = this;
    var template = $('#datatable_text_template').html();
    template = $(template);
    txt = $('input', template);
    if (this.Width) { txt.css('width', this.Width); };
    txt.val(this.Default);
    if (this.IsDateField) {
        txt.datepicker().addClass('datepicker-start');
        after = '<span class="add-on" style="display:inline-block;"><i class="icon-calendar" style="margin: 1px 0 0 -23.5px; pointer-events: none; position: relative;"></i></span>';
        template.append(after);
    }
    txt.attr("id", this.Id);
    if (this.OnChange) {
        txt.blur(function () {
            self.OnChange();
        });
    }
    return template;
}
DataTableActionText.prototype.Disable = function () { }
DataTableActionText.prototype.Enable = function () { }

var DataTableActionSelection = function (tooltip, icon, action, opts) {
    var self = this;
    this.Action = action;//What to do with the selected
    if (typeof this.Action == 'function') {
        this.Action.bind(self);
    }
    this.Icon = icon;
    this.Items = [];
    this.IsMultiSelect = true;//Checkboxes or radios
    this.Tooltip = tooltip;
    this.Id = this.Tooltip.replace(/ /g, '');
    this.IsDropDown = true;
    var tmp = $('#datatable_dropdown_template').html();
    tmp = Desertfire.Replace(tmp, 'Icon', this.Icon);
    tmp = Desertfire.Replace(tmp, 'Tooltip', this.Tooltip);
    this.Container = $(tmp);
    this.TimeoutWait = 700;
    if (opts) {
        for (var prop in opts) {
            this[prop] = opts[prop];
        }
    }
    $('.dropdown-toggle', this.Container).expandedDropdown();
    $('.btn-group', this.Container).attr('id', this.Id);
    var filterTimer = 0;
    var selection_text = $('.selection_text', this.Container);
    $('.dropdown-menu', this.Container).click(function (event) {
        event.preventDefault();
        Desertfire.multi_select_click(event, self.Container, self.Id);
        if (self.Action) {
            clearTimeout(filterTimer);
            filterTimer = setTimeout(function () {
                self.Action()
            }, self.TimeoutWait);
        }
        var selectCount = $('li.active a', self.Container).length;
        if (selectCount == 0) {
            //selection_text.html('No Filter Selected');
        }
        else if (selectCount == 1) {
            selection_text.html($('li.active a', self.Container).data('name'));
        }
        else if (selectCount > 1) {
            selection_text.html(selectCount + " selected");
        }
        else {
            $('li.single', self.Container).click();
        }
    });
}
DataTableActionSelection.prototype.AddItem = function (item) {
    this.Items.push(item);
    $('.dropdown-menu', this.Container).append(item.Print(this.IsMultiSelect, this.Id));
    //print thing here too
    return this;
}
DataTableActionSelection.prototype.AddFromElement = function (element) {
    $('.dropdown-menu', this.Container).append(element.html());
    $('.dropdown-menu-buttons span', this.Container).addClass('has-tooltip');
    $('input', this.Container).attr('name', this.Id);
}
DataTableActionSelection.prototype.Print = function () {
    $('a.dropdown-toggle', this.Container).addClass('has-tooltip');
    this.Container.tooltip();
    if (this.Color != null) {
        $('.btn', this.Container).addClass(this.Color);
    }
    else {
        $('.btn', this.Container).addClass("btn-primary");
    }
    return this.Container;
}
DataTableActionSelection.prototype.GetSelected = function () {
    return Desertfire.get_selected(this.Container);
}
DataTableActionSelection.prototype.SetSelectedIndex = function (index) {
    //I need this to not fire anything
    $('li:first', this.Container).click();
}
DataTableActionSelection.prototype.SetSelectedValue = function (value) {
    $('li[data=' + value + ']', this.Container).each(function (ele) {
        var li = $(this);
        if (li.data('value') == value) {
            li.mousedown();
        }
    });
}
DataTableActionSelection.prototype.SelectCookies = function (defaultIndex) {
    var self = this;
    var hasCookie = false;
    self.Container.find("a:not(.btn)").each(function () {
        var anchor = $(this);
        var value = anchor.data("value");
        var cookievalue = $.trim(self.Id + '_' + value);
        if (removeCookie(cookievalue)) {
            anchor.click();
            hasCookie = true;
        }
    });
    if (!hasCookie) {
        self.SetSelectedIndex(defaultIndex);
    }
    return self;
}

DataTableActionSelection.prototype.Disable = function () {
    $('a', this.Container).addClass("disabled");
    this.IsDisabled = true;
}
DataTableActionSelection.prototype.Enable = function () {
    $('a', this.Container).removeClass("disabled");
    this.IsDisabled = false;
}
var DataTableSelectionItem = function (name, value, action) {
    this.Value = value;
    this.Name = name;
    this.Action = typeof action == "function" ? action.bind(this) : action;
    this.IsLink = typeof action != "function";
}
DataTableSelectionItem.prototype.Print = function (isMultiSelect, name) {
    var self = this;
    var tmp = $('#datatable_dropdown_item_template').html();
    tmp = Desertfire.Replace(tmp, 'Value', this.Value);
    tmp = Desertfire.Replace(tmp, 'Name', this.Name);
    var inputType = isMultiSelect ? 'checkbox' : 'radio';
    tmp = Desertfire.Replace(tmp, 'InputType', inputType);
    tmp = $(tmp);
    if (!isMultiSelect) { tmp.addClass("single") }
    $('input', tmp).attr('name', name);
    if (this.IsLink) {
        tmp.attr("href", this.Action);
    }
    else {
        tmp.click(function () { if (!tmp.hasClass('disabled')) { self.Action() } });
        tmp.find('input').remove();
    }
    return $(tmp);
}

var DataTableSelectionItemAction = function () {
    this.Icon = 'icon-pencil';
    this.Tooltip = 'Nothing';
    this.Action = null;
    this.IsLink = false;
}
DataTableWrapper.prototype.HideActionsTop = function () {
    $('.table-actions-top', this.Parent).hide();
}
DataTableWrapper.prototype.ShowActionsTop = function () {
    $('.table-actions-top', this.Parent).show();
}
DataTableWrapper.prototype.DisableSelectable = function () {
    $('tbody', this.Table).off("click");
    var table = $(this.Table);
    this.Table.removeClass("selectable");
}
DataTableWrapper.prototype.EnableSelectable = function () {
    var prevClick = 0;
    var self = this;
    $('tbody', this.Table).click(function (event) {
        if (event.timeStamp - prevClick > 200) {
            prevClick = event.timeStamp;
            $(event.target.parentNode).toggleClass(DataTable.SelectedClass);
            self.EnableDisableButtons();
        }
    });
    this.Table.addClass("selectable");
}

var mytable;
DataTableWrapper.prototype.CustomSearch = function () {
    mytable = this;
    console.log(this);
    this.Table.column(1)
                    .search(s)
                    .draw();
}