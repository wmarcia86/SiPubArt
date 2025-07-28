using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Article
{
    public class DetailsModel : PageModel
    {
        public required string ArticleId { get; set; }

        public void OnGet(string id)
        {
            ArticleId = id;
        }
    }
}
