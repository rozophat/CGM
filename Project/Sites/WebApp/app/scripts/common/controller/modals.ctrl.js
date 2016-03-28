app.controller('ConfirmModalCtrl', function ($scope, $modalInstance) {
	$scope.Ok = function () {
		$modalInstance.close();
	};

	$scope.Cancel = function () {
		$modalInstance.dismiss();
	};
});

//app.controller('basicConfirmModalCtrl', function ($scope, $modalInstance) {
//	$scope.Ok = function () {
//		$modalInstance.close();
//	};

//	$scope.Cancel = function () {
//		$modalInstance.dismiss();
//	};
//});

//app.controller('messageConfirmModalCtrl', function ($scope, $modalInstance, message) {
//	$scope.message = message;

//	$scope.Ok = function () {
//		$modalInstance.close();
//	};

//	$scope.Cancel = function () {
//		$modalInstance.dismiss();
//	};
//});


//app.controller('warningModalCtrl', function ($scope, $modalInstance, content) {
//	$scope.Content = content;
//	$scope.Ok = function () {
//		$modalInstance.close();
//	};
//});

//app.controller('ConfirmModalCtrl', function ($scope, $modalInstance, content) {
//	$scope.Content = content;
//	$scope.Ok = function () {
//		$modalInstance.close();
//	};

//	$scope.Cancel = function () {
//		$modalInstance.dismiss();
//	};
//});