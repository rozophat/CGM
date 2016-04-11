app.controller('CardGroupViewController', function ($rootScope, $scope, $stateParams, $state, $q, $modal, ngTableParams, commonService, notifyService, cardGroupService, $translate, $translatePartialLoader) {
    $translatePartialLoader.addPart('card-group');
    $translatePartialLoader.addPart('card');

    $scope.CardGroup = {};
    $scope.id = $stateParams.id;

    $scope.currTab = $stateParams.tab !== "" ? parseInt($stateParams.tab) : 1;
    $scope.currPage = $stateParams.page !== "" ? parseInt($stateParams.page) : 1;

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
                    type: $scope.SearchType  !== undefined ? $scope.SearchType : '',
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

    $scope.Search = function() {
        $scope.tableCGParams.reload();
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