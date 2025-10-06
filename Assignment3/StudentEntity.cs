using Students.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Students.Entities
{
    public class StudentEntity
    {
        public StudentEntity()
        {

        }
        public StudentEntity(StudentModel studentModel)
        {
            this.FirstName = studentModel.FirstName;
            this.LastName = studentModel.LastName;
            this.FavoriteCharacter = studentModel.FavoriteCharacter;
            this.CreatedTime = studentModel.CreatedTime;
            Views = studentModel.ViewDates.Count;
            
        }
        [MinLength(1)]
        [Required]
        public string? FirstName { get; set; }
        [MinLength(1)]
        [Required]
        public string? LastName { get; set; }

        [MinLength(1)]
        [Required]
        public string? FavoriteCharacter { get; set; }

        public DateTime CreatedTime { get; set; }

        //ViewDate Object containter
        public class ViewEntity
        {
            public string? ViewDate { get; set; }
        }

        public int Views { get; set; }

        public StudentModel ToModel()
        {
            return new StudentModel()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                FavoriteCharacter = this.FavoriteCharacter,
            };
        }
    }
}
