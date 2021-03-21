using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using EMSWeb.Filters;

namespace EMSWeb.Controllers
{


    public class ResourcesLibController : Controller
    {
        private string _connectionString;
        private IResourceLibService _resourceLibService;
        private ILanguageService _languageService;
        private ISubjectService _subjectService;
        private ITeacherSupportDocumentService _teacherSupportDocumentService;
        private IKnowledgeService _knowledgeService;
        private IWebHostEnvironment _hostingEnvironment;
        public ResourcesLibController(IConfiguration configuration,
            IResourceLibService resourceLibService,
            ILanguageService languageService,
            ISubjectService subjectService,
            IKnowledgeService knowledgeService,
            ITeacherSupportDocumentService teacherSupportDocumentService, IWebHostEnvironment hostingEnvironment)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _resourceLibService = resourceLibService;
            _languageService = languageService;
            _subjectService = subjectService;
            _teacherSupportDocumentService = teacherSupportDocumentService;
            _knowledgeService = knowledgeService;
            _hostingEnvironment = hostingEnvironment;
        }

         
        public async Task<ActionResult> Index()
        {
            ViewBag.Languages = await _languageService.GetList();
            ViewBag.Subjects = await _subjectService.GetList();
            ViewBag.KnowledgeSharebyCountry = await _knowledgeService.GetList();
            ViewBag.TeacherSupportDocuments = await _teacherSupportDocumentService.GetList();
            return View("List");
        }

        // GET: ResourcesLib/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ResourcesLib/Create
        public ActionResult Create()
        {
            return View();
        }

        public async Task<AjaxResourceList> GetLanguagesById(string id = "2")
        {
            return new AjaxResourceList(await _resourceLibService.GetListByLanguage(id));
        }

        public async Task<AjaxResourceList> GetSubjectssById(string id = "2")
        {
            return new AjaxResourceList(await _resourceLibService.GetListBySubjects(id));
        }

        public async Task<AjaxResourceList> GetKnowledgeSharedById(string id = "2")
        {
            return new AjaxResourceList(await _resourceLibService.GetListByKnowledgeShared(id));
        }

        public async Task<AjaxResourceList> GetTeachersDocById(string id = "2")
        {
            return new AjaxResourceList(await _resourceLibService.GetListByTeachersDoc(id));
        }


        // GET: /ResourcesLib/Language/5
        [HttpGet]
        public async Task<ActionResult> ByLanguage(int id)
        {
            ViewBag.Id = id;
            ViewBag.Mode = "language";
            ViewBag.Languages = await _languageService.GetList();
            ViewBag.Subjects = await _subjectService.GetList();
            ViewBag.KnowledgeSharebyCountry = await _knowledgeService.GetList();
            ViewBag.TeacherSupportDocuments = await _teacherSupportDocumentService.GetList();
            return View("List");
        }

        [HttpGet]
        public async Task<ActionResult> BySubject(int id)
        {
            ViewBag.Id = id;
            ViewBag.Mode = "subject";
            ViewBag.Languages = await _languageService.GetList();
            ViewBag.Subjects = await _subjectService.GetList();
            ViewBag.KnowledgeSharebyCountry = await _knowledgeService.GetList();
            ViewBag.TeacherSupportDocuments = await _teacherSupportDocumentService.GetList();
            return View("List");
        }

        [HttpGet]
        public async Task<ActionResult> ByKnowledgeShared(int id)
        {
            ViewBag.Id = id;
            ViewBag.Mode = "knowledgeshared";
            ViewBag.Languages = await _languageService.GetList();
            ViewBag.Subjects = await _subjectService.GetList();
            ViewBag.KnowledgeSharebyCountry = await _knowledgeService.GetList();
            ViewBag.TeacherSupportDocuments = await _teacherSupportDocumentService.GetList();
            return View("List");
        }

        [HttpGet]
        public async Task<ActionResult> ByTeacherDoc(int id)
        {
            ViewBag.Id = id;
            ViewBag.Mode = "teacherdoc";
            ViewBag.Languages = await _languageService.GetList();
            ViewBag.Subjects = await _subjectService.GetList();
            ViewBag.KnowledgeSharebyCountry = await _knowledgeService.GetList();
            ViewBag.TeacherSupportDocuments = await _teacherSupportDocumentService.GetList();
            return View("List");
        }

        // GET: ResourcesLib/Edit/5
        [ClaimRequirement(ClaimType.Admin)]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _resourceLibService.Get(id);
            var languages = await _languageService.GetList();
            ViewBag.Languages = (languages != null) ? languages.Select(x => new SelectListItem(x.Name, x.Id)) : new List<SelectListItem>();
            var subjects = await _subjectService.GetList();
            ViewBag.Subjects = (subjects != null) ? subjects.Select(x => new SelectListItem(x.Name, x.Id)) : new List<SelectListItem>();
            return View(model);
        }

        public async Task<FileResult> Download(string name)
        {
            var bytes = await _resourceLibService.DownloadFile(name);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }

        // POST: ResourcesLib/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimRequirement(ClaimType.Admin)]
        public async Task<ActionResult> Edit(ResourceModel model)
        {
            if (model.Id > 0)
            {
                await _resourceLibService.Update(model);
            }
            else
            {
                await _resourceLibService.Create(model);
            }
            return RedirectToAction("Edit", new { Id = model.Id } );
        }
        [HttpGet]
        [ClaimRequirement(ClaimType.Admin)]
        public async Task<ActionResult> Add()
        {
            var model = new ResourceModel();
            var languages = await _languageService.GetList();
            ViewBag.Languages = (languages != null) ? languages.Select(x => new SelectListItem(x.Name, x.Id)) : new List<SelectListItem>();
            var subjects = await _subjectService.GetList();
            ViewBag.Subjects = (subjects != null) ? subjects.Select(x => new SelectListItem(x.Name, x.Id)) : new List<SelectListItem>();
            return View("Edit", model);
        }
        // GET: ResourcesLib/Delete/5
        [ClaimRequirement(ClaimType.Admin)]
        public async Task<ActionResult> Delete(int id)
        {
            await _resourceLibService.Delete(id);
            return RedirectToAction("Index");
        }

        // POST: ResourcesLib/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimRequirement(ClaimType.Admin)]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public FileResult downloadFile(string filePath, string fileName)
        {
            string wwwrootPath = _hostingEnvironment.WebRootPath;

            filePath = filePath.Replace("_", "\\");
            string fullfilePath = wwwrootPath + "\\" + filePath + "\\" + fileName;
            string oldFile = fullfilePath;
            string newFile = DateTime.Now.Millisecond.ToString() + "_newFile.pdf";
            string fullpathNewFile = wwwrootPath + "\\" + filePath + "\\" + DateTime.Now.Millisecond.ToString() + "_newFile.pdf";
            // open the reader
            PdfReader reader = new PdfReader(oldFile);

            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            FileStream fs = new FileStream(fullpathNewFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // the pdf content
            PdfContentByte cb = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 8);
            string bookmarkTex = $"Licenced to EMS site for use in the academic year {DateTime.Now.Year}/{DateTime.Now.Year + 1}";
            try
            {
                LoggedInUser userSession = SessionHelper.GetObjectFromJson<LoggedInUser>(HttpContext.Session, "userObject");
                bookmarkTex = $"Licenced to {userSession.OrganisationName}  for use in the academic year {DateTime.Now.Year}/{DateTime.Now.Year + 1}";
            }
            catch (Exception ex)
            {

            }

            for (int pageNr = 1; pageNr <= reader.NumberOfPages; pageNr++)
            {
                // create the new page and add it to the pdf
                PdfImportedPage page = writer.GetImportedPage(reader, pageNr);
                //cb.AddTemplate(page, 0, 0);
                iTextSharp.text.Rectangle psize = reader.GetPageSizeWithRotation(pageNr);
                if (isPortrait(reader, pageNr))
                {
                    cb.AddTemplate(page, 0, 0);
                }
                else
                {
                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, psize.Height);
                    //cb.AddTemplate(page, 0, 1, -1, 0, page., 0);
                }
                Rectangle mediabox = reader.GetPageSize(pageNr);
                cb.SetFontAndSize(BaseFont.CreateFont(), 12f);
                cb.BeginText();
                if (isPortrait(reader, pageNr))
                {
                    cb.ShowTextAligned(Element.ALIGN_CENTER, bookmarkTex, mediabox.GetRight(1) - 10, mediabox.GetTop(300), 90);
                }
                else
                {
                    cb.ShowTextAligned(Element.ALIGN_CENTER, bookmarkTex, mediabox.GetTop(1) - 10, mediabox.GetRight(300), 90);
                }
                cb.EndText();
                cb.Stroke();


        
            }
            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
            IFileProvider provider = new PhysicalFileProvider(wwwrootPath + "\\" + filePath + "\\");
            IFileInfo fileInfo = provider.GetFileInfo(newFile);
            var readStream = fileInfo.CreateReadStream();
            //System.IO.File.Delete(newFile);
            var mimeType = "application/pdf";
            return File(readStream, mimeType, newFile);
        }

        public Boolean isPortrait(PdfReader reader, int pagenumber)
        {
            Rectangle currentPageRectangle = reader.GetPageSizeWithRotation(pagenumber);
            if (currentPageRectangle.Width > currentPageRectangle.Height)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}