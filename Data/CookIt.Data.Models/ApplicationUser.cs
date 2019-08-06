// ReSharper disable VirtualMemberCallInConstructor
namespace CookIt.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookIt.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Addresses = new HashSet<Address>();
            this.Review = new HashSet<Review>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        // Personal Info
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Review> Review { get; set; }

        public virtual ICollection<Order> Issuers { get; set; }

        public virtual ICollection<Order> Couriers { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
