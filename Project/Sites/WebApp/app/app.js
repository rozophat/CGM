var app = angular.module('app',
    ['ngAnimate', 'ngSanitize', 'ui.router', 'ui.router.stateHelper', 'LocalStorageModule', 'angular-loading-bar', 'toaster', 'ui.bootstrap',
        'ngPatternRestrict', 'ngTable', 'pascalprecht.translate', 'mgcrea.ngStrap.datepicker', 'mgcrea.ngStrap.popover', 'angularFileUpload', 'customTemplates'
    ]);

var urlApiCardGroup = config.buildUrl("CardGroup");
var urlApiCard = config.buildUrl("Card");
var urlApiUser = config.buildUrl("User");

var verNo = '?v=0.0.1';

app.config(['$httpProvider', '$stateProvider', '$locationProvider', '$urlRouterProvider', 'stateHelperProvider', 'cfpLoadingBarProvider', '$datepickerProvider', '$translateProvider', '$translatePartialLoaderProvider',
   function ($httpProvider, $stateProvider, $locationProvider, $urlRouterProvider, stateHelperProvider, cfpLoadingBarProvider, $datepickerProvider, $translateProvider, $translatePartialLoaderProvider) {

   	//$locationProvider.html5Mode(true);

   	var viewUrl = baseUrl + "views/";

   	$httpProvider.interceptors.push('authInterceptorService');

   	cfpLoadingBarProvider.includeSpinner = false;

   	$urlRouterProvider.otherwise('/login');

   	$stateProvider
   		.state('card-groups', {
   		    url: "/card-groups/:page/:searchValue",
   			templateUrl: viewUrl + 'card-group/card-groups.tpl.html' + verNo,
   			controller: 'CardGroupsController',
   			access: {
   				loginRequired: true
   			}
   		})

		.state('card-group-view', {
		    url: "/card-group/view/:id/:tab/:page",
		    templateUrl: viewUrl + 'card-group/card-group-view.tpl.html' + verNo,
		    controller: 'CardGroupViewController',
			access: {
				loginRequired: true
			}
		})

		.state('card-group-add', {
		    url: "/card-group/add",
		    templateUrl: viewUrl + 'card-group/card-group-add.tpl.html' + verNo,
		    controller: 'CardGroupAddController',
			access: {
				loginRequired: true
			}
		})

        .state('cards', {
            url: "/cards/:page/:searchValue",
            templateUrl: viewUrl + 'card/cards.tpl.html' + verNo,
            controller: 'CardsController',
            access: {
                loginRequired: true
            }
        })

         .state('card-add', {
             url: "/card/add",
             templateUrl: viewUrl + 'card/card-add.tpl.html' + verNo,
             controller: 'CardAddController',
             access: {
                 loginRequired: true
             }
         })

        .state('card-view', {
            url: "/card/view/:id",
            templateUrl: viewUrl + 'card/card-view.tpl.html' + verNo,
            controller: 'CardViewController',
            access: {
                loginRequired: true
            }
        })
		//.state('listings', {
		//	url: "/listings/:page/:searchValue",
		//	templateUrl: viewUrl + 'listing/listings.tpl.html' + verNo,
		//	controller: 'ListingsController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('listing-view', {
		//	url: "/listing/view/:id/:tab/:page",
		//	templateUrl: viewUrl + 'listing/listing-view.tpl.html' + verNo,
		//	controller: 'ListingViewController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})


		//.state('banners', {
		//	url: "/banners/:page/:searchValue",
		//	templateUrl: viewUrl + 'banner/banners.tpl.html' + verNo,
		//	controller: 'BannersController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('banner-add', {
		//	url: "/banner/add/:accountId",
		//	templateUrl: viewUrl + 'banner/banner-add.tpl.html' + verNo,
		//	controller: 'BannerAddController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('banner-view', {
		//	url: "/banner/view/:id/:tab/:page",
		//	templateUrl: viewUrl + 'banner/banner-view.tpl.html' + verNo,
		//	controller: 'BannerViewController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('address-add', {
		//	url: "/listing/address/add/:listingId",
		//	templateUrl: viewUrl + 'listing/address/listing-address-add.tpl.html' + verNo,
		//	controller: 'ListingAddressAddController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('address-view', {
		//	url: "/listing/address/view/:id",
		//	templateUrl: viewUrl + 'listing/address/listing-address-view.tpl.html' + verNo,
		//	controller: 'ListingAddressViewController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('keywords', {
		//	url: "/keywords",
		//	templateUrl: viewUrl + 'keyword/keywords.tpl.html' + verNo,
		//	controller: 'KeywordsController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})
	   
		//.state('categories', {
		//	url: "/categories/:page/:searchValue",
		//	templateUrl: viewUrl + 'category/categories.tpl.html' + verNo,
		//	controller: 'CategoriesController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('category-view', {
		//	url: "/category/view/:id",
		//	templateUrl: viewUrl + 'category/category-view.tpl.html' + verNo,
		//	controller: 'CategoryViewController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		//.state('category-add', {
		//	url: "/category/add",
		//	templateUrl: viewUrl + 'category/category-add.tpl.html' + verNo,
		//	controller: 'CategoryAddController',
		//	access: {
		//		loginRequired: true,
		//	}
		//})

		.state('users', {
			url: "/users",
			templateUrl: viewUrl + 'user/users.tpl.html' + verNo,
			controller: 'UsersController',
			access: {
				loginRequired: true,
			}
		})

		.state('login', {
			url: "/login",
			templateUrl: viewUrl + 'authentication/login.tpl.html' + verNo,
			controller: 'LoginController'
		})
	   
		.state('resetpassword', {
			url: "/resetpassword?userid&code",
			templateUrl: viewUrl + 'authentication/reset-password.tpl.html' + verNo,
			controller: 'ResetPasswordController'
		})
	   
		.state('success-resetpassword', {
			url: "/resetpassword/success",
			templateUrl: viewUrl + 'authentication/reset-password-confirmation.tpl.html' + verNo,
			controller: 'ResetPasswordConfirmController'
		})
   	;

   	//datetime
   	angular.extend($datepickerProvider.defaults, {
   		dateFormat: 'dd/MM/yyyy',
   		startWeek: 1,
   		trigger: 'manual',
   		placement: 'bottom-right',
   	});

   	//language loader
   	$translatePartialLoaderProvider.addPart('common');

   	$translateProvider.useLoader('$translatePartialLoader', {
   		urlTemplate: '/dist/assets/translations/{lang}/{part}.json'
   	});

   	$translateProvider.preferredLanguage('en-AU');

}]);

app.run(function ($rootScope, $translate, authService, $location, $state) {
	authService.fillAuthData();

	$rootScope.$on('$translatePartialLoaderStructureChanged', function () {
		$translate.refresh();
	});

	$rootScope.previousState = "";
	$rootScope.currentState = "";
	$rootScope.$on('$stateChangeSuccess', function (ev, to, toParams, from) {
		$rootScope.previousState = from.name;
		$rootScope.currentState = to.name;
	});

	$rootScope.$state = $state;

	$rootScope.$on('$stateChangeStart', function (event, to, toParams, from, fromParams) {
		if (!authService.authentication.isAuth && to.name != "resetpassword") {
			$location.path("/login");
			return;
		}
	});

	//if (!authService.authentication.isAuth) {
	//	$location.path("/login");
	//}
});