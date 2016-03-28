app.controller('BannerViewController', function ($scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, accountService, localStorageService, FileUploader) {
	$scope.Banner = {};
	$scope.BannerImageTypes = [];
	$scope.Categories = [];
	var originalData = [];
	$scope.id = $stateParams.id;

	$scope.currTab = $stateParams.tab !== "" ? parseInt($stateParams.tab) : 1;
	$scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;

	$scope.webapiUrl = config.apiServiceBaseUri;
	
	getImageType();

	getCategories();
	
	function getCategories() {
		var ajaxCategoriesPromise = commonService.request(urlApiCategory + "/GetCategories");
		ajaxCategoriesPromise.then(function (response) {
			if (response !== null && response !== undefined && response.length > 0) {
				$scope.Categories = response;
			}
		});
	}

	function loadScreen() {
		$scope.defaultTab($scope.currTab);
	}

	//UPDATE ACCOUNT INFORMATION
	var sendData = { params: { id: $scope.id } };
	var ajaxPromise = commonService.request(urlApiAccount + "/GetBannerInfo", sendData);
	ajaxPromise.then(function (response) {
		if (!commonService.isNullOrUndefined(response)) {
			$scope.Banner = response;
		}
	});

	$scope.updateBanner = function () {
		$scope.$broadcast('show-errors-check-validity');
		if (!$scope.bannerEditForm.$valid) {
			return;
		}

		accountService.updateBanner(function () {
			//display notification
			notifyService.popUpdateSuccessful();

			$scope.doTheBack();
		}, $scope.Banner);
	};
	
	//IMAGES
	$scope.tableBIParams = new ngTableParams({
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
					bannerId: $scope.id,
					search: ''
				}
			};
			var ajaxListingImagePromise = commonService.request(urlApiAccount + "/BannerImageDatatable", pagingInfo);
			ajaxListingImagePromise.then(function (response) {
				params.total(response.Total);
				originalData = angular.copy(response.Data);
				$defer.resolve($scope.banenrImage = response.Data);
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
		updateBannerImage(row);
	};

	function getImageType() {
		var ajaxBannerImageTypePromise = commonService.request(urlApiAccount + "/GetBannerImageType");
		ajaxBannerImageTypePromise.then(function (response) {
			if (response !== null && response.length > 0) {
				$scope.BannerImageTypes = response;
			}
		});
	}

	$scope.getImageTypeName = function (imageType) {
		for (var i = 0; i < $scope.BannerImageTypes.length; i++) {
			if ($scope.BannerImageTypes[i].Id == imageType) {
				return $scope.BannerImageTypes[i].Name;
			}
		}
		return "";
	};

	var updateBannerImage = function (row) {
		accountService.updateBannerImage(function () {
			//display notification
			notifyService.popUpdateSuccessful();
		}, row);
	};

	//UPLOADNG IMAGE PROCESSING
	var uploader = $scope.uploader = new FileUploader({
		headers: {
			Authorization: 'Bearer ' + localStorageService.get('authorizationData').token
		},
		url: urlApiAccount + '/UploadBannerFile/' + $scope.id
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
		$scope.tableBIParams.reload();
	};

	console.info('uploader', uploader);

	var dialogTemplatePath = "dist/views/modal/";
	$scope.showDeleteBIConfirmation = function (bannerId, id) {
		var modalInstance = $modal.open({
			templateUrl: dialogTemplatePath + 'delete-confirmation-modal.tpl.html',
			controller: 'ConfirmModalCtrl',
			windowClass: "confirmation-modal",
		});

		modalInstance.result.then(function () {
			deleteBannerImage(bannerId, id);
		}, function () {
		});
	};

	var deleteBannerImage = function (bannerId, id) {
		accountService.deleteBannerImage(function () {
			//reload datatable
			$scope.tableBIParams.reload();

			//display notification
			notifyService.popDeleteSuccessful();
		}, bannerId, id);
	};

	//TAB Handler
	$scope.defaultTab = function (tabId) {
		$scope.tab = tabId;
	};

	$scope.setTab = function (tabId) {
		$scope.tab = tabId;
		var page = 1;
		$state.transitionTo('banner-view', { id: $scope.id, tab: tabId, page: page }, { location: "replace", reload: false, notify: false });
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