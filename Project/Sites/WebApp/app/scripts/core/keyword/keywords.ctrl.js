app.controller('KeywordsController', function ($scope, notifyService,
    commonService, keywordService, ngTableParams, $modal) {
	
	var originalData = [];

	initData();

	$scope.tableParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			KeywordName: "asc"
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
			var ajaxPromise = commonService.request(urlApiKeyword + "/Datatable", pagingInfo);
			ajaxPromise.then(function (response) {
				params.total(response.Total);
				originalData = angular.copy(response.Data);
				$defer.resolve($scope.Keywords = response.Data);
			});
		}
	});

	$scope.Search = function () {
		$scope.tableParams.reload();
	};

	$scope.cancel = function(row, rowForm) {
		var originalRow = $scope.resetRow(row, rowForm);
		angular.extend(row, originalRow);
	};

	$scope.resetRow = function(row, rowForm) {
		row.isEditing = false;
		rowForm.$setPristine();
		return _.findWhere(originalData, {Id: row.Id});
	};

	$scope.save = function(row, rowForm) {
		var originalRow = $scope.resetRow(row, rowForm);
		angular.extend(originalRow, row);
		updateKeyword(row);
	};

	var updateKeyword = function (row) {
		keywordService.updateKeyword(function () {
			//display notification
			notifyService.popUpdateSuccessful();
		}, row);
	};

	var dialogTemplatePath = "dist/views/modal/";
	$scope.showDeleteConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteKeyword(id);
		}, function () {
		});
	};

	var deleteKeyword = function (id) {
		keywordService.deleteKeyword(function () {
			//reload datatable
			$scope.tableParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	$scope.createKeyword = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.keywordAddForm.$valid) {
			return;
		}

		keywordService.createKeyword(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response == "0") {
					//reset form field
					reset();
					$scope.tableParams.reload();
					notifyService.popCreateSuccessful();
				} else if(response == "1") {
					notifyService.popExistKeyword();
				}
			}
		}, $scope.Keyword);
	};

	$scope.getKeywordType = function (code) {
		return keywordService.getKeywordType(code);
	};

	function initData() {
		$scope.Keyword = {};
		$scope.Keyword.KeywordType = "1";
	}

	function reset() {
		initData();
		$scope.$broadcast('show-errors-reset');
	}

});