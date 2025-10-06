using System.ComponentModel.DataAnnotations;

namespace Students.Models
{   
    // Student model to construct each student entity
    public class StudentModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FavoriteCharacter {  get; set; }
        public DateTime CreatedTime { get; private set; } = DateTime.Now;

        public List<string> ViewDates { get; } = new List<string>();

        //Views in string array cause it only exists within the context of students
     
    }
}
