var Upload = function (library) {
    this.Library = library;
    this.File = null;
    this.Files = [];
    this.Turnover = null;
    var self = this;
    this.Done = false;
    this.Library = library;
    this.Relation = library.Relation;
    var data = { 
        Content: $('#upload_turnover').clone().attr("id", "").addClass("turnover").wrap('<div>').parent().html(),
        Selctor: ''};
    setTimeout(function () {
        self.Turnover = Desertfire.attach_turnover(data, $('<div></div>'));
        self.start();
       $('.fileinput-button', self.Turnover).click();
    }, 100);
}
Upload.prototype.start = function () {
    var self = this;
    var uploadButton = $('.upload_button', self.Turnover);
    uploadButton.change(function (e) {
        self.start_files(this.files, true);
    });
    $('.fileinput-button', self.Turnover).click(function(e) { uploadButton.click(); });
    var bodyClick = function (e) {
        self.end(bodyClick);
    }
    $('.upload_finish', self.Turnover).click(bodyClick);
    $('body').click(bodyClick);
    var row = $($('#blank_screen').html());
    self.change_content(row);
    row.find('.close').click(bodyClick);
}
Upload.prototype.end = function (bodyClick) {
    //I need to cycle through each file and submit the turnover form;
    $('body').unbind("click",bodyClick);
    Desertfire.close_turnover(this.Turnover);
    if (this.Files.length > 0 && !this.Done) {
        for (var i = 0; i < this.Files.length; i++) {
            this.show_response(this.Files[i]);
        }
        this.end_updates(0);
    }
    else {
        this.Turnover.remove();
    }
}
Upload.prototype.end_updates = function (index) {
    var self = this;
    var file = this.Files[index];
    this.show_response(file);
    index++;
    //var formComplete = function () { self.get_new_data(); };//Doesn't seem to be needed since turnover does this anyway.
    formComplete = function () { self.Turnover.remove(); }
    if (index < this.Files.length) {
        formComplete = function () { self.end_updates(index); }
    }
    var form = $('form', this.Turnover);
    if (file.uploaded && !form.data('ignore')) {
        form.data('runafter', formComplete).submit();
    }
    else {
        formComplete();
    }
}
Upload.prototype.start_files = function(files, doNameCheck){
    doNameCheck = doNameCheck || false;
    var i;
    var self = this;
    for (i = 0; i < files.length; i++) {
        var file = files[i];
        file.uploaded = false;
        this.add_uploaded_row(file,doNameCheck);
    }
    i = 0;
    var nextupload = function () {
        if (i < files.length) {
            self.upload_file(files[i],null,doNameCheck,nextupload);
        }
        i++;
    }
    nextupload();
}
Upload.prototype.show_response = function (file) {
    if (!file.response) { return;}
    //So I need the currently displayed file and I need to create an object with all the inputs and switch between them.
    var self = this;
    var turnoverContainer = $('.uploaded_turnover',self.Turnover);
    if (self.File) {
        self.File.formData = {};
        $('input, textarea,select', turnoverContainer).each(function () {
            self.File.formData[this.name] = { value: this.value, checked: this.checked };
        })
    }
    console.log(file.response);
    if (file.response[0].result == undefined) {
        self.change_content($(file.response).html());
    } else {
        self.change_content($(file.response[0].result.Message).html());
    }
    //self.change_content("<h3>File Uploaded Successfully.</h3>");
    $('tr.active').removeClass('active');

    file.row.addClass('active');
    if (file.uploaded) {
    } else {
        $('#RenameBtn',self.Turnover).unbind('click').click(function () {
            file.submitName = $('input[name="ContentName"]').val();
            self.upload_file(file, null, true);
            file.response = $('#file_uploading').html();
            turnoverContainer.html($(file.response).html());
        })
        var replaceBtn = $('#ReplaceBtn',self.Turnover);
        if (replaceBtn.length > 0) {
            var replaceUrl = replaceBtn.data("submit");
            replaceBtn.unbind('click').click(function () {
                file.response = $('#file_replacing').html();
                turnoverContainer.html($(file.response).html());
                //Do replace
                self.upload_file(file, replaceUrl, false);
            });
        }
    }
    $('form input[name="runafter"]', this.Turnover).val('');
    //if (file.uploaded) { self.Library.turnover_load(); }
    for (var inputName in file.formData) {
        var dataObj = file.formData[inputName];
        var field = $('[name="' + inputName + '"]', turnoverContainer);
        field.val(dataObj.value).attr('checked',dataObj.checked).change();
    }
    $('.remove', this.Turnover).unbind('click').click(function (e) {
        var id = $(this).data('id');
        var deleteFn = function () {
            self.blank_content();
            file.row.remove();
            file.response = null;
        }
        if (id) {
            self.change_content($('#file_delete').html());
            $('.remove', turnoverContainer).click(function () {
                self.Library.delete([id]);
                deleteFn();
            })
            $('.cancel', turnoverContainer).click(function () {
                self.show_response(file);
            });
        }
        else {
            deleteFn();
        }
    });
    self.File = file;
    $('.close', turnoverContainer).unbind('click').bind('click', function () {
        self.blank_content();
    })
}
Upload.prototype.change_content = function (content) {
    var self = this;
    var turnoverContainer = $('.uploaded_turnover',self.Turnover);
    turnoverContainer.html(content);
    $('.turnover', turnoverContainer).addClass('in');
    setTimeout(function(){
        $('.upload_functions', self.Turnover).height(turnoverContainer.height());
    },100);
}
Upload.prototype.blank_content = function () {
    var self = this;
    $('tr.active').removeClass('active');
    var row = $($('#blank_screen').html());
    self.change_content(row);
    row.find('.close').click(function () { self.end(); });
}
Upload.prototype.check_file_name = function (file,callback) {
    var self = this;
    callback = callback || function () { };
    var fileName = file.submitName;
    
    //Comm.send_json('/contents/UploadNameCheck/' + this.Library.Library + '?filename=' + encodeURIComponent(fileName), {}, function (data) {
    //    Desertfire.stopLoader();
    //    var response = $(data.Content);
    //    file.response = response;
    //    self.rename_status(file);
    //    callback();
    //});
}
Upload.prototype.check_names = function (index,files) {
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        this.check_file_name(file);
    }
}
Upload.prototype.upload_file = function (file,url,doNameCheck,callback) {
    var self = this;
    callback = callback || function () { }
    self.uploading_status(file);
    url = url || '/contents/Upload';
    //if (doNameCheck) { self.check_file_name(file,callback); return;}
    var newFileUpload = $('.upload_button',self.Turnover).clone();
    newFileUpload.fileupload({
        formData: {
            Relation: self.Library.Relation
            //name: file.submitName
        },
        done: function (e, data) {
            file.uploaded = true;
            file.response = $(data);
            self.finished_status(file);
            callback();
            self.Library.UploadCallback(data);
        },
        fail: function (e, data) {
            self.invalid_status(file);
            callback();
        }
    });
    newFileUpload.fileupload('add', {
        files: [file], url: url + "?library="+ self.Library
    });
}
Upload.prototype.add_uploaded_row = function(file,doNameCheck){
    var self = this;
    var name = file.name;
    var type = file.name.split('.').pop().toUpperCase();
    file.submitName = name;
    var row = $('#upload_row').html();
    row = Desertfire.Replace(row, 'FileName', name);
    row = Desertfire.Replace(row, 'FileType', type);
    file.row = $(row);
    self.uploading_status(file);
    $('.uploaded_list tbody', this.Turnover).append(file.row);
    this.Files.push(file);
    file.row.click(function () {
        self.show_response(file);
    });
    if (self.check_file_type(file)) {
        //self.upload_file(file,null, doNameCheck);
    }
    else {
        self.invalid_status(file);
    }
}
Upload.prototype.visible_status = function (file) {
    var self = this;
    if ($('.uploaded_turnover', self.Turnover).html().trim() == '' || self.File == file || self.File == null) {
        self.show_response(file);
    }
}
Upload.prototype.invalid_status = function (file) {
    var self = this;
    //if (self.Library.Library == "Admin.PlayerVersions") {
    //    file.response = $('#wrong_file_type_only_zip').html();
    //} else {
    //    if (self.Library.Library.toLowerCase().indexOf("voice") >= 0)
    //        file.response = $('#wrong_file_type_Only_Audio').html();
    //    else
    //        file.response = $('#wrong_file_type').html();
    //}
    
    file.row.find('.file-status').hide();
    file.row.find('.invalid-type').show();
    this.visible_status(file);
}
Upload.prototype.rename_status = function (file) {
    file.row.find('.file-status').hide();
    file.row.find('.name-check').show();
    this.visible_status(file);
}
Upload.prototype.finished_status = function (file) {
    file.row.find('.file-status').hide();
    file.row.find('.finished').show();
    this.visible_status(file);
}
Upload.prototype.uploading_status = function (file) {
    file.response = $('#file_uploading').html();
    file.row.find('.file-status').hide();
    file.row.find('.uploading').show();
    this.visible_status(file);
}
Upload.prototype.check_file_type = function(file) {
    var fileType = file.type.toLowerCase();
    return (/video|image|flash|zip/).test(fileType) || file.name.substr(file.name.length - 4, 4) == ".zip" || file.name.substr(file.name.length - 4, 4) == '.swi';
}

Upload.replace = function (url, progressdiv,btn) {
    var progress = $('#' + progressdiv);
    var file = btn.files[0];
    var newFileUpload = $("<input type='file' />"); //$(btn).clone(); // this is wrong over here as whenever it changes it will again call Upload.replace which in turn loops the same upload again and again. :)
    var status = {
        uploading: '<table id="uploaded_list"><tr><td class="file-status uploading"></td><td>Uploading...</td></tr></table>',
        done: '<table id="uploaded_list"><tr><td class="file-status done"><i class="icon-ok"></i></td><td>Finished Uploading</td></tr></table>',
        errorfiletype:'<table id="uploaded_list"><tr><td class="file-status invalid-type"><i class="icon-ban-circle"></i></td><td>Incorrect file type</td></tr></table>'
    }
    progress.html(status.uploading);
    newFileUpload.fileupload({
        formData: {name: file.submitName},
        done: function (e, data) {
            progress.html(status.done);
            Comm.master_callback(data.result);
        },
        fail: function (e, data) {
            progress.html(data.errorfiletype);
        }
    });
    newFileUpload.fileupload('add', {
        files: [file], url: url
    });
}
