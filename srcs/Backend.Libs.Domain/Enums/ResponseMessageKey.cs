﻿namespace Backend.Libs.Domain.Enums;

public enum ResponseMessageKey
{
    /**
     * Default
     */
    SUCCESS = 200001,
    BAD_REQUEST = 400001,
    UNAUTHORIZED = 401001,
    NOT_FOUND = 404001,
    INTERNAL_SERVER_ERROR = 500001,
    
    /**
     * Bad request
     */
    BAD_REQUEST_NULL_LOGIN = 400002,
    BAD_REQUEST_NULL_PASSWORD = 400003,
    BAD_REQUEST_INVALID_LOGIN = 400004,
    BAD_REQUEST_INVALID_PASSWORD = 400005,
    BAD_REQUEST_DIFFERENT_PASSWORD_CONFIRMATION = 400006,
    BAD_REQUEST_INVALID_EMAIL_FORMAT = 400007,
    BAD_REQUEST_UNAVAILABLE_USERNAME = 400008,
    BAD_REQUEST_UNAVAILABLE_EMAIL = 400009,
    
    /**
     * Internal server error
     */
        
    INTERNAL_SERVER_ERROR_ENTITY_SAVE_ERROR = 500002

}