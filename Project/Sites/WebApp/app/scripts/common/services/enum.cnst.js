app.constant('Enum', {
	FormStatus: {
		"Add": 1,
		"Edit": 2,
		"Delete": 3,
		"Reset": 4
	},
	
	Status: {
		"Deactive": "0",
		"Active": "1",
	},
	
	Authorised: {
		"Authorised": 0,
		"LoginRequired": 1,
		"NotAuthorised": 2
	},
	PermissionCheckType: {
		"AtLeastOne": "0",
		"CombinationRequired": "1",
		"NotRequired": "2"
	},
	
	CategoryType: {
		"Business": 1,
		"Event": 2,
		"Public Places": 3,
		"Public Services": 4
	},
	
	KeywordType: {
		"Verb": 1,
		"Noun": 2
	}
});