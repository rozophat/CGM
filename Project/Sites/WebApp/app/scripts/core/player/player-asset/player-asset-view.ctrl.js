app.controller('PlayerAssetViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, playerService, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('player');

	$scope.PlayerAsset = {};
	$scope.id = $stateParams.id;

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiPlayer + "/GetPlayerAssetInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.PlayerAsset = response;
		}
	});

	$scope.updatePlayerAsset = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.playerAssetEditForm.$valid) {
			return;
		}

		playerService.updatePlayerAsset(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.PlayerAsset);
	};

	var _nextSearchText = null;
	var _nextSearchDeferral = null;
	$scope.AjaxRequestRunning = false;
	$scope.getASCard = function ($viewValue) {
		_nextSearchText = $viewValue;
		_nextSearchDeferral = $q.defer();

		if (!$scope.AjaxRequestRunning)
			triggerNextSearch(_nextSearchText, _nextSearchDeferral);

		return _nextSearchDeferral.promise;
	};

	function triggerNextSearch(currentSearchText, currentDeferral) {
		$scope.AjaxRequestRunning = true;
		var sentData = { params: { value: currentSearchText } };
		var ajaxPsPromise = commonService.request(urlApiCard + "/GetAutoSuggestCard", sentData);

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

	$scope.onCardSelect = function (item) {
		if (!commonService.isNullOrUndefined(item)) {
			$scope.UsedCardId = item.Id;
			$scope.Question1 = item.Question1;
			$scope.Question2 = item.Question2;
			$scope.Question3 = item.Question3;
		}
	};


	var _nextAssetSearchText = null;
	var _nextAssetSearchDeferral = null;
	$scope.AjaxRequestRunning = false;
	$scope.getASAsset = function ($viewValue) {
	    _nextAssetSearchText = $viewValue;
	    _nextSearchDeferral = $q.defer();

	    if (!$scope.AjaxRequestRunning)
	        triggerNextAssetSearch(_nextAssetSearchText, _nextAssetSearchDeferral);

	    return _nextAssetSearchDeferral.promise;
	};

	function triggerNextAssetSearch(currentSearchText, currentDeferral) {
	    $scope.AjaxRequestRunning = true;
	    var sentData = { params: { value: currentSearchText } };
	    var ajaxPsPromise = commonService.request(urlApiAsset + "/GetAutoSuggestAsset", sentData);

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
	        if (_nextAssetSearchText !== currentSearchText)
	            triggerNextSearch(_nextAssetSearchText, _nextAssetSearchDeferral);
	    });
	}

	$scope.onAssetSelect = function (item) {
	    if (!commonService.isNullOrUndefined(item)) {
	        $scope.PlayerAsset.AssetId = item.Id;
	        $scope.PlayerAsset.AssetName = item.Name;
	    }
	};

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};
});