using System;

namespace Model
{
    public class Post
    {
        public Post(string communityName, string author)
        {
            Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
            this.IsDeleted = false;
            this.CommunityName = communityName;
            this.Author = author;
        }

        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string CommunityName { get; set; }
        public string Author { get; set; }

        public override string ToString() => Id + "\t" + CommunityName + "\t" + Author;
    }
}