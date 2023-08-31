namespace VC.Data
{
    public static class SettingsStorage
    {
        //Route names
        public const string RouteNameForAccountController = "api/account";
        public const string RouteNameForUserController = "api/user";
        public const string RouteNameForCompanyController = "api/company";

        //Endpoint names
        public const string EndpointNameForSignIn = "signIn";
        public const string EndpointNameForConfirmEmail = "confirmEmail";

        //Collection names
        public const string CollectionNameForApplicationRole = "roles";
        public const string CollectionNameForApplicationUser = "users";
        public const string CollectionNameForCompany = "companies";
    }
}
