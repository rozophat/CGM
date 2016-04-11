app.controller('PlayerViewController', function ($rootScope, $scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, playerService, $translate, $translatePartialLoader) {
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
		if ($scope.currTab == 3) {
			$scope.tableStarParams.$params.page = $scope.currPage;
		}
		if ($scope.currTab == 4) {
			$scope.tableAssetParams.$params.page = $scope.currPage;
		}
	}

    //set language title for order table, container table, tooltipn message
	$rootScope.$on('$translateChangeSuccess', function () {
	    translateHeaderTable();
	});

	translateHeaderTable();
	function translateHeaderTable() {
	    $scope.translateHeaderTables = {
	        'PlayerFullName': $translate.instant('PLAYER_TABLE_HEADER_FULLNAME'),
	        'GroupName': $translate.instant('PLAYER_TABLE_HEADER_NICKNAME'),
	        'PurchasedDate': $translate.instant('PLAYER_TABLE_HEADER_ADDRESS'),
	        'PurchaseSource': $translate.instant('PLAYER_TABLE_HEADER_POINTS'),
	        'PointsToWinStar': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'TransactionId': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'StoreCost': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'StarsCost': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'CreatedDate': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'Used': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'IsPurchased': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'PurchaseTransactionId': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'AssetName': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR'),
	        'UsedDate': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR')
	    };
	}

	var dialogTemplatePath = "dist/views/modal/";

	//UPDATE PLAYER INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiPlayer, sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.Player = response;
		}
	});

	$scope.updatePlayer = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.playerEditForm.$valid) {
			return;
		}

		playerService.updatePlayer(function () {
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
	
	$scope.showDeleteCGConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deletePlayerCardGroup(id);
		}, function () {
		});
	};

	var deletePlayerCardGroup = function (id) {
		playerService.deletePlayerCardGroup(function () {
			//reload datatable
			$scope.tableCGParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

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
	
	$scope.showDeleteStarConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deletePlayerStar(id);
		}, function () {
		});
	};

	var deletePlayerStar = function (id) {
		playerService.deletePlayerStar(function () {
			//reload datatable
			$scope.tableStarParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

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

	$scope.showDeleteAssetConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deletePlayerAsset(id);
		}, function () {
		});
	};

	var deletePlayerAsset = function (id) {
		playerService.deletePlayerAsset(function () {
			//reload datatable
			$scope.tableAssetParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	//TAB Handler
	$scope.defaultTab = function (tabId) {
		$scope.tab = tabId;
	};

	$scope.setTab = function (tabId) {
		$scope.tab = tabId;
		var page = 1;
		if (tabId == 2) {
			page = $scope.tableCGParams.$params.page;
		}
		if (tabId == 3) {
			page = $scope.tableStarParams.$params.page;
		}
		if (tabId == 4) {
			page = $scope.tableAssetParams.$params.page;
		}
		$state.transitionTo('player-view', { id: $scope.id, tab: tabId, page: page }, { location: "replace", reload: false, notify: false });
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