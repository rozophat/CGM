app.factory('keywordService', function ($http, Enum) {
	return {
		createKeyword: function (callback, keyword) {
			$http.post(urlApiKeyword, keyword).success(callback);
		},

		updateKeyword: function (callback, keyword) {
			$http.put(urlApiKeyword, keyword).success(callback);
		},

		deleteKeyword: function (callback, id) {
			$http.delete(urlApiKeyword + '/' + id).success(callback);
		},
		
		getKeywordType: function(code) {
			if (code == Enum.KeywordType.Verb) {
				return "Verb";
			}
			if (code == Enum.KeywordType.Noun) {
				return "Noun";
			}

			return "";
		}
	};
});