app.controller('CardGroupAddController', function ($scope, $state, commonService, notifyService, cardGroupService, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card-group');

    $scope.createCardGroup = function () {
        $scope.$broadcast('show-errors-check-validity');
        if (!$scope.cardGroupAddForm.$valid) {
            return;
        }

        cardGroupService.createCardGroup(function () {
            //reset form field
            //reset();

            //display notification
            notifyService.popCreateSuccessful();

            $state.go("card-groups");
        }, $scope.CardGroup);
    };

    function reset() {
        $scope.CardGroup = {};
        $scope.$broadcast('show-errors-reset');
    }

    //Back Routing
    $scope.doTheBack = function () {
        window.history.back();
    };
});