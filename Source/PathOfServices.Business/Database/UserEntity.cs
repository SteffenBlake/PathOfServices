using System.Collections.Generic;

namespace PathOfServices.Business.Database
{
    public class UserEntity : EntityBase
    {
        public string Name { get; set; }
        public List<OrderEntity> Orders { get; set; }
    }
}
