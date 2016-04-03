app.controller('CardAddController', function ($scope, $state, commonService, notifyService, cardService, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card');

    $scope.createCard = function () {
        $scope.$broadcast('show-errors-check-validity');
        if (!$scope.cardAddForm.$valid) {
            return;
        }

        cardService.createCard(function () {
            //reset form field
            //reset();

            //display notification
            notifyService.popCreateSuccessful();

            $state.go("cards");
        }, $scope.Card);
    };

    function reset() {
        $scope.Card = {};
        $scope.$broadcast('show-errors-reset');
    }

    //Back Routing
    $scope.doTheBack = function () {
        window.history.back();
    };
});