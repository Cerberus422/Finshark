namespace api.Dtos.Comment;

public class UpdateCommentsRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}