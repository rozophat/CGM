//display error when user submit form
app.directive('showErrors', function ($timeout) {
	return {
		restrict: 'A',
		require: '^form',
		link: function(scope, el, attrs, formCtrl) {
			var inputName = el.attr('name');

			scope.$on('show-errors-check-validity', function() {
				formCtrl.$dirty = true;
				formCtrl.$pristine = false;
				formCtrl[inputName].$dirty = true;
				formCtrl[inputName].$pristine = false;
			});

			scope.$on('show-errors-reset', function () {
				formCtrl.$dirty = false;
				formCtrl.$pristine = true;
				formCtrl[inputName].$dirty = false;
				formCtrl[inputName].$pristine = true;
			});
		}
	};
});
