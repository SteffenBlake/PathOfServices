using System.ComponentModel.DataAnnotations.Schema;

namespace PathOfServices.Business.Database
{
    public class ReputationEntity : EntityBase
    {
        [ForeignKey(nameof(FromId))]
        public UserEntity From { get; set; }
        public string FromId { get; set; }

        [ForeignKey(nameof(ToId))]
        public UserEntity To { get; set; }
        public string ToId { get; set; }

        public ReputationType RepType { get; set; }
    }
}
