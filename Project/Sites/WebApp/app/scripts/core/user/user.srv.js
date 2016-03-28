app.factory('userService', function ($http) {
	return {
		createUser: function (callback, user) {
			$http.post(urlApiUser + "/Register", user).success(callback);
		},

		updateUser: function (callback, user) {
			user = { UserName: user.UserName, Email: user.Email, IsActive: user.IsActive };
			$http.put(urlApiUser + "/Update", user).success(callback);
		},

		//method for delete
		deleteUser: function (callback, user) {
			$http.post(urlApiUser + "/DoDeleteAccount", user).success(callback);
		},
	};
});