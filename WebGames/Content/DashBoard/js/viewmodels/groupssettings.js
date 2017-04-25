function ViewModel() {

    var vm = {

        User_Groups: ko.observableArray(),
        init: init,
        GetSavedGroups: GetSavedGroups,
        GetRankedPlayers: GetRankedPlayers,
        SaveGroups: SaveGroups,
        Loading: ko.observable(false),
        ReplacePlayer: ReplacePlayer,
        ViewReplaceModal: ViewReplaceModal,
        InitDatable: InitDatable,
        SelectedUser: null,
        CancelEditing: CancelEditing,
    };

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
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetSavedGroups", {},
            function (res) { //success
                 //debugger
                if (res.success) {
                    if (res.user_groups.length) {
                        SetUserGroups(res.user_groups);
                    }
                }
                else {
                    $.custom['Logger'].Error("Δεν βρέθηκαν ομάδες", "")
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
        var userGroups = ko.mapping.toJS(vm.User_Groups);
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
                if (res.success) {
                    $.custom['Logger'].Success("Οι Ομάδες Αποθηκεύτηκαν", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "")
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });

    }

    var columns = [
        { "title": "Θέση", "searchable": true, "visible": true, "target": 0 },
        { "title": "UserId", "searchable": false, "visible": false, "target": 1 },
        { "title": "Όνομα", "searchable": true, "target": 2 },
        { "title": "Group", "searchable": false, "visible": false, "target": 3 },
        {
            "title": "Ρυθμίσεις", "searchable": false, "target": 4, createdCell: function (td, cellData, rowData, row, col) {
                //debugger;
                var html = '';
                var id = rowData[1];
                if (UserGroupDict[id]) {
                    html = '<div class="alert alert-danger">Επιλεγμένος</div>';
                }
                else {
                    html = '<button type="button" class="btn btn-warning" onclick="newView.ReplacePlayer(' + row + ')">Select</button>';
                }
                $(td).html(html)
            }
        },
    ];

    var table;
    var UserGroupDict = null;
    function InitDatable() {
        if (table) return;
        table = $('#datatable-user-replace').DataTable({
            "ajax": {
                "url": "/Dashboard/GetRankedPlayersDT",
                "data": function (d) {
                    UserGroupDict = {};
                    var userGroups = vm.User_Groups();
                    for (var i = 0; i < userGroups.length; i++) {
                        for (var j = 0; j < userGroups[i].Players.length; j++) {
                            UserGroupDict[userGroups[i].Players[j].UserId()] = userGroups[i].Group
                        }
                    }
                }
            },
            "columns": columns
        });
    };

    function ViewReplaceModal(User) {
        $('#usergroupEditorModal').modal();
        vm.SelectedUser = User;

        InitDatable();
    };

    function ReplacePlayer(rowIndex) {
        var data = table.row(rowIndex).data();      
        vm.SelectedUser.Rank(data[0]);
        vm.SelectedUser.UserId(data[1]);
        vm.SelectedUser.User_FullName(data[2]);
        vm.SelectedUser = null;
        $('#usergroupEditorModal').modal('hide');
    };

    function CancelEditing() {
        vm.SelectedUser = null;
    };

    return vm;
};

var newView = new ViewModel();
// Init
$(document).ready(function () {
    newView.init();
    ko.applyBindings(newView);
});
