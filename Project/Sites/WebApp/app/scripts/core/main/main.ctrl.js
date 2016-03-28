app.controller('MainController', function ($rootScope, $scope, $location, authService) {
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