(function () {
    var app = angular.module("app-category");


    var firstClick = true;

    function toggleClass() {
        if (firstClick) {
            $(".changePassword span").removeClass("glyphicon-chevron-right");
            $(".changePassword span").addClass("glyphicon-chevron-down");
            firstClick = false;
        } else {
            $(".changePassword span").removeClass("glyphicon-chevron-down");
            $(".changePassword span").addClass("glyphicon-chevron-right");
            firstClick = true;
        }
    }

    $(".changePassword").click(function () {
        $(".newPasswordData").toggle("blind", 100);
        toggleClass();
    });

    $(".newPasswordData").hide();
    app.controller("profileController", ["$scope", "$http", function ($scope, $http) {
        var userInfo;
        $scope.Editor = false;
        if (sessionStorage.email) {
            $scope.userLogged = true;
            $scope.userEmail = sessionStorage.email;
            $scope.userId = sessionStorage.ID;
            $scope.user = sessionStorage.User;
            userInfo = angular.fromJson(sessionStorage.userInfo);
            $scope.profilePhoto = userInfo.profile_photo_url;
            angular.element('#logout').css("display", "inline-block");
        } else {
            window.location.href = "/Home/login"
        }
        $http.get("/api/categories/GetUserSearchStrings?userid=" + sessionStorage.ID).then(function (result) {
            $scope.userStr = result.data;
        });

        $http.get("/api/categories/GetUserWords?userid=" + sessionStorage.ID).then(function (result) {
            $scope.userWords = result.data;
        });

        $scope.changePhoto = function () {
            bootbox.prompt("Add new proflie photo url", function (result) {
                if (result) {
                    var user = angular.fromJson(sessionStorage.userInfo);
                    user.profile_photo_url = result;
                    $http.post("/api/addUser", user).then(function (result) {
                        if (result.data === "Ok") {
                            $http.get("/api/getUsers?email=" + user.email).then(function (res) {
                                var userInfo = angular.fromJson(sessionStorage.userInfo);
                                userInfo.profile_photo_url = res.data[0].profile_photo_url;
                                sessionStorage.userInfo = angular.toJson(userInfo);
                                $scope.profilePhoto = res.data[0].profile_photo_url;
                            });
                        }
                    });
                }
            }) ;
        };

        if (userInfo.Role === "Editor" || userInfo.Role === "Admin") {
            $scope.categories = ["Skills", "Job Title", "Education", "Certification", "Company"];
            $scope.approved = ["True", "False"];

            $http.get("/api/editorWords").then(function (result) {
                angular.forEach(result.data, function (item) {
                    switch (item.CategoryID) {
                        case ("1"):
                            item.Category = "Job Title";
                            break;
                        case ("2"):
                            item.Category = "Skills";
                            break;
                        case ("3"):
                            item.Category = "Certification";
                            break;
                        case ("4"):
                            item.Category = "Education";
                            break;
                        case ("5"):
                            item.Category = "Company";
                            break;
                    }
                    switch (item.Available) {
                        case (true):
                            item.Approved = "True";
                            break;
                        case (false):
                            item.Approved = "False";
                            break;
                    }
                });
                $scope.Words = result.data;
            });

            $http.get("/api/editorSearchStrings").then(function (result) {
                angular.forEach(result.data, function (item) {
                    switch (item.CategoryID) {
                        case ("1"):
                            item.Category = "Job Title";
                            break;
                        case ("2"):
                            item.Category = "Skills";
                            break;
                        case ("3"):
                            item.Category = "Certification";
                            break;
                        case ("4"):
                            item.Category = "Education";
                            break;
                        case ("5"):
                            item.Category = "Company";
                            break;
                    }
                    switch (item.Available) {
                        case (true):
                            item.Approved = "True";
                            break;
                        case (false):
                            item.Approved = "False";
                            break;
                    }
                });
                $scope.SearchStrigns = result.data;
            });

            $scope.save = function (item) {
                if (item.Word) {
                    var msg = item.Word + ' will be approved !';
                    bootbox.confirm(msg, function (result) {
                        if (result) {
                            if (item.Approved == "True") {
                                item.Available = true;
                            } else {
                                item.Available = false;
                            }

                            $http.post("/api/addWord", item).then(function (result) {
                                if (result.data == "OK") {
                                    delete item.Approved;
                                    var index = $scope.Words.indexOf(item);
                                    $scope.Words.splice(index, 1);
                                }
                            });
                        }
                    });
                }
                if (item.SearchString) {
                    var msg = item.SearchString + ' will be approved !';
                    bootbox.confirm(msg, function (result) {
                        if (result) {
                            if (item.Approved == "True") {
                                item.Available = true;
                            } else {
                                item.Available = false;
                            }

                            $http.post("/api/AddSearchString", item).then(function (result) {
                                if (result.data == "OK") {
                                    delete item.Approved;
                                    var index = $scope.SearchStrigns.indexOf(item);
                                    $scope.SearchStrigns.splice(index, 1);
                                }
                            });
                        }
                    });
                }
            }

            $scope.viewGroup = function (item) {
                if (item.Word) {
                   
                            $http.get("/api/categories/GetSimilarWords?category=" + item.CategoryID + "&group=" + item.WordsGroup).then(function (results) {
                                $scope.groupSynonymResult = results.data;
                            });
                        }
                else if (item.SearchString) {
                    $http.get("/api/categories/GetSimilarSearchStrings?category=" + item.CategoryID + "&group=" + item.SearchStringGroupID).then(function (results) {
                          $scope.groupSearchStringResult = results.data;
                    });
                }
            };

            $scope.remove = function (item) {
                if (item.Word) {
                    var msg = item.Word + ' will be removed !';
                    bootbox.confirm(msg, function (result) {
                        if (result) {
                            var index = $scope.Words.indexOf(item);
                            $scope.Words.splice(index, 1);
                            $http.post("/api/removeWord", item).then(function (result) {
                                if (result.data == "Ok") {

                                }
                            });
                        }
                    });
                }
                else if (item.SearchString) {
                    var msg = item.SearchString + ' will be removed !';
                    bootbox.confirm(msg, function (result) {
                        if (result) {
                            var index = $scope.SearchStrigns.indexOf(item);
                            $scope.SearchStrigns.splice(index, 1);
                            $http.post("/api/removeSeachString", item).then(function (result) {
                                if (result.data == "Ok") {

                                }
                            });
                        }
                    });
                }
            }
            $(".pendingData").show();

            $scope.savePassword = function () {
                if ($scope.pw1 !== $scope.pw2) {
                    $(".errorPasswordMsg").show();
                } else {
                    var user = angular.fromJson(sessionStorage.userInfo);
                    user.user_password = $scope.pw1;
                    $(".errorPasswordMsg").hide();
                    $http.post("/api/addUser", user).then(function () {
                        if ("Ok") {
                            bootbox.alert("Your password has been successfully changed.")
                            $scope.pw1 = '';
                            $scope.pw2 = '';
                            $(".newPasswordData").hide();
                            toggleClass();
                        }
                    });
                }
            }
        }
    }])
})();