app.controller('ListingAddressViewController', function ($scope, $stateParams, $q, $modal, ngTableParams, commonService, notifyService, accountService) {
	$scope.Address = {};
	$scope.id = $stateParams.id;

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiAccount + "/GetListingAddressInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.Address = response;
		}
	});

	$scope.updateAddress = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.addressEditForm.$valid) {
			return;
		}

		accountService.updateListingAddress(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.Address);
	};
	
	//TAB Handler
	$scope.defaultTab = function (tabId) {
		$scope.tab = tabId;
	};

	$scope.setTab = function (tabId) {
		$scope.tab = tabId;
		console.log('Show Tab');
	};

	$scope.isSet = function (tabId) {
		return $scope.tab === tabId;
	};
	
	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};
});