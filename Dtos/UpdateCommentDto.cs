using Reddit.Model;

namespace MyNamespace
{
    public class UpdateCommentDto
    {
        public string ObjectId { get; set; }
        public string UserId { get; set; }
        public TypeEnum Type { get; set; }
    }
}