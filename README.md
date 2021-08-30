The application is written in the **Asp.Net Core - using .NET 5**

## Requirements

- [Install](https://www.microsoft.com/net/download/windows#/current) the latest .NET 5 SDK

## Running the API

```
dotnet run --project src\API

```
OR

```
dotnet publish src\API -c Release && dotnet src\API\bin\Release\net5.0\publish\API.dll

```

## Running via Docker


```
docker build --rm -f "src\API\Dockerfile" -t powerplantcodingchallenge:latest "." && docker run -d -p 8888:8888 powerplantcodingchallenge

```

## Websocket

Used SignalR.

route: '/hub/notification'

SignalR supports the following techniques for handling real-time communication (in order of graceful fallback):

    WebSockets
    Server-Sent Events
    Long Polling

SignalR automatically chooses the best transport method that is within the capabilities of the server and client.

But it's possible to force to use only websocket:

```
 endpoints.MapHub<NotificationsHub>("/hub/notification", options =>
 {
    options.Transports = HttpTransportType.WebSockets;
 });

```