using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Controllers;
using EMSWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace EMSWeb
{
	public class SidebarActionFilter : ActionFilterAttribute
    {
        private ILanguageService _languageService;
        private ISubjectService _subjectService;
        private ITeacherSupportDocumentService _teacherSupportDocumentService;
        private IKnowledgeService _knowledgeService;

        public SidebarActionFilter(ILanguageService languageService,
            ISubjectService subjectService,
            IKnowledgeService knowledgeService,
            ITeacherSupportDocumentService teacherSupportDocumentService)
        {
            _languageService = languageService;
            _subjectService = subjectService;
            _teacherSupportDocumentService = teacherSupportDocumentService;
            _knowledgeService = knowledgeService;
        }

        public override async Task OnActionExecutionAsync(
         ActionExecutingContext context,
         ActionExecutionDelegate next)
        {
            // Do something before the action executes.
            var loggedInUser = context.HttpContext.Session.GetObjectFromJson<LoggedInUser>("userObject"); ;
            
            // next() calls the action method.
            var resultContext = await next();
            // resultContext.Result is set.
            if (!(context.Controller is Controller))
            {
                return;
            }
            ((Controller)context.Controller).ViewBag.MenuLanguages = await _languageService.GetList();
            ((Controller)context.Controller).ViewBag.MenuSubjects = await _subjectService.GetList();
            ((Controller)context.Controller).ViewBag.MenuKnowledgeSharebyCountry = await _knowledgeService.GetList();
            ((Controller)context.Controller).ViewBag.MenuTeacherSupportDocuments = await _teacherSupportDocumentService.GetList();
            if (loggedInUser != null)
            {
                ((Controller)context.Controller).ViewBag.PermTwoCanTalk = loggedInUser.TwoCanTalk;
                ((Controller)context.Controller).ViewBag.PermTextTutor = loggedInUser.TextTutor;
                ((Controller)context.Controller).ViewBag.PermTalkingTutor = loggedInUser.TalkingTutor;
                ((Controller)context.Controller).ViewBag.PermAdmin = loggedInUser.PermAdmin; 
                ((Controller)context.Controller).ViewBag.PhraseBook = loggedInUser.PhraseBook;
            }

        }
    }
}
