using System;
using System.Collections.Generic;
using BinanceTrader.Com;

namespace BinanceTrader.Exchange
{
  public class ExchangeSymbol
  {
    public string Symbol { get; }
    public string BaseAsset { get; }
    public string QuoteAsset { get; }
    public bool   IceBergAllowed { get; }
    public string Status { get; }
    public Dictionary<string, Dictionary<string, string>> Filters { get; }

    
    
    // symbol filter types
    public const string PRICE_FILTER = "PRICE_FILTER";
    public const string PRECENT_PRICE = "PERCENT_PRICE";
    public const string LOT_SIZE = "LOT_SIZE";
    public const string MIN_NOTIONAL = "MIN_NOTIONAL";
    public const string ICEBERG_PARTS = "ICEBERG_PARTS";
    public const string MARKET_LOT_SIZE = "MARKET_LOT_SIZE";
    public const string MAX_NUM_ORDERS = "MAX_NUM_ORDERS";
    public const string MAX_NUM_ICBERG_ORDERS = "MAX_NUM_ICEBERG_ORDERS";


    public ExchangeSymbol(SymbolObject obj)
    {
      Symbol      = obj.Symbol;
      BaseAsset   = obj.BaseAsset;
      QuoteAsset  = obj.QuoteAsset;
      IceBergAllowed = obj.IceBergAllowed;
      Status = obj.Status;
      Filters = new Dictionary<string, Dictionary<string, string>>();
      SetFilters(obj.Filters);
    }

    private void SetFilters(Dictionary<string, string>[] filters)
    {
      for (int i = 0; i < filters.Length; i++)
      {
        var filterType = filters[i]["filterType"];
        Filters.Add(filterType, new Dictionary<string, string>());
        var keys = filters[0].Keys;
        filters[i].Remove("filterType");
        foreach (var item in filters[i])
        {
          Filters[filterType].Add(item.Key, item.Value);
        }

      }
    }

    // quote value
    public double GetMinOrderValue()
    {
      return Convert.ToDouble(Filters[MIN_NOTIONAL]["minNotional"]);
    }

    // base value
    public double GetMinTradeAmount()
    {
      return Convert.ToDouble(Filters[LOT_SIZE]["minQty"]);
    }
    
  }
}