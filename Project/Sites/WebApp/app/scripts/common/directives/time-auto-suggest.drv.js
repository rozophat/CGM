//use for mapping with mask.js - PHAT NGUYEN 08/08/2014
app.directive('timeAutoSuggest', function () {
	return {
		require: 'ngModel',
		restrict: 'A',
		priority: 101, //must be set it more than priority of mask.js. We need to validate value before the mask validate
		link: function (scope, element, attrs, controller) {
			controller.$parsers.unshift(function (viewValue) {
				var number = viewValue.match(/\d/g);
				number = number !== null ? number.join("") : "";
				var valLength = number.length;
				if (valLength > 4) {
					valLength = 4;
					number = number.substring(0, number.length - 1);
				}
				var numbers = number.split('');
				var returnValue = "";
				if (valLength == 1) {
					if (numbers[0] <= 2) {
						returnValue = numbers[0] + "hh:mm";
					}
					if (numbers[0] > 2) {
						returnValue = "0" + numbers[0] + "h:mm";
					}
				}
				if (valLength == 2) {
					if (numbers[0] < 2) {
						returnValue = numbers[0] + numbers[1] + "h:mm";
					}
					if (numbers[0] == 2) {
						if (numbers[1] >= 4) {
							returnValue = "20h:mm";
						} else {
							returnValue = "2" + numbers[1] + "h:mm";
						}
					}
				}
				else if (valLength == 3) {
					if (numbers[0] < 2) {
						returnValue = numbers[0] + number[1];
					} else if (numbers[0] > 2) {
						returnValue = "0" + numbers[0];
					} else {
						if (numbers[1] < 4) {
							returnValue += "2" + numbers[1];
						} else {
							returnValue += "20";
						}
					}

					if (numbers[2] < 6) {
						returnValue += numbers[2] + ":mm";
					} else {
						returnValue += ":0" + numbers[2] + "m";
					}
				}
				else if (valLength == 4) {
					if (numbers[0] < 2) {
						returnValue = numbers[0] + number[1];
					} else if (numbers[0] > 2) {
						returnValue = "0" + numbers[0];
					} else {
						if (numbers[1] < 4) {
							returnValue += "2" + numbers[1];
						} else {
							returnValue += "20";
						}
					}

					if (numbers[2] < 6) {
						returnValue += ":" + numbers[2] + numbers[3] + "m";
					} else {
						returnValue += ":0" + numbers[2] + "m";
					}
				}
				//set the value to the view
				element.val(returnValue);
				//must be return value for the next $parsers.unshift from mask.js
				return returnValue;
			});
		}
	};
});
