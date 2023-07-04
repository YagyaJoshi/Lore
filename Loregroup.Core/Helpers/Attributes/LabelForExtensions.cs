using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

    public static class LabelForExtensions {
        public static MvcHtmlString BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes) {
            var dict = new RouteValueDictionary(htmlAttributes);
            return html.BootstrapLabelFor(expression, dict);
        }
        public static MvcHtmlString BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) {
            var htmlAttributes = new Dictionary<string, object>();
            return html.BootstrapLabelFor(expression, htmlAttributes);
        }

        public static MvcHtmlString BootstrapLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes) {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            htmlAttributes.Add("class", "col-md-3 control-label");
            return html.LabelFor(expression, htmlAttributes);
        }

    }
