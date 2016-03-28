app.directive('pwCheck', function () {
	return {
		require: 'ngModel',
		scope: {
			otherModelValue: '=pwCheck'
		},
		link: function (scope, element, attributes, ngModel) {

			function validate() {
				if (ngModel.$modelValue == scope.otherModelValue) {
					ngModel.$setValidity("pwCheck", true);
				} else {
					ngModel.$setValidity("pwCheck", false);
				}
			}

			scope.$watch('otherModelValue', function () {
				validate();
			});
		}
	};
});