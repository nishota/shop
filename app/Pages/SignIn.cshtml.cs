using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace app.Pages
{
    [AllowAnonymous]
    public class SignInModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
