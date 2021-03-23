using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager
{
    public class Product : IValidatableObject
    {
        // - un nom
        // - un code (unique)
        // - une date de début de validité
        // - une date de fin de validité
        // La date de début doit être antérieure à la date de fin de validité.
        [Key]
        public Guid ProductId { get; set; }

        [Required]
        [Column(TypeName = "varchar(16)")]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set;} 

        [Required]
        public DateTime EndDate { get; set; }



        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
             List<ValidationResult> errors = new List<ValidationResult>();
            if (EndDate < StartDate)
            {
                errors.Add(new ValidationResult($"{nameof(EndDate)} needs to be greater than  StartDate.", new List<string> { nameof(EndDate) }));
            }
            return errors;
        }
    }
}
