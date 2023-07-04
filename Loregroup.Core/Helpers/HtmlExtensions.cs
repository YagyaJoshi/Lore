using System;
using System.Web.Mvc;

namespace Loregroup.Core.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Box(this HtmlHelper helper, string title, MvcHtmlString body)
        {
            TagBuilder span = new TagBuilder("div");
            span.AddCssClass("fieldset");
            span.SetInnerText(title.ToString());

            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("boxContent");
            div.InnerHtml = body.ToString();

            return MvcHtmlString.Create(span.ToString() + div.ToString());
        }
    } 

}
