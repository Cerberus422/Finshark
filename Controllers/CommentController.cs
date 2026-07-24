using api.Data;
using Microsoft.AspNetCore.Mvc;

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
    
    // Delete
    [HttpDelete("{stockid}")]
    public IActionResult DeleteAllComments([FromRoute] int stockid) // Deletes all Comments from one Stock ( leaves stock without any comments)
    {
        var commentModel = _context.Comments
            .Where(c => c.StockId == stockid)
            .ToList();

        
        if (commentModel.Count == 0)
        {
            return NotFound();
        }

        _context.Comments.RemoveRange(commentModel);

        _context.SaveChanges();

        return NoContent();
    }
    
    [HttpDelete("{stockid}/{commentid}")]
    public IActionResult DeleteOneComment([FromRoute] int stockid, int commentid) // Deletes one Comment from one Stock ( leaves stock with rest comments)
    {
        var commentModel = _context.Comments
            .FirstOrDefault(c => c.StockId == stockid && c.Id == commentid);
        
        if (commentModel == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(commentModel);

        _context.SaveChanges();

        return NoContent();
    }
    
}