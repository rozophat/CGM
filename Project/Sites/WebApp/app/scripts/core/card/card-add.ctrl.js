app.controller('CardAddController', function ($scope, $state, $q, commonService, notifyService, cardService, $translate, $translatePartialLoader) {
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

    var _nextSearchText = null;
    var _nextSearchDeferral = null;
    $scope.AjaxRequestRunning = false;
    $scope.getASCardGroup = function ($viewValue) {
        _nextSearchText = $viewValue;
        _nextSearchDeferral = $q.defer();

        if (!$scope.AjaxRequestRunning)
            triggerNextSearch(_nextSearchText, _nextSearchDeferral);

        return _nextSearchDeferral.promise;
    };

    function triggerNextSearch(currentSearchText, currentDeferral) {
        $scope.AjaxRequestRunning = true;
        var sentData = { params: { value: currentSearchText } };
        var ajaxPsPromise = commonService.request(urlApiCardGroup + "/GetAutoSuggestCardGroup", sentData);

        ajaxPsPromise.then(function (response) {

            // Handle HTTP success
            if (response && response instanceof Array) {
                var items = [];
                angular.forEach(response, function (item) {
                    items.push(item);
                });
                currentDeferral.resolve(items);
            } else {
                // Handle bad response
                currentDeferral.reject();
            }
        }, function (response) {
            // Handle HTTP error
            currentDeferral.reject();
        });

        ajaxPsPromise.finally(function () {
            $scope.AjaxRequestRunning = false;

            // If a different search is already waiting for execution, trigger it now!
            if (_nextSearchText !== currentSearchText)
                triggerNextSearch(_nextSearchText, _nextSearchDeferral);
        });
    }

    $scope.onCardGroupSelect = function (item) {
        if (!commonService.isNullOrUndefined(item)) {
            $scope.Card.GroupName = item.Name;
            $scope.Card.GroupId = item.Id;
        }
    };

    //Back Routing
    $scope.doTheBack = function () {
        window.history.back();
    };
});