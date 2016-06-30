﻿(function () {
    var app = angular.module("app-category");
    app.controller("loginController", ["$scope", "$http", function($scope, $http) {
        
        $scope.validEmail = false;
        $scope.invalidPassword = false;
        $scope.userLogged = false;
        var userInfo;

        if (sessionStorage.email) {
            $scope.userLogged = true;
            $scope.userEmail = sessionStorage.email;
            $scope.userId = sessionStorage.ID;
            window.location.href = "/Home/Index";
        } else {
            $scope.userLogged = false;
            angular.element('.loginScreen').css("display", "block");
        }

        $scope.login = function () {
            var data = { email: $scope.email, user_password: $scope.user_password };

            $http.post("/api/login", data).then(function (result) {
                if (result.data == "Invalid password" || result.data == "Email not found!") {
                    $scope.invalidPassword = true;
                    $scope.errorLogin = result.data;
                }
               if (angular.fromJson(result.data).Name) {
                    $scope.invalidPassword = false;
                   userInfo = angular.fromJson(result.data)
                   sessionStorage.userInfo = result.data;
                   sessionStorage.email = userInfo.email;
                   sessionStorage.ID = userInfo.ID;
                   sessionStorage.User = userInfo.Name + " " + userInfo.Surname;
                   window.location.href = "/Home/Index";
                }
              
            });
        }

        $scope.logout = function () {
            sessionStorage.removeItem("email");
            sessionStorage.removeItem("User");
            sessionStorage.removeItem("ID");
            sessionStorage.removeItem("userInfo");
            window.location.href = "/Home/login";
        }

        $scope.back = function () {
            window.history.back();
        };

        $("li").on("click",function(){
            //this works like your code above
            setTimeout(function () { 
            var act = document.getElementsByClassName('item active')[0].getElementsByTagName('img')[0].src.split('Content')[1]
            sessionStorage.backgroundImg = act.substr(1);
            }, 1000);
        });

    }]);

})();