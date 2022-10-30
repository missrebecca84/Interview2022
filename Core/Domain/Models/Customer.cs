using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Models
{
    public record Customer
    {
        /// <summary>
        /// Identifer of the Customer
        /// </summary>
        public Guid Id { get; set; }

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
