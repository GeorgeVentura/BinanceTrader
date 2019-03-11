/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   SymbolObject.cs
 * 
 *    
 */

using System.Collections.Generic;

namespace BinanceTrader.Com
{
  public class SymbolObject
  {
    public string                       Symbol;
    public string                       Status;
    public string                       BaseAsset;
    public string                       QuoteAsset;
    public string[]                     OrderTypes;
    public bool                         IceBergAllowed;
    public Dictionary<string, string>[] Filters;
  }
}