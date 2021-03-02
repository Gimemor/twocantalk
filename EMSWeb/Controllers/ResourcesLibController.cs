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
using MySqlConnector;
using Nancy.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
namespace EMSWeb.Controllers
{
    public class AjaxViewModel
    {
        public string theDate { get; set; }
        public string StudentStatue { get; set; }
    }

    public class AjaxLanguageViewModel
    {
        public string Filename { get; set; }
        public string Subjects { get; set; }
        public string Language { get; set; }
        public string Mime_type { get; set; }
        public string Tags { get; set; }
        public uint Id { get; set; }
    }

    public class ResourcesLibController : Controller
    {
        private string _connectionString;
        private IResourceLibService _resourceLibService;
        private ILanguageService _languageService;
        private ISubjectService _subjectService;
        private ITeacherSupportDocumentService _teacherSupportDocumentService;
        private IKnowledgeService _knowledgeService;
        private  IWebHostEnvironment _hostingEnvironment;
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
        public AjaxLanguageViewModel BindToAjaxLanguageViewModel(MySqlDataReader d)
        {
            return new AjaxLanguageViewModel
            {
                Id = (uint)d["Id"],
                Filename = d["Filename"].ToString(),
                Subjects = d["Subjects"].ToString(),
                Language = d["Language"].ToString(),
                Mime_type = d["Mime_type"].ToString(),
                Tags = d["Tags"].ToString()
            };
        }

        // GET: ResourcesLib
        public async Task<ActionResult> Index()
        {
            LoggedInUser userSession = SessionHelper.GetObjectFromJson<LoggedInUser>(HttpContext.Session, "userObject");
            if(userSession is null)
            {
                RedirectToAction("Index", "Login");
            }
            Resources model = new Resources();
            if (TempData.Keys.Contains("Id"))
            {
                ViewBag.Id = TempData["Id"];
            }
            if (TempData.Keys.Contains("Mode"))
            {
                ViewBag.Mode = TempData["Mode"];
            }
            ViewBag.Languages = await _languageService.GetList();
            ViewBag.Subjects = await _subjectService.GetList();
            ViewBag.KnowledgeSharebyCountry = await _knowledgeService.GetList();
            ViewBag.TeacherSupportDocuments = await _teacherSupportDocumentService.GetList();

            return View("List", model);
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


        public string GetLangListData(string id = "0")
        {
            AjaxViewModel aViewModel = new AjaxViewModel { StudentStatue = "stat4", theDate = "12/24/2005" };
            AjaxViewModel aViewModel2 = new AjaxViewModel { StudentStatue = "stat5", theDate = "12/24/2005" };
            AjaxViewModel aViewModel3 = new AjaxViewModel { StudentStatue = "stat6", theDate = "12/24/2005" };

            IList<AjaxViewModel> data = new List<AjaxViewModel>();
            data.Add(aViewModel);
            data.Add(aViewModel2);
            data.Add(aViewModel3);

            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(data);
            json = "{ \"data\": " + json;
            json = json + " }";
            return json;
        }

        public string GetLanguagesById(string id = "2")
        {
            IList<AjaxLanguageViewModel> data = new List<AjaxLanguageViewModel>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT f.id, f.Filename, s.name as Subjects, l.name as Language,Mime_type,Tags FROM files as f INNER JOIN languages as l ON f.language = l.id  INNER JOIN subjects as s on f.subject1 = s.id      WHERE f.deleted = 0 and f.language =" + id))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                // trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
                var d = cmd.ExecuteReader();
                if (d.HasRows)
                {
                    while (d.Read())
                    {
                        data.Add(BindToAjaxLanguageViewModel(d));
                    }
                }
                // trvResourcesByLanguages.DataBind();
                con.Close();
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(data);
            json = "{ \"data\": " + json;
            json = json + " }";
            return json;
        }

        public string GetSubjectssById(string id = "2")
        {
            IList<AjaxLanguageViewModel> data = new List<AjaxLanguageViewModel>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                string sql = $"SELECT files.id, files.filename, files.Tags as Tags, CONCAT(subjects.code, IF(isnull(subjects2.code), '', IF(subjects.code <> subjects2.code, CONCAT(', ', subjects2.code), '')), IF(isnull(subjects3.code), '', IF(subjects.code <> subjects3.code, IF(subjects2.code <> subjects3.code, CONCAT(', ', subjects3.code), ''), ''))) as subjects,";
                sql += $" subjects.name As subject1, subjects2.name AS subject2, subjects3.name AS subject3, languages.name AS language, files.mime_type, DATE_FORMAT(files.last_uploaded_timestamp, '%d/%m/%Y') As last_uploaded_date";
                sql += $" FROM(subjects AS subjects2 RIGHT JOIN(subjects INNER JOIN(files INNER JOIN languages ON files.language = languages.id) ON subjects.id = files.subject1) ON subjects2.id = files.subject2) LEFT JOIN subjects AS subjects3 ON files.subject3 = subjects3.id";
                sql += $" WHERE (files.subject1={id} OR files.subject2={id} OR files.subject3={id}) AND files.deleted=0;";
                using (MySqlCommand cmd = new MySqlCommand(sql))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    // trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
                    var d = cmd.ExecuteReader();
                    if (d.HasRows)
                    {
                        while (d.Read())
                        {
                            data.Add(BindToAjaxLanguageViewModel(d));
                        }
                    }
                    // trvResourcesByLanguages.DataBind();
                    con.Close();
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(data);
            json = "{ \"data\": " + json;
            json = json + " }";
            return json;
        }
        public string GetKnowledgeSharedById(string id = "2")
        {
            IList<AjaxLanguageViewModel> data = new List<AjaxLanguageViewModel>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand($"SELECT *, '' as Subjects, 'English' as Language, '' as Tags FROM country_knowledge_share_files WHERE country_knowledge_share_files.deleted = 0 AND country_knowledge_share_files.country_id = {id} ORDER BY country_knowledge_share_files.filename;"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    // trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
                    var d = cmd.ExecuteReader();
                    if (d.HasRows)
                    {
                        while (d.Read())
                        {
                            data.Add(BindToAjaxLanguageViewModel(d));
                        }
                    }
                    // trvResourcesByLanguages.DataBind();
                    con.Close();
                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(data);
            json = "{ \"data\": " + json;
            json = json + " }";
            return json;
        }
        public string GetTeachersDocById(string id = "2")
        {
            IList<AjaxLanguageViewModel> data = new List<AjaxLanguageViewModel>();
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand("SELECT Id, filename,'' Subjects,'English' Language,Mime_type, '' as Tags  FROM files_teachers_support_documents WHERE deleted = 0 Order By filename;"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                con.Open();
                // trvResourcesByLanguages.DataSource = cmd.ExecuteReader();
                var d = cmd.ExecuteReader();
                if (d.HasRows)
                {
                    while (d.Read())
                    {
                        data.Add(BindToAjaxLanguageViewModel(d));
                    }
                }
                // trvResourcesByLanguages.DataBind();
                con.Close();
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(data);
            json = "{ \"data\": " + json;
            json = json + " }";
            return json;
        }
        // GET: /ResourcesLib/Language/5
        [HttpGet]
        public ActionResult ByLanguage(int id)
        {
            TempData["Id"] = id;
            TempData["Mode"] = "language";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult BySubject(int id)
        {
            TempData["Id"] = id;
            TempData["Mode"] = "subject";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ByKnowledgeShared(int id)
        {
            TempData["Id"] = id;
            TempData["Mode"] = "knowledgeshared";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ByTeacherDoc(int id)
        {
            TempData["Id"] = id;
            TempData["Mode"] = "teacherdoc";
            return RedirectToAction("Index");
        }

        // GET: ResourcesLib/Edit/5
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
            return RedirectToAction("Index");
        }
        [HttpGet]
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
        public async Task<ActionResult> Delete(int id)
        {
            await _resourceLibService.Delete(id);
            return RedirectToAction("Index");
        }

        // POST: ResourcesLib/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                Rectangle mediabox = reader.GetPageSize(pageNr);
                Rectangle cropbox = reader.GetCropBox(pageNr);
                cb.SetFontAndSize(BaseFont.CreateFont(), 12f);
                cb.BeginText();
                cb.ShowTextAligned(Element.ALIGN_CENTER, bookmarkTex, mediabox.GetRight(1) - 15, mediabox.GetTop(300), 90);
                cb.EndText();
                cb.Stroke();
            }

            // create the new page and add it to the pdf
            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);

            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
            IFileProvider provider = new PhysicalFileProvider(wwwrootPath + "\\" + filePath + "\\");
            IFileInfo fileInfo = provider.GetFileInfo(newFile);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/pdf";
            return File(readStream, mimeType, newFile);
        }
    }
}