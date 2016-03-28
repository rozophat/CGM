app.factory('categoryService', function ($http, Enum) {
	return {
		createCategory: function (callback, data) {
			$http.post(urlApiCategory, data).success(callback);
		},

		updateCategory: function (callback, data) {
			$http.put(urlApiCategory, data).success(callback);
		},

		deleteCategory: function (callback, id) {
			$http.delete(urlApiCategory + '/' + id).success(callback);
		},

		getCategoryType: function (code) {
			if (code == Enum.CategoryType.Business) {
				return "Business";
			}
			if (code == Enum.CategoryType.Event) {
				return "Event";
			}
			if (code == Enum.CategoryType["Public Places"]) {
				return "Public Places";
			}
			if (code == Enum.CategoryType["Public Services"]) {
				return "Public Services";
			}
			
			return "";
		},
		
	};
});