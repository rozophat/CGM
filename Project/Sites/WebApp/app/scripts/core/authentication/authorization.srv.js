app.factory('authorizedService', ['authService', 'Enum', function (authService, Enum) {

	var authorize = function (loginRequired, requiredPermissions, permissionCheckType) {
		var result = Enum.Authorised.Authorised,
			user = authService.authentication,
			loweredPermissions = [],
			hasPermission = true,
			permission, i;

		permissionCheckType = permissionCheckType || Enum.PermissionCheckType.AtLeastOne;
		if (loginRequired === true && user === undefined) {
			result = Enum.Authorised.LoginRequired;
		} else if ((loginRequired === true && user !== undefined) &&
			(requiredPermissions === undefined || requiredPermissions === null || requiredPermissions.length === 0)) {
			// Login is required but no specific permissions are specified.
			result = Enum.Authorised.Authorised;
		} else if (requiredPermissions) {
			loweredPermissions = [];
			if (user !== null && user !== undefined && user.permissions.length > 0) {
				var permissions = JSON.parse(user.permissions.replace(", ]", "]"));
				angular.forEach(permissions, function (per) {
					loweredPermissions.push(per.toLowerCase());
				});
			}

			for (i = 0; i < requiredPermissions.length; i += 1) {
				permission = requiredPermissions[i].toLowerCase();

				if (permissionCheckType === Enum.PermissionCheckType.CombinationRequired) {
					hasPermission = hasPermission && loweredPermissions.indexOf(permission) > -1;
					// if all the permissions are required and hasPermission is false there is no point carrying on
					if (hasPermission === false) {
						break;
					}
				} else if (permissionCheckType === Enum.PermissionCheckType.AtLeastOne) {
					hasPermission = loweredPermissions.indexOf(permission) > -1;
					// if we only need one of the permissions and we have it there is no point carrying on
					if (hasPermission) {
						break;
					}
				} else if (permissionCheckType === Enum.PermissionCheckType.NotRequired) {
					hasPermission = loweredPermissions.indexOf(permission) <= -1;
					// if we only need one of the permissions and we have it there is no point carrying on
					if (hasPermission) {
						break;
					}
				}
			}

			result = hasPermission ? Enum.Authorised.Authorised : Enum.Authorised.NotAuthorised;
		}

		return result;
	};

	return {
		authorize: authorize
	};
}]);