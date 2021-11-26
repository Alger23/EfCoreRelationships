using System.Collections.Generic;

namespace EfCoreRelationships.Models
{
    // one to many relationships
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Context { get; set; }

        public Blog Blog { get; set; }
    }
}