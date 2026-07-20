using api.Models;
using api.Dtos.Stock;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap
        };
    }
    
    public static StockWithCommentsDto ToStockWithCommentsDto(this Stock stockModel)
    {
        return new StockWithCommentsDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,

            Comments = stockModel.Comments
                .Select(c => c.ToCommentDto())
                .ToList()
        };
    }

    public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockModel) // WITHOUT comments
    {
        return new Stock
        {
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap
        };
    }

    public static Stock ToStockFromCreateDto(this CreateStockRequestWithCommentsDto stockModel) // WITH comments
    {
        return new Stock
        {
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,

            Comments = stockModel.Comments
                .Select(c => c.ToCommentFromCreateDto())
                .ToList()
        };
    }
}   