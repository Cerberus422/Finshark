using api.Data;
using api.Mappers;
using api.Dtos.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{

    private readonly ApplicationDBContext _context;
    public CommentController(ApplicationDBContext context)
    {
        _context = context;
    }
    
    //Put
    [HttpPut("stock/{stockid}/comment/{commentid}")]
    public async Task<IActionResult> UpdateComments([FromRoute] int stockid, [FromRoute] int commentid, [FromBody] UpdateCommentsRequestDto updateDto) // // Updating Comments by Id of Stock and Comment
    {
      
        var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.StockId == stockid && c.Id == commentid);
        
        if (commentModel == null)
        {
            return NotFound();
        }
        
        commentModel.Title = updateDto.Title;
        commentModel.Content = updateDto.Content;
        commentModel.CreatedOn = updateDto.CreatedOn;
        
        await _context.SaveChangesAsync();
        
        return Ok(commentModel.ToCommentDto());
    }
    
    // Delete
    [HttpDelete("stock/{stockid}")]
    public async Task<IActionResult> DeleteAllComments([FromRoute] int stockid) // Deletes all Comments from one Stock ( leaves stock without any comments)
    {
        var commentModel = await _context.Comments
            .Where(c => c.StockId == stockid)
            .ToListAsync();

        
        if (commentModel.Count == 0)
        {
            return NotFound();
        }

         _context.Comments.RemoveRange(commentModel);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("stock/{stockid}/comment/{commentid}")]
    public async Task<IActionResult> DeleteOneComment([FromRoute] int stockid, int commentid) // Deletes one Comment from one Stock ( leaves stock with rest comments)
    {
        var commentModel = await _context.Comments
            .FirstOrDefaultAsync(c => c.StockId == stockid && c.Id == commentid);
        
        if (commentModel == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(commentModel);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
}