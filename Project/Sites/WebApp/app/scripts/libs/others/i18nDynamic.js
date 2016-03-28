"use strict";

var i18nModule = angular.module('i18nDynamic', ['ngLocale']);

i18nModule.factory("i18nLanguage", ['$rootScope', '$locale', '$log', function ($rootScope, $locale, $log) {

	var that = this;
	that.language = 'en';
	that.dictionary = {};
	var PLURAL_CATEGORY = { ZERO: "zero", ONE: "one", TWO: "two", FEW: "few", MANY: "many", OTHER: "other" };
	// Come from angularJS localization files
	// Add language that you need for your application
	that.angularLocale = {
		'vi': {
			"DATETIME_FORMATS": {
				"AMPMS": [
				  "SA",
				  "CH"
				],
				"DAY": [
				  "Ch\u1ee7 nh\u1eadt",
				  "Th\u1ee9 hai",
				  "Th\u1ee9 ba",
				  "Th\u1ee9 t\u01b0",
				  "Th\u1ee9 n\u0103m",
				  "Th\u1ee9 s\u00e1u",
				  "Th\u1ee9 b\u1ea3y"
				],
				"MONTH": [
				  "th\u00e1ng m\u1ed9t",
				  "th\u00e1ng hai",
				  "th\u00e1ng ba",
				  "th\u00e1ng t\u01b0",
				  "th\u00e1ng n\u0103m",
				  "th\u00e1ng s\u00e1u",
				  "th\u00e1ng b\u1ea3y",
				  "th\u00e1ng t\u00e1m",
				  "th\u00e1ng ch\u00edn",
				  "th\u00e1ng m\u01b0\u1eddi",
				  "th\u00e1ng m\u01b0\u1eddi m\u1ed9t",
				  "th\u00e1ng m\u01b0\u1eddi hai"
				],
				"SHORTDAY": [
				  "CN",
				  "Th 2",
				  "Th 3",
				  "Th 4",
				  "Th 5",
				  "Th 6",
				  "Th 7"
				],
				"SHORTMONTH": [
				  "thg 1",
				  "thg 2",
				  "thg 3",
				  "thg 4",
				  "thg 5",
				  "thg 6",
				  "thg 7",
				  "thg 8",
				  "thg 9",
				  "thg 10",
				  "thg 11",
				  "thg 12"
				],
				"fullDate": "EEEE, 'ng\u00e0y' dd MMMM 'n\u0103m' y",
				"longDate": "'Ng\u00e0y' dd 'th\u00e1ng' M 'n\u0103m' y",
				"medium": "dd-MM-yyyy HH:mm:ss",
				"mediumDate": "dd-MM-yyyy",
				"mediumTime": "HH:mm:ss",
				"short": "dd/MM/yyyy HH:mm",
				"shortDate": "dd/MM/yyyy",
				"shortTime": "HH:mm"
			},
			"NUMBER_FORMATS": {
				"CURRENCY_SYM": "\u20ab",
				"DECIMAL_SEP": ",",
				"GROUP_SEP": ".",
				"PATTERNS": [
				  {
				  	"gSize": 3,
				  	"lgSize": 3,
				  	"macFrac": 0,
				  	"maxFrac": 3,
				  	"minFrac": 0,
				  	"minInt": 1,
				  	"negPre": "-",
				  	"negSuf": "",
				  	"posPre": "",
				  	"posSuf": ""
				  },
				  {
				  	"gSize": 3,
				  	"lgSize": 3,
				  	"macFrac": 0,
				  	"maxFrac": 2,
				  	"minFrac": 2,
				  	"minInt": 1,
				  	"negPre": "-",
				  	"negSuf": "\u00a0\u00a4",
				  	"posPre": "",
				  	"posSuf": "\u00a0\u00a4"
				  }
				]
			},
			"id": "vi-vn",
			"pluralCat": function (n) { return PLURAL_CATEGORY.OTHER; }
		},
		'en': { "DATETIME_FORMATS": { "MONTH": ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], "SHORTMONTH": ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], "DAY": ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], "SHORTDAY": ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], "AMPMS": ["AM", "PM"], "medium": "MMM d, y h:mm:ss a", "short": "M/d/yy h:mm a", "fullDate": "EEEE, MMMM d, y", "longDate": "MMMM d, y", "mediumDate": "MMM d, y", "shortDate": "M/d/yy", "mediumTime": "h:mm:ss a", "shortTime": "h:mm a" }, "NUMBER_FORMATS": { "DECIMAL_SEP": ".", "GROUP_SEP": ",", "PATTERNS": [{ "minInt": 1, "minFrac": 0, "macFrac": 0, "posPre": "", "posSuf": "", "negPre": "-", "negSuf": "", "gSize": 3, "lgSize": 3, "maxFrac": 3 }, { "minInt": 1, "minFrac": 2, "macFrac": 0, "posPre": "\u00A4", "posSuf": "", "negPre": "(\u00A4", "negSuf": ")", "gSize": 3, "lgSize": 3, "maxFrac": 2 }], "CURRENCY_SYM": "$" }, "pluralCat": function (n) { if (n == 1) { return PLURAL_CATEGORY.ONE; } return PLURAL_CATEGORY.OTHER; }, "id": "en" },
		'en-us': { "DATETIME_FORMATS": { "MONTH": ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], "SHORTMONTH": ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], "DAY": ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], "SHORTDAY": ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], "AMPMS": ["AM", "PM"], "medium": "MMM d, y h:mm:ss a", "short": "M/d/yy h:mm a", "fullDate": "EEEE, MMMM d, y", "longDate": "MMMM d, y", "mediumDate": "MMM d, y", "shortDate": "M/d/yy", "mediumTime": "h:mm:ss a", "shortTime": "h:mm a" }, "NUMBER_FORMATS": { "DECIMAL_SEP": ".", "GROUP_SEP": ",", "PATTERNS": [{ "minInt": 1, "minFrac": 0, "macFrac": 0, "posPre": "", "posSuf": "", "negPre": "-", "negSuf": "", "gSize": 3, "lgSize": 3, "maxFrac": 3 }, { "minInt": 1, "minFrac": 2, "macFrac": 0, "posPre": "\u00A4", "posSuf": "", "negPre": "(\u00A4", "negSuf": ")", "gSize": 3, "lgSize": 3, "maxFrac": 2 }], "CURRENCY_SYM": "$" }, "pluralCat": function (n) { if (n == 1) { return PLURAL_CATEGORY.ONE; } return PLURAL_CATEGORY.OTHER; }, "id": "en-us" },
		'jp': {
			"DATETIME_FORMATS": {
				"AMPMS": [
				  "\u5348\u524d",
				  "\u5348\u5f8c"
				],
				"DAY": [
				  "\u65e5\u66dc\u65e5",
				  "\u6708\u66dc\u65e5",
				  "\u706b\u66dc\u65e5",
				  "\u6c34\u66dc\u65e5",
				  "\u6728\u66dc\u65e5",
				  "\u91d1\u66dc\u65e5",
				  "\u571f\u66dc\u65e5"
				],
				"MONTH": [
				  "1\u6708",
				  "2\u6708",
				  "3\u6708",
				  "4\u6708",
				  "5\u6708",
				  "6\u6708",
				  "7\u6708",
				  "8\u6708",
				  "9\u6708",
				  "10\u6708",
				  "11\u6708",
				  "12\u6708"
				],
				"SHORTDAY": [
				  "\u65e5",
				  "\u6708",
				  "\u706b",
				  "\u6c34",
				  "\u6728",
				  "\u91d1",
				  "\u571f"
				],
				"SHORTMONTH": [
				  "1\u6708",
				  "2\u6708",
				  "3\u6708",
				  "4\u6708",
				  "5\u6708",
				  "6\u6708",
				  "7\u6708",
				  "8\u6708",
				  "9\u6708",
				  "10\u6708",
				  "11\u6708",
				  "12\u6708"
				],
				"fullDate": "y\u5e74M\u6708d\u65e5EEEE",
				"longDate": "y\u5e74M\u6708d\u65e5",
				"medium": "yyyy/MM/dd H:mm:ss",
				"mediumDate": "yyyy/MM/dd",
				"mediumTime": "H:mm:ss",
				"short": "yyyy/MM/dd H:mm",
				"shortDate": "yyyy/MM/dd",
				"shortTime": "H:mm"
			},
			"NUMBER_FORMATS": {
				"CURRENCY_SYM": "\u00a5",
				"DECIMAL_SEP": ".",
				"GROUP_SEP": ",",
				"PATTERNS": [
				  {
				  	"gSize": 3,
				  	"lgSize": 3,
				  	"macFrac": 0,
				  	"maxFrac": 3,
				  	"minFrac": 0,
				  	"minInt": 1,
				  	"negPre": "-",
				  	"negSuf": "",
				  	"posPre": "",
				  	"posSuf": ""
				  },
				  {
				  	"gSize": 3,
				  	"lgSize": 3,
				  	"macFrac": 0,
				  	"maxFrac": 2,
				  	"minFrac": 2,
				  	"minInt": 1,
				  	"negPre": "\u00a4-",
				  	"negSuf": "",
				  	"posPre": "\u00a4",
				  	"posSuf": ""
				  }
				]
			},
			"id": "ja-jp",
			"pluralCat": function (n) { return PLURAL_CATEGORY.OTHER; }
		}
	};

	/**
	 * Constructor
	 */
	function i18nLanguageService() {
		this.setLocale($locale.id);
	}


	// Replace the format content with the new local selected
	i18nLanguageService.prototype.loadAngularLocale = function(language) {

		$locale.DATETIME_FORMATS = that.angularLocale[language].DATETIME_FORMATS;
		$locale.NUMBER_FORMATS = that.angularLocale[language].NUMBER_FORMATS;
		$locale.id = language;
	};

	i18nLanguageService.prototype.getTranslation = function (msgKey, locale) {
		return that.dictionary[msgKey];
	};

	i18nLanguageService.prototype.setLocale = function (newLocale) {
		this.loadAngularLocale(newLocale);
	};

	return new i18nLanguageService();
}]);



i18nModule.filter("i18nDyn", ['$locale', 'i18nLanguage', '$log', function ($locale, i18nLanguage, $log) {

	return function (msgKey) {
		$log.info('Filter applied for ' + msgKey);
		return i18nLanguage.getTranslation(msgKey, $locale);
	};

}]);





