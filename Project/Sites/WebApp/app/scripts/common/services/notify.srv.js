app.service("notifyService", function ($http, toaster, $filter) {
	this.popCreateSuccessful = function () {
		toaster.pop('success', "Create Successfully", "");
	};
	
	this.popExistKeyword = function () {
		toaster.pop('error', "The keyword has already been created", "");
	};
	
	this.popExistUser = function () {
		toaster.pop('error', "The user name has already been taken", "");
	};
	
	this.popExistEmail = function () {
		toaster.pop('error', "The email has already been taken", "");
	};
	
	this.popUpdateSuccessful = function () {
		toaster.pop('success', "Update Successfully", "");
	};

	this.popDeleteSuccessful = function () {
		toaster.pop('success', "Delete Successfully", "");
	};
	
	this.popAssignKeywordSuccessful = function () {
		toaster.pop('success', "Assign Keyword Successfully", "");
	};
	
	this.popAssignKeywordUnsuccessful = function () {
		toaster.pop('error', "Assign Keyword Unsuccessfully", "");
	};
	
	this.popAddContactSuccessful = function () {
		toaster.pop('success', "Add Contact Successfully", "");
	};

	this.popAddContactUnsuccessful = function () {
		toaster.pop('error', "Add Contact Unsuccessfully", "");
	};
	
	this.popConfirmEmailSent = function () {
		toaster.pop('success', "Reset Password Email Sent", "Please check your email and reset your password.");
	};
	
	this.popResetPasswordSuccessful = function () {
		toaster.pop('success', "Reset Password Email Sent", "Please check your email and reset your password.");
	};
});