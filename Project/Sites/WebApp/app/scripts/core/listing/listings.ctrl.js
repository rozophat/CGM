app.controller('ListingsController', function ($scope, notifyService, $stateParams, $state,
    commonService, accountService, ngTableParams, $modal) {

	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;
	$scope.currSearchValue = $stateParams.searchValue !== "" ? $stateParams.searchValue : '';

	$scope.tableParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			Title: "desc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			$state.transitionTo('listings', { page: params.page(), searchValue: $scope.SearchValue }, { location: "replace", reload: false, notify: false });
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
			var ajaxPromise = commonService.request(urlApiAccount + "/ListingDatatable", pagingInfo);
			ajaxPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.listings = response.Data);
			});
		}
	});

	$scope.tableParams.$params.page = $scope.currPage;
	$scope.SearchValue = $scope.currSearchValue;

	$scope.Search = function () {
		$scope.tableParams.reload();
	};

	var dialogTemplatePath = "dist/views/modal/";
	$scope.showDeleteConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteAccountListing(id);
		}, function () {
		});
	};

	var deleteAccountListing = function (id) {
		accountService.deleteAccountListing(function () {
			//reload datatable
			$scope.tableParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

});