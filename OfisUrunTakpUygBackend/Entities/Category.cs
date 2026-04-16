using Entities;

namespace OfisUrunTakip.WebApi.Entity
{
    public class Category : EntityBase
    {

        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
