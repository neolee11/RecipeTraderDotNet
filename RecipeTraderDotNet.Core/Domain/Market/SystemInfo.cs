namespace RecipeTraderDotNet.Core.Domain.Market
{
    public class SystemInfo
    {
        public int TotalUsers { get; set; }
        public decimal TotalCurrency { get; set; }

        public override string ToString()
        {
            return $"Total System Users : {TotalUsers}\nTotal System Balance : {TotalCurrency}";
        }
    }
}