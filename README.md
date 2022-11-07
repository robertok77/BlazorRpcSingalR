# Blazor Rpc with SingalR

Sample code for RPC using signalr with blazor webassembly.

## Getting Started

Run the application and go to Primes page. 

* Each client, when it starts, it receives from server current maximum value to find prime numbers
* Client calculates them locally and send result back to server
* Another client gets new limit to find next range of prime numbers

_Three clients are generating primes independently:_

![rpc01](https://user-images.githubusercontent.com/14275269/200368299-e0132135-6e45-477c-9d79-f5640747764d.jpg)
<br/>
_Pic 1. 1st client display top 5 primes from 2 to 10_000_
<br/>
<br/>
![rpc02](https://user-images.githubusercontent.com/14275269/200368549-aa3ca146-7441-4b4e-96f6-47f65c057d96.jpg)
<br/>
_Pic 2. 2nd client display top 5 primes from 10_000 to 20_000_
<br/>
<br/>
![rpc03](https://user-images.githubusercontent.com/14275269/200368840-848447eb-eb27-4d04-a5dc-f2b856279d01.jpg)
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
 
