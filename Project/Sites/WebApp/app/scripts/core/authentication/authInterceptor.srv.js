app.factory('authInterceptorService', ['$rootScope', '$q', '$injector', '$location', '$timeout', 'localStorageService', function ($rootScope, $q, $injector, $location, $timeout, localStorageService) {

	var authInterceptorServiceFactory = {};

	//var _ajaxCount = 0;
	//var isFromApi = false;
	$rootScope.IsAjaxLoading = true;

	var _request = function (config) {
		config.headers = config.headers || {};

		var authData = localStorageService.get('authorizationData');
		if (authData) {
			config.headers.Authorization = 'Bearer ' + authData.token;
		}
		
		return config;
	};

	var _response = function (response) {
		var http = $injector.get('$http');

		if (http.pendingRequests.length <= 0) {
			$rootScope.IsAjaxLoading = false;
		}

		return response;
	};

	var _responseError = function (rejection) {
		var http = $injector.get('$http');
		var authService = $injector.get('authService');

		if (rejection.status === 401) {
			var authData = localStorageService.get('authorizationData');

			if (authData) {
				if (authData.useRefreshTokens) {
					$location.path('/lock');
					return $q.reject(rejection);
				}
			}
			authService.logOut();
			$location.path('/login');
		}

		if (http.pendingRequests.length <= 0) {
			$rootScope.IsAjaxLoading = false;
		}

		return $q.reject(rejection);
	};

	authInterceptorServiceFactory.request = _request;
	authInterceptorServiceFactory.response = _response;
	authInterceptorServiceFactory.responseError = _responseError;

	return authInterceptorServiceFactory;
}]);