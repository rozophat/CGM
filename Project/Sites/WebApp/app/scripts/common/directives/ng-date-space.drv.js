app.directive('ngDateSpace', function ($timeout) {
	return function (scope, element) {
		element.bind("keydown keypress", function (e) {
			var code = e.keyCode || e.which;
			if (code === 32) {
				e.preventDefault();
				scope.$apply(function () {
						scope.showDatePicker = !scope.showDatePicker;
				});
			}
			if (code === 13) {
				e.preventDefault();
				scope.$apply(function () {
					//If we want aftern press enter focus again to the dateinput, uncomment $timeout below
					//$timeout(function () {
						if (scope.showDatePicker) {
							scope.showDatePicker = false;
						}
					//});
				});
			}
		});
	};
});
