app.controller('CardViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, cardService, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card');

    $scope.Card = {};
    $scope.id = $stateParams.id;

    //UPDATE ACCOUNT INFORMATION
    var sendData = { params: { id: $scope.id } };
    var ajaxPromise = commonService.request(urlApiCard, sendData);
    ajaxPromise.then(function (response) {
        if (!commonService.isNullOrUndefined(response)) {
            $scope.Card = response;
        }
    });

    $scope.updateCard = function () {
        $scope.$broadcast('show-errors-check-validity');
        if (!$scope.cardEditForm.$valid) {
            return;
        }

        cardService.updateCard(function () {
            //display notification
            notifyService.popUpdateSuccessful();

            $scope.doTheBack();
        }, $scope.Card);
    };

    //Back Routing
    $scope.doTheBack = function () {
        window.history.back();
    };
});