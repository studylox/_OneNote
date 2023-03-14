using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NETCoreMVCwithMSGraph.Models;
using static System.Formats.Asn1.AsnWriter;


namespace NETCoreMVCwithMSGraph.Controllers
{
    [Authorize]
    public class SectionController : Controller
    {

        private readonly ILogger<OneNoteController> _logger;
        private readonly GraphServiceClient _graphClient;



        public SectionController(GraphServiceClient graphClient, ILogger<OneNoteController> logger)
        {
            _graphClient = graphClient;
            _logger = logger;

        }


        
        [AuthorizeForScopes(Scopes = new[] { "Notes.Read.All" })]
        public async Task<ActionResult> OneNoteSectionsCatalogue(string id)
        {
            ViewBag.DisplayName = User.GetDisplayName();

          
            var sections = await _graphClient.Me.Onenote.Notebooks[id].Sections.Request().GetAsync();

            var model = new List<SectionViewModel>();

            foreach (var section in sections)
            {
                var sectionViewModel = new SectionViewModel
                {
                    Name = section.DisplayName,
                    Id = section.Id,
                    CreatedDateTime = section.CreatedDateTime.GetValueOrDefault()

                    
                    
                    
                };

                model.Add(sectionViewModel);
            }

            return View(model);
        }

       
        [AuthorizeForScopes(Scopes = new[] { "Notes.ReadWrite.All" })]
        
        public async Task<ActionResult> AddNewSection(string notebookId, string sectionName)
        {
          
            var newSection = new OnenoteSection
            {
                DisplayName = sectionName
            };

          
            var createdSection = await _graphClient.Me.Onenote.Notebooks[notebookId].Sections.Request().AddAsync(newSection);


            
           
            return Redirect("/Section/OneNoteSectionsCatalogue/"+notebookId);



        }




    }
}
