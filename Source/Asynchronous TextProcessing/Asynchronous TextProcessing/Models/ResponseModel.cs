namespace Asynchronous_TextProcessing.Models
{
    public class CheckResponseBaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string State { get; set; }
    }
    public class TextProcessResponseModel : CheckResponseBaseModel
    {
        public string Text { get; set; }
    }
    public class UserResponseModel
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
