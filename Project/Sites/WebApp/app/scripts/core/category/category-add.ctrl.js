app.controller('CategoryAddController', function ($scope, $state, $q, commonService, notifyService, categoryService) {

	initData();

	function initData() {
		$scope.Category = {};
		$scope.Category.Type = "1";
	}

	$scope.createCategory = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.categoryAddForm.$valid) {
			return;
		}

		categoryService.createCategory(function (response) {
			if (response.Successful === true || response.Successful == "true") {
				//display notification
				notifyService.popCreateSuccessful();

				$scope.doTheBack();
			} else {
				$scope.message = response.Message;
				$scope.error = true;
			}
		}, $scope.Category);
	};

	function reset() {
		$scope.Category = {};
		$scope.$broadcast('show-errors-reset');
	}

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
					categories.push(item);
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

angular.module("customTemplates", [
					"template/typeahead/category-match.tpl.html"]);

angular.module("template/typeahead/category-match.tpl.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/typeahead/category-match.tpl.html",
		"<a tabindex=\"-1\">" +
			"<span bind-html-unsafe=\"match.label.Name | typeaheadHighlight:query\"></span>" +
		"</a>"
	);
}]);