angular.module('ui.bootstrap.intTooltip', ['ui.bootstrap.position', 'ui.bootstrap.bindHtml', "template/tooltip/intTooltip-html-unsafe-popup.html", "template/tooltip/intTooltip-popup.html"])

/**
 * The $tooltip service creates tooltip- and popover-like directives as well as
 * houses global options for them.
 */
.provider('$intTooltip', function () {
	// The default options tooltip and popover.
	var defaultOptions = {
		placement: 'top',
		animation: true,
		popupDelay: 0
	};

	// Default hide triggers for each show trigger
	var triggerMap = {
		'mouseenter': 'mouseleave',
		'click': 'click',
		'focus': 'blur'
	};

	// The options specified to the provider globally.
	var globalOptions = {};

	/**
	 * `options({})` allows global configuration of all tooltips in the
	 * application.
	 *
	 *   var app = angular.module( 'App', ['ui.bootstrap.tooltip'], function( $tooltipProvider ) {
	 *     // place tooltips left instead of top by default
	 *     $tooltipProvider.options( { placement: 'left' } );
	 *   });
	 */
	this.options = function (value) {
		angular.extend(globalOptions, value);
	};

	/**
	 * This allows you to extend the set of trigger mappings available. E.g.:
	 *
	 *   $tooltipProvider.setTriggers( 'openTrigger': 'closeTrigger' );
	 */
	this.setTriggers = function setTriggers(triggers) {
		angular.extend(triggerMap, triggers);
	};

	/**
	 * This is a helper function for translating camel-case to snake-case.
	 */
	function snake_case(name) {
		var regexp = /[A-Z]/g;
		var separator = '-';
		return name.replace(regexp, function (letter, pos) {
			return (pos ? separator : '') + letter.toLowerCase();
		});
	}

	/**
	 * Returns the actual instance of the $tooltip service.
	 * TODO support multiple triggers
	 */
	this.$get = ['$window', '$compile', '$timeout', '$parse', '$document', '$position', '$interpolate', function ($window, $compile, $timeout, $parse, $document, $position, $interpolate) {
		return function $intTooltip(type, prefix, defaultTriggerShow) {
			var options = angular.extend({}, defaultOptions, globalOptions);

			/**
			 * Returns an object of show and hide triggers.
			 *
			 * If a trigger is supplied,
			 * it is used to show the tooltip; otherwise, it will use the `trigger`
			 * option passed to the `$tooltipProvider.options` method; else it will
			 * default to the trigger supplied to this directive factory.
			 *
			 * The hide trigger is based on the show trigger. If the `trigger` option
			 * was passed to the `$tooltipProvider.options` method, it will use the
			 * mapped trigger from `triggerMap` or the passed trigger if the map is
			 * undefined; otherwise, it uses the `triggerMap` value of the show
			 * trigger; else it will just use the show trigger.
			 */
			function getTriggers(trigger) {
				var show = trigger || options.trigger || defaultTriggerShow;
				var hide = triggerMap[show] || show;
				return {
					show: show,
					hide: hide
				};
			}

			var directiveName = snake_case(type);

			var startSym = $interpolate.startSymbol();
			var endSym = $interpolate.endSymbol();
			var template =
			  '<div ' + directiveName + '-popup ' +
				 'title="' + startSym + 'itt_title' + endSym + '" ' +
				 'content="' + startSym + 'itt_content' + endSym + '" ' +
				 'placement="' + startSym + 'itt_placement' + endSym + '" ' +
				  //Phat Nguyen - 18/08/2014
				  //add infoType to change the color of the tooltip
				 'infotype="' + startSym + 'itt_infotype' + endSym + '" ' +
				 'animation="itt_animation" ' +
				 'is-open="itt_isOpen"' +
				 '>' +
			  '</div>';

			return {
				restrict: 'EA',
				scope: true,
				compile: function (tElem, tAttrs) {
					var tooltipLinker = $compile(template);

					return function link(scope, element, attrs) {
						var tooltip;
						var transitionTimeout;
						var popupTimeout;
						var appendToBody = angular.isDefined(options.appendToBody) ? options.appendToBody : false;
						var triggers = getTriggers(undefined);
						var hasEnableExp = angular.isDefined(attrs[prefix + 'Enable']);

						var positionTooltip = function () {

							var ttPosition = $position.positionElements(element, tooltip, scope.itt_placement, appendToBody);
							ttPosition.top += 'px';
							ttPosition.left += 'px';

							// Now set the calculated positioning.
							tooltip.css(ttPosition);
						};

						// By default, the tooltip is not open.
						// TODO add ability to start tooltip opened
						scope.itt_isOpen = false;

						function toggleTooltipBind() {
							if (!scope.itt_isOpen) {
								showTooltipBind();
							} else {
								hideTooltipBind();
							}
						}

						// Show the tooltip with delay if specified, otherwise show it immediately
						function showTooltipBind() {
							if (hasEnableExp && !scope.$eval(attrs[prefix + 'Enable'])) {
								return;
							}
							if (scope.itt_popupDelay) {
								// Do nothing if the tooltip was already scheduled to pop-up.
								// This happens if show is triggered multiple times before any hide is triggered.
								if (!popupTimeout) {
									popupTimeout = $timeout(show, scope.itt_popupDelay, false);
									popupTimeout.then(function (reposition) { reposition(); });
								}
							} else {
								//Phat Nguyen - 16/9/2014
								//because we use it with another tooltip, 
								//so we need it display after the other display for the conflict render reason
								$timeout(function() {
									show();
									$timeout(function () {
										hide();
									}, 3000);
								});
							}
						}

						function hideTooltipBind() {
							scope.$apply(function () {
								hide();
							});
						}

						// Show the tooltip popup element.
						function show() {

							popupTimeout = null;

							// If there is a pending remove transition, we must cancel it, lest the
							// tooltip be mysteriously removed.
							if (transitionTimeout) {
								$timeout.cancel(transitionTimeout);
								transitionTimeout = null;
							}

							// Don't show empty tooltips.
							if (!scope.itt_content) {
								return angular.noop;
							}

							createTooltip();

							// Set the initial positioning.
							tooltip.css({ top: 0, left: 0, display: 'block' });

							// Now we add it to the DOM because need some info about it. But it's not 
							// visible yet anyway.
							if (appendToBody) {
								$document.find('body').append(tooltip);
							} else {
								element.after(tooltip);
							}

							positionTooltip();

							// And show the tooltip.
							scope.itt_isOpen = true;
							scope.$digest(); // digest required as $apply is not called

							// Return positioning function as promise callback for correct
							// positioning after draw.
							return positionTooltip;
						}

						// Hide the tooltip popup element.
						function hide() {
							// First things first: we don't show it anymore.
							scope.itt_isOpen = false;

							//if tooltip is going to be shown after delay, we must cancel this
							$timeout.cancel(popupTimeout);
							popupTimeout = null;

							// And now we remove it from the DOM. However, if we have animation, we 
							// need to wait for it to expire beforehand.
							// FIXME: this is a placeholder for a port of the transitions library.
							if (scope.itt_animation) {
								if (!transitionTimeout) {
									transitionTimeout = $timeout(removeTooltip, 3000);
								}
							} else {
								removeTooltip();
							}
						}

						function createTooltip() {
							// There can only be one tooltip element per directive shown at once.
							if (tooltip) {
								removeTooltip();
							}
							tooltip = tooltipLinker(scope, function () { });

							// Get contents rendered into the tooltip
							scope.$digest();
						}

						function removeTooltip() {
							transitionTimeout = null;
							if (tooltip) {
								tooltip.remove();
								tooltip = null;
							}
						}

						/**
						 * Observe the relevant attributes.
						 */
						attrs.$observe(type, function (val) {
							scope.itt_content = val;

							if (!val && scope.itt_isOpen) {
								hide();
							}
						});

						attrs.$observe(prefix + 'Title', function (val) {
							scope.itt_title = val;
						});

						attrs.$observe(prefix + 'Placement', function (val) {
							scope.itt_placement = angular.isDefined(val) ? val : options.placement;
						});

						attrs.$observe(prefix + 'Infotype', function (val) {
							scope.itt_infotype = angular.isDefined(val) ? val : options.infotype;
						});

						attrs.$observe(prefix + 'PopupDelay', function (val) {
							var delay = parseInt(val, 10);
							scope.itt_popupDelay = !isNaN(delay) ? delay : options.popupDelay;
						});

						var unregisterTriggers = function () {
							element.unbind(triggers.show, showTooltipBind);
							element.unbind(triggers.hide, hideTooltipBind);
						};

						attrs.$observe(prefix + 'Trigger', function (val) {
							unregisterTriggers();

							triggers = getTriggers(val);

							if (triggers.show === triggers.hide) {
								element.bind(triggers.show, toggleTooltipBind);
							} else {
								element.bind(triggers.show, showTooltipBind);
								element.bind(triggers.hide, hideTooltipBind);
							}
						});

						var animation = scope.$eval(attrs[prefix + 'Animation']);
						scope.itt_animation = angular.isDefined(animation) ? !!animation : options.animation;

						attrs.$observe(prefix + 'AppendToBody', function (val) {
							appendToBody = angular.isDefined(val) ? $parse(val)(scope) : appendToBody;
						});

						// if a tooltip is attached to <body> we need to remove it on
						// location change as its parent scope will probably not be destroyed
						// by the change.
						if (appendToBody) {
							scope.$on('$locationChangeSuccess', function closeTooltipOnLocationChangeSuccess() {
								if (scope.itt_isOpen) {
									hide();
								}
							});
						}

						// Make sure tooltip is destroyed and removed.
						scope.$on('$destroy', function onDestroyTooltip() {
							$timeout.cancel(transitionTimeout);
							$timeout.cancel(popupTimeout);
							unregisterTriggers();
							removeTooltip();
						});
					};
				}
			};
		};
	}];
})

.directive('intTooltipPopup', function () {
	return {
		restrict: 'EA',
		replace: true,
		scope: { content: '@', placement: '@', animation: '&', isOpen: '&' },
		templateUrl: 'template/tooltip/intTooltip-popup.html'
	};
})

.directive('intTooltip', ['$intTooltip', function ($intTooltip) {
	return $intTooltip('intTooltip', 'intTooltip', 'mouseenter');
}])

.directive('intTooltipHtmlUnsafePopup', function () {
	return {
		restrict: 'EA',
		replace: true,
		scope: { content: '@', placement: '@', animation: '&', isOpen: '&', infotype: '@' },
		templateUrl: 'template/tooltip/intTooltip-html-unsafe-popup.html'
	};
})

.directive('intTooltipHtmlUnsafe', ['$intTooltip', function ($intTooltip) {
	return $intTooltip('intTooltipHtmlUnsafe', 'intTooltip', 'mouseenter');
}]);

angular.module("template/tooltip/intTooltip-html-unsafe-popup.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/tooltip/intTooltip-html-unsafe-popup.html",
    "<div class=\"tooltip {{placement}} {{infotype}}\" ng-class=\"{ in: isOpen(), fade: animation() }\">\n" +
    "  <div class=\"tooltip-arrow\"></div>\n" +
    "  <div class=\"tooltip-inner\" bind-html-unsafe=\"content\"></div>\n" +
    "</div>\n" +
    "");
}]);

angular.module("template/tooltip/intTooltip-popup.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/tooltip/intTooltip-popup.html",
    "<div class=\"tooltip {{placement}}\" ng-class=\"{ in: isOpen(), fade: animation() }\">\n" +
    "  <div class=\"tooltip-arrow\"></div>\n" +
    "  <div class=\"tooltip-inner\" ng-bind=\"content\"></div>\n" +
    "</div>\n" +
    "");
}]);