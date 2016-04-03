app.controller('CardGroupViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, cardGroupService, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card-group');

    $scope.CardGroup = {};
    $scope.id = $stateParams.id;

    $scope.currTab = $stateParams.tab !== "" ? parseInt($stateParams.tab) : 1;
    $scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;

    function loadScreen() {
        $scope.defaultTab($scope.currTab);
        //if ($scope.currTab == 2) {
        //    $scope.tableALIParams.$params.page = $scope.currPage;
        //}
    }

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

    //TAB Handler
    $scope.defaultTab = function (tabId) {
        $scope.tab = tabId;
    };

    $scope.setTab = function (tabId) {
        $scope.tab = tabId;
        var page = 1;
        //if (tabId == 4) {
        //    page = $scope.tablePAParams.$params.page;
        //}
        $state.transitionTo('card-group-view', { id: $scope.id, tab: tabId, page: page }, { location: "replace", reload: false, notify: false });
        console.log('Show Tab');
    };

    $scope.isSet = function (tabId) {
        return $scope.tab === tabId;
    };

    //Back Routing
    $scope.doTheBack = function () {
        window.history.back();
    };

    loadScreen();
});