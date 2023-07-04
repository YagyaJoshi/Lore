using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

public static class TextBoxForExtensions {
    #region placeholder textbox
    public static MvcHtmlString TextBoxWithPHFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.TextBoxWithPHFor(expression, dict);
    }
    public static MvcHtmlString TextBoxWithPHFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.TextBoxWithPHFor(expression, htmlAttributes);
    }
    public static MvcHtmlString TextBoxWithPHFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!String.IsNullOrEmpty(labelText)) {
            if (htmlAttributes == null) {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("placeholder", labelText);
        }
        return html.TextBoxFor(expression, htmlAttributes);
    }
    #endregion

    #region Bootstrap textbox
    public static MvcHtmlString BsTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.BsTextBoxFor(expression, dict);
    }
    public static MvcHtmlString BsTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.BsTextBoxFor(expression, htmlAttributes);
    }
    public static MvcHtmlString BsTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!String.IsNullOrEmpty(labelText)) {
            if (htmlAttributes == null) {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("class", "form-control");
        }
        return html.TextBoxFor(expression, htmlAttributes);
    }
    #endregion


    #region TextAreaFor
    public static MvcHtmlString BsTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
    {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.BsTextAreaFor(expression, dict);
    }
    public static MvcHtmlString BsTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
    {
        var htmlAttributes = new Dictionary<string, object>();
        return html.BsTextAreaFor(expression, htmlAttributes);
    }
    public static MvcHtmlString BsTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
    {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!string.IsNullOrEmpty(labelText))
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("class", "form-control");
            htmlAttributes.Add("placeholder", labelText);
        }
        return helper.TextAreaFor(expression, htmlAttributes);
    }
    #endregion

    #region Bootstrap textbox with placeholder
    public static MvcHtmlString BsTextBoxWithPHFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.BsTextBoxWithPHFor(expression, dict);
    }
    public static MvcHtmlString BsTextBoxWithPHFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.BsTextBoxWithPHFor(expression, htmlAttributes);
    }
    public static MvcHtmlString BsTextBoxWithPHFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!String.IsNullOrEmpty(labelText)) {
            if (htmlAttributes == null) {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("class", "form-control");
            htmlAttributes.Add("placeholder", labelText);
        }
        return html.TextBoxFor(expression, htmlAttributes);
    }
    #endregion

    #region Bootstrap date picker textbox
    public static MvcHtmlString DatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.DatePickerFor(expression, dict);
    }
    public static MvcHtmlString DatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.DatePickerFor(expression, htmlAttributes);
    }
    public static MvcHtmlString DatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!String.IsNullOrEmpty(labelText)) {
            if (htmlAttributes == null) {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("class", "form-control date-picker");
        }

        return html.TextBoxFor(expression, htmlAttributes);
    }
    #endregion

    #region Bootstrap date-time picker textbox
    public static MvcHtmlString DateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.DateTimePickerFor(expression, dict);
    }
    public static MvcHtmlString DateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.DateTimePickerFor(expression, htmlAttributes);
    }
    public static MvcHtmlString DateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!String.IsNullOrEmpty(labelText)) {
            if (htmlAttributes == null) {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("class", "form-control date-time-picker");
        }

        return html.TextBoxFor(expression, htmlAttributes);
    }
    #endregion
}
