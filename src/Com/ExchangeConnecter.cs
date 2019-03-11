/*
 *    @author: George Ventura
 *    @email:  ventura18@pm.me
 *    @file:   ExchangeConnecter.cs
 * 
 *    
 */

using System;
using WebSocketSharp;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BinanceTrader.Exchange;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BinanceTrader.Com
{
  public class ExchangeConnecter
  {
    private const string URL = "wss://stream.binance.com:9443/ws/";

    private readonly ExchangeState _exchangeState;
    public BlockingCollection<string> rawQueue;    // todo: make private
    //public Queue<string> rawQueue;
    
    // HACK
    private enum SslProtocolsHack
    {
      Tls   = 192,
      Tls11 = 768,
      Tls12 = 3072
    }

    public ExchangeConnecter(ExchangeState exchangeState)
    {
      rawQueue = new BlockingCollection<string>();
      //rawQueue = new Queue<string>();
      _exchangeState = exchangeState;
    }

    public bool isEmpty()
    {
      if (rawQueue.Count > 0)
        return false;
      return true;
    }
    
    public string GetRawData()
    {
      return rawQueue.Take();
    }
    
    public void Start()
    {
      Console.WriteLine("getting url");
      var suffix = GenerateTickerURL();
      var ws = new WebSocket(URL + suffix);

      ws.OnMessage += (senser, e) => rawQueue.Add(e.Data);
      
      ws.OnClose += (sender, e) =>
      {
        var sslProtocolHack = (System.Security.Authentication.SslProtocols)
          (SslProtocolsHack.Tls12 | 
           SslProtocolsHack.Tls11 | 
           SslProtocolsHack.Tls);
        // HACK: TlsHandshakeFailure (this is an issue with the websocket client libary)
        if (e.Code == 1015 && 
            ws.SslConfiguration.EnabledSslProtocols != sslProtocolHack)
        {
          ws.SslConfiguration.EnabledSslProtocols = sslProtocolHack;
          ws.Connect();
        }
      };
      ws.Connect();
    }

    public static double GetPing()
    {
      var n = 10;
      var sum = 0.0;
      var url = "https://api.binance.com/api/v1/ping";
      var client = new WebClient();
      for (int i = 0; i < n; i++)
      {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var reponse = client.DownloadString(url);
        watch.Stop();
        sum += watch.ElapsedMilliseconds;
      }
      return sum/n;
    }
    
    //
    public void InitValues()
    {
      var client = new WebClient();
      var response = client.DownloadString("https://api.binance.com/api/v3/ticker/bookTicker");
      var response2    = response.Substring(1, response.Length - 3);
      var responseArry = response2.Split("},");
      for (var i = 0; i < responseArry.Length; i++)
      {
        responseArry[i] = responseArry[i].Replace("symbol", "s")
                        .Replace("bidPrice", "b")
                        .Replace("bidQty", "B")
                        .Replace("askPrice", "a")
                        .Replace("askQty", "A");
        responseArry[i] = responseArry[i].Insert(1, "\"E\":1546755019884,");
        rawQueue.Add(responseArry[i]);
      }
    }

    private string GenerateTickerURL()
    {
      var url = new StringBuilder();
      foreach (var symbol in _exchangeState.GetAllSymbolStrings())
      {
        url.Append(symbol.ToLower()).Append("@ticker/");
      }

      return url.ToString();
    }


    public static SymbolObject[] RequestExchangeState()
    {
      var symbolList = new List<SymbolObject>();
      var client = new WebClient();
      var response = client.DownloadString("https://api.binance.com/api/v1/exchangeInfo");
      dynamic deserial    = JObject.Parse(response);
      var     deserialStr = deserial.symbols.ToString();
      var     symbols     = JsonConvert.DeserializeObject<List<SymbolObject>>(deserialStr);
      foreach (var sym in symbols)
      {
        if (sym.Status == "TRADING")
          symbolList.Add(sym);
      }
      return symbolList.ToArray();
    }
    
    
    
  }

}