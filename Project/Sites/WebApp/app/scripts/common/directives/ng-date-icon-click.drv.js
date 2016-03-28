app.directive('ngDateIconClick', function () {
	return function (scope, element) {
		element.bind('click', function () {
			var input;
			if (!element.hasClass("disabled")) {
				if (!element[0].disabled) {
					input = element.prev('input');
					if (input.is(':focus')) {
						input.trigger('blur');
					} else {
						input.trigger('click');
					}
				}
			}
		});
	};
});