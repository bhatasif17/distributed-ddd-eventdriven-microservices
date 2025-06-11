namespace Wakett.Application.Commands;

public class AddPositionCommand
{
    public string InstrumentId { get; set; }
    public decimal Quantity { get; set; }
    public decimal InitialRate { get; set; }
    public int Side { get; set; }
    public decimal CurrentProfitLoss { get; set; }
}