using System;

namespace Candal.Models
{
    public class Product
    {
        //[Column("Name")]
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime Validation { get; set; }
    }
}
