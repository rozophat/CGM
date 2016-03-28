app.directive('loading', function ($http, $timeout) {
	return {
		restrict: 'A',
		link: function (scope, elm, attrs) {
			scope.isAjaxRequestLoading = function () {
				return $http.pendingRequests.length > 0;
			};

			scope.$watch(scope.isAjaxRequestLoading, function (v) {
				$timeout(function() {
					if ($http.pendingRequests.length > 0) {
						elm.show();
					} else {
						elm.hide();
					}
				}, 2000);
			});
		}
	};
});