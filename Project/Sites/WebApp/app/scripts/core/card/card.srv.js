app.factory('cardService', function ($http) {
    return {
        createCard: function (callback, card) {
            $http.post(urlApiCard, card).success(callback);
        },

        updateCard: function (callback, account) {
            $http.put(urlApiCard, account).success(callback);
        },

        deleteCard: function (callback, id) {
            $http.delete(urlApiCard + '/' + id).success(callback);
        }
    };
});