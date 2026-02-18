using System.ComponentModel.DataAnnotations;

namespace Gargoyles.Models
{
    public class GargoyleModel
    {
        public string? Name { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public string? Gender { get; set; }

        //We don't want the entity to know about this field, this is why dependency injection is good
        public DateTime LastUpdated { get; set; }

        public string ETag()
        {
            return this.LastUpdated.ToString();
        }
    }
}
