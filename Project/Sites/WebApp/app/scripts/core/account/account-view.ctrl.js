app.controller('AccountViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, accountService, keywordService) {
	$scope.Account = {};
	$scope.Categories = [];
	$scope.id = $stateParams.id;

	$scope.currTab = $stateParams.tab !== "" ? parseInt($stateParams.tab) : 1;
	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;

	getCategories();

	function loadScreen() {
		$scope.defaultTab($scope.currTab);
		if ($scope.currTab == 6) {
			$scope.tableALIParams.$params.page = $scope.currPage;
		}
		if ($scope.currTab == 7) {
			$scope.tableABParams.$params.page = $scope.currPage;
		}
	}

	function getCategories() {
		var ajaxCategoriesPromise = commonService.request(urlApiCategory + "/GetCategories");
		ajaxCategoriesPromise.then(function (response) {
			if (response !== null && response !== undefined && response.length > 0) {
				$scope.Categories = response;
			}
		});
	}

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiAccount, sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.Account = response;
		}
	});
	
	$scope.updateAccount = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.accountEditForm.$valid) {
			return;
		}

		accountService.updateAccount(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.Account);
	};
	
	//ASSIGN KEYWORD
	$scope.tableAKParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			KeywordName: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					id: $scope.id,
					search: ''
				}
			};
			var ajaxAccontKeywordPromise = commonService.request(urlApiAccount + "/KeywordDatatable", pagingInfo);
			ajaxAccontKeywordPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.accountKeywords = response.Data);
			});
		}
	});
	
	var _nextSearchText = null;
	var _nextSearchDeferral = null;
	$scope.AjaxRequestRunning = false;
	$scope.getASKeyword = function ($viewValue) {
		_nextSearchText = $viewValue;
		_nextSearchDeferral = $q.defer();

		if (!$scope.AjaxRequestRunning)
			triggerNextSearch(_nextSearchText, _nextSearchDeferral);

		return _nextSearchDeferral.promise;
	};

	function triggerNextSearch(currentSearchText, currentDeferral) {
		$scope.AjaxRequestRunning = true;
		var sentData = { params: { value: currentSearchText } };
		var ajaxKPromise = commonService.request(urlApiKeyword + "/GetAutoSuggestKeywords", sentData);

		ajaxKPromise.then(function (response) {

			// Handle HTTP success
			if (response && response instanceof Array) {
				var keywords = [];
				angular.forEach(response, function (item) {
					keywords.push(item.KeywordName);
				});
				currentDeferral.resolve(keywords);
			} else {
				// Handle bad response
				currentDeferral.reject();
			}
		}, function (response) {
			// Handle HTTP error
			currentDeferral.reject();
		});

		ajaxPromise.finally(function () {
			$scope.AjaxRequestRunning = false;

			// If a different search is already waiting for execution, trigger it now!
			if (_nextSearchText !== currentSearchText)
				triggerNextSearch(_nextSearchText, _nextSearchDeferral);
		});
	}

	$scope.assignKeyword = function () {
		var ajaxAssignKeywordPromise = commonService.request(urlApiAccount + "/AssignKeywordToAccount", { params: { accountId: $scope.id, keywordName: $scope.NewKeyword } });
		ajaxAssignKeywordPromise.then(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response.Successful === true || response.Successful == "true") {
					notifyService.popAssignKeywordSuccessful();

					//reload datatable
					$scope.tableAKParams.reload();
				} else if ((response.Successful === false || response.Successful == "false") && response.Message == "nexist") {
					showCreateNewKeywordConfirmation($scope.NewKeyword);
					//notifyService.popAssignKeywordUnsuccessful();
				}
			}
		});
	};

	var dialogTemplatePath = "dist/views/modal/";
	var showCreateNewKeywordConfirmation = function (keyword) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'create-new-keyword-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			createNewKeyword(keyword);
			$scope.NewKeyword = "";
		}, function () {
		});
	};

	var createNewKeyword = function (keyword) {
		var newKeyword = { KeywordName: keyword };
		keywordService.createKeyword(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response == "0") {
					notifyService.popCreateSuccessful();
				} 
			}
		}, newKeyword);
	};

	$scope.showDeleteAccountKeywordConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteAccountKeyword(id);
		}, function () {
		});
	};

	var deleteAccountKeyword = function (id) {
		accountService.deleteAccountKeyword(function () {
			//reload datatable
			$scope.tableAKParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	$scope.getKeywordType = function (code) {
		return keywordService.getKeywordType(code);
	};

	//ADD CONTACT
	$scope.tableACOParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			ContactName: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					id: $scope.id,
					search: ''
				}
			};
			var ajaxAccontContactPromise = commonService.request(urlApiAccount + "/ContactDatatable", pagingInfo);
			ajaxAccontContactPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.accountContacts = response.Data);
			});
		}
	});
	
	$scope.getASContact = function ($viewValue) {
		_nextSearchText = $viewValue;
		_nextSearchDeferral = $q.defer();

		if (!$scope.AjaxRequestRunning)
			triggerNextContactSearch(_nextSearchText, _nextSearchDeferral);

		return _nextSearchDeferral.promise;
	};

	function triggerNextContactSearch(currentSearchText, currentDeferral) {
		$scope.AjaxRequestRunning = true;
		var sentData = { params: { accountId: $scope.id, value: currentSearchText } };
		var ajaxKPromise = commonService.request(urlApiAccount + "/GetAutoSuggestContacts", sentData);

		ajaxKPromise.then(function (response) {

			// Handle HTTP success
			if (response && response instanceof Array) {
				var contacts = [];
				angular.forEach(response, function (item) {
					contacts.push(item.Id + " - " + item.FirstName + " " + item.LastName);
				});
				currentDeferral.resolve(contacts);
			} else {
				// Handle bad response
				currentDeferral.reject();
			}
		}, function (response) {
			// Handle HTTP error
			currentDeferral.reject();
		});

		ajaxPromise.finally(function () {
			$scope.AjaxRequestRunning = false;

			// If a different search is already waiting for execution, trigger it now!
			if (_nextSearchText !== currentSearchText)
				triggerNextSearch(_nextSearchText, _nextSearchDeferral);
		});
	}

	$scope.onSelect = function ($item) {
		var label = $item;
		var items = label.split(" - ");
		$scope.NewContactId = items[0];
		$scope.NewContactName = items[1];
	};

	$scope.addContact = function () {
		var ajaxAssignContactPromise = commonService.request(urlApiAccount + "/AddContactToAccount", { params: { accountId: $scope.id, contactId: $scope.NewContactId } });
		ajaxAssignContactPromise.then(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response == "true") {
					$scope.NewContactId = "";
					$scope.NewContactName = "";
					notifyService.popAddContactSuccessful();

					//reload datatable
					$scope.tableACOParams.reload();
				} else {
					notifyService.popAddContactUnsuccessful();
				}
			}
		});
	};
	
	$scope.showDeleteAccountContactConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteAccountContact(id);
		}, function () {
		});
	};

	var deleteAccountContact = function (id) {
		accountService.deleteAccountContact(function () {
			//reload datatable
			$scope.tableACOParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	//READ ACCOUNT COMMENT
	$scope.tableACParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			Rating: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					id: $scope.id,
					search: ''
				}
			};
			var ajaxAccontCommentPromise = commonService.request(urlApiAccount + "/CommentDatatable", pagingInfo);
			ajaxAccontCommentPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.accountComments = response.Data);
			});
		}
	});

	$scope.showDeleteAccountCommentConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteAccountComment(id);
		}, function () {
		});
	};

	var deleteAccountComment = function (id) {
		accountService.deleteAccountComment(function () {
			//reload datatable
			$scope.tableACParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};
	
	//READ ACCOUNT LIKE
	$scope.tableALParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			ByAccountName: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					id: $scope.id,
					search: ''
				}
			};
			var ajaxAccontLikePromise = commonService.request(urlApiAccount + "/LikeDatatable", pagingInfo);
			ajaxAccontLikePromise.then(function (response) {
				params.total(response.Total);
				$scope.Total = response.Total;
				$defer.resolve($scope.accountLikes = response.Data);
			});
		}
	});
	
	//ACCOUNT LISTING
	$scope.tableALIParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			Title: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 6) {
				$state.transitionTo('account-view', { id: $scope.id, tab: 6, page: params.page() }, { location: "replace", reload: false, notify: false });
			}
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					id: $scope.id,
					search: ''
				}
			};
			var ajaxAccontListingPromise = commonService.request(urlApiAccount + "/AccountListingDatatable", pagingInfo);
			ajaxAccontListingPromise.then(function (response) {
				params.total(response.Total);
				$scope.Total = response.Total;
				$defer.resolve($scope.accountListings = response.Data);
			});
		}
	});
	
	$scope.createListing = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.listingAddForm.$valid) {
			return;
		}

		$scope.Listing.AccountId = $scope.id;

		accountService.createListing(function () {
			//reload datatable
			$scope.tableALIParams.reload();

			//reset form field
			resetListingAddForm();

			//display notification
			notifyService.popCreateSuccessful();
		}, $scope.Listing);
	};

	function resetListingAddForm() {
		$scope.Listing = {};
		$scope.$broadcast('show-errors-reset');
	}

	$scope.showDeleteAccountListingConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteAccountListing(id);
		}, function () {
		});
	};

	var deleteAccountListing = function (id) {
		accountService.deleteAccountListing(function () {
			//reload datatable
			$scope.tableALIParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	//ACCOUNT BANNER
	$scope.tableABParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			Title: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 7) {
				$state.transitionTo('account-view', { id: $scope.id, tab: 7, page: params.page() }, { location: "replace", reload: false, notify: false });
			}
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					sortBy: sortInfo.slice(1),
					reverse: sortInfo.charAt(0) == "-",
					id: $scope.id,
					search: ''
				}
			};
			var ajaxAccontBannerPromise = commonService.request(urlApiAccount + "/AccountBannerDatatable", pagingInfo);
			ajaxAccontBannerPromise.then(function (response) {
				params.total(response.Total);
				$scope.Total = response.Total;
				$defer.resolve($scope.accountBanners = response.Data);
			});
		}
	});
	
	$scope.showDeleteAccountBannerConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteAccountBanner(id);
		}, function () {
		});
	};

	var deleteAccountBanner = function (id) {
		accountService.deleteAccountBanner(function () {
			//reload datatable
			$scope.tableABParams.reload();

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
		if (tabId == 6) {
			page = $scope.tableALIParams.$params.page;
		}
		if (tabId == 7) {
			page = $scope.tableABParams.$params.page;
		}
		$state.transitionTo('account-view', { id: $scope.id, tab: tabId, page: page }, { location: "replace", reload: false, notify: false });
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