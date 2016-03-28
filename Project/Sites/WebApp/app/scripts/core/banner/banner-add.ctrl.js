app.controller('BannerAddController', function ($scope, $stateParams, commonService, notifyService, accountService) {
	$scope.Banner = {};
	$scope.Categories = [];
	$scope.accountId = $stateParams.accountId;

	getCategories();
	function getCategories() {
		var ajaxCategoriesPromise = commonService.request(urlApiCategory + "/GetCategories");
		ajaxCategoriesPromise.then(function (response) {
			if (response !== null && response !== undefined && response.length > 0) {
				$scope.Categories = response;
			}
		});
	}

	$scope.createBanner = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.bannerAddForm.$valid) {
			return;
		}

		$scope.Banner.AccountId = $scope.accountId;
		accountService.createBanner(function () {
			//reset form field
			reset();

			//display notification
			notifyService.popCreateSuccessful();

			$scope.doTheBack();
		}, $scope.Banner);
	};

	function reset() {
		$scope.Banner = {};
		$scope.$broadcast('show-errors-reset');
	}

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};

});