using System.ComponentModel.DataAnnotations;

namespace MyUnitTestExperience.ViewModels
{
    public class CreateInternalEmployeeViewModel
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;  
    }
}
