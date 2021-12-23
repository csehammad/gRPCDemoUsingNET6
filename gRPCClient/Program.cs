// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using gRPCDemoUsingNET6.Protos;

 

var channel = GrpcChannel.ForAddress("https://localhost:7143");
int Count = 0;
try
{
    var client = new SalesService.SalesServiceClient(channel);

    using var call = client.GetSalesData(new Request { Filters = "" }
      , deadline: DateTime.UtcNow.AddSeconds(60)
    );


  
    await foreach (var each in call.ResponseStream.ReadAllAsync())
    {


        Console.WriteLine(String.Format("New Order Receieved from {0}-{1},Order ID = {2}, Unit Price ={3}, Ship Date={4}", each.Country, each.Region, each.OrderID, each.UnitPrice, each.ShipDate));
        Count++;




    }
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
{
    Console.WriteLine("Service timeout.");
}

Console.WriteLine("Stream ended: Total Records: "+Count.ToString());
Console.Read();






 