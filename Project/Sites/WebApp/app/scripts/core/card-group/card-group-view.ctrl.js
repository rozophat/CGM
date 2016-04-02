app.controller('CardGroupViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, cardGroupService, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card-group');

    $scope.CardGroup = {};
    $scope.id = $stateParams.id;

    //UPDATE ACCOUNT INFORMATION
    var sendData = { params: { id: $scope.id } };
    var ajaxPromise = commonService.request(urlApiCardGroup, sendData);
    ajaxPromise.then(function (response) {
        if (!commonService.isNullOrUndefined(response)) {
            $scope.CardGroup = response;
        }
    });

    $scope.updateCardGroup = function () {
        $scope.$broadcast('show-errors-check-validity');
        if (!$scope.cardGroupEditForm.$valid) {
            return;
        }

        cardGroupService.updateCardGroup(function () {
            //display notification
            notifyService.popUpdateSuccessful();

            $scope.doTheBack();
        }, $scope.CardGroup);
    };

    //Back Routing
    $scope.doTheBack = function () {
        window.history.back();
    };
});