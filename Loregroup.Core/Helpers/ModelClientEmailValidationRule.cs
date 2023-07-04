using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Loregroup.Core.Validation
{
    public class ModelClientEmailValidationRule : ModelClientValidationRule
    {
        public ModelClientEmailValidationRule(String errorMessage, String regEx)
        {
            ErrorMessage = errorMessage;
            ValidationType = "emailvalidation";
            ValidationParameters.Add("regex", regEx);
        }
    }
}
