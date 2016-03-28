var baseUrl = "/dist/";
var config = {
    //for dev
    "apiUrl": "http://cgmapi.local/api",
    "apiServiceBaseUri": "http://cgmapi.local/",
};

//authentication
config.clientId = 'ngAuthApp';
config.buildUrl = function(resourceUrl) {
	return config.apiUrl + "/" + resourceUrl;
};
