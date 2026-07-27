using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    // Put
    Task<Comment?> UpdateAsync(int stockid, int commentid, UpdateCommentsRequestDto updateDto); // Updating Comments by Id of Stock and Comment
    
    // Delete
    Task<List<Comment>?> DeleteByStockIdAsync(int stockid);  // Deletes all Comments from one Stock ( leaves stock without any comments)
    Task<Comment?> DeleteByStockAndCommentIdAsync(int stockid, int commentid);// Deletes one Comment from one Stock ( leaves stock with rest comments)
}