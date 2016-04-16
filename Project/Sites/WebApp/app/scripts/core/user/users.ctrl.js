app.controller('UsersController', function ($rootScope, $scope, notifyService,
    commonService, userService, ngTableParams, $modal, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('user');

	$rootScope.$on('$translateChangeSuccess', function () {
		translateHeaderTable();
	});

	translateHeaderTable();
	function translateHeaderTable() {
		$scope.translateHeaderTables = {
			'UserName': $translate.instant('USER_ADMIN_HEADER_USERNAME'),
			'Email': $translate.instant('USER_ADMIN_HEADER_EMAIL')
		};
	}

	var originalData = [];
	$scope.tableParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			UserName: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					search: $scope.SearchValue !== undefined ? $scope.SearchValue : ''
				}
			};
			var ajaxPromise = commonService.request(urlApiUser + "/Datatable", pagingInfo);
			ajaxPromise.then(function (response) {
				params.total(response.Total);
				originalData = angular.copy(response.Data);
				$defer.resolve($scope.Users = response.Data);
			});
		}
	});

	$scope.Search = function () {
		$scope.tableParams.reload();
	};

	$scope.cancel = function (row, rowForm) {
		var originalRow = $scope.resetRow(row, rowForm);
		angular.extend(row, originalRow);
	};

	$scope.resetRow = function (row, rowForm) {
		row.isEditing = false;
		rowForm.$setPristine();
		return _.findWhere(originalData, { UserName: row.UserName });
	};

	$scope.save = function (row, rowForm) {
		var originalRow = $scope.resetRow(row, rowForm);
		angular.extend(originalRow, row);
		updateUser(row);
	};

	var updateUser = function (row) {
		userService.updateUser(function () {
			//display notification
			notifyService.popUpdateSuccessful();
		}, row);
	};

	var dialogTemplatePath = "dist/views/modal/";
	$scope.showDeleteConfirmation = function (user) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteUser(user);
		}, function () {
		});
	};

	var deleteUser = function (user) {
		userService.deleteUser(function () {
			//reload datatable
			$scope.tableParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, user);
	};

	$scope.createUser = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.userAddForm.$valid) {
			return;
		}

		userService.createUser(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response == "0") {
					//reset form field
					reset();
					$scope.tableParams.reload();
					notifyService.popCreateSuccessful();
				} else if (response == "1") {
					notifyService.popExistUser();
				} else if (response == "2") {
					notifyService.popExistEmail();
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
			$scope.userAddForm.ConfirmPassword.$setValidity("pwCheck", true);
		} else {
			$scope.userAddForm.ConfirmPassword.$setValidity("pwCheck", false);
		}
	}

	$scope.$watch('User.ConfirmPassword', function () {
		validate();
	});

});