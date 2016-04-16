app.controller('CardGroupViewController', function ($rootScope, $scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, cardGroupService, $translate, $translatePartialLoader) {
	$translatePartialLoader.addPart('card-group');
	$translatePartialLoader.addPart('card');

	$scope.CardGroup = {};
	$scope.id = $stateParams.id;

	$scope.currTab = $stateParams.tab !== "" ? parseInt($stateParams.tab) : 1;
	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;
	var dialogTemplatePath = "dist/views/modal/";
	function loadScreen() {
		$scope.defaultTab($scope.currTab);
		if ($scope.currTab == 2) {
			$scope.tableCGParams.$params.page = $scope.currPage;
		}
	}

	//set language title for order table, container table, tooltipn message
	$rootScope.$on('$translateChangeSuccess', function () {
		translateHeaderTable();
	});

	translateHeaderTable();
	function translateHeaderTable() {
		$scope.translateHeaderTables = {
			'GroupName': $translate.instant('CARD_TABLE_HEADER_GROUPNAME'),
			'CreatedDate': $translate.instant('CARD_TABLE_HEADER_CREATEDDATE'),
			'UpdatedDate': $translate.instant('CARD_TABLE_HEADER_UPDATEDDATE'),
			'Type': $translate.instant('CARD_TABLE_HEADER_TYPE'),
			'Difficulty': $translate.instant('CARD_TABLE_HEADER_DIFFICULTY'),
			'Question': $translate.instant('CARD_TABLE_HEADER_QUESTION')
		};
	}

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiCardGroup, sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.CardGroup = response;
		}
	});

	$scope.updateCardGroup = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.cardGroupEditForm.$valid) {
			return;
		}

		cardGroupService.updateCardGroup(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.CardGroup);
	};

	//CARDS
	$scope.tableCGParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			CreatedDate: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 2) {
				$state.transitionTo('card-group-view', { id: $scope.id, tab: 2, page: params.page() }, { location: "replace", reload: false, notify: false });
			}
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					id: $scope.id,
					type: $scope.SearchType !== undefined ? $scope.SearchType : '',
					difficulty: $scope.SearchDifficulty,
					search: $scope.SearchValue !== undefined ? $scope.SearchValue : ''
				}
			};
			var ajaxCardsPromise = commonService.request(urlApiCardGroup + "/CardDatatable", pagingInfo);
			ajaxCardsPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.cards = response.Data);
			});
		}
	});

	$scope.Search = function () {
		$scope.tableCGParams.reload();
	};

	var deleteCardFromGroup = function (id) {
		cardGroupService.deleteCardFromGroup(function () {
			//reload datatable
			$scope.tableCGParams.reload();
			// $scope.tableCGParams_Unselected.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	$scope.showDeleteCardFromGroup = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal"
		});

		modalInstance.result.then(function () {
			deleteCardFromGroup(id);
		}, function () {
		});
	};

	$scope.allCards = [];
	$scope.selectedCards = [];

	//UNSELECTED CARDS
	$scope.tableCGParams_Unselected = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			CreatedDate: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 3) {
				$state.transitionTo('card-group-view', { id: $scope.id, tab: 3, page: params.page() }, { location: "replace", reload: false, notify: false });
			}

			if (commonService.isNullOrUndefined($scope.allCards) || $scope.allCards.length <= 0) {
				var ajaxCardsPromise = commonService.request(urlApiCardGroup + "/AllCardDatatable");
				ajaxCardsPromise.then(function (response) {
					$scope.allCards = response.Data;
					params.total($scope.allCards.length);
					$defer.resolve($scope.allCards.slice((params.page() - 1) * params.count(), params.page() * params.count()));
				});
			} else {
				var filterData = $scope.allCards;
				if (!commonService.isNullOrUndefined($scope.UnSelectCarSearch.Value)) {
					var searchVal = $scope.UnSelectCarSearch.Value;
					filterData = _.filter($scope.allCards, function (item) {
						return item.Question1.includes(searchVal) || item.Question2.includes(searchVal) || item.Question3.includes(searchVal);

					});
				}

				if (!commonService.isNullOrUndefined($scope.UnSelectCarSearch.Type)) {
					var searchType = $scope.UnSelectCarSearch.Type;
					filterData = _.filter($scope.allCards, function (item) {
						return item.Type.includes(searchType);

					});
				}

				if (!commonService.isNullOrUndefined($scope.UnSelectCarSearch.Difficulty)) {
					var searchDifficulty = $scope.UnSelectCarSearch.Difficulty;
					filterData = _.filter($scope.allCards, function (item) {
						return item.Difficulty == searchDifficulty;
					});
				}

				params.total(filterData.length);
				$defer.resolve(filterData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
			}
		}
	});

	$scope.SearchUnselecteCard = function () {
		$scope.tableCGParams_Unselected.reload();
	};

	var deleteSelectedCard = function (id) {
		$scope.selectedCards = _.reject($scope.selectedCards, function (item) { return item.Id == id; });
		$scope.tableCGParams_Selected.reload();

		var card = _.find($scope.allCards, function (i) { return i.Id == id; });
		card.isChecked = false;
	};

	$scope.selectEntity = function (item) {
		if (item.isChecked) {
			$scope.selectedCards.push(item);
			$scope.tableCGParams_Selected.reload();
		} else {
			deleteSelectedCard(item.Id);
		}

	};

	//SELECTED CARDS
	$scope.tableCGParams_Selected = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			CreatedDate: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			params.total($scope.selectedCards.length);

			var crrPage = params.page();
			var acPage = $scope.selectedCards.length > 0 ? Math.ceil(($scope.selectedCards.length) / 10) : 1;

			if (crrPage > acPage) {
				$scope.tableCGParams_Selected.$params.page = acPage;
				$defer.resolve($scope.selectedCards.slice((acPage - 1) * params.count(), acPage * params.count()));
			} else {
				$defer.resolve($scope.selectedCards.slice((params.page() - 1) * params.count(), params.page() * params.count()));
			}
		}
	});


	$scope.showDeletedSelectionConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal"
		});

		modalInstance.result.then(function () {
			deleteSelectedCard(id);
		}, function () {
		});
	};

	$scope.updateCardToGroup = function () {
		var selectedIdList = getSelectedIdList();

		var data = { params: { selectedIds: selectedIdList.toString(), groupId: $scope.id } };

		var ajaxUpdateCardsToGroupPromise = commonService.request(urlApiCardGroup + "/UpdateCardsToGroup", data);
		ajaxUpdateCardsToGroupPromise.then(function (response) {
			notifyService.popUpdateSuccessful();
			$scope.allCards = [];
			$scope.selectedCards = [];
			$scope.tableCGParams.reload();
			$scope.tableCGParams_Unselected.reload();
			$scope.tableCGParams_Selected.reload();
		});
	};

	function getSelectedIdList() {
		return _.pluck($scope.selectedCards, 'Id');
	}

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
			page = $scope.tableCGParams_Unselected.$params.page;
		}
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