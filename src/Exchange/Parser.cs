/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   Parser.cs
 * 
 *    
 */

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BinanceTrader.Exchange
{
  public class Parser
  {
    private readonly ExchangeState _exchangeState;
    
    public Parser(ExchangeState exchangeState)
    {
      _exchangeState = exchangeState;
    }
    
    private string[] GetMatches(string rawString)
    {
      const string pattern = "\"E\":\\d+|\"s\":\"[a-zA-Z]+\"|\"b\":\"\\d+.\\d+\"|\"B\":\"\\d+.\\d+\"|\"a\":\"\\d+.\\d+\"|\"A\":\"\\d+.\\d+\"";
      var matches = Regex.Matches(rawString, pattern)
        .Cast<Match>()
        .Select(m => m.Value)
        .ToArray();
      return matches;
    }
    
    public Tick ParseData(string rawString)
    {
      var matches = GetMatches(rawString);

      var tickStr = new string[6];

      for (int i = 0; i < matches.Length; i++)
      {
        var tempStr = matches[i].Replace("\"", String.Empty)
          .Replace(" ", String.Empty);
        var temp = tempStr.Split(":");
        tickStr[i] = temp[1];
      }
      
      var tick = new Tick();
      tick.EventTime = Convert.ToInt64(tickStr[0]);
      tick.Symbol    = tickStr[1];

      try
      {
        var asset = _exchangeState.GetSymbol(tick.Symbol);
        tick.BaseAsset  = asset.BaseAsset;
        tick.QuoteAsset = asset.QuoteAsset;
      
        tick.BidPrice   = Convert.ToDouble(tickStr[2]);
        tick.BidQuanity = Convert.ToDouble(tickStr[3]);
        tick.AskPrice   = Convert.ToDouble(tickStr[4]);
        tick.AskQuanity = Convert.ToDouble(tickStr[5]);
        tick.TimeStamp  = DateTime.UtcNow;
      
        return tick;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
      
    }
  }
}