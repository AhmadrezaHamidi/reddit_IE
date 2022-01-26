namespace Reddit.Model
{
     public class Comment
    {
        public Comment(string id, string objectId, string userId, TypeEnum type)
        {
            Id = Guid.NewGuid().ToString();
            ObjectId = objectId;
            UserId = userId;
            Type = type;
        }

        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string UserId { get; set; }
        public TypeEnum Type { get; set; }
    }
}