using System.Collections.Generic;

namespace EfCoreRelationships.Models
{
    // One to Many relationships
    public class Box
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BoxItem> Items { get; set; }
    }

    public class BoxItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int BoxId { get; set; }
        public Box Box { get; set; }
    }
}