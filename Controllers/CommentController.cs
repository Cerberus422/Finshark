using api.Data;
using api.Mappers;
using api.Dtos.Comment;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{

    private readonly ApplicationDBContext _context;
    private readonly ICommentRepository _commentRepo;
    public CommentController(ApplicationDBContext context, ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
        _context = context;
    }
    
    //Put
    [HttpPut("stock/{stockid}/comment/{commentid}")]
    public async Task<IActionResult> UpdateComments([FromRoute] int stockid, [FromRoute] int commentid, [FromBody] UpdateCommentsRequestDto updateDto) // Updating Comments by Id of Stock and Comment
    {

        var commentModel =
            await _commentRepo.UpdateAsync(stockid, commentid, updateDto);
        
        if (commentModel == null)
        {
            return NotFound();
        }
        
        return Ok(commentModel.ToCommentDto());
    }
    
    // Delete
    [HttpDelete("stock/{stockid}")]
    public async Task<IActionResult> DeleteAllComments([FromRoute] int stockid) // Deletes all Comments from one Stock ( leaves stock without any comments)
    {
        var commentModel = await _commentRepo.DeleteByStockIdAsync(stockid);
        
        if (commentModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpDelete("stock/{stockid}/comment/{commentid}")]
    public async Task<IActionResult> DeleteOneComment([FromRoute] int stockid, int commentid) // Deletes one Comment from one Stock ( leaves stock with rest comments)
    {
        var commentModel = await _commentRepo.DeleteByStockAndCommentIdAsync(stockid, commentid);
            
        
        if (commentModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }
    
}