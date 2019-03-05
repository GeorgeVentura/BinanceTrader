using System.Collections.Generic;
using BinanceTrader.Com;

namespace BinanceTrader.Exchange
{
  public class ExchangeState
  {
    public static double EXCHANGE_FEE = 0.001;
    private Dictionary<string, ExchangeSymbol> symbols;
    
    public ExchangeState()
    {
      symbols = new Dictionary<string, ExchangeSymbol>();
    }

    public List<string> GetAllSymbolStrings()
    {
      var symbolStrings = new List<string>();
      foreach (var symbol in symbols.Keys)
      {
        symbolStrings.Add(symbol);
      }
      return symbolStrings;
    }

    public List<ExchangeSymbol> GetAllSymbols()
    {
      var symbolList = new List<ExchangeSymbol>();
      foreach (var symbol in symbols.Values)
      {
        symbolList.Add(symbol);
      }

      return symbolList;
    }

    public void InitExchange()
    {
      var exchangeSymbols = ExchangeConnecter.RequestExchangeState();
      foreach (var symbolObj in exchangeSymbols)
      {
        AddSymbol(symbolObj);
      }
    }

    private void AddSymbol(SymbolObject symbolObj)
    {
      var symbol = new ExchangeSymbol(symbolObj);
      symbols.Add(symbol.Symbol, symbol);
    }

    public ExchangeSymbol GetSymbol(string symbol)
    {
      return symbols[symbol];
    }
    
    public string GetSymbolByAsset(string assetA,
      string assetB)
    {
      assetA = assetA.ToUpper();
      assetB = assetB.ToUpper();
      if (symbols.ContainsKey(assetA + assetB))
        return symbols[assetA + assetB].Symbol;
      if (symbols.ContainsKey(assetB + assetA))
        return symbols[assetB + assetA].Symbol;
      return null;    // todo: add error handling.
    }
  }
}