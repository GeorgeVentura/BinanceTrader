using System;
using System.Collections.Generic;
using BinanceTrader.Exchange;
using BinanceTrader.Graph;
using BinanceTrader.Order;
using BinanceTrader.Com;

namespace BinanceTrader
{
  public class OrderGenerator
  {
    private GraphNet        graph;
    private readonly ExchangeState _exchangeState;

    public OrderGenerator(GraphNet graph, ExchangeState exchangeState)
    {
      this.graph = graph;  // todo: make read only
      _exchangeState = exchangeState;
    }
    
    public MultiOrder GenerateOrder(List<String> path, double startAmount)
    {
      var timestamp = DateTime.UtcNow;
      var orders = new List<SingleOrder>();
        
      for (var i = 1; i < path.Count; i++)
      {
        var edge = graph.GetEdge(path[i - 1], path[i]);
        var origin = edge.Orgin;
        var dest = edge.Dest;
        var direction = edge.EdgeDirection;
        var vol = edge.Volume;
        var weight = edge.Weight;
        
        var order = new SingleOrder(origin, dest, weight,
                                    vol, direction);
        orders.Add(order);
      }
      return new MultiOrder(startAmount, orders, timestamp, _exchangeState);
    }
    
  }
}