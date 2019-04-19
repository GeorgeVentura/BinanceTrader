/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   Controller.cs
 * 
 *    
 */

using System;
using BinanceTrader.Com;
using BinanceTrader.Exchange;
using BinanceTrader.Graph;


namespace BinanceTrader
{
  public class Controller
  {

    private GraphNet graph;
    private ExchangeState exchangeState;
    private ExchangeConnecter exchangeConnection;
    private Parser parser;
    
    public Controller(string startAsset)
    {
      exchangeState = new ExchangeState();
      exchangeConnection = new ExchangeConnecter(exchangeState);
      parser = new Parser(exchangeState);
      
      exchangeState.InitExchange();
      exchangeConnection.InitValues();
      
      graph = new GraphNet(exchangeState);
      
      exchangeConnection.Start();
      
      var orderGen = new OrderGenerator(graph, exchangeState);

      // todo: put this in a method
      while (true)
      {
        try
        {
          var tick = parser.ParseData(exchangeConnection.GetRawData());
          UpdateEdge(tick);
          if (exchangeConnection.isEmpty())
          {
            var path = graph.BellmanFord(startAsset);
            if (path.Count > 0 && path[0] != "NA" && path[0] == startAsset)
            {
              var order = orderGen.GenerateOrder(path, 2.0);
              var we = graph.GetSumEdgeWeight(path);
              Console.WriteLine("-----------------------");
              Console.WriteLine(order);
              Console.WriteLine("Profit Rate: " + Math.Pow(10, -we)*1);

            }
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }
    }

    private void UpdateEdge(Tick tick)
    {
      graph.UpdateEdge(tick.BaseAsset,
                        tick.QuoteAsset,
                        new Tuple<double, double>(tick.AskPrice, tick.AskQuanity));
      
      graph.UpdateEdge(tick.QuoteAsset, 
                       tick.BaseAsset,
                       new Tuple<double, double>(tick.BidPrice, tick.BidQuanity));
    }
    
    
  }
}