using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NETCoreMVCwithMSGraph.Models;
using System.ComponentModel;
using System.Diagnostics;



namespace NETCoreMVCwithMSGraph.Controllers
{
    [Authorize]
    public class OneNoteController : Controller
    {


        private readonly ILogger<OneNoteController> _logger;
        private readonly GraphServiceClient _graphClient;

        

        public OneNoteController(GraphServiceClient graphClient, ILogger<OneNoteController> logger)
        {
            _graphClient = graphClient;
            _logger = logger;

        }

       

        
        [AuthorizeForScopes(Scopes = new[] { "Notes.Read.All" })]
        public async Task<ActionResult> OneNoteCatalogue()
        {
            ViewBag.DisplayName = User.GetDisplayName();

          
            var notebooks = await _graphClient.Me.Onenote.Notebooks.Request().GetAsync();
            var model = new List<NotebookViewModel>();

            foreach (var notebook in notebooks)
            {
                var notebookViewModel = new NotebookViewModel
                {
                    Name = notebook.DisplayName,
                    Id = notebook.Id,
                    CreatedDateTime = notebook.CreatedDateTime.GetValueOrDefault()
                };

                model.Add(notebookViewModel);
            }

            return View(model);
        }


       
        [AuthorizeForScopes(Scopes = new[] { "Notes.ReadWrite.All" })]
        public async Task<ActionResult> AddNewNotebook(string displayName)
        {


            var newNote = new Notebook
            {
                DisplayName = displayName
                
            };

            await _graphClient.Me.Onenote.Notebooks.Request().AddAsync(newNote);


            return RedirectToAction("OneNoteCatalogue");
           

        }













    }
}
