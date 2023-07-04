using System.Linq;

namespace System.Web.Mvc {
    public static class HtmlExtensionMethods {
        /// <summary>
        /// Returns an error alert that lists each model error, much like the standard ValidationSummary only with
        /// altered markup for the Twitter bootstrap styles.
        /// </summary>
        public static MvcHtmlString ValidationSummaryBootstrap(this HtmlHelper helper, bool closeable) {
            # region Equivalent view markup
            // var errors = ViewData.ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage));
            //
            // if (errors.Count() > 0)
            // {
            //     <div class="alert alert-error alert-block">
            //         <button type="button" class="close" data-dismiss="alert">&times;</button>
            //         <strong>Validation error</strong> - please fix the errors listed below and try again.
            //         <ul>
            //             @foreach (var error in errors)
            //             {
            //                 <li class="text-error">@error</li>
            //             }
            //         </ul>
            //     </div>
            // }
            # endregion

            var errors = helper.ViewContext.ViewData.ModelState.SelectMany(state => state.Value.Errors.Select(error => error.ErrorMessage));

            int errorCount = errors.Count();

            if (errorCount == 0) {
                //return new MvcHtmlString(string.Empty);
            }

            var div = new TagBuilder("div");
            div.AddCssClass("alert");
            div.AddCssClass("alert-danger");
            div.AddCssClass("alert-dismissable");
            div.AddCssClass("hide");

            string message;

            if (errorCount == 1) {
                message = errors.First();
            } else
            {
                message = "";//"Please fix the errors listed below and try again.";
                //div.AddCssClass("alert-block");
            }

            if (closeable) {
                var button = new TagBuilder("button");
                button.AddCssClass("close");
                button.MergeAttribute("type", "button");
                button.MergeAttribute("data-dismiss", "alert");
                button.MergeAttribute("aria-hidden", "true");
                button.InnerHtml = "&times;";
                div.InnerHtml += button.ToString();
            }

            div.InnerHtml += "<strong>Alert</strong> -<br /> " + "<div class='validationMessageList'>"+ message + "</div>";

            if (errorCount > 1) {
                var divOut = new TagBuilder("div");

                foreach (var error in errors) {
                    var divIn = new TagBuilder("div");
                    divIn.AddCssClass("clear");
                    divIn.SetInnerText(error);
                    divOut.InnerHtml += divIn.ToString();
                }

                div.InnerHtml += divOut.ToString();
            }

            return new MvcHtmlString(div.ToString());
        }

        /// <summary>
        /// Overload allowing no arguments.
        /// </summary>
        public static MvcHtmlString ValidationSummaryBootstrap(this HtmlHelper helper) {
            return ValidationSummaryBootstrap(helper, true);
        }
    }
}