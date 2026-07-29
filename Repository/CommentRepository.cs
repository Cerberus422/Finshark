using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;

    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetByIdAsync(int commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int stockid, int commentid, UpdateCommentsRequestDto updateDto)
    {
        var commentModel = 
            await _context.Comments
                .FirstOrDefaultAsync(c => c.StockId == stockid && c.Id == commentid);
        
        if (commentModel == null)
        {
            return null;
        }
        
        commentModel.Title = updateDto.Title;
        commentModel.Content = updateDto.Content;
        commentModel.CreatedOn = updateDto.CreatedOn;
        
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<List<Comment>?> DeleteByStockIdAsync(int stockid)
    {
        var commentModel = await _context.Comments
            .Where(c => c.StockId == stockid)
            .ToListAsync();

        
        if (commentModel.Count == 0)
        {
            return null;
        }

        _context.Comments.RemoveRange(commentModel);

        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> DeleteByStockAndCommentIdAsync(int stockid, int commentid)
    {
        var commentModel = await _context.Comments
            .FirstOrDefaultAsync(c => c.StockId == stockid && c.Id == commentid);
        
        if (commentModel == null)
        {
            return null;
        }

        _context.Comments.Remove(commentModel);

        await _context.SaveChangesAsync();

        return commentModel;
    }
}