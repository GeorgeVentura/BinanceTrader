/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   GraphNet.cs
 * 
 *    
 */

using System;
using System.Collections.Generic;
using BinanceTrader.Exchange;

namespace BinanceTrader.Graph
{
  public class GraphNet
  {

    private Dictionary<string, Dictionary<string, Edge>> graph;
    private readonly ExchangeState _exchangeState;
    
    public GraphNet(ExchangeState exchangeState)
    {
      graph = new Dictionary<string, Dictionary<string, Edge>>();
      _exchangeState = exchangeState;
      
      //initlize the graph
      var symbols = _exchangeState.GetAllSymbols();
      foreach (var symbol in symbols)
      {
        AddEdge(symbol.BaseAsset, symbol.QuoteAsset,
          new Tuple<double, double>(123.123, 0), 
          new Tuple<double, double>(123.123, 0));
      }
    }

    //debug
    public double GetSumEdgeWeight(List<string> path)
    {
      var sum = 0.0;

      for (int i = 1; i < path.Count; i++)
      {
        var edge = GetEdge(path[i - 1], path[i]);
//        Console.Write(edge.ToString());
        sum += edge.Weight;
      }
      
      return sum;
    }
    
    //debug
    public double GetSumActEdgeRate(List<string> path)
    {
      var sum = 1.0;

      for (int i = 1; i < path.Count; i++)
      {
        var edge = GetEdge(path[i - 1], path[i]);
        if (edge.EdgeDirection == Edge.Direction.BID)
          sum *= edge.actualWeight;
        if (edge.EdgeDirection == Edge.Direction.ASK)
          sum *= 1 / edge.actualWeight;
      }
      
      return sum;
    }
    
    // helper function for AddEdge()
    private void AddNode(string node)
    {
      if (!graph.ContainsKey(node))
      {
        graph.Add(node, new Dictionary<string, Edge>());
      }
    }
    
    public void UpdateEdge2(string u, string v, double weight, double volume)
    {
      if (graph.ContainsKey(u) && graph[u].ContainsKey(v))
      {
        graph[u][v].Weight = weight;
        graph[u][v].Volume = volume;
      }
      else
      {
        Console.WriteLine("Edge not found");  // todo: error handling
      }
    }
    
    public void UpdateEdge(string u, string v, Tuple<double, double> priceData)
    {
      if (graph.ContainsKey(u) && graph[u].ContainsKey(v))
      {
        graph[u][v].Weight = priceData.Item1;
        graph[u][v].Volume = priceData.Item2;
      }
      else
      {
        Console.WriteLine("Edge not found");  // todo: error handling
      }
    }
    
    public Edge GetEdge(string u, string v)
    {
      return graph[u][v];
    }

    public List<Edge> GetAllEdges()
    {
      var edgeList = new List<Edge>();
      foreach (var nodeA in graph.Keys)
      {
        foreach (var nodeB in graph[nodeA].Keys)
        {
          edgeList.Add(graph[nodeA][nodeB]);
        }
      }

      return edgeList;
    }

    private void AddEdge(string baseAsset, string quoteAsset,
      Tuple<double, double> bid,
      Tuple<double, double> ask)
    {
      AddNode(baseAsset);
      AddNode(quoteAsset);

      if (!graph[baseAsset].ContainsKey(quoteAsset) || !graph[quoteAsset].ContainsKey(baseAsset))
      {
        // TODO URGENT, Bid and Ask are backwards.
        graph[baseAsset][quoteAsset] = new Edge(baseAsset, quoteAsset, ask.Item1, ask.Item2, Edge.Direction.ASK);
        graph[quoteAsset][baseAsset] = new Edge(quoteAsset, baseAsset, bid.Item1, bid.Item2, Edge.Direction.BID);
      }
    }
    
       // Cycle detection algorithm 
    public List<string> BellmanFord(string src)
    {
      var dist = new Dictionary<string, double>();
      var pred = new Dictionary<string, string>();

      foreach (var node in graph.Keys)
      {
        dist.Add(node, int.MaxValue);
        pred.Add(node, null);
      }
      dist[src] = 0;
      
      // Relax all edge |V| - 1 times
      for (int i = 0; i < graph.Count-1; i++)
      {
        foreach (var u in graph.Keys)
        {
          foreach (var v in graph[u].Keys)
          {
            Relax(u, v, dist, pred);
          }
        } 
      }

      // Detect negative cycles
      foreach (var u in graph.Keys)
      {
        foreach (var v in graph[u].Keys)
        {
          if (dist[v] < dist[u] + graph[u][v].Weight)
          {
            return RetraceLoop(pred, src);
          }
        }
      }
      // todo: change this temp return, if no cycle if found.
      var temp = new List<string>();
      temp.Add("NA");
      return temp;
    }
    
    private void Relax(string node, string neighbour,
                       Dictionary<string, double> d,
                       Dictionary<string, string> p)
    {
      var dist = graph[node][neighbour];
      if (d[neighbour] > d[node] + dist.Weight)
      {
        d[neighbour] = d[node] + dist.Weight;
        p[neighbour] = node;
      }
    }
    
    private List<string> RetraceLoop(Dictionary<string, string> pred, string src)
    {
      var arbitrageLoop = new List<string>();
      arbitrageLoop.Add(src);
      var prevNode = src;
      while (true)
      {
        prevNode = pred[prevNode];
        if (!arbitrageLoop.Contains(prevNode))
        {
          arbitrageLoop.Add(prevNode);
        }
        else
        {
          arbitrageLoop.Add(prevNode);
          arbitrageLoop = arbitrageLoop.GetRange(arbitrageLoop.IndexOf(prevNode),
                                                 arbitrageLoop.Count - arbitrageLoop.IndexOf(prevNode));
          arbitrageLoop.Reverse();
          return arbitrageLoop;
        }
      }
    }
    
  }
}