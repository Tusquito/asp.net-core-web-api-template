using Backend.Libs.Domain.Attributes;

namespace Backend.Libs.Domain.Enums;

public enum ResultMessageKey
{
    /**
     * Default
     */
    [ResultMessageType(ResultType.Ok)]
    Ok = 200001,
    [ResultMessageType(ResultType.Created)]
    Created = 201001,
    [ResultMessageType(ResultType.NoContent)]
    NoContent = 204001,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequest = 400001,
    [ResultMessageType(ResultType.UnknownError)]
    Unauthorized = 401001,
    [ResultMessageType(ResultType.NotFound)]
    NotFound = 404001,
    [ResultMessageType(ResultType.UnknownError)]
    InternalServerError = 500001,
    [ResultMessageType(ResultType.Maintenance)]
    ServiceUnavailable = 503001,
    
    /**
     * Bad request
     */
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestLoginInputMissingOnLogin = 400002,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestPasswordInputMissingOnLogin = 400003,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestInvalidUsernameFormat = 400004,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestInvalidPasswordFormat = 400005,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestDifferentPasswordConfirmation = 400006,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestInvalidEmailFormat = 400007,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestUnavailableUsername = 400008,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestUnavailableEmail = 400009,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestWrongPassword = 400010,
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestInvalidPasswordLength = 400011, // Min,Max
    [ResultMessageType(ResultType.BadRequest)]
    BadRequestInvalidUsernameLength= 400012, // Min,Max

    /**
     * Not Found
     */
    [ResultMessageType(ResultType.NotFound)]
    NotFoundAccountById = 404002,
    [ResultMessageType(ResultType.NotFound)]
    NotFoundAccountByLogin = 404003,
    
    /**
     * Internal server error
     */
    [ResultMessageType(ResultType.UnknownError)]
    InternalServerErrorEntitySaveError = 500002,
}