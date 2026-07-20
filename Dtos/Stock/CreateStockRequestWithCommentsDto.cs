using api.Dtos.Comment;

namespace api.Dtos.Stock;

public class CreateStockRequestWithCommentsDto
{
   
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Purchase { get; set; }
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }
    public List<CreateCommentRequestDto> Comments { get; set; } = new List<CreateCommentRequestDto>();
}