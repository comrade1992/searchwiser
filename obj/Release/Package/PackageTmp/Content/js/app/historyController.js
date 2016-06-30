(function (){
    var app = angular.module("app-category");
    app.controller("historyController", ["$scope", "$http", function($scope, $http) {
        $scope.historyList = [];
        $http.get("/api/getHistory?userid=" + parseInt(sessionStorage.ID)).then(function (results) {
            angular.forEach(results.data, function (item) {
                item.SubmitDate = moment(item.SubmitDate).format("DD-MM-YYYY");
                $scope.historyList.push(item);
            });
        });


        if (sessionStorage.email) {
            $scope.userLogged = true;
            $scope.userEmail = sessionStorage.email;
            $scope.userId = sessionStorage.ID;
            $scope.user = sessionStorage.User;
            angular.element('#logout').css("display", "inline-block");
        } else {
            window.location.href = "/Home/login";
        }

        $scope.search = function (item) {
            window.location = "/home/index/" + item.GroupID;
        };
    }]);
})();