using Boc.Domain;

namespace Boc.Commands
{
    public class Trade : Command
    {
        public string SecurityId { get; set; }
        public TradeDirection TradeDirection { get; set; }
        public Money Notional { get; set; }
    }
}
