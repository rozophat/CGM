app.factory('cardGroupService', function ($http) {
    return {
        createCardGroup: function (callback, cardGroup) {
            $http.post(urlApiCardGroup, cardGroup).success(callback);
        },

        updateCardGroup: function (callback, account) {
            $http.put(urlApiCardGroup, account).success(callback);
        },

        deleteCardGroup: function (callback, id) {
            $http.delete(urlApiCardGroup + '/' + id).success(callback);
        }
    };
});