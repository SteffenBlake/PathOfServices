using System.Collections.Generic;

namespace PathOfServices.Business.Database
{
    public class ServiceCategoryEntity : EntityBase
    {
        public string Name { get; set; }

        public List<ServiceEntity> Services { get; set; }
    }
}
