using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Models
{
    public class Customer
    {
        /// <summary>
        /// Identifer of the Customer
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Full name of the Customer
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name is Required")]
        public string FullName { get; set; }

        /// <summary>
        /// Date of birth of the Customer stored as date only
        /// Note: In order for the required attribute to be ran on a DateTime property, it must be nullable
        /// </summary>
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Valid Date is required for DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
