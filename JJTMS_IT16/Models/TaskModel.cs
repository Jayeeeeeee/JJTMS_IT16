using System.ComponentModel.DataAnnotations;

namespace JJTMS_IT16.Models
{
    public class TaskModel
    {
        [Key] public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUser { get; set; }  // User assigned to the task
        public int ProjectId { get; set; }  // Link task to project
        public ProjectModel Project { get; set; }
    }
}
