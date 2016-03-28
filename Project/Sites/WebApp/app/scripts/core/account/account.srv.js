app.factory('accountService', function ($http) {
	return {
		createAccount: function (callback, account) {
			$http.post(urlApiAccount, account).success(callback);
		},
		
		createListing: function (callback, listing) {
			$http.post(urlApiAccount + '/CreateListing', listing).success(callback);
		},
		
		createListingAddress: function (callback, listing) {
			$http.post(urlApiAccount + '/CreateListingAddress', listing).success(callback);
		},
		
		createBanner: function (callback, banner) {
			$http.post(urlApiAccount + '/CreateBanner', banner).success(callback);
		},
		
		updateAccount: function (callback, account) {
			$http.put(urlApiAccount, account).success(callback);
		},
		
		updateListing: function (callback, listing) {
			$http.put(urlApiAccount + "/UpdateListing", listing).success(callback);
		},
		
		updateListingAddress: function (callback, address) {
			$http.put(urlApiAccount + "/UpdateListingAddress", address).success(callback);
		},

		updateBanner: function (callback, banner) {
			$http.put(urlApiAccount + "/UpdateBanner", banner).success(callback);
		},
		
		updateListingImage: function (callback, image) {
			$http.put(urlApiAccount + "/UpdateListingImage", image).success(callback);
		},
		
		updateBannerImage: function (callback, image) {
			$http.put(urlApiAccount + "/UpdateBannerImage", image).success(callback);
		},

		deleteAccount: function (callback, id) {
			$http.delete(urlApiAccount + '/' + id).success(callback);
		},
		
		deleteAccountKeyword: function (callback, id) {
			$http.delete(urlApiAccount + '/DeleteAccountKeyword/' + id).success(callback);
		},
		
		deleteListingKeyword: function (callback, id) {
			$http.delete(urlApiAccount + '/DeleteListingKeyword/' + id).success(callback);
		},

		deleteAccountContact: function (callback, id) {
			$http.delete(urlApiAccount + '/DeleteAccountContact/' + id).success(callback);
		},

		deleteAccountComment: function (callback, id) {
			$http.delete(urlApiAccount + '/DeleteAccountComment/' + id).success(callback);
		},
		
		deleteAccountListing: function (callback, id) {
			$http.delete(urlApiAccount + '/DeleteAccountListing/' + id).success(callback);
		},
		
		deleteAccountBanner: function (callback, id) {
			$http.delete(urlApiAccount + '/DeleteAccountBanner/' + id).success(callback);
		},
		
		deleteListingAddress: function (callback, id) {
			$http.delete(urlApiAccount + '/DeleteListingAddress/' + id).success(callback);
		},
		
		deleteListingImage: function (callback, listingId, id) {
			$http.delete(urlApiAccount + '/DeleteListingImage/' + listingId + "/" + id).success(callback);
		},
		
		deleteBannerImage: function (callback, bannerId, id) {
			$http.delete(urlApiAccount + '/DeleteBannerImage/' + bannerId + "/" + id).success(callback);
		},
	};
});