function ViewModel() {

    var vm = {
        GameQuestions: ko.observableArray([]),
        GetGameQuestions: GetGameQuestions,
        EditQuestion: EditQuestion,
        RemoveQuestion: RemoveQuestion,
        SelectedQuestion: ko.observable(),
        AddQuestion: AddQuestion,
        AddOption: AddOption,
        SaveQuestion: SaveQuestion,
        SaveQuestions: SaveQuestions,
        CancelQuestion: CancelQuestion
    };

    // Game Question Settings
    function GetGameQuestions() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetGameQuestions", {},
            function (res) { //success

                if (res.success) {
                    var data = ko.mapping.fromJS((res.data || {}));
                    vm.GameQuestions(data());
                }
            },
            function (error, hrx, code) { });
    };

    function EditQuestion(question) {
        // debugger
        var tmpQ = ko.mapping.fromJS(ko.toJS(question));
        vm.SelectedQuestion(tmpQ);
        vm.SelectedQuestion.Question = question;
    };

    function RemoveQuestion(QuestionId) {
        var tmp = vm.GameQuestions() || [];
        var newList = [];
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].QuestionId != QuestionId)
                newList.push(tmp[i]);
        }
        vm.GameQuestions(newList);
    };

    function AddQuestion() {
        // debugger
        var id = null;
        var UniqueId = null;
        var questions = vm.GameQuestions() || [];
        do {
            id = GetRandomNumber(1, 1000);
            UniqueId = true;
            for (var i = 0; i < questions.length; i++) {
                if (id == questions[i].QuestionId())
                    UniqueId = false;
                break;
            }
        } while (!UniqueId)

        var newQuestion = { QuestionId: ko.observable(id), QuestionText: ko.observable(''), Options: ko.observableArray([]), AnswerIndex: ko.observable(0) }
        vm.EditQuestion(newQuestion);
    };

    function SaveQuestion() {
        // debugger
        var sq = vm.SelectedQuestion();
        var model = vm.SelectedQuestion.Question;
        for (var prop in sq) {
            if (model[prop]) {
                model[prop](sq[prop]());
            }
        }
        $('#questionEditorModal').modal('hide');
        vm.SelectedQuestion(null);
        var tmp = vm.GameQuestions();
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].QuestionId() == model.QuestionId()) {
                return;
            }
        }
        tmp.push(model);
        vm.GameQuestions(tmp);
    };

    function CancelQuestion() {
        vm.SelectedQuestion(null);
    };

    function SaveQuestions() {
        var questions = ko.mapping.toJS(vm.GameQuestions);

        $.custom.Server["SendRequest"]("POST", "/Dashboard/SaveGame5Questions", { questions: questions },
            function (res) { //success
                if (res.success) {
                    $.custom['Logger'].Success("Οι ερωτήσεις αποθηκεύτηκαν.", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    };

    function AddOption() {
        // debugger;
        var options = vm.SelectedQuestion().Options();
        options.push('Option' + (options.length + 1));
        vm.SelectedQuestion().Options(options);
    };

    return vm;
};

var newView = new ViewModel();

// Init
newView.GetGameQuestions();

ko.applyBindings(newView);