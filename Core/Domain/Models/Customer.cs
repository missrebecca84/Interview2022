using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Models
{
    public class Customer
    {
        /// <summary>
        /// Identifer of the Customer
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Full name of the Customer
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name is Required")]
        public string FullName { get; set; }

        /// <summary>
        /// Date of birth of the Customer stored as date only
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Valid Date is required for DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
    }
}
