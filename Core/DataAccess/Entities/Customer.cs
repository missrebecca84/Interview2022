using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DataAccess.Entities
{
    public class Customer
    {
        /// <summary>
        /// Identifier of the Customer entity
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Full name of the Customer
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Date of birth of the Customer stored as date only
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}
