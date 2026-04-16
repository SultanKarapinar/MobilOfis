using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
