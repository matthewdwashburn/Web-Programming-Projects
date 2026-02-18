using Gargoyles.Models;
using System.ComponentModel.DataAnnotations;

namespace Gargoyles.Entities
{
    public class GargoyleEntity
    {

        public GargoyleEntity() 
        {

        }

        public GargoyleEntity(GargoyleModel model)
        {
            Name = model.Name;
            Color = model.Color;
            Size = model.Size;
            Gender = model.Gender;
        }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
        public string? Name { get; set; }

        [MinLength(3, ErrorMessage = "Color must be at least 3 characters long")]
        public string? Color { get; set; }

        [MinLength(3, ErrorMessage = "Size must be at least 3 characters long")]
        public string? Size { get; set; }

        [MinLength(3, ErrorMessage = "Gender must be at least 3 characters long")]
        public string? Gender { get; set; }

        public GargoyleModel ToModel()
        {
            return new GargoyleModel
            {
                Name = this.Name,
                Color = this.Color,
                Size = this.Size,
                Gender = this.Gender
            };
        }
    }
}
