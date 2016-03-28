// PHAT NGUYEN 08/08/2014
// use for mapping with mask.js(priority: 101) and datepicker(priority: 99) 
// (only validation format date after user finish input)
app.directive('validateDateAfterBlur', function () {
	return {
		require: 'ngModel',
		restrict: 'A',
		priority: 99,	//must be set it less than priority of mask.js, and more than datepicker. 
							//We need to validate value the mask validate
		link: function (scope, element, attrs, controller) {
			controller.$parsers.unshift(function (viewValue) {
				if (viewValue !== undefined) {
					//get number array from viewValue
					var number = viewValue.match(/\d/g);
					//if the number.length == 8(using datepicker) or 6(using monthpicker), it means the user have finish input date...
					//then return to datepicker for validation
					if (number !== null && (number.length == 8 || number.length == 6)) {
						controller.$dirty = true;
						return viewValue;
					}
				}
				//Phat Nguyen - 12-25-2014
				//if date is null,"", undefined, must be set dirty = true
				controller.$dirty = false;
				return "";
			});
		}
	};
});