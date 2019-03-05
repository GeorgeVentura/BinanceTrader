using System;
using System.Collections.Generic;
using System.IO;
using cTrader.Com;
using cTrader.Exchange;

namespace cTrader.Graph
{
  public class GraphUtil
  {

    public static void WriteGraphToFile(string fileName, GraphNet graph)
    {
      var edgeListStr = GenerateGraphString(graph);
      File.WriteAllText(fileName, string.Join("", edgeListStr.ToArray()));
    }

    private static List<string> GenerateGraphString(GraphNet graph)
    {
      var edgeStrList = new List<string>();
      var edges = graph.GetAllEdges();
      foreach (var edge in edges)
      {
        edgeStrList.Add(edge.ToString() + "\n");
      }

      return edgeStrList;
    }

    public static GraphNet GenerateGraphFromFile(string fileName)
    {
      var state = CreateExchangeState();
      state.InitExchange();
      var graph = new GraphNet(state);
      var fileContents = File.ReadAllLines(fileName);
      foreach (var line in fileContents)
      {
        var contents = line.Split(",");
        var nodes = contents[0].Split("=>");
        var _weight = Convert.ToDouble(contents[1].Substring(7));
        var volume = Convert.ToDouble(contents[2].Substring(7));
        var side = contents[3].Substring(10);
        double weight = 0;
        if (side == "BUY")
        {
          weight = -Math.Pow(10, _weight);
        }
        if (side == "SELL")
        {
          weight = Math.Pow(10, _weight);
        }
        graph.UpdateEdge2(nodes[0], nodes[1], weight, volume);
      }

      return graph;
    }

    public static ExchangeState CreateExchangeState()
    {
      return new ExchangeState();
    }
    
  }
}