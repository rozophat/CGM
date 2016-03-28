app.controller('ResetPasswordController', function ($scope, notifyService, authService, $stateParams,
    commonService) {
	$scope.User = {};
	$scope.isSuccessReset = false;
	var userid = $stateParams.userid;
	var code = $stateParams.code;

	var sendData = { params: { id: userid } };
	var ajaxPromise = commonService.request(urlApiUser + "/GetUserEmail", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.User.Email = response.email;
		}
	});


	$scope.resetPassword = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.resetPasswordForm.$valid) {
			return;
		}
		$scope.User.Code = code;
		authService.resetPassword(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response.error == "0") {
					$scope.isSuccessReset = true;
				} else if (response.error == "1") {
					$scope.error = true;
					$scope.errorList = response.errorList;
				}
			}
		}, $scope.User);
	};

	function reset() {
		$scope.User = {};
		$scope.$broadcast('show-errors-reset');
	}

	function validate() {
		if ($scope.User.Password == $scope.User.ConfirmPassword) {
			$scope.resetPasswordForm.ConfirmPassword.$setValidity("pwCheck", true);
		} else {
			$scope.resetPasswordForm.ConfirmPassword.$setValidity("pwCheck", false);
		}
	}

	$scope.$watch('User.ConfirmPassword', function () {
		validate();
	});

});