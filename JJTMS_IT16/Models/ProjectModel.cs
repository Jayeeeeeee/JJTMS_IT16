using System.ComponentModel.DataAnnotations;

namespace JJTMS_IT16.Models
{
    public class ProjectModel
    {
        [Key] public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
