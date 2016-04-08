app.controller('PlayerViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, playerService, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('player');

	$scope.Player = {};
	$scope.id = $stateParams.id;

	$scope.currTab = $stateParams.tab !== "" ? parseInt($stateParams.tab) : 1;
	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;

	function loadScreen() {
		$scope.defaultTab($scope.currTab);
		if ($scope.currTab == 2) {
		    $scope.tableCGParams.$params.page = $scope.currPage;
		}
	}

	//UPDATE PLAYER INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiPlayer, sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.Player = response;
		}
	});

	$scope.updatePlayerGroup = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.playerGroupEditForm.$valid) {
			return;
		}

		playerService.updatePlayerGroup(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.Player);
	};

	//CARD-GROUP
	$scope.tableCGParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			PurchasedDate: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 2) {
				$state.transitionTo('player-view', { id: $scope.id, tab: 2, page: params.page() }, { location: "replace", reload: false, notify: false });
			}
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					playerId: $scope.id,
					search: ''
				}
			};
			var ajaxCardGroupPromise = commonService.request(urlApiPlayer + "/PlayerCardGroupDatatable", pagingInfo);
			ajaxCardGroupPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.PlayerCardGroups = response.Data);
			});
		}
	});
	
	//STARS
	$scope.tableStarParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			CreatedDate: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 2) {
				$state.transitionTo('player-view', { id: $scope.id, tab: 3, page: params.page() }, { location: "replace", reload: false, notify: false });
			}
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					playerId: $scope.id,
					search: ''
				}
			};
			var ajaxPlayerStarPromise = commonService.request(urlApiPlayer + "/PlayerStarDatatable", pagingInfo);
			ajaxPlayerStarPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.PlayerStars = response.Data);
			});
		}
	});
	
	//ASSETS
	$scope.tableAssetParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			CreatedDate: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 4) {
				$state.transitionTo('player-view', { id: $scope.id, tab: 4, page: params.page() }, { location: "replace", reload: false, notify: false });
			}
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					playerId: $scope.id,
					search: ''
				}
			};
			var ajaxAssetPromise = commonService.request(urlApiPlayer + "/PlayerAssetDatatable", pagingInfo);
			ajaxAssetPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.PlayerAssets = response.Data);
			});
		}
	});

	//TAB Handler
	$scope.defaultTab = function (tabId) {
		$scope.tab = tabId;
	};

	$scope.setTab = function (tabId) {
		$scope.tab = tabId;
		var page = 1;
		//if (tabId == 4) {
		//    page = $scope.tablePAParams.$params.page;
		//}
		$state.transitionTo('card-group-view', { id: $scope.id, tab: tabId, page: page }, { location: "replace", reload: false, notify: false });
		console.log('Show Tab');
	};

	$scope.isSet = function (tabId) {
		return $scope.tab === tabId;
	};

	//Back Routing
	$scope.doTheBack = function () {
		window.history.back();
	};

	loadScreen();
});