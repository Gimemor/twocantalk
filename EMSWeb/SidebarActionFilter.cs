using EMSWeb.BusinessServices.Services.Interfaces;
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

        }
    }
}
