using System;
using System.Collections.Generic;
using System.Text;
using BinanceTrader.Exchange;
using BinanceTrader.Graph;

namespace BinanceTrader.Order
{
  public class MultiOrder
  {
    private SingleOrder[] orders;
    public DateTime TimeStamp { get; }
    private readonly ExchangeState _exchangeState;

    public double GetStartValue()
    {
      return orders[0].StartVolume;
    }

    public double GetEndValue()
    {
      return orders[orders.Length - 1].TargetVolume;
    }

    public double NetProfit
    {
      get { return orders[orders.Length - 1].TargetVolume - orders[0].StartVolume; }
    }

    public MultiOrder(double startValue,
                       List<SingleOrder> orders,
                       DateTime timeStamp,
                       ExchangeState exchangeState)
    {
      this.orders = orders.ToArray();
      TimeStamp = timeStamp;
      _exchangeState = exchangeState;
      orders[0].StartVolume = startValue;   // set start value
//      NormalizeVolume3();
//      NormalizeVolume4();
    }

    // test prototype
    private void BackTrack(int index)
    {
      for (int i = index-1; i >= 0; i--)
      {
        if (orders[i].Direction == Edge.Direction.BID)
        {
          orders[i].BaseVolume = orders[i + 1].BaseVolume;
        }
        else if (orders[i].Direction == Edge.Direction.ASK)
        {
          orders[i].QuoteVolume = orders[i + 1].QuoteVolume;
        }
      }
    }
    
    
    private void BackTrack2(int startIndex)
    {
      for (int i = startIndex-1; i >= 0; i--)
      {
        // Bid
        if (orders[i].Direction == Edge.Direction.BID)
        {
          if (orders[i + 1].Direction == Edge.Direction.ASK) //ask
            orders[i].BaseVolume = orders[i + 1].BaseVolume;
          // Bid->Bid
          if (orders[i + 1].Direction == Edge.Direction.BID)
            orders[i].BaseVolume = orders[i + 1].QuoteVolume;
        }


        // Ask
        if (orders[i].Direction == Edge.Direction.ASK)
        {
          if (orders[i + 1].Direction == Edge.Direction.BID) //bid
            orders[i].QuoteVolume = orders[i + 1].QuoteVolume;
          // Ask->Ask
          if (orders[i + 1].Direction == Edge.Direction.ASK) //ask
            orders[i].QuoteVolume = orders[i + 1].BaseVolume;
          //orders[i].BaseValue = orders[i - 1].QuoteValue;
        }
      }
    }

    // test prototype
    public void Normalize()
    {
      if (orders[0].BaseVolume > orders[0].MaxVolume)
        orders[0].BaseVolume = orders[0].MaxVolume;

      for (int i = 1; i < orders.Length; i++)
      {
        if (orders[i - 1].Direction == Edge.Direction.BID)
        {
          orders[i].BaseVolume = orders[i - 1].BaseVolume;
          if (orders[i].BaseVolume > orders[i].MaxVolume)
          {
            //back track
            orders[i].BaseVolume = orders[i].MaxVolume;
            BackTrack(i);
            Console.WriteLine("backtrack-Bid");
          }
        }
        else if (orders[i - 1].Direction == Edge.Direction.ASK)
        {
          orders[i].QuoteVolume = orders[i - 1].QuoteVolume;
          if (orders[i].BaseVolume > orders[i].MaxVolume)
          {
            //back track
            orders[i].BaseVolume = orders[i].MaxVolume;
            BackTrack(i);
            Console.WriteLine("backtrack-Ask");
          }
        }
      }
    }
    
       public void NormalizeVolume4()
    {
      if (orders[0].BaseVolume > orders[0].MaxVolume)
        orders[0].BaseVolume = orders[0].MaxVolume;
      
      for (int i = 1; i < orders.Length; i++)
      {
//        Console.WriteLine(this.ToString());
        if (orders[i - 1].Direction == Edge.Direction.BID)
        {
          
          if (orders[i].Direction == Edge.Direction.ASK) // Bid->Ask
          {
            orders[i].BaseVolume = orders[i - 1].BaseVolume;
//            Console.WriteLine("Bid->Ask");
          } //

          if (orders[i].Direction == Edge.Direction.BID) // Bid->Bid
          {
            orders[i].QuoteVolume = orders[i - 1].BaseVolume;
//            Console.WriteLine("Bid->Bid");
          } //
        }
        //---------------------------------------------------

        if (orders[i - 1].Direction == Edge.Direction.ASK)
        {
          if (orders[i].Direction == Edge.Direction.BID) // Ask->Bid
          {
//            Console.WriteLine("Ask->Bid");
            orders[i].QuoteVolume = orders[i - 1].QuoteVolume;
          }
          //Ask->Ask
          if (orders[i].Direction == Edge.Direction.ASK)
          {
//            Console.WriteLine("ask->ask");
            orders[i].BaseVolume = orders[i - 1].QuoteVolume;
              
          }
        }
      }
    }
    
    public override string ToString()
    {
      var str = new StringBuilder();
      foreach (var order in orders)
      {
        str.Append(order.ToString() + "\n");
      }

      return str.ToString();
    }
    
    
  }
}
