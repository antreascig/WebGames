function ViewModel() {

    var vm = {
        Games: ko.observableArray(),
        GamesDict: {},
        SetActiveGameKey: SetActiveGameKey,
        RefreshScores: RefreshScores,
        EditPlayerScore: EditPlayerScore,
        ActiveGameKey: ko.observable(),
        ActiveGroupNumber: ko.observable(''),
        InitGameScoresDT: InitGameScoresDT,
        InitGroupScoresDT: InitGroupScoresDT
    };

    var columns = [
        { "title": "Θέση", "searchable": false },
        { "title": "UserId", "searchable": false, "visible": false },
        { "title": "Όνομα", "searchable": true },
        { "title": "Κατάστημα", "searchable": true },
        { "title": "Tokens", "searchable": false, "visible": false },
        { "title": "Σκορ", "searchable": true },
    ];

    var group_columns = [
        { "title": "Ομάδα", "searchable": false },
        { "title": "Σκορ", "searchable": false },
        {
            "title": "Controls", "searchable": false, "target": 3, createdCell: function (td, cellData, rowData, row, col) {
                // debugger;
                var html = '<button type="button" class="btn btn-warning" disabled>View</button>';
                $(td).html(html)
            }
        }
    ];


    var table;
    var group_table;

    function InitGameScoresDT() {

        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetGameSettings", {},
            function (res) { //success

                if (res.success) {
                    // debugger;
                    for (var i = 0; i < res.data.length; i++) {
                        var game = res.data[i];
                        vm.GamesDict[game.GameKey] = game
                    }

                    vm.Games(res.data || []);

                    if (res.data.length)
                        vm.SetActiveGameKey(res.data[0].GameKey);

                    table = $('#datatable-games').DataTable({
                        "fnServerData": function (sSource, aoData, fnCallback) {
                            // debugger

                            $.custom.Server["SendRequest"]("POST", "/Dashboard/GetScores?GameKey=" + vm.ActiveGameKey(), aoData,
                                function (a, b, c, d) {
                                    fnCallback(a, b, c, d);
                                },
                                function (error, hrx, code) { }, false, 'json');

                            // $.ajax({
                            //     "dataType": 'json',
                            //     "type": "POST",
                            //     "url": "/Dashboard/GetScores?GameKey=" + vm.ActiveGameKey(),
                            //     "data": aoData,
                            //     "success":function (a, b, c, d) {
                            //         fnCallback(a, b, c, d);
                            //     } 
                            // });
                        },
                        // "ajax": "/production/data.json",
                        "columns": columns
                    });
                    table.ajax.reload();
                }

            },
            function (error, hrx, code) { });
    };

    function InitGroupScoresDT() {
        group_table = $('#datatable-groups').DataTable({
            "lengthMenu": [12],
            "fnServerData": function (sSource, aoData, fnCallback) {
                // debugger

                $.custom.Server["SendRequest"]("POST", "/Dashboard/GetGroupScores?group=" + vm.ActiveGroupNumber(), aoData,
                    function (a, b, c, d) {
                        fnCallback(a, b, c, d);
                    },
                    function (error, hrx, code) { }, false, 'json');

                // $.ajax({
                //     "dataType": 'json',
                //     "type": "POST",
                //     "url": "/Dashboard/GetGroupScores?group=" + vm.ActiveGroupNumber(),
                //     "data": aoData,
                //     "success": function (a, b, c, d) {
                //         fnCallback(a, b, c, d);
                //     }
                // });
            },
            // "ajax": "/production/data.json",
            "columns": group_columns
        });
        group_table.ajax.reload();
    }

    function SetActiveGameKey(GameKey) {
        vm.ActiveGameKey(GameKey);
        if (GameKey == 'Groups') {
            if (!group_table) {
                vm.InitGroupScoresDT();
                group_table.draw();
            }
            else {
                group_table.ajax.reload();
            }

        } else if (table)
            table.ajax.reload();
    }

    // General Game Settings
    function RefreshScores() {
        table.ajax.reload();
    };

    function EditPlayerScore(rowIndex) {
        var data = table.row(rowIndex).data();
        alert(data.Name);
    };

    return vm;
};

var newView = new ViewModel();

// Init
$(document).ready(function () {
    newView.InitGameScoresDT();
    ko.applyBindings(newView);
});
