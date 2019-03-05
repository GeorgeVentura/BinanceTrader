/*
 *    @author: George Ventura
 *    @file:   Tick 1.cs
 * 
 *    @description: This class holds data of the negative paths found.
 */

using System;

namespace BinanceTrader
{
  // todo: make this into a class??
  public struct Tick
  {
    public string Symbol;
    public string BaseAsset;
    public string QuoteAsset;
    public double AskPrice;
    public double AskQuanity;
    public double BidPrice;
    public double BidQuanity;
    public Int64  EventTime;
    public DateTime TimeStamp;

    private DateTime ConvertEventTime()
    {
      var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return dt.AddMilliseconds(EventTime);
    }
    
    public override string ToString()
    {
      // todo: use stringbuilder instead??
      return "[" + TimeStamp.ToString("MM/dd/yyyy-H:mm:ss.fff") + "]" +
//             "[" + TimeStamp.ToUniversalTime().Subtract(
//                                                        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//                                                       ).TotalMilliseconds + "]" +
//             EventTime + "," +
             ConvertEventTime().ToString("MM/dd/yyyy-H:mm:ss.fff") + "," + 
             Symbol + "," +
             BaseAsset + "," +
             QuoteAsset + "," +
             AskPrice + "," +
             AskQuanity + "," +
             BidPrice + "," +
             BidQuanity + ",";
    }
  }
}