using System;

namespace cTrader.Order
{
  class VolumeQuantity
  {
    private double _quantity;
    public event EventHandler QuantChanged;
    private string Asset { get; }
    public bool IsBase { get; }

    public double Quantity
    {
      get
      {
        return _quantity;
      }
      set
      {
        _quantity = value;
      }
    }

    public VolumeQuantity(string symbol, 
                          double quantity, 
                          bool isBase)
    {
      Asset = symbol;
      Quantity = quantity;
      IsBase = isBase;
    }
  }
}
