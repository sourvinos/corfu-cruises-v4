using API.Features.Reservations.Customers;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.Infrastructure.Users {

    public class UserExtended : IdentityUser, IMetadata {

        // FKs
        public int? CustomerId { get; set; }
        // Fields
        public string Displayname { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFirstFieldFocused { get; set; }
        public bool IsNewEmailPending { get; set; }
        public bool IsNewEmailSent { get; set; }
        public bool IsResetPasswordEmailPending { get; set; }
        public bool IsResetPasswordEmailSent { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public Customer Customer { get; set; }

    }

}