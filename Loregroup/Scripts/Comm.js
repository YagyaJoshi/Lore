Comm = {};
Comm.xhr_array = [];

Comm.clear_errors = function () {
    $('.alert:not(#LoadingAnimation)').alert('close');
}

Comm.send_json = function (url, object, callback) {
   
    Desertfire.startLoader();
    //when doing multiple send_json calls, for example on the Calendars page, subsequent calls are killing error messages, which is unfortunate
    // as a result, we need a better way to clean up error messages
    //$('.alert:not(#LoadingAnimation)').alert('close');
    for (cnt = 0; cnt < Comm.xhr_array.length; cnt++) {
        if (Comm.xhr_array[cnt].url == url) {
            Comm.xhr_array[cnt].xhr.abort();
            Comm.xhr_array.splice(cnt, 1);
        }
    }

    callback = typeof callback !== 'undefined' ? callback : master_callback;
    var xhr = $.post($("#hdnUrl").val() + url, object, function (data) {
        for (cnt = 0; cnt < Comm.xhr_array.length; cnt++) {
            if (Comm.xhr_array[cnt].url == url) {
                Comm.xhr_array.splice(cnt, 1);
            }
        }
        
        Desertfire.stopLoader();
        callback(data);
    }, "json").fail(function (jqxhr, status, err) {
        Desertfire.stopLoader();
        if (err == "Unknown") { Comm.send_json(url, object, callback) }
    });

    Comm.xhr_array.push({ "url": url, "xhr": xhr });

    return undefined;
}
