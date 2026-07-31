using api.Models;
using api.Dtos.Stock;

namespace api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stockModel) // Get Stocks
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
    
    public static StockWithCommentsDto ToStockWithCommentsDto(this Stock stockModel) // Get Stocks with Comments
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

    public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockModel) // Post Stock WITHOUT Comments
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

    public static Stock ToStockFromCreateDto(this CreateStockRequestWithCommentsDto stockModel) // Post Stock WITH Comments
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
    
    public static Stock ToStockFromUpdateDto(this UpdateStockRequestDto stockModel) // Update Stock 
    {
        return new Stock
        {
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap,
         };
    }
}   