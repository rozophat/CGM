app.controller('ListingAddressAddController', function ($scope, $stateParams, commonService, notifyService, accountService) {
	$scope.Address = {};
	$scope.listingId = $stateParams.listingId;

	$scope.createAddress = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.addressAddForm.$valid) {
			return;
		}

		$scope.Address.ListingId = $scope.listingId;
		accountService.createListingAddress(function () {
			//reset form field
			reset();

			//display notification
			notifyService.popCreateSuccessful();

			$scope.doTheBack();
		}, $scope.Address);
	};

	function reset() {
		$scope.Address = {};
		$scope.$broadcast('show-errors-reset');
	}

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};

});