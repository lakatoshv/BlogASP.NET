using System.Text;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using Blog.Core.HelperClasses;

//.............................
namespace Blog.Helpers
{
    public static class PagingHelpers
{
    public static MvcHtmlString PageLinks(this HtmlHelper html,
        PageInfo pageInfo, Func<int, string> pageUrl)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 1; i <= pageInfo.TotalPages; i++)
        {
            TagBuilder li = new TagBuilder("li");
            li.AddCssClass("page-item");
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", pageUrl(i));
            a.AddCssClass("page-link");

            if (i == 1 && i != pageInfo.PageNumber)
                a.InnerHtml = "<span aria-hidden='true'>&laquo;</span><span class='sr-only'>Previous</span>";
            else if ((i == pageInfo.TotalPages) && i != pageInfo.PageNumber)
                a.InnerHtml = "<span aria-hidden='true'>&raquo;</span><span class='sr-only'>Next</span>";
            else 
                a.InnerHtml = i.ToString();

            // если текущая страница, то выделяем ее,
            // например, добавляя класс
            if (i == pageInfo.PageNumber)
            {
                li.AddCssClass("active");
                var style = "z-index: 1; color: #fff; background-color: #007bff; border-color: #007bff;";
                a.MergeAttribute("style", style);
                    li.AddCssClass("disabled");
            }
            
            li.InnerHtml = a.ToString();
            result.Append(li.ToString());
        }

        return MvcHtmlString.Create(result.ToString());
    }
}

}