using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

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

    // Get
    [HttpGet]
    public async Task<IActionResult> GetAll() // Get all Comments
    {
        var comments = await _commentRepo.GetAllAsync();

        var commentDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentDto);
    }
    
    
    [HttpGet("{commentId}")] // Get Comment by Id
    public async Task<IActionResult> GetById([FromRoute] int commentId)
    {
        var comment = await _commentRepo.GetByIdAsync(commentId);

        if (comment == null)
        {
            return NoContent();
        }

        return Ok(comment.ToCommentDto());
    }
    
    
    // Post
    [HttpPost("{stockId}")] // Post Comment by StockId
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
    {
        if (!await _stockRepo.StockExists(stockId))
        {
            return BadRequest("Stock does not exist");
            
        }
        
        var commentModel = commentDto.ToCommentFromCreate(stockId);

        await _commentRepo.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { commentId = commentModel.Id }, commentModel.ToCommentDto());

    }
    
    // Update
    [HttpPut("{Id}")] // Update Comment by Id
    public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateCommentRequestDto updateDto)
    {
        var comment = await _commentRepo.UpdateAsync(Id, updateDto.ToCommentFromUpdate());
        
        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto());
    }
    
    // Delete
    [HttpDelete("{Id}")] // Delete Comment by Id
    public async Task<IActionResult> Delete([FromRoute] int Id)
    {
        var comment = await _commentRepo.DeleteAsync(Id);

        if (comment == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}