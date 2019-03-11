/*
/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   Edge.cs
 * 
 *    
 */

//todo: add last updated timestamp

using System;

namespace BinanceTrader.Graph
{
  public class Edge
  {
    public string Orgin { get; }
    public string Dest { get; }
    private double _weight;
    public double Volume { get; set; }
    public Direction EdgeDirection { get; }
    
    //debug
    public double actualWeight;
    
    // todo: add normalized weight

    public double Price
    {
      get => Math.Pow(10, _weight);
    }
    
    public double Weight
    {
      get => _weight;
      set
      {
        if (EdgeDirection == Direction.BID)
        {
          actualWeight = value;
          _weight = -Math.Log10(value);
        }
        else
        {
          actualWeight = value;
          _weight = Math.Log10(value);
        }
      }
    }

    public enum Direction
    {
      BID, ASK
    }

    public Edge(string origin, string dest,
                double weight, double volume,
                Direction direction)
    {
      this.Orgin = origin;
      this.Dest = dest;
      this.Volume = volume;
      this.EdgeDirection = direction;
      this.Weight = weight;
    }

    public string GetSide()
    {
      if (EdgeDirection == Direction.BID)
        return "BUY";
      if (EdgeDirection == Direction.ASK)
        return "SELL";
      return "NONE";
    }

    public override string ToString()
    {
      return Orgin + "=>" + Dest +
             "," + "weight:" + Weight +
             ",volume:" + Volume +
             ",direction:" + GetSide();
    }
    
  }
}