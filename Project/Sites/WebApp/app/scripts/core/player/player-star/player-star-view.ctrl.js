app.controller('PlayerStarViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, playerService, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('player');

	$scope.PlayerStar = {};
	$scope.id = $stateParams.id;

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiPlayer + "/GetPlayerStarInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.PlayerStar = response;
		}
	});

	$scope.updatePlayerStar = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.playerStarEditForm.$valid) {
			return;
		}

		playerService.updatePlayerStar(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.PlayerStar);
	};

	var _nextSearchText = null;
	var _nextSearchDeferral = null;
	$scope.AjaxRequestRunning = false;
	$scope.getASPlayerCardGroup = function ($viewValue) {
	    _nextSearchText = $viewValue;
	    _nextSearchDeferral = $q.defer();

	    if (!$scope.AjaxRequestRunning)
	        triggerNextSearch(_nextSearchText, _nextSearchDeferral);

	    return _nextSearchDeferral.promise;
	};

	function triggerNextSearch(currentSearchText, currentDeferral) {
	    $scope.AjaxRequestRunning = true;
	    var sentData = { params: { value: currentSearchText } };
	    var ajaxPsPromise = commonService.request(urlApiPlayer + "/GetAutoSuggestPlayerCardGroup", sentData);

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

	$scope.onPlayerCardGroupSelect = function (item) {
	    if (!commonService.isNullOrUndefined(item)) {
	        $scope.PlayerStar.PlayerCardGroupId = item.Id;
	        $scope.PlayerFullName = item.PlayerFullName;
	        $scope.GroupName = item.GroupName;
	    }
	};

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};
});