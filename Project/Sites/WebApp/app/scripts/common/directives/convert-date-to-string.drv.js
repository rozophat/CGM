app.directive('convertDateToString', function ($filter, $parse) {
	return {
		restrict: 'A',
		require: 'ngModel',
		scope: false,
		link: function (scope, el, attrs, ngModel) {
			scope.$on('covert-date-to-string', function () {
				var newValue = $filter('date')(ngModel.$modelValue, 'MM-dd-yyyy');
				var modelGetter = $parse(attrs.ngModel);
				var modelSetter = modelGetter.assign;

				modelSetter(scope, newValue);
			});
		}
	};
});