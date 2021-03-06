﻿app.controller('PlayersController', function ($rootScope, $scope, notifyService, $stateParams, $state,
    commonService, playerService, ngTableParams, $modal, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('player');

	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;
	$scope.currSearchValue = $stateParams.searchValue !== "" ? $stateParams.searchValue : '';

	$rootScope.$on('$translateChangeSuccess', function () {
		translateHeaderTable();
	});

	translateHeaderTable();
	function translateHeaderTable() {
		$scope.translateHeaderTables = {
			'FullName': $translate.instant('PLAYER_TABLE_HEADER_FULLNAME'),
			'NickName': $translate.instant('PLAYER_TABLE_HEADER_NICKNAME'),
			'Address': $translate.instant('PLAYER_TABLE_HEADER_ADDRESS'),
			'Points': $translate.instant('PLAYER_TABLE_HEADER_POINTS'),
			'PointsToWinStar': $translate.instant('PLAYER_TABLE_HEADER_POINTSTOWINSTAR')
		};
	}

	$scope.tableParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			FullName: "desc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			$state.transitionTo('players', { page: params.page(), searchValue: $scope.SearchValue }, { location: "replace", reload: false, notify: false });
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					search: $scope.SearchValue !== undefined ? $scope.SearchValue : ''
				}
			};
			var ajaxPromise = commonService.request(urlApiPlayer + "/Datatable", pagingInfo);
			ajaxPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.accounts = response.Data);
			});
		}
	});

	$scope.tableParams.$params.page = $scope.currPage;
	$scope.SearchValue = $scope.currSearchValue;

	$scope.Search = function () {
		$scope.tableParams.reload();
	};

	var deletePlayer = function (id) {
		playerService.deletePlayer(function () {
			//reload datatable
			$scope.tableParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	var dialogTemplatePath = "dist/views/modal/";
	$scope.showDeleteConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal"
		});

		modalInstance.result.then(function () {
			deletePlayer(id);
		}, function () {
		});
	};
});