## RabbitMQ C2 Calculator
 
A simple calculator for C2 style created by .net core 2.0 and rabbitmq.client in c#.

### Usage

1. Firstly, you need to install [Erlang enviorment](https://www.erlang.org/) for rabbitmq.

2. Then, install [Rabbitmq-Server](http://www.rabbitmq.com/download.html) to your target machine. More specific examples for rabbitmq, you can refer to [here](http://www.cnblogs.com/yangecnu/p/4227535.html). After you install rabbitmq server, you shuld change these code in Program.cs for your own.

```
    string rbmqUsername = "ziyuandev";
    string rbmqPassword = "123456";
    string rbmqServerUrl = "localhost";//ip or ip+port, also can replace ip with domain name
```

3. Furthermore, [.net core sdk](https://www.microsoft.com/net/download/windows) is neccessary.

4. Next, go into root directory in terminal (like powershell in windows),and run these command.This will restore the project and run it.

```
dotnet restore
dotnet run
```
5. Finally, when your project is running,you can try these examples.

```
1+2
1-2
1*2
1/2
1/0
123.4/234.5
q
```
