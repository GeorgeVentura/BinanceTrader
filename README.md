# BinanceTrader

##### A cryptocurrency trading bot written in C#, which takes advantage of arbitrage opportunities within the Binance Exchange Platform.

###
###

This bot uses the Bellman–Ford algorithm to find a negative path cycle of a given currency token.

![](https://www.georgeventura.com/assets/trading_2.png)

##### Usage:

```cs
    // @file: Program.cs
    
    string startAsset = "XRP";   // your start and end currency
    var controller = new Controller(startAsset);
```

##### Build
```$
    $ dotnet build
```

##### Run
```$
    $ dotnet cTrader.dll 
```


##### Output:
![](https://s3.amazonaws.com/null001/binance_output.png)
##### Dependencies
  - NetCore 2.0+
  - websocket-sharp https://github.com/sta/websocket-sharp



