using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

public static class PasswordForExtensions {
    #region placeholder textbox
    public static MvcHtmlString PasswordWithPhFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.PasswordWithPhFor(expression, dict);
    }
    public static MvcHtmlString PasswordWithPhFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.PasswordWithPhFor(expression, htmlAttributes);
    }

    public static MvcHtmlString PasswordWithPhFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!String.IsNullOrEmpty(labelText)) {
            if (htmlAttributes == null) {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("placeholder", labelText);
        }
        return html.PasswordFor(expression, htmlAttributes);
    }
    #endregion

    #region bootstrap password textbox
    public static MvcHtmlString BsPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.BsPasswordFor(expression, dict);
    }
    public static MvcHtmlString BsPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.BsPasswordFor(expression, htmlAttributes);
    }

    public static MvcHtmlString BsPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        if (!String.IsNullOrEmpty(labelText)) {
            if (htmlAttributes == null) {
                htmlAttributes = new Dictionary<string, object>();
            }
            htmlAttributes.Add("class", "form-control");
        }
        return html.PasswordFor(expression, htmlAttributes);
    }
    #endregion

    #region bootstrap password textbox with placeholder
    public static MvcHtmlString BsPasswordWithPhFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
        var dict = new RouteValueDictionary(htmlAttributes);
        return html.BsPasswordWithPhFor(expression, dict);
    }
    public static MvcHtmlString BsPasswordWithPhFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
        var htmlAttributes = new Dictionary<string, object>();
        return html.BsPasswordWithPhFor(expression, htmlAttributes);
    }

    public static MvcHtmlString BsPasswordWithPhFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
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
        return html.PasswordFor(expression, htmlAttributes);
    }
    #endregion
}