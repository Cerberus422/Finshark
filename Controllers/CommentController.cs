using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]

public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;

    public CommentController(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepo.GetAllAsync();

        var commentDto = comments.Select(c => c.ToCommentDto());

        return Ok(commentDto);
    }
    
    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetById([FromRoute] int commentId)
    {
        var comment = await _commentRepo.GetByIdAsync(commentId);

        if (comment == null)
        {
            return NoContent();
        }

        return Ok(comment.ToCommentDto());
    }
}