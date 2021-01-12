using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathOfServices.Business.Database
{
    public class OAuthTokenEntity
    {
        [Key]
        public string Value { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        public string UserId { get; set; }

        public DateTime? Expiry { get; set; }

        [NotMapped]
        public bool IsExpired => Expiry.HasValue && Expiry.Value > DateTime.Now;
    }
}