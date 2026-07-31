using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    // Get
    Task<Comment?> GetByIdAsync(int commentId); // Getting comment By Id
    // Create
    Task<Comment> CreateAsync(Comment commentModel); // Creating Comment 
    // Put
    Task<Comment?> UpdateAsync(int commentid, Comment updateDto); // Updating Comments by Id of Stock and Comment
    
    // Delete
    Task<List<Comment>?> DeleteByStockIdAsync(int stockid);  // Deletes all Comments from one Stock ( leaves stock without any comments)
    Task<Comment?> DeleteByCommentIdAsync( int commentid);// Deletes one Comment ( leaves stock with rest comments)
}