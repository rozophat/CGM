app.controller('AccountAddController', function ($scope, $state, commonService, notifyService, accountService) {
	$scope.createAccount = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.accountAddForm.$valid) {
			return;
		}

		accountService.createAccount(function () {
			//reset form field
			//reset();
			
			//display notification
			notifyService.popCreateSuccessful();
			
			$state.go("accounts");
		}, $scope.Account);
	};

	function reset() {
		$scope.Account = {};
		$scope.$broadcast('show-errors-reset');
	}

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};
});