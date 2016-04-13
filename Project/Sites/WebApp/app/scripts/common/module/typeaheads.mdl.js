angular.module("customTemplates", [
					"template/typeahead/typeahead-card-group-match.html",
					"template/typeahead/typeahead-asset-match.html",
                    "template/typeahead/typeahead-card-match.html",
                    "template/typeahead/typeahead-player-cardgroup-match.html"]);

angular.module("template/typeahead/typeahead-card-group-match.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/typeahead/typeahead-card-group-match.html",
		"<a tabindex=\"-1\">" +
			"<span bind-html-unsafe=\"match.label.Name | typeaheadHighlight:query\"></span>" +
		"</a>"
	);
}]);

angular.module("template/typeahead/typeahead-asset-match.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/typeahead/typeahead-asset-match.html",
		"<a tabindex=\"-1\">" +
			"<span bind-html-unsafe=\"match.label.Name | typeaheadHighlight:query\"></span>" +
		"</a>"
	);
}]);

angular.module("template/typeahead/typeahead-card-match.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/typeahead/typeahead-card-match.html",
		"<a tabindex=\"-1\">" +
			"<span ng-if=\"match.label.Question1\" bind-html-unsafe=\"match.label.Question1 | typeaheadHighlight:query\"></span><br>" +
			"<span ng-if=\"match.label.Question2\" bind-html-unsafe=\"match.label.Question2 | typeaheadHighlight:query\"></span><br>" +
			"<span ng-if=\"match.label.Question3\" bind-html-unsafe=\"match.label.Question3 | typeaheadHighlight:query\"></span><br>" +
		"</a>"
	);
}]);

angular.module("template/typeahead/typeahead-player-cardgroup-match.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/typeahead/typeahead-player-cardgroup-match.html",
		"<a tabindex=\"-1\">" +
			"<span bind-html-unsafe=\"match.label.PlayerFullName | typeaheadHighlight:query\"></span> - " +
			"<span bind-html-unsafe=\"match.label.GroupName | typeaheadHighlight:query\"></span><br>" +
		"</a>"
	);
}]);