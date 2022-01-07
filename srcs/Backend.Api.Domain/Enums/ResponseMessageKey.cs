namespace Backend.Domain.Enums
{
    public enum ResponseMessageKey
    {
        // Default responses errors
        DEFAULT_SUCCESS,
        DEFAULT_BAD_REQUEST,
        DEFAULT_NOT_FOUND,
        DEFAULT_INTERNAL_SERVER_ERROR,
        
        //Register
        REGISTER_INVALID_PASSWORD_CONFIRMATION,
        REGISTER_USERNAME_ALREADY_TAKEN,
        REGISTER_EMAIL_ALREADY_TAKEN,
        REGISTER_SUCCESS,
        REGISTER_ACCOUNT_SAVING_FAILED
    }
}