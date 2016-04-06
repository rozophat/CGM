angular.module("customTemplates", [
					"template/typeahead/typeahead-card-group-match.html"]);

angular.module("template/typeahead/typeahead-card-group-match.html", []).run(["$templateCache", function ($templateCache) {
    $templateCache.put("template/typeahead/typeahead-card-group-match.html",
		"<a tabindex=\"-1\">" +
			"<span bind-html-unsafe=\"match.label.Name | typeaheadHighlight:query\"></span>" +
		"</a>"
	);
}]);