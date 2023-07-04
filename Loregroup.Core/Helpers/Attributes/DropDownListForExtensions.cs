using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using Loregroup.Core.Helpers;

public static class DropDownListForExtensions {
    public static MvcHtmlString BootstrapDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.BootstrapDropDownListFor(expression, selectList, htmlAttributes);
    }

    public static MvcHtmlString BootstrapDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.BootstrapDropDownListFor(expression, selectList, htmlAttributes);
    }

    public static MvcHtmlString BootstrapDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        htmlAttributes.Add("class", "form-control");
        return html.DropDownListFor(expression, selectList, null /* optionLabel */, htmlAttributes);
    }

    public static IHtmlString EnBsDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html,
        Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
    {
        var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        var enumType = Nullable.GetUnderlyingType(metadata.ModelType) ?? metadata.ModelType;
        var enumValues = Enum.GetValues(enumType).Cast<object>();

        htmlAttributes.Add("class", "form-control");

        var items = from enumValue in enumValues
            select new SelectListItem
            {
                Text = GetDisplayAttributeFrom((Enum) enumValue, enumType),
                Value = ((int) enumValue).ToString(),
                Selected = enumValue.Equals(metadata.Model)
            };

        return html.DropDownListFor(expression, items, string.Empty, htmlAttributes);
    }

    private static string GetDisplayAttributeFrom(Enum enumValue, Type enumType)
    {
        var memInfo = enumType.GetMember(enumValue.ToString()).FirstOrDefault();
        string displayName = "";
        if (memInfo != null)
        {
            var displayAttr = memInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttr != null)
            {
                displayName = displayAttr.Name;
            }
            else
            {
                displayName = enumValue.ToString(); 
            }
        }
        else
        {
            displayName = enumValue.ToString();   
        }
        return displayName;
    }

}
