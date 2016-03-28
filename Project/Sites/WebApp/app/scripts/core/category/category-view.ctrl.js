app.controller('CategoryViewController', function ($scope, $stateParams, $q, commonService, notifyService, categoryService) {
	$scope.Category = {};
	$scope.id = $stateParams.id;

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiCategory + "/GetCategoryInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.Category = response;
		}
	});

	$scope.updateCategory = function () {
		$scope.$broadcast('show-errors-check-validity');

		if (!$scope.categoryEditForm.$valid) {
			return;
		}

		categoryService.updateCategory(function (response) {
			if (response.Successful === true || response.Successful == "true") {
				//display notification
				notifyService.popUpdateSuccessful();
				
				$scope.categoryEditForm.$setPristine();
				$scope.$broadcast('show-errors-reset');
				
				//$scope.message = "";
				//$scope.error = false;
				$scope.doTheBack();
			} else {
				$scope.message = response.Message;
				$scope.error = true;
			}

		}, $scope.Category);
	};

	var _nextSearchText = null;
	var _nextSearchDeferral = null;
	$scope.AjaxRequestRunning = false;
	$scope.getASCategory = function ($viewValue) {
		_nextSearchText = $viewValue;
		_nextSearchDeferral = $q.defer();

		if (!$scope.AjaxRequestRunning)
			triggerNextSearch(_nextSearchText, _nextSearchDeferral);

		return _nextSearchDeferral.promise;
	};

	function triggerNextSearch(currentSearchText, currentDeferral) {
		$scope.AjaxRequestRunning = true;
		var sentData = { params: { value: currentSearchText } };
		var ajaxPromise = commonService.request(urlApiCategory + "/GetAutoSuggestCategories", sentData);

		ajaxPromise.then(function (response) {

			// Handle HTTP success
			if (response && response instanceof Array) {
				var categories = [];
				angular.forEach(response, function (item) {
					if (item.Id != $scope.Category.Id) {
						categories.push(item);
					}
				});
				currentDeferral.resolve(categories);
			} else {
				// Handle bad response
				currentDeferral.reject();
			}
		}, function (response) {
			// Handle HTTP error
			currentDeferral.reject();
		});

		ajaxPromise.finally(function () {
			$scope.AjaxRequestRunning = false;

			// If a different search is already waiting for execution, trigger it now!
			if (_nextSearchText !== currentSearchText)
				triggerNextSearch(_nextSearchText, _nextSearchDeferral);
		});
	}

	$scope.onCategorySelect = function (item) {
		if (!commonService.isNullOrUndefined(item)) {
			$scope.Category.ParentId = item.Id;
			$scope.Category.ParentCategoryName = item.Name;
		}
	};

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};
});