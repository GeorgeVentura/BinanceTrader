/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   SingleOrder.cs
 * 
 *    
 */

using System;
using BinanceTrader.Graph;

namespace BinanceTrader.Order
{
  public class SingleOrder
  {
    public string Origin { get; }
    public string Dest { get; }
    public double Weight { get; }
    public double Rate { get; }
    public double MaxVolume { get; }
    public Edge.Direction Direction { get; }
    public string Symbol;

    private double _baseVolume;
    private double _quoteVolume;

    public double BaseVolume
    {
      get
      {
        return _baseVolume;
      }
      set
      {
        _baseVolume = value;
        _quoteVolume = Rate / _baseVolume;
      }
    }

    public double QuoteVolume
    {
      get
      {
        return _quoteVolume;
      }
      set
      {
        _quoteVolume = value;
        _baseVolume = _quoteVolume * Rate;
      }
    }

    public double TargetVolume
    {
      get
      {
        if (Direction == Edge.Direction.BID)
          return BaseVolume;
        if (Direction == Edge.Direction.ASK)
          return QuoteVolume;
        return 0.0;
      }
      set
      {
        if (Direction == Edge.Direction.BID)
          BaseVolume = value;
        else if (Direction == Edge.Direction.ASK)
          QuoteVolume = value;
      }
    }

    public double StartVolume
    {
      get
      {
        if (Direction == Edge.Direction.BID)
          return QuoteVolume;
        if (Direction == Edge.Direction.ASK)
          return BaseVolume;
        return 0.0;
      }
      set
      {
        if (Direction == Edge.Direction.BID)
          QuoteVolume = value;
        else if (Direction == Edge.Direction.ASK)
          BaseVolume = value;
      }
    }

    public SingleOrder(string origin,
                        string dest,
                        double weight,
                        double maxVolume,
                        Edge.Direction direction)
    {
      Origin = origin;
      Dest = dest;
      Direction = direction;
      Weight = weight;
      Rate = GetNormalizedWeight(weight);
      MaxVolume = maxVolume;
    }

    private double GetNormalizedWeight(double weight)
    {
      if (Direction == Edge.Direction.BID)
        return Math.Pow(10, -weight);
      else
        return Math.Pow(10, weight);
    }
    
    public override string ToString()
    {
      return Origin + "=>" + Dest + ", price:" + Rate + 
             ", maxvol:" + MaxVolume + ", basevol:" + BaseVolume + 
             ", quotevol:" + QuoteVolume + ", direction:" + Direction;
    }
  }
}
