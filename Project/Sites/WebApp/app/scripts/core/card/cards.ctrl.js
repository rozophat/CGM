app.controller('CardsController', function ($rootScope, $scope, notifyService, $stateParams, $state,
    commonService, cardService, ngTableParams, $modal, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card');

    $scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;
    $scope.currSearchValue = $stateParams.searchValue !== "" ? $stateParams.searchValue : '';

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

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,           // count per page
        sorting: {
            CreatedDate: "desc"
        }
    }, {
        total: 0, // length of data
        getData: function ($defer, params) {
            $state.transitionTo('cards', { page: params.page(), searchValue: $scope.SearchValue }, { location: "replace", reload: false, notify: false });
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
            var ajaxPromise = commonService.request(urlApiCard + "/Datatable", pagingInfo);
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

    var deleteCard = function (id) {
        cardService.deleteCard(function () {
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
            deleteCard(id);
        }, function () {
        });
    };
});