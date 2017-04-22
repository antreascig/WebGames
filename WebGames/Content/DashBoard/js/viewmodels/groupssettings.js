function ViewModel() {

    var vm = {

        User_Groups: ko.observableArray(),
        GetSavedGroups: GetSavedGroups,
        GetRankedPlayers: GetRankedPlayers,
        SaveGroups: SaveGroups,
        Loading: ko.observable(false),
        ReplacePlayer: ReplacePlayer,

        RefreshGroups: RefreshGroups,
        InitDatable: InitDatable,
        SelectedUser: ko.observable(),
        CancelEditing: CancelEditing,
        SaveUser: SaveUser,
        ResetTokens: ResetTokens,
        resetTime: resetTime
    };

    var columns = [
        { "title": "Θέση", "visible": false, "searchable": false, "target": 0 },
        { "title": "UserId", "searchable": false, "visible": false, "target": 1 },
        { "title": "Όνομα", "searchable": true, "target": 1 },
        {
            "title": "Ρυθμίσεις", "searchable": false, "target": 3, createdCell: function (td, cellData, rowData, row, col) {
                // debugger;
                var html = '<button type="button" class="btn btn-warning" onclick="newView.ReplacePlayer(' + row + ')">Edit</button>';
                $(td).html(html)
            }
        },
    ];
    var table;

    function init() {
        vm.GetSavedGroups();

    }

    function SetUserGroups(user_groups) {

        var Groups = [];
        for (var i = 0; i < 12; i++) {
            Groups.push({ Group: i + 1, Players: [] });
        }
        //debugger
        for (var i = 0; i < user_groups.length; i++) {
            var GroupIndex = user_groups[i].Group - 1;
            var asObs = ko.mapping.fromJS(user_groups[i]);
            Groups[GroupIndex].Players.push(asObs);
        }
        vm.User_Groups(Groups);
        console.log(Groups);
        //ko.mapping.fromJS(user_groups, vm.User_Groups);
    }

    function GetSavedGroups() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetSavedGroups", { },
            function (res) { //success
                // debugger
                if (res.success && res.user_groups.length) {
                    SetUserGroups(res.user_groups);
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "")
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    }

    function GetRankedPlayers() {
        vm.Loading(true);
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetRankedPlayers", {},
            function (res) { //success
                // debugger
                vm.Loading(false);

                if (res.success && res.ranked_groups.length) {
                    SetUserGroups(res.ranked_groups);
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "")
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    }

    function SaveGroups() {
        var userGroups = vm.User_Groups();
        var dataToSave = [];

        for (var i = 0; i < userGroups.length; i++) {
            for (var j = 0; j < userGroups[i].Players.length; j++) {
                dataToSave.push({
                    UserId: userGroups[i].Players[j].UserId,
                    GroupNumber: userGroups[i].Group
                });
            }
        }

        $.custom.Server["SendRequest"]("POST", "/Dashboard/SaveGroups", { user_groupsJSON: JSON.stringify(dataToSave) },
            function (res) { //success
                // debugger
                if (res.success ) {
                    $.custom['Logger'].Success("Οι Ομάδες Αποθηκεύτηκαν", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "")
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });

    }
    

    function InitDatable() {
        table = $('#datatable-users').DataTable({
            // "ajax": "/Dashboard/" + view.url,
            "ajax": "/Dashboard/GetUsers",
            "columns": columns
        });
    };

    // General Game Settings
    function RefreshGroups() {
        table.ajax.reload();
    };

    function ReplacePlayer(rowIndex) {
        var data = table.row(rowIndex).data();
        var id = data[0];
        var name = data[1];
        $('#userEditorModal').modal();
    };

    function CancelEditing() {
        vm.SelectedUser(null);
    };

    function ResetTokens(GameData) {
        GameData.Tokens(GameData.Default());
    };

    function SaveUser() {
        var User = vm.SelectedUser();
        var gameTokens = {};
        for (var i = 0; i < User.GameScores.length; i++) {
            var gameData = User.GameScores[i];
            gameTokens[gameData.Key()] = gameData.Tokens();
        }
        $.custom.Server["SendRequest"]("POST", "/Dashboard/SaveUserGameTokens", { userId: User.UserId, gameTokensJSON: JSON.stringify(gameTokens) },
            function (res) { //success
                // debugger
                if (res.success) {
                    $.custom['Logger'].Success("Τα Tokens Αποθηκεύτηκαν", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
        vm.SelectedUser(null);

    };

    function resetTime() {
        var User = vm.SelectedUser();
        $.custom.Server["SendRequest"]("POST", "/Dashboard/ResetGameTime", { userId: User.UserId },
            function (res) { //success
                // debugger
                if (res.success) {
                    $.custom['Logger'].Success("Ο χρόνος διαγράφηκε", "");
                    User.PlayTimeToday('0 λεπτά 0 δευτερόλεπτα');
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    };

    return vm;
};

var newView = new ViewModel();
// Init
$(document).ready(function () {
    //newView.InitDatable();
    ko.applyBindings(newView);
});
