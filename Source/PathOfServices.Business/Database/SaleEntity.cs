using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathOfServices.Business.Database
{
    public class SaleEntity : EntityBase
    {
        public decimal Value { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public ServiceEntity Service { get; set; }
        public string ServiceId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        public string UserId { get; set; }

        public DateTime Expires { get; set; }
    }
}
