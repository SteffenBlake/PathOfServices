using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathOfServices.Business.Database
{
    public class OAuthCodeEntity
    {
        [Key]
        public string Value { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        public string UserId { get; set; }
    }
}
