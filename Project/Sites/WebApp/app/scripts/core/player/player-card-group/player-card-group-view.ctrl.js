app.controller('PlayerCardGroupViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, playerService, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('player');

	$scope.PlayerCardGroup = {};
	$scope.id = $stateParams.id;

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiPlayer + "/GetPlayerCardGroupInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.PlayerCardGroup = response;
		}
	});

	$scope.updatePlayerCardGroup = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.playerCardGroupEditForm.$valid) {
			return;
		}

		playerService.updatePlayerCardGroup(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.PlayerCardGroup);
	};

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};
});