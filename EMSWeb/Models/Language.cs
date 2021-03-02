using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMSWeb.Models
{
    public class Language
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class _Subject
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class _KnowledgeSharebyCountry
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class _TeachersSupportDocuments
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class _Resources
    {
        public List<Language> Languages { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<KnowledgeSharebyCountry> KnowledgeSharebyCountry { get; set; }
        public List<TeachersSupportDocuments> TeacherSupportDocuments { get; set; }
    }
}
