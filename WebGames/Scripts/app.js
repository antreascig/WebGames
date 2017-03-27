function ViewModel() {
    var self = this;

    var tokenKey = 'accessToken';

    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();
    self.errors = ko.observableArray([]);

    function showError(jqXHR) {

        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState) {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    self.callApi = function () {
        self.result('');
        self.errors.removeAll();

        var token = localStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/values/Admin',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    };

    self.callApiPlayer = function () {
        self.result('');
        self.errors.removeAll();

        var token = localStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/values/Player',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    };


    self.logout = function () {
         //Log out from the cookie based logon.
        //var token = localStorage.getItem(tokenKey);
        //var headers = {};
        //if (token) {
        //    headers.Authorization = 'Bearer ' + token;
        //}

        $.ajax({
            type: 'POST',
            url: '/Account/LogOff',
            data: {
                id: "logoutForm"
            }
            //headers: headers
        }).done(function (data) {
            
        }).fail(showError);
    };
}

var app = new ViewModel();
ko.applyBindings(app);