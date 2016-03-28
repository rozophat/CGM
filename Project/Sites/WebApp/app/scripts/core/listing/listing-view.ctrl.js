app.controller('ListingViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, accountService, localStorageService, keywordService, FileUploader) {
	$scope.Listing = {};
	$scope.ListingImageTypes = [];
	$scope.Categories = [];
	var originalData = [];
	$scope.id = $stateParams.id;

	$scope.currTab = $stateParams.tab !== "" ? parseInt($stateParams.tab) : 1;
	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;

	$scope.webapiUrl = config.apiServiceBaseUri;

	getImageType();
	getCategories();
	
	function loadScreen() {
		$scope.defaultTab($scope.currTab);
		if ($scope.currTab == 6) {
			$scope.tableLAParams.$params.page = $scope.currPage;
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
	var ajaxPromise = commonService.request(urlApiAccount + "/GetListingInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.Listing = response;
		}
	});

	$scope.updateListing = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.listingEditForm.$valid) {
			return;
		}

		accountService.updateListing(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.Listing);
	};
	
	//ASSIGN KEYWORD
	$scope.tableLKParams = new ngTableParams({
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
			var ajaxListingKeywordPromise = commonService.request(urlApiAccount + "/ListingKeywordDatatable", pagingInfo);
			ajaxListingKeywordPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.listingKeywords = response.Data);
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
		var ajaxAssignKeywordPromise = commonService.request(urlApiAccount + "/AssignKeywordToListing", { params: { listingId: $scope.id, keywordName: $scope.NewKeyword } });
		ajaxAssignKeywordPromise.then(function (response) {
			if (!commonService.isNullOrUndefined(response)) {
				if (response.Successful === true || response.Successful == "true") {
					notifyService.popAssignKeywordSuccessful();

					//reload datatable
					$scope.tableLKParams.reload();
				} else if ((response.Successful === false || response.Successful == "false") && response.Message == "nexist") {
					showCreateNewKeywordConfirmation($scope.NewKeyword);
				}
			}
		});
	};

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

	$scope.showDeleteListingKeywordConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteListingKeyword(id);
		}, function () {
		});
	};

	var deleteListingKeyword = function (id) {
		accountService.deleteListingKeyword(function () {
			//reload datatable
			$scope.tableLKParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, id);
	};

	$scope.getKeywordType = function (code) {
		return keywordService.getKeywordType(code);
	};

	//READ LISTING COMMENT
	$scope.tableLCParams = new ngTableParams({
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
			var ajaxListingCommentPromise = commonService.request(urlApiAccount + "/ListingCommentDatatable", pagingInfo);
			ajaxListingCommentPromise.then(function (response) {
				params.total(response.Total);
				$defer.resolve($scope.listingComments = response.Data);
			});
		}
	});

	//READ LISTING LIKE
	$scope.tableLLParams = new ngTableParams({
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
			var ajaxListingLikePromise = commonService.request(urlApiAccount + "/ListingLikeDatatable", pagingInfo);
			ajaxListingLikePromise.then(function (response) {
				params.total(response.Total);
				$scope.Total = response.Total;
				$defer.resolve($scope.listingLikes = response.Data);
			});
		}
	});
	
	//ADDRESSES
	$scope.tableLAParams = new ngTableParams({
		page: 1,            // show first page
		count: 10,           // count per page
		sorting: {
			AddressLine1: "asc"
		}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			if ($scope.tab == 6) {
				$state.transitionTo('listing-view', { id: $scope.id, tab: 6, page: params.page() }, { location: "replace", reload: false, notify: false });
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
			var ajaxListingAddressPromise = commonService.request(urlApiAccount + "/ListingAddressDatatable", pagingInfo);
			ajaxListingAddressPromise.then(function (response) {
				params.total(response.Total);
				$scope.Total = response.Total;
				$defer.resolve($scope.addresses = response.Data);
			});
		}
	});
	
	//IMAGES
	$scope.tableLIParams = new ngTableParams({
		page: 1,            // show first page
		count: 5,           // count per page
		//sorting: {
		//	DayOfWeek: "asc"
		//}
	}, {
		total: 0, // length of data
		getData: function ($defer, params) {
			var sortInfo = params.orderBy()[0];
			var pagingInfo = {
				params: {
					page: params.page(),
					itemsPerPage: params.count(),
					//sortBy: "",
					//reverse: sortInfo.charAt(0) == "-",
					listingId: $scope.id,
					search: ''
				}
			};
			var ajaxListingImagePromise = commonService.request(urlApiAccount + "/ListingImageDatatable", pagingInfo);
			ajaxListingImagePromise.then(function (response) {
				params.total(response.Total);
				originalData = angular.copy(response.Data);
				$defer.resolve($scope.listingImage = response.Data);
			});
		}
	});

	$scope.cancel = function (row, rowForm) {
		var originalRow = $scope.resetRow(row, rowForm);
		angular.extend(row, originalRow);
	};

	$scope.resetRow = function (row, rowForm) {
		row.isEditing = false;
		rowForm.$setPristine();
		return _.findWhere(originalData, { Id: row.Id });
	};

	$scope.save = function (row, rowForm) {
		var originalRow = $scope.resetRow(row, rowForm);
		angular.extend(originalRow, row);
		updateListingImage(row);
	};

	function getImageType() {
		var ajaxListingImageTypePromise = commonService.request(urlApiAccount + "/GetListingImageType");
		ajaxListingImageTypePromise.then(function (response) {
			if (response !== null && response.length > 0) {
				$scope.ListingImageTypes = response;
			}
		});
	}

	$scope.getImageTypeName = function(imageType) {
		for (var i = 0; i < $scope.ListingImageTypes.length; i++)
		{
			if ($scope.ListingImageTypes[i].Id == imageType) {
				return $scope.ListingImageTypes[i].Name;
			}
		}
		return "";
	};

	var updateListingImage = function (row) {
		accountService.updateListingImage(function () {
			//display notification
			notifyService.popUpdateSuccessful();
		}, row);
	};

	//UPLOADNG IMAGE PROCESSING
	var uploader = $scope.uploader = new FileUploader({
		headers: {
			Authorization: 'Bearer ' + localStorageService.get('authorizationData').token
		},
		url: urlApiAccount + '/UploadListingFile/' + $scope.id
	});

	// FILTERS
	uploader.filters.push({
		name: 'extensionFilter',
		fn: function (item, options) {
			var filename = item.name;
			var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
			if (extension == "jpg" || extension == "png" || extension == "gif" || extension == "jpeg")
				return true;
			else {
				alert('Invalid file format. Please select a image file and try again.');
				return false;
			}
		}
	});

	uploader.filters.push({
		name: 'sizeFilter',
		fn: function (item, options) {
			var fileSize = item.size;
			fileSize = parseInt(fileSize) / (1024 * 1024);
			if (fileSize <= 5)
				return true;
			else {
				alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
				return false;
			}
		}
	});

	uploader.filters.push({
		name: 'itemResetFilter',
		fn: function (item, options) {
			if (this.queue.length < 5)
				return true;
			else {
				alert('You have exceeded the limit of uploading files.');
				return false;
			}
		}
	});

	// CALLBACKS
	uploader.onWhenAddingFileFailed = function (item, filter, options) {
		console.info('onWhenAddingFileFailed', item, filter, options);
	};

	uploader.onAfterAddingFile = function (fileItem) {
		console.info('Files ready for upload.');
	};

	uploader.onSuccessItem = function (fileItem, response, status, headers) {
		console.info('Selected file has been uploaded successfully.');
	};

	uploader.onErrorItem = function (fileItem, response, status, headers) {
		console.info('We were unable to upload your file. Please try again.');
	};

	uploader.onCancelItem = function (fileItem, response, status, headers) {
		console.info('File uploading has been cancelled.');
	};

	uploader.onAfterAddingAll = function (addedFileItems) {
		console.info('onAfterAddingAll', addedFileItems);
	};

	uploader.onBeforeUploadItem = function (item) {
		console.info('onBeforeUploadItem', item);
	};

	uploader.onProgressItem = function (fileItem, progress) {
		console.info('onProgressItem', fileItem, progress);
	};

	uploader.onProgressAll = function (progress) {
		console.info('onProgressAll', progress);
	};

	uploader.onCompleteItem = function (fileItem, response, status, headers) {
		console.info('onCompleteItem', fileItem, response, status, headers);
	};

	uploader.onCompleteAll = function () {
		console.info('onCompleteAll');
		$scope.tableLIParams.reload();
	};

	console.info('uploader', uploader);

	var dialogTemplatePath = "dist/views/modal/";
	$scope.showDeleteLIConfirmation = function (listingId, id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteListingImage(listingId, id);
		}, function () {
		});
	};

	var deleteListingImage = function (listingId, id) {
		accountService.deleteListingImage(function () {
			//reload datatable
			$scope.tableLIParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, listingId, id);
	};
	
	//DELETE LISTING ADDRESS
	$scope.showDeleteLAConfirmation = function (id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteListingAddress(id);
		}, function () {
		});
	};

	var deleteListingAddress = function (id) {
		accountService.deleteListingAddress(function () {
			//reload datatable
			$scope.tableLAParams.reload();

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
			page = $scope.tableLAParams.$params.page;
		}
		$state.transitionTo('listing-view', { id: $scope.id, tab: tabId, page: page }, { location: "replace", reload: false, notify: false });
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