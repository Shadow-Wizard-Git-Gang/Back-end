namespace VC.Data
{
    public static class SettingsStorage
    {
        //Route names
        public const string RouteNameForAccountController = "api/account";
        public const string RouteNameForUserController = "api/user";

        //Endpoint names
        public const string EndpointNameForSignIn = "signIn";
        public const string EndpointNameForConfirmEmail = "confirmEmail";
        public const string EndpointNameForResettingPassword = "resetPassword";
        public const string EndpointNameForSettingNewPassword = "setNewPassword";

        //Collection names
        public const string CollectionNameForApplicationRole = "roles";
        public const string CollectionNameForApplicationUser = "users";
    }
}
