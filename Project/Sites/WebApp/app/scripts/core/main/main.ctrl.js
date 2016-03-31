app.controller('MainController', function ($rootScope, $scope, $location, authService, $translate, $translatePartialLoader, commonService) {
	$scope.changeLang = function (keyLang) {
		commonService.setLanguage(keyLang);
		$translate.use(keyLang);
		//commonService.setElementFocus(null);
	};

	$scope.logOut = function () {
		authService.logOut();
		$location.path('/login');
	};

	$scope.authentication = authService.authentication;
	
	var tc = this;

	tc.defaultTab = function(tabId) {
		tc.tab = tabId;
	};

	tc.setTab = function (tabId) {
		tc.tab = tabId;
		console.log('Show Tab');
	};

	tc.isSet = function (tabId) {
		return tc.tab === tabId;
	};
});

app.controller('tabController', ['$scope', function ($scope) {

	var tc = this;

	tc.defaultTab = function(tabId) {
		tc.tab = tabId;
	};

	tc.setTab = function (tabId) {
		tc.tab = tabId;
		console.log('Show Tab');
	};

	tc.isSet = function (tabId) {
		return tc.tab === tabId;
	};

}]);