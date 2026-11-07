using System.Text.Json.Serialization;

namespace UserManager.DTOs
{
    public class UserUpdateDto : UserBaseDto
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
