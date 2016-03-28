app.service("commonService", function ($rootScope, $http, $q, $filter, Enum, $timeout, $location,
							$anchorScroll) {
	this.request = function (url, data) {
		var deferred = $q.defer();
		var promise = deferred.promise;

		$http.get(url, data)
			.success(function (returnedData) {
				deferred.resolve(returnedData);
			})
			.error(function (returnedData) {
				deferred.reject(returnedData);
			});
		return promise;
	};
	
	this.postRequest = function (url, data) {
		var deferred = $q.defer();
		var promise = deferred.promise;

		$http.post(url, data)
			.success(function (returnedData) {
				deferred.resolve(returnedData);
			})
			.error(function (returnedData) {
				deferred.reject(returnedData);
			});
		return promise;
	};

	this.logResponseError = function (message, url, sentData, reveicedData) {
		console.error(message);
		console.error("URL accessed: ", url);
		console.error("Transmitted data: ", sentData);
		console.error("Response Data: ", reveicedData);
	};

	this.getAutoSuggestion = function(url, data) {
		return this.request(url, data);
	};

	//using when pass param from client -> api
	this.convertDatetimeToString = function (date, time) {
		if (this.isNullOrUndefined(date)) {
			return "";
		}
		var formatDate = $filter('date')(date, 'MM-dd-yyyy');
		if (this.isNullOrUndefined(time)) {
			time = "00:00";
		}
		return (formatDate + " " + time);
	};
	
	//using when pass param from client -> api
	this.convertDateToString = function (date) {
		if (date !== null) {
			var formatDate = $filter('date')(date, 'MM-dd-yyyy');
			return formatDate;
		}
		return "";
	};
	
	this.getDateFromDateTimeDDMMYYYY = function (date) {
		if (date !== null) {
			var formatDate = $filter('date')(date, 'dd/MM/yyyy');
			return formatDate;
		}
		return null;
	};
	
	this.getDateFromDateTimeYYYYMMDD = function (date) {
		if (date !== null) {
			var formatDate = $filter('date')(date, 'yyyy-MM-dd');
			return formatDate;
		}
		return null;
	};

	this.getDateFromDateTimeDDMM = function (date) {
		if (date !== null) {
			var formatDate = $filter('date')(date, 'dd/MM');
			return formatDate;
		}
		return null;
	};
	this.getDateHourFromDateTime = function (date) {
		if (date !== null) {
			var formatDate = $filter('date')(date, 'dd/MM HH:mm');
			return formatDate;
		}
		return null;
	};
	
	this.getHHSS = function (date) {
		if (date !== null) {
			var formatDate = $filter('date')(date, 'HH:ss');
			return formatDate;
		}
		return null;
	};
	
	this.convertDateTimeToString = function (date) {
		if (date !== null) {
			var formatDate = $filter('date')(date, 'MM-dd-yyyy HH:ss');
			return formatDate;
		}
		return null;
	};

	this.getEnumName = function (key) {
		return Enum.SomeEnums.filter(function(en) {
			return en.id === key;
		})[0].name;
	};

	//using when get time from api -> client
	this.convertToFormatTime = function(date) {
		if (date !== null) {
			var time = date.split('T')[1];
			var orTime = time.split(':');
			var fmTime = orTime[0] + ":" + orTime[1];
			return fmTime;
		}
		return "";
	};

	//using when get date from api -> client
	this.convertToFormatDate = function(date) {
		if (date !== null) {
			var orDate = new Date(date);
			//using when type is DateTime
			//var utcMilliseconds = orDate.getTime() + orDate.getTimezoneOffset() * 60000;
			
			//using when type is Date
			var utcMilliseconds = orDate.getTime();
			var fmDate = $filter('date')(utcMilliseconds, 'MM-dd-yyyy');
			return fmDate;
		}
		return null;
	};

	this.substractTwoDate = function (date1, date2) {
		try {
			date1 = new Date(date1.getFullYear() + "/" + (date1.getMonth() + 1) + "/" + date1.getDate());
			date2 = new Date(date2.getFullYear() + "/" + (date2.getMonth() + 1) + "/" + date2.getDate());
			return parseInt(Math.round(date1 - date2) / (1000 * 60 * 60 * 24));
		}
		catch (err) {
			return 100;
		}
	};

	this.setScrollGoTo = function (location) {
		$timeout(function () {
			$location.hash(location);
			$anchorScroll();
		});
	};
	
	this.GoTop = function () {
		$timeout(function () {
			$(".go-top").click();
		});
	};

	this.setElementFocus = function (elmName) {
		$timeout(function () {
			if (elmName === null) {
				setPrevFocus();
			} else {
				angular.element('#' + elmName).trigger('focus');
			}
		}, 300);
	};

	function setPrevFocus() {
		var obj = $rootScope.prevFocus;
		if (obj !== "") {
			if (obj[0] !== document.activeElement && document.activeElement.attributes.class.value.indexOf("btn") <= -1) {
				$timeout(function() {
					obj.trigger('focus');
				});
			}
		}
	}

	this.bodyFormClick = function () {
		setPrevFocus();
	};
	
	this.hideTooltip = function (elmId) {
		angular.element('#' + elmId).scope().tt_isOpen = false;
	};

	this.isNullOrUndefined = function(val) {
		if (val === null || typeof val === "undefined" || val === ''|| val.toString() == "null") {
			return true;
		}
		return false;
	};

	this.isDataChangeDate = function (newValue, oldValue){
		if ((newValue === null && oldValue === undefined) ||
			(newValue === undefined && oldValue === null)||
			(newValue === "" && oldValue === undefined)) {
			return true;
		}
		return false;
	};

	this.convertToNumber = function(val) {
		var value = parseFloat(val);
		if (isNaN(value)) {
			return 0;
		}
		return value;
	};
	
	this.getDateErrorMessage = function (form, isInputInValid, isInputDirty, isFormatError, isLimitError, isGreaterError, isRequiredError, errorMessages) {
		if (form !== undefined && isInputInValid) {
			if (isInputDirty) {
				if (isFormatError) {
					return errorMessages.FormatDate;
				}
				else if (isRequiredError) {
					return errorMessages.RequiredDate;
				}
			}
			if (isLimitError) {
				return errorMessages.LimitDate;
			}
			if (isGreaterError) {
				return errorMessages.GreaterDate;
			}
		}
		return "";
	};

	this.getDateErrorTrigger = function (form, isInputDirty, isFormatError, isLimitError, isGreaterError, isRequiredError) {
		if (form !== undefined) {
			if (isFormatError || isLimitError || isGreaterError || (isRequiredError && isInputDirty)) {
				return "focus";
			}
		}
		return "never";
	};

	this.getDateForMonthPicker = function() {
		var currDate = new Date();
		var currMonth = ("0" + (currDate.getMonth() + 1)).slice(-2);
		var currYear = currDate.getFullYear();
		return "01/" + currMonth + "/" + currYear;
	};

	this.getDateForMonthPickerForSearch = function () {
		var currDate = new Date();
		var currMonth = ("0" + (currDate.getMonth() + 1)).slice(-2);
		var currYear = currDate.getFullYear();
		return currMonth + "/" + currYear;
	};

	this.convertMonthToString = function(month) {
		if (typeof month == "object") {
			var currDate = new Date(month);
			var currMonth = ("0" + (currDate.getMonth() + 1)).slice(-2);
			var currYear = currDate.getFullYear();
			return currMonth + "/" + currYear;
		}
		if (angular.isString(month)) {
			return month;
		}
		return "";
	};

	this.roundingTax = function(number, roundingTaxI) {
		if (roundingTaxI == Enum.RoundingMethod.Up) {
			return Math.ceil(number);
		}
		else if (roundingTaxI == Enum.RoundingMethod.Down) {
			return Math.floor(number);
		}
		return Math.round(number);
	};

	this.roundingRevenue = function (number, roundingRevenueI) {
		if (roundingRevenueI == Enum.RoundingMethod.Up) {
			return Math.ceil(number);
		}
		else if (roundingRevenueI == Enum.RoundingMethod.Down) {
			return Math.floor(number);
		}
		return Math.round(number);
	};

	this.exportPDF = function(promise) {
		promise.then(function (response) {
			var file = new Blob([response], { type: 'application/pdf' });
			var fileUrl = URL.createObjectURL(file);
			window.open(fileUrl);
		});
	};
	
	this.exportExcel = function (promise, fileName) {
		promise.then(function (response) {
			var file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
			saveAs(file, fileName + ".xlsx");
		});
	};
	
	this.setupBackupNotification = function () {
		if ($rootScope.BasicInfo !== null && $rootScope.BasicInfo !== undefined) {
			if ($rootScope.BasicInfo.LatestBackupDate !== null &
				$rootScope.BasicInfo.BackupNotificationDay !== null &
				$rootScope.BasicInfo.BackupPlanDay !== null) {
				var bkpNotificationDay = parseInt($rootScope.BasicInfo.BackupNotificationDay);
				var bkpPlanDay = parseInt($rootScope.BasicInfo.BackupPlanDay);
				var lastestBkpDateTime = new Date($rootScope.BasicInfo.LatestBackupDate);
				var offsetLastestBkpDate = lastestBkpDateTime.getTime() + lastestBkpDateTime.getTimezoneOffset() * 60000;
				var lastestBkpDate = getDateFromDateTime(new Date(offsetLastestBkpDate));

				var planBkpDate = lastestBkpDate.setDate(lastestBkpDate.getDate() + bkpPlanDay);

				var currDate = getDateFromDateTime(new Date());
				var remainDay = (new Date(planBkpDate) - currDate) / 86400000;

				if (remainDay === 0)
				{
					$rootScope.BackupDbStatus = 0; //Backup today
				}
				else if (remainDay < 0)
				{
					$rootScope.BackupDbStatus = 1; //Backup late
				}
				else
				{
					if (remainDay > bkpNotificationDay)
					{
						$rootScope.BackupDbStatus = -1; //Don't display notification
					}
					else if (remainDay <= bkpNotificationDay)
					{
						$rootScope.BackupDbStatus = 2; //Backup notification
						$rootScope.BackupRemainDay = remainDay;
					}
				}
			}
		}
	};

	this.setupBackupNotificationAfterBackup = function () {
		if ($rootScope.BasicSetting !== null) {
			var bkpNotificationDay = parseInt($rootScope.BasicInfo.BackupNotificationDay);
			var bkpPlanDay = parseInt($rootScope.BasicInfo.BackupPlanDay);
			if (bkpPlanDay === 0) {
				$rootScope.BackupDbStatus = 0; //Backup today
			}
			else {
				if (bkpPlanDay > bkpNotificationDay) {
					$rootScope.BackupDbStatus = -1; //Don't display notification
				}
				else if (bkpPlanDay <= bkpNotificationDay) {
					$rootScope.BackupDbStatus = 2; //Backup notification
					$rootScope.BackupRemainDay = bkpPlanDay;
				}
			}
		}
	};

	var getDateFromDateTime = function(datetime) {
		var year = datetime.getFullYear();
		var month = datetime.getMonth();
		var date = datetime.getDate();
		return new Date(year, month, date);
	};
});