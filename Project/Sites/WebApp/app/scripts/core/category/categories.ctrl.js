app.controller('CategoriesController', function ($scope, notifyService, $stateParams, $state,
    commonService, categoryService, ngTableParams, $modal) {

	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;
	$scope.currSearchValue = $stateParams.searchValue !== "" ? $stateParams.searchValue : '';

	$scope.tableParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			Name: "desc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			$state.transitionTo('categories', { page: params.page(), searchValue: $scope.SearchValue }, { location: "replace", reload: false, notify: false });
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
			var ajaxPromise = commonService.request(urlApiCategory + "/Datatable", pagingInfo);
			ajaxPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.categories = response.Data);
			});
		}
	});

	$scope.tableParams.$params.page = $scope.currPage;
	$scope.SearchValue = $scope.currSearchValue;

	$scope.Search = function () {
		$scope.tableParams.reload();
	};

	$scope.getCategoryType = function(code) {
		return categoryService.getCategoryType(code);
	};

	var dialogTemplatePath = "dist/views/modal/";
	$scope.showDeleteConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteCategory(id);
		}, function () {
		});
	};

	var deleteCategory = function (id) {
		categoryService.deleteCategory(function () {
			//reload datatable
			$scope.tableParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

});