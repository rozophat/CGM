app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

	var serviceBase = config.apiServiceBaseUri;
	var authServiceFactory = {};

	var _authentication = {
		isAuth: false,
		userName: "",
		useRefreshTokens: false
	};

	var _externalAuthData = {
		provider: "",
		userName: "",
		externalAccessToken: ""
	};

	var _login = function (loginData) {

		var data = "grant_type=password&username=" + loginData.UserName + "&password=" + loginData.Password;

		if (loginData.useRefreshTokens) {
			data = data + "&client_id=" + config.clientId;
		}

		var deferred = $q.defer();

		$http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

			if (loginData.useRefreshTokens) {
				localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.UserName, refreshToken: response.refresh_token, useRefreshTokens: true });
			}
			else {
				localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.UserName, refreshToken: "", useRefreshTokens: false });
			}
			_authentication.isAuth = true;
			_authentication.userName = loginData.UserName;
			_authentication.useRefreshTokens = loginData.useRefreshTokens;
			if (_authentication.userName == "rootadmin") {
				_authentication.isRootAdmin = true;
			} else {
				_authentication.isRootAdmin = false;
			}

			deferred.resolve(response);

		}).error(function (err, status) {
			_logOut();
			deferred.reject(err);
		});

		return deferred.promise;

	};

	var _logOut = function () {

		localStorageService.remove('authorizationData');

		_authentication.isAuth = false;
		_authentication.userName = "";
		_authentication.useRefreshTokens = false;

	};

	var _fillAuthData = function () {

		var authData = localStorageService.get('authorizationData');
		if (authData) {
			_authentication.isAuth = true;
			_authentication.userName = authData.userName;
			_authentication.useRefreshTokens = authData.useRefreshTokens;
			if (_authentication.userName == "rootadmin") {
				_authentication.isRootAdmin = true;
			} else {
				_authentication.isRootAdmin = false;
			}
		}

	};

	var _refreshToken = function () {
		var deferred = $q.defer();

		var authData = localStorageService.get('authorizationData');

		if (authData) {

			if (authData.useRefreshTokens) {

				var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + config.clientId;

				localStorageService.remove('authorizationData');

				$http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

					localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

					deferred.resolve(response);

				}).error(function (err, status) {
					_logOut();
					deferred.reject(err);
				});
			}
		}

		return deferred.promise;
	};

	var _obtainAccessToken = function (externalData) {

		var deferred = $q.defer();

		$http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

			localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

			_authentication.isAuth = true;
			_authentication.userName = response.userName;
			_authentication.useRefreshTokens = false;
			if (_authentication.userName == "rootadmin") {
				_authentication.isRootAdmin = true;
			} else {
				_authentication.isRootAdmin = false;
			}
			deferred.resolve(response);

		}).error(function (err, status) {
			_logOut();
			deferred.reject(err);
		});

		return deferred.promise;

	};

	var _resetPassword = function(callback, resetPasswordData) {
		$http.post(urlApiUser, resetPasswordData).success(callback);
	};

	authServiceFactory.login = _login;
	authServiceFactory.logOut = _logOut;
	authServiceFactory.fillAuthData = _fillAuthData;
	authServiceFactory.authentication = _authentication;
	authServiceFactory.refreshToken = _refreshToken;

	authServiceFactory.obtainAccessToken = _obtainAccessToken;
	authServiceFactory.externalAuthData = _externalAuthData;
	authServiceFactory.resetPassword = _resetPassword;
	return authServiceFactory;
}]);