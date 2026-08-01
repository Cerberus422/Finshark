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

   
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;
    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
    }
    //Get
    [HttpGet("stock-with-comments/{commentId:int}")]  // Comments by Id
    public async Task<IActionResult> GetById([FromRoute] int commentId)
    {
        var comment = await _commentRepo.GetByIdAsync(commentId);

        if (comment == null)
        {
            return BadRequest("Comment does not exist");
        }
        
        return Ok(comment.ToCommentDto());
    }
    // Creaete
    [HttpPost("write-comment/{stockId:int}")] // Posting new Comment to existing Stock
    public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
    {
        if (!await _stockRepo.StockExists(stockId))
        {
            return BadRequest("Stock does not exist");
        }
        
        var commentModel = commentDto.ToCommentFromCreateDto(stockId);

        await _commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { commentId = commentModel.Id }, commentModel.ToCommentDto());
    }
    
    //Put
    [HttpPut("comment/{commentid:int}")] // Updating Comments 
    public async Task<IActionResult> UpdateComments([FromRoute] int commentid, [FromBody] UpdateCommentsRequestDto updateDto) 
    {

        var commentModel =
            await _commentRepo.UpdateAsync(commentid, updateDto.ToCommentFromUpdateDto());
        
        if (commentModel == null)
        {
            return NotFound();
        }
        
        return Ok(commentModel.ToCommentDto());
    }
    
    // Delete
    [HttpDelete("stock/{stockid:int}")] // Deletes all Comments from one Stock ( leaves stock without any comments)
    public async Task<IActionResult> DeleteAllComments([FromRoute] int stockid) 
    {
        var commentModel = await _commentRepo.DeleteByStockIdAsync(stockid);
        
        if (commentModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpDelete("comment/{commentid:int}")] // Deletes one Comment ( leaves stock with rest comments)
    public async Task<IActionResult> DeleteOneComment([FromRoute]  int commentid) 
    {
        var commentModel = await _commentRepo.DeleteByCommentIdAsync(commentid);
            
        
        if (commentModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }
    
}