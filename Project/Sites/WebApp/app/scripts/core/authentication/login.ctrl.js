app.controller('LoginController', function ($rootScope, $state, $scope, $location, commonService, authService, notifyService) {
	$scope.login = function () {
		$scope.isLoading = true;
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.userLoginForm.$valid) {
			$scope.isLoading = false;
			return;
		}
		
		authService.login($scope.LoginData).then(function (response) {
			if (response.userName == "rootadmin") {
				$location.path('/users');
			} else {
				$state.go('accounts');
			}
		},
		 function (err) {
		 	if (err.error == "invalid_grant") {
		 		$scope.message = err.error_description;
		 		$scope.error = true;
		 	}
		 })
		.finally(function () {
			$scope.isLoading = false;
		 });
	};
	
	$scope.forgetPassword = function () {
		if (!$scope.forgetPasswordForm.$valid) {
			$scope.forgetPasswordForm.Email.$dirty = true;
			$scope.forgetPasswordForm.Email.$pristine = false;
			return;
		}

		var sentData = {
			params: {
				email: $scope.ForgetPassword.Email
			}
		};
		var ajaxPromise = commonService.request(urlApiUser + "/ForgotPassword", sentData);
		ajaxPromise.then(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response == "2") {
					$scope.notExistEmail = true;
				} else if (response == "1") {
					$scope.wrongFormatEmail = true;
				} else {
					notifyService.popConfirmEmailSent();
				}
			}
		});
	};
});
