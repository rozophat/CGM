app.controller('CardGroupsController', function ($rootScope, $scope, notifyService, $stateParams, $state,
    commonService, cardGroupService, ngTableParams, $modal, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card-group');

    $scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;
    $scope.currSearchValue = $stateParams.searchValue !== "" ? $stateParams.searchValue : '';

    //set language title for order table, container table, tooltipn message
    $rootScope.$on('$translateChangeSuccess', function () {
        translateHeaderTable();
    });

    translateHeaderTable();
    function translateHeaderTable() {
        $scope.translateHeaderTables = {
            'Name': $translate.instant('CARD_GROUP_TABLE_HEADER_NAME'),
            'Description': $translate.instant('CARD_GROUP_TABLE_HEADER_DESCRIPTION'),
            'CreatedDate': $translate.instant('CARD_GROUP_TABLE_HEADER_CREATEDDATE'),
            'UpdatedDate': $translate.instant('CARD_GROUP_TABLE_HEADER_UPDATEDDATE'),
            'AppleProductCode': $translate.instant('CARD_GROUP_TABLE_HEADER_APPLEPRODUCTCODE'),
            'GoogleProductCode': $translate.instant('CARD_GROUP_TABLE_HEADER_GOOGLEPRODUCTCODE'),
            'Type': $translate.instant('CARD_GROUP_TABLE_HEADER_TYPE'),
            'PriceInStars': $translate.instant('CARD_GROUP_TABLE_HEADER_PRICEINSTARS'),
            'Price': $translate.instant('CARD_GROUP_TABLE_HEADER_PRICE'),
            'Active': $translate.instant('CARD_GROUP_TABLE_HEADER_ACTIVE')
        };
    }

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,           // count per page
        sorting: {
            Name: "desc"
        }
    }, {
        total: 0, // length of data
        getData: function ($defer, params) {
            $state.transitionTo('card-groups', { page: params.page(), searchValue: $scope.SearchValue }, { location: "replace", reload: false, notify: false });
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
            var ajaxPromise = commonService.request(urlApiCardGroup + "/Datatable", pagingInfo);
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

    var deleteCardGroup = function (id) {
        cardGroupService.deleteCardGroup(function () {
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
            windowClass: "confirmation-modal",
        });

        modalInstance.result.then(function () {
            deleteCardGroup(id);
        }, function () {
        });
    };
});