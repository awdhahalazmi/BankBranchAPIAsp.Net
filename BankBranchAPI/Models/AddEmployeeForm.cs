using System.ComponentModel.DataAnnotations;

namespace BankBranchAPI.Models
{
    public class AddEmployeeForm
    {
        [Required]
        public string Name { get; set; }


        [Required]

        public string Position { get; set; }

        [ValidCivilId]
        public string CivilId { get; set; }
         public int BankId { get; set; }
        public int Id { get; internal set; }
    }
}
