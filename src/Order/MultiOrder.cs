/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   MultiOrder.cs
 * 
 *    
 */

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
