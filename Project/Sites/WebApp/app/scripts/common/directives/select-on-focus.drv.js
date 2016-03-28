app.directive('selectOnFocus', function ($timeout) {
	return {
		restrict: 'ACE',
		link: function(scope, element, attrs) {
			element.bind('focus', function () {
				scope.$apply(function() {
					$timeout(function () {
						element[0].select();
					});
				});
			});
		}
	};
});