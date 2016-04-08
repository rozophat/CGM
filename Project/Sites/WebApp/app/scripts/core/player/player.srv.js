app.factory('playerService', function ($http) {
	return {
		//createPlayer: function (callback, player) {
		//	$http.post(urlApiCard, player).success(callback);
		//},

		updatePlayer: function (callback, player) {
			$http.put(urlApiCard, player).success(callback);
		},

		deletePlayer: function (callback, id) {
			$http.delete(urlApiCard + '/' + id).success(callback);
		}
	};
});