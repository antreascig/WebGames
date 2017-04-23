function ServerAPi() {
    var self = this;

    self.SaveGameTime = function (remainingTime, success, fail) {
        var data = {
            remainingTime: remainingTime,
            timestamp: new Date().getTime()
        };
        self.SendRequest('GET', "/Games/SaveGameTime", data, success, fail, false);
    };

    self.GetGameTime = function ( success, fail) {
        self.SendRequest('GET', "/Games/GetGameTime", {}, success, fail, false);
    };

    self.SaveGameScore = function (score, level, success, fail) {
        var data = {
            score: score,
            level: level,
            timestamp: new Date().getTime()
        };
        //debugger
        if (window.customGameKey) {
            data.customGameKey = window.customGameKey;
        }

        self.SendRequest('GET', "/Games/Save_Game_Score", data, success, fail, false);
    };

    self.Get_Random_Mastermind_Solution = function (success, fail) {
        self.SendRequest('GET', "/Games/Get_Random_Game3_Solution", {}, success, fail, false);
    };

    self.CheckQuestion = function (questionId, answerIndex, isDemo, success, fail) {
        isDemo = isDemo || false;
        var data = {
            questionId: questionId,
            answerIndex: answerIndex,
            isDemo: isDemo
        };
        self.SendRequest('GET', "/Games/CheckQuestion", data, success, fail, false);
    };


    self.SendRequest = function (type, url, data, success, fail, requireAuth) {

        if (requireAuth) {
            $.custom.AntiForgeryToken["AddAntiForgeryToken"](data);
        }
        //if ((type || "").toLowerCase() == "get") {
        //    data = JSON.stringify(data);
        //}
        if (!fail) {
            fail = function () {
                console.log("error");
            };
        }


        $.ajax({
            type: type,
            url: url,
            data: data
        }).done(function (resData) {
            success(resData);
        }).fail(fail);
    };

}

var Server = new ServerAPi();

if (!$.custom) {
    $.custom = {};
}

$.custom["Server"] = Server;