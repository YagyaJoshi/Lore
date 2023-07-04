using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Core.Exceptions.BaseExceptions;

namespace Loregroup.Core.Exceptions
{
    public class UserDoesNotExistsException : BaseException
    {
    }
    public class CitiesDoesNotExistException : BaseException
    { }
    public class UserLockedFromAtteptsException : BaseException
    {
    }

    public class InvalidPasswordException : BaseException
    {
    }

    public class RoleDoesNotExistsException : BaseException
    {
    }

    public class RoleNotSavingException : BaseException
    {
    }

    public class PermissionDoesNotExistsException : BaseException
    {
    }

    public class PermissionNotSavingException : BaseException
    {
    }

    public class InvalidServerNameException : BaseException
    { }

    public class InvalidPortNumberException : BaseException
    { }

    public class ValidUserException : BaseException
    { }

    public class SymbolDoesNotExistsException : BaseException
    { }

    public class SymbolNotSavingException : BaseException
    { }

    public class QuestionDoesNotExistsException : BaseException
    { }

    public class QuestionNotSavingException : BaseException
    { }

    public class TranningVideoDoesNotExistsException : BaseException
    { }

    public class TranningVideoNotSavingException : BaseException
    { }

    public class TemplateDoesNotExistsException : BaseException
    { }

    public class TemplateNotSavingException : BaseException
    { }

    public class AppUsersettingsDoesNotExistsException : BaseException
    { }

    public class MessageSettingsNotSavingException : BaseException
    { }

    public class CompanyDoesNotExistsException : BaseException
    { }

    public class CompanyGeneralDetailNotSavingException : BaseException
    { }

    public class NotificationNotGettingException : BaseException
    { }
}
