# Blazor Rpc with SingalR

Sample code for RPC using signalr with blazor webassembly.

## Getting Started

Run the application and go to Primes page. 

* Each client, when it starts, it receives from server current maximum value to find prime numbers
* Client calculates them locally and send result back to server
* Another client gets new limit to find next range of prime numbers

_Three clients are generating primes independently:_


![rpc01 - Copy - Copy](https://user-images.githubusercontent.com/14275269/200371058-df4b038e-b66d-4ada-a5dd-d6e2ea074164.jpg)
<br/>
_Pic 1. 1st client display top 5 primes from 2 to 10_000_
<br/>
<br/>
![rpc02 - Copy - Copy](https://user-images.githubusercontent.com/14275269/200371354-62ac4c29-aa7a-492c-87fe-0302cf3fc7c3.jpg)
<br/>
_Pic 2. 2nd client display top 5 primes from 10_000 to 20_000_
<br/>
<br/>
![rpc03 - Copy - Copy](https://user-images.githubusercontent.com/14275269/200371392-c3764944-8006-409a-8f4c-a5480c118737.jpg)
<br/>
_Pic 3. 3rd client display top 5 primes from 20_000 to 30_000_
<br/>
## Technologies
* ASP.NET Core 5.0
* Blazor WebAssembly
* SignalR


### Prerequisites
You will need the following tools:

* [Visual Studio Code or Visual Studio 2019](https://visualstudio.microsoft.com/vs/) (version 16.8.3 or later)
* [.NET Core SDK 5]
 
