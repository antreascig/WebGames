function ServerAPi() {

    this.SendRequest = function (type, url, data, success, fail, requireAuth) {

        if (requireAuth) {
            $.custom.AntiForgeryToken["AddAntiForgeryToken"](data);
        }
        //if ((type || "").toLowerCase() == "get") {
        //    data = JSON.stringify(data);
        //}

        $.ajax({
            type: type,
            url: url,
            data: data
        }).done(function (resData) {
            success(resData)
        }).fail(fail);
    };

}

var Server = new ServerAPi();

if (!$.custom) {
    $.custom = {};
}

$.custom["Server"] = Server;