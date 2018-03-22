(function () {
    'use strict';

    var appModule = angular.module("bowlerApp", []);

    appModule.controller('tableController', controller);

    controller.$inject = ['$scope','$http'];

    function controller($scope,$http) {
        var self = this;      
        self.scope = $scope;  
        self.http = $http; 
       
        self.scope.firstThrow = "";
        self.scope.secondThrow = "";
        self.scope.thirdThrow = "";

        self.scope.frames = [];
        self.scope.currentRound = 1;

        self.scope.sendResult = function () {
            var currentFrame = {
                first: self.scope.firstThrow <= 10 && self.scope.firstThrow >= 0 && self.scope.firstThrow != "" ? self.scope.firstThrow : null,
                second: self.scope.secondThrow <= 10 && self.scope.secondThrow >= 0 && self.scope.secondThrow != "" ? self.scope.secondThrow : 0,
                third: self.scope.thirdThrow <= 10 && self.scope.thirdThrow >= 0 && self.scope.thirdThrow != "" ? self.scope.thirdThrow : 0
            };

            if (currentFrame.first !== null && currentFrame.second !== null && (self.scope.currentRound < 10 || currentFrame.third !== null)) { //TODO: Handle last round better
                self.scope.frames.push(new Frame(self.scope.currentRound, { First: currentFrame.first, Second: currentFrame.second, Third: currentFrame.third }, "Strike", 0));     

                self.http.post("/Home/RegisterScore", JSON.stringify(self.scope.frames))                
                .then(function mySuccess(response) {
                    self.scope.frames = response.data;                
                    self.scope.currentRound++;
                }, function myError(response) {
                });
            }
        }

        self.scope.returnMark = function(mark) {
            switch (mark) {
                case 0:
                    return 'Strike';
                case 1:
                    return 'Spare';
                case 2:
                    return 'Open';
            }
        }
    }
})();
