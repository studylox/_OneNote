using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NETCoreMVCwithMSGraph.Models;
using static System.Collections.Specialized.BitVector32;
using static System.Formats.Asn1.AsnWriter;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NETCoreMVCwithMSGraph.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        private readonly ILogger<OneNoteController> _logger;
        private readonly GraphServiceClient _graphClient;



        public PageController(GraphServiceClient graphClient, ILogger<OneNoteController> logger)
        {
            _graphClient = graphClient;
            _logger = logger;

        }
       

        [AuthorizeForScopes(Scopes = new[] { "Notes.Read.All" })]
      
            public async Task<ActionResult> OneNotePagesCatalogue(string id)
            {
          
                ViewBag.DisplayName = User.GetDisplayName();

                var pages = await _graphClient.Me.Onenote.Sections[id].Pages.Request().GetAsync();
                var model = new List<PageViewModel>();

                foreach (var page in pages)
                {
                    var pageViewModel = new PageViewModel
                    {
                        Name = page.Title,
                        Id = page.Id,
                        CreatedDateTime = page.CreatedDateTime.GetValueOrDefault()
                    };

                    model.Add(pageViewModel);
                }
           
                return View(model);
            }


       



        




    }

}
