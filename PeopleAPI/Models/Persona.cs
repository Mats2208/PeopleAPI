using System.ComponentModel.DataAnnotations;

namespace PeopleAPI.Models
{
    public class Persona
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }       
        public string? LastName { get; set; }
    }
}