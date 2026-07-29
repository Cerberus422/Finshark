using api.Models;
using api.Dtos.Comment;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment commentModel) // Get Comments
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn
        };
    }
    
    public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto commentModel) // Post Comment without sockoId
    {
        return new Comment
        {
            Title = commentModel.Title,
            Content = commentModel.Content,
        };
    }
    
    public static Comment ToCommentFromCreateDto(this CreateCommentRequestDto commentModel, int stockId) // Post Comment by stockId
    {
        return new Comment
        {
            Title = commentModel.Title,
            Content = commentModel.Content,
            StockId = stockId
        };
    }
}
