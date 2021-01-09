using System;
using System.ComponentModel.DataAnnotations;

namespace PathOfServices.Business.Database
{
    public class EntityBase
    {
        [Key] 
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
