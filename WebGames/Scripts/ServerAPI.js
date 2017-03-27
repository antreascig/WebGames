function ServerAPi() {

    this.SendRequest = function (type, url, data, requiresAuth, success, fail) {

        if (requiresAuth) {
            var token = localStorage.getItem(tokenKey);
            var headers = {};
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
        }


        $.ajax({
            type: type,
            url: url,
            headers: headers,
            data: data
        }).done(function (resData) {
            success(resData)
        }).fail(fail);
    };

}

var Server = new ServerAPi();

if (!$.apis) {
    $.apis = {};
}
$.apis["Server"] = Server;