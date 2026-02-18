using System.ComponentModel.DataAnnotations;

namespace Students.Entities
{
    public class StudentsEntity
    {
        public List<StudentEntity>? FavoriteCharacters { get; set; }
    }

    public class StudentPatchEntity
    {
        [MinLength(1)]
        public string? FirstName { get; set; }

        [MinLength(1)]
        public string? LastName { get; set; }

        [MinLength(1)]
        public string? FavoriteCharacter { get; set; }
    }
}
