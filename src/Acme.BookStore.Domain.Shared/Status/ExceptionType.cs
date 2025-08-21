namespace Acme.BookStore.Status;

public enum ExceptionType
{
    UserFriendlyException,
    EntityNotFoundException,
    AbpAuthorizationException,
    AbpValidationException,
    NotImplementedException,
    BusinessException,
    Exception
}