using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Nancy.Json;

namespace EMSWeb.Controllers
{
    public class AjaxViewModel
    {
        public string theDate { get; set; }
        public string StudentStatue { get; set; }
    }

    public class AjaxLangualeViewModel
    {
        public string Filename { get; set; }
        public string Subjects { get; set; }
        public string Language { get; set; }
        public string Mime_type { get; set; }
        public string Tags { get; set; }
    }
    public class ResourcesLibController : Controller
    {

        public async Task<ActionResult> AddNew(string firstname)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var root = "~/Content/Images";
                    foreach (var formFile in Request.Form.Files)
                    {
                        if (formFile.Length > 0)
                        {
                            var filePath = Path.Combine(root,Path.GetRandomFileName());

                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }
                    return Json(new { success = true, message = "File uploaded successfully" });
                }
                return Json(new { success = false, message = "Please select a file !" });

                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        // GET: ResourcesLib
        public  ActionResult Index()
        {
            Resources model = new Resources();
            model.Languages = new List<Languages>();
            model.Subjects = new List<Subject>();
            model.KnowledgeSharebyCountry = new List<KnowledgeSharebyCountry>();
            model.TeacherSupportDocuments = new List<TeachersSupportDocuments>();
            try
            {

                //using var connection = new MySqlConnection("DefaultConnection");
                //await connection.OpenAsync();

                //using var command = new MySqlCommand("SELECT * FROM languages WHERE `active`=1 Order By name;", connection);
                //using var reader = await command.ExecuteReaderAsync();
                //while (await reader.ReadAsync())
                //{
                //    var value = reader.GetValue(0);
                //    // do something with 'value'
                //}
                using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT id,Name FROM languages WHERE `active`=1 Order By name"))
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
                                model.Languages.Add(new Languages { Id = d["Id"].ToString(), Name = d["Name"].ToString() });
                            }
                        }
                    }
                    con.Close();
                }
                using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
                {
                    //Subjects
                    using (MySqlCommand cmd = new MySqlCommand("SELECT id,Name FROM subjects Order By name"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        var d = cmd.ExecuteReader();
                        if (d.HasRows)
                        {
                            while (d.Read())
                            {
                                model.Subjects.Add(new Subject { Id = d["Id"].ToString(), Name = d["Name"].ToString() });
                            }
                        }
                    }
                    con.Close();
                }
                using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
                {
                    //Knowledge by counyty
                    using (MySqlCommand cmd = new MySqlCommand("SELECT id,Name FROM countries Order By name"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        var d = cmd.ExecuteReader();
                        if (d.HasRows)
                        {
                            while (d.Read())
                            {
                                model.KnowledgeSharebyCountry.Add(new KnowledgeSharebyCountry { Id = d["Id"].ToString(), Name = d["Name"].ToString() });
                            }
                        }
                    }
                    con.Close();
                }
                //Teachers
                //using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
                //{
                //    using (MySqlCommand cmd = new MySqlCommand("SELECT Id, filename AS name FROM files_teachers_support_documents WHERE deleted = 0 Order By filename;"))
                //    {
                //        cmd.CommandType = CommandType.Text;
                //        cmd.Connection = con;
                //        con.Open();
                //         var d = cmd.ExecuteReader();
                //        if (d.HasRows)
                //        {
                //            while (d.Read())
                //            {
                //                model.TeacherSupportDocuments.Add(new TeachersSupportDocuments { Id = d["Id"].ToString(), Name = d["Name"].ToString() });
                //            }
                //        }
                //    }
                //    con.Close();
                //}

            }
            catch (Exception ex)
            {

            }
            return View(model);
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

       
        public string GetLangListData(string id="0")
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
            IList<AjaxLangualeViewModel> data = new List<AjaxLangualeViewModel>();
            using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT f.Filename, s.name as Subjects, l.name as Language,Mime_type,Tags FROM f1_emasuk_devresources.files as f INNER JOIN f1_emasuk_devresources.languages as l ON f.language = l.id  INNER JOIN f1_emasuk_devresources.subjects as s on f.subject1 = s.id      WHERE f.deleted = 0 and f.language ="+id))
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
                            data.Add(new AjaxLangualeViewModel { Filename = d["Filename"].ToString(), Subjects = d["Subjects"].ToString(), Language = d["Language"].ToString(), Mime_type = d["Mime_type"].ToString(), Tags = d["Tags"].ToString() });
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

        public string GetSubjectssById(string id = "2")
        {
            IList<AjaxLangualeViewModel> data = new List<AjaxLangualeViewModel>();
            using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
            {
                string sql = $"SELECT files.id, files.filename, CONCAT(subjects.code, IF(isnull(subjects2.code), '', IF(subjects.code <> subjects2.code, CONCAT(', ', subjects2.code), '')), IF(isnull(subjects3.code), '', IF(subjects.code <> subjects3.code, IF(subjects2.code <> subjects3.code, CONCAT(', ', subjects3.code), ''), ''))) as subjects,";
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
                            data.Add(new AjaxLangualeViewModel { Filename = d["Filename"].ToString(), Subjects = d["Subjects"].ToString(), Language = d["Language"].ToString(), Mime_type = d["Mime_type"].ToString() });
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
            IList<AjaxLangualeViewModel> data = new List<AjaxLangualeViewModel>();
            using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
            {
                using (MySqlCommand cmd = new MySqlCommand($"SELECT *, '' as Subjects, 'English' as Language FROM country_knowledge_share_files WHERE country_knowledge_share_files.deleted = 0 AND country_knowledge_share_files.country_id = {id} ORDER BY country_knowledge_share_files.filename;"))
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
                            data.Add(new AjaxLangualeViewModel { Filename = d["Filename"].ToString(), Subjects = d["Subjects"].ToString(), Language = d["Language"].ToString(), Mime_type = d["Mime_type"].ToString() });
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
            IList<AjaxLangualeViewModel> data = new List<AjaxLangualeViewModel>();
            using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT Id, filename,'' Subjects,'English' Language,Mime_type  FROM files_teachers_support_documents WHERE deleted = 0 Order By filename;"))
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
                            data.Add(new AjaxLangualeViewModel { Filename = d["Filename"].ToString(), Subjects = d["Subjects"].ToString(), Language = d["Language"].ToString(), Mime_type = d["Mime_type"].ToString() });
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
        // GET: ResourcesLib/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        public FileResult Download(string name)
        {
            string path = $"../files/resources/{name}";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        // POST: ResourcesLib/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ResourcesLib/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
    }
}