app.controller('PlayerStarViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, playerService, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('player');

	$scope.PlayerStar = {};
	$scope.id = $stateParams.id;

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiPlayer + "/GetPlayerStarInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.PlayerStar = response;
		}
	});

	$scope.updatePlayerStar = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.playerStarEditForm.$valid) {
			return;
		}

		playerService.updatePlayerStar(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.PlayerStar);
	};

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};
});