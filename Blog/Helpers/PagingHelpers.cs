  using System.Text;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using Blog.Core.HelperClasses;

namespace Blog.Helpers
{
    /// <summary>
    /// Paging helpers.
    /// </summary>
    public static class PagingHelpers
    {
        /// <summary>
        /// Pages the links.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="pageInfo">The page information.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(
            this HtmlHelper html,
            PageInfo pageInfo, 
            Func<int, string> pageUrl)
        {
            var result = new StringBuilder();
            for (var i = 1; i <= pageInfo.TotalPages; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");
                var a = new TagBuilder("a");
                a.MergeAttribute("href", pageUrl(i));
                a.AddCssClass("page-link");

                if (i == 1 && i != pageInfo.PageNumber)
                {
                    a.InnerHtml = "<span aria-hidden='true'>&laquo;</span><span class='sr-only'>Previous</span>";
                }
                else if ((i == pageInfo.TotalPages) && i != pageInfo.PageNumber)
                {
                    a.InnerHtml = "<span aria-hidden='true'>&raquo;</span><span class='sr-only'>Next</span>";
                }
                else
                {
                    a.InnerHtml = i.ToString();
                }

                // if this is the current page, then select it, for example, adding a class
                if (i == pageInfo.PageNumber)
                {
                    li.AddCssClass("active");
                    const string style = "z-index: 1; color: #fff; background-color: #007bff; border-color: #007bff;";
                    a.MergeAttribute("style", style);
                    li.AddCssClass("disabled");
                }

                li.InnerHtml = a.ToString();
                result.Append(li);
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}
