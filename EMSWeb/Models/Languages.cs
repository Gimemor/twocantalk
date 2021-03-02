using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.Models
{
    public class Languages
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class Subject
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class KnowledgeSharebyCountry
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class TeachersSupportDocuments
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class Resources
    {
        public List<Languages> Languages { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<KnowledgeSharebyCountry> KnowledgeSharebyCountry { get; set; }
        public List<TeachersSupportDocuments> TeacherSupportDocuments { get; set; }
        public string SelectedLanguage { get; set; }
        public List<SelectListItem> LanguagesList { get; set; }
        public string Subject { get; set; }
        public string Subject3 { get; set; }
        public string Subject2 { get; set; }
    }
    public class FileData
    {
        public string SelectedLanguage
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Subject2
        {
            get;
            set;
        }
        public string Subject3
        {
            get;
            set;
        }
    }
}
