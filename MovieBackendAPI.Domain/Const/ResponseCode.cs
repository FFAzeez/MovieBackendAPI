namespace MovieBackendAPI.Domain.Const
{
    public class ResponseCode
    {
        public const string SUCCESSFUL = "00";
        public const string REQUIRE_2FA = "05";
        public const string NEEDS_REMOVE_DEVICES = "20";
        public const string BCDC_TOKEN_REQUIRED = "21";
        public const string BCDC_PASSWORD_REQUIRED = "22";
        public const string BCDC_VERIFICATION_FAILED = "25";
        public const string USER_DOES_NOT_HAVE_SECURITY_QUESTIONS = "40";
        public const string ACCEPT_TnC = "01";
        public const string VERIFY_PHONENUMBER = "02";
        public const string SET_PASSWORD = "03";
        public const string EXISTING_USER = "05";
        public const string USER_DOESNT_EXIST = "95";
        public const string FAILED = "99";
        public const string ACCOUNT_LINKED = "94";
        public const string TEMP_PASSWORD = "45";
        public const string TEMP_PASSWORD_EXPIRED = "50";
        public const string NOT_REQUIRED = "30";
        public const string USER_SIM_BLOCKED = "60";
        public const string USER_BLOCKED = "65";
        public const string SIMILAR_PIN = "80";
        public const string PIN_CONTAINS_DIGITS = "81";
        public const string SEQUENTIAL_PIN = "82";
        public const string REPETITIVE_PIN = "83";
        public const string OLD_PIN = "84";
        public const string RESET_SECURITY_QUESTIONS = "41";
        public const string IMSI_CHECK_SUCCESSFUL = "70";
        public const string IMSI_CHECK_FAILED = "71";
        public const string PROFILE_UPDATE_FAILED = "72";
        public const string SIM_SWAPPED = "73";
        public const string COOL_OFF = "74";
        public static string DuplicateRequest = "86";
        public static string BadRequest = "88";
        public static string Error = "99";
        public static string NOTFOUND = "404";
        public const string GENERIC_EXCEPTION = "99";
    }


}
