using System.ComponentModel.DataAnnotations.Schema;

namespace PathOfServices.Business.Database
{
    public class ServiceEntity : EntityBase
    {
        public string Key { get; set; }

        public string Description { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public ServiceCategoryEntity Category { get; set; }
        public string CategoryId { get; set; }
    }
}
