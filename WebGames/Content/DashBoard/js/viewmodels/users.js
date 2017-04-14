function ViewModel() {

    var vm = {
        RefreshUsers: RefreshUsers,
        EditPlayer: EditPlayer,
        InitDatable: InitDatable,
        SelectedUser: ko.observable(),
        CancelEditing: CancelEditing,
        SaveUser: SaveUser,
        ResetTokens: ResetTokens,
    };

    var columns = [
        { "title": "Id", "visible": false, "searchable": false, "target": 0 },
        { "title": "Όνομα", "searchable": true, "target": 1 },
        { "title": "Κατάστημα", "searchable": true, "target": 2 },
        {
            "title": "Ρυθμίσεις", "searchable": false, "target": 3, createdCell: function (td, cellData, rowData, row, col) {
                // debugger;
                var html = '<button type="button" class="btn btn-warning" onclick="newView.EditPlayer(' + row + ')">Edit</button>';
                $(td).html(html)
            }
        },
    ];
    var table;

    function InitDatable() {
        table = $('#datatable-users').DataTable({
            // "ajax": "/Dashboard/" + view.url,
            "ajax": "/Dashboard/GetUsers",
            "columns": columns
        });
    };

    // General Game Settings
    function RefreshUsers() {
        table.ajax.reload();
    };

    function EditPlayer(rowIndex) {
        var data = table.row(rowIndex).data();
        var id = data[0];
        $.custom.Server["SendRequest"]("GET", "/Dashboard/GetUserDetails", { userId: id },
            function (res) { //success
                // debugger
                if (res && res.success) {
                    var gs = [];
                    for (var key in res.Games) {
                        res.Games[key].Key = key;
                        res.Games[key].Default = res.Games[key].Tokens;
                        var obs = ko.mapping.fromJS(res.Games[key]);
                        gs.push(obs);
                    }
                    vm.SelectedUser({
                        UserId: id,
                        GameScores: gs
                    });
                }
            },
            function (error, hrx, code) { });
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

    return vm;
};

var newView = new ViewModel();
// Init
$(document).ready(function () {
    newView.InitDatable();
    ko.applyBindings(newView);
});
