using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<Comment?> GetByIdAsync(int commentId);
    Task<Comment> CreateAsync(Comment commentModel);
}