

function ViewModel() {

    var vm = {
        init: init,
        RefreshSchedule: RefreshSchedule,
        Games: ko.observableArray([]),
        ScheduleDays: ko.observableArray(),
        SaveSchedule: SaveSchedule,
        AddDay: AddDay,
        EditDay: EditDay,
        RemoveDay: RemoveDay,
        SelectedDay: ko.observable(),
        CancelDay: CancelDay,
        SaveDay: SaveDay,
    };

    // General Game Settings
    function init() {
        vm.RefreshSchedule();
        GetGames();
    };

    function RefreshSchedule() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetSchedule", {},
            function (res) { //success
                    // debugger

                if (res.success) {
                    var data = ko.mapping.fromJS((res.data || {}));
                    vm.ScheduleDays(data());
                }
            },
            function (error, hrx, code) { });
    };

    function GetGames() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetGameSettings", {},
            function (res) { //success
                // debugger
                if (res.success) {
                    // debugger
                    var data = [];
                    for( var i = 0; i < res.data.length; i++ ) {
                        var tag = {id: res.data[i].GameKey, text: res.data[i].Name };
                        data.push(tag);
                    }
                    vm.Games(data);
                }
            },
            function (error, hrx, code) { });
    };

    function SaveSchedule() {
        var schedule = ko.mapping.toJS(vm.ScheduleDays);
        $.custom.Server["SendRequest"]("POST", "/Dashboard/SaveSchedule", { schedule: schedule },
            function (res) { //success

                if (res.success) {
                    $.custom['Logger'].Success("Το πρόγραμμα παιχνιδιών αποθηκεύτηκε.", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    };

    function AddDay() {
        var newDay = {
            Day: ko.observable(),
            GameKey: ko.observable()
        };
        EditDay(newDay);
    };

    function EditDay(ScheduleDay) {
        var tmpQ = ko.mapping.fromJS(ko.toJS(ScheduleDay));
        vm.SelectedDay(tmpQ);
        var day = moment();
        if (tmpQ.Day()) {
            var stringDay = tmpQ.Day();
            day = moment(stringDay);
            $('#single_cal4').val(stringDay);
        }

        $('#single_cal4').daterangepicker({
            startDate: day,
            singleDatePicker: true,
            calender_style: "picker_4"
        }, function (start, end, label) {
            var value = start.format("YYYY-MM-DD");
            var tmp = vm.SelectedDay();
            tmp.Day(value);
            console.log('Day: ', value);
        });
    };

    function RemoveDay(Day) {
        var tmp = vm.ScheduleDays() || [];
        var newList = [];
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].Day != Day)
                newList.push(tmp[i]);
        }
        vm.ScheduleDays(newList);
    };

    function CancelDay() {
        vm.SelectedDay(null);
    };

    function SaveDay() {
        var value = vm.SelectedDay();
        value.GameName = ko.observable("");
        var tmp = vm.ScheduleDays();

        $('#scheduleEditor').modal('hide');
        vm.SelectedDay(null);

        var gameList = vm.Games();

        for (var i = 0; i < gameList.length; i++) {
            if (gameList[i].id == value.GameKey()) {
                value.GameName( gameList[i].text );
                break;
            }
        }
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].Day() == value.Day()) {
                tmp[i].GameKey(value.GameKey());
                //tmp[i].GameName(value.GameName()); 
                return;
            }
        }
        tmp.push(value);
        vm.ScheduleDays(tmp);
    };

    return vm;
};

var newView = new ViewModel();

// Init
newView.init();


ko.applyBindings(newView);