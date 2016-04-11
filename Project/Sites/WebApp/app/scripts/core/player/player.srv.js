app.factory('playerService', function ($http) {
	return {
		updatePlayer: function (callback, player) {
			$http.put(urlApiPlayer, player).success(callback);
		},
		
		updatePlayerCardGroup: function (callback, cgPlayer) {
			$http.put(urlApiPlayer + "/UpdatePlayerCardGroup", cgPlayer).success(callback);
		},
		
		updatePlayerStar: function (callback, sPlayer) {
			$http.put(urlApiPlayer + "/UpdatePlayerStar", sPlayer).success(callback);
		},
		
		updatePlayerAsset: function (callback, aPlayer) {
			$http.put(urlApiPlayer + "/UpdatePlayerAsset", aPlayer).success(callback);
		},

		deletePlayer: function (callback, id) {
			$http.delete(urlApiPlayer + '/' + id).success(callback);
		},

		deletePlayerCardGroup: function (callback, id) {
			$http.delete(urlApiPlayer + '/DeletePlayerCardGroup/' + id).success(callback);
		},
		
		deletePlayerStar: function (callback, id) {
			$http.delete(urlApiPlayer + '/DeletePlayerStar/' + id).success(callback);
		},
		
		deletePlayerAsset: function (callback, id) {
			$http.delete(urlApiPlayer + '/DeletePlayerAsset/' + id).success(callback);
		},
	};
});