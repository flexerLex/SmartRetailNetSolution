using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Server;
using Newtonsoft.Json;
using System.Data.Common;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


//Topics structur:
//var message = $"{{\"customerId\": \"{customerId}\", \"action\": \"{customerAction}\", \"timestamp\": \"{DateTime.Now}\"}}";

//("store/entrance", $"Покупатель {customer.Id} вошел в магазин.")
//
//
//
//
//("store/checkout", $"Покупатель {customer.Id} оплачивает товары.");
//("store/entrance", $"Покупатель {customer.Id} вошел в магазин.")
//
//

    public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Simulation-Work-IOT-Device-Store is started");
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .Build();

        await mqttClient.ConnectAsync(options);

        // Инициализация устройств
        EntranceGate entrance = new EntranceGate(mqttClient);
        Checkout checkout = new Checkout(mqttClient);
        ExitGate exit = new ExitGate(mqttClient);

        var monitor = new StoreMonitoringService();
        await monitor.StartAsync();


        var customers = new List<Customer>
        {
        new Customer(1),
        new Customer(2),
        new Customer(3),
        new Customer(4),
        new Customer(5)
        //new Customer(6),
        //new Customer(7),
        //new Customer(8)
        };

        var tasks = customers.Select(customer => customer.StartShoppingJourney(entrance, checkout, exit));

        await Task.WhenAll(tasks);

    }
}








class StoreMonitoringService
{
    private readonly IMqttClient _mqttClient;
    private int _customerCount = 0;

    public StoreMonitoringService()
    {
        // Инициализация MQTT клиента
        var mqttFactory = new MqttFactory();
        _mqttClient = mqttFactory.CreateMqttClient();

        // Обработчик входящих сообщений
        _mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var topic = e.ApplicationMessage.Topic;
            if (topic == "store/customers/update")
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                if (message == "increment")
                {
                    // Увеличиваем счетчик покупателей
                    Interlocked.Increment(ref _customerCount);
                }
                else if (message == "decrement")
                {
                    // Уменьшаем счетчик покупателей
                    Interlocked.Decrement(ref _customerCount);
                }

                //await
            }

            //await
            // Публикуем актуальное количество покупателей в магазине
            SendMessageAsync("store/customers/count", _customerCount.ToString());
            return Task.CompletedTask;
        };
    }

    public async Task StartAsync()
    {
        // Подключение к MQTT брокеру
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Адрес и порт MQTT брокера
            .Build();

        await _mqttClient.ConnectAsync(options, CancellationToken.None);

        // Подписываемся на топик обновлений количества покупателей
        await _mqttClient.SubscribeAsync("store/customers/update");

        Console.WriteLine("Store Monitoring Service is running...");
    }

    private async Task SendMessageAsync(string topic, string message)
    {
        var mqttMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(Encoding.UTF8.GetBytes(message))
            .Build();

        await _mqttClient.PublishAsync(mqttMessage);
    }
}









class Customer
{
    public int Id { get; set; }
    //public ShoppingCart Cart { get; set; }

    public Customer(int id)
    {
        Id = id;
    }

    public async Task StartShoppingJourney(EntranceGate entrance, Checkout checkout, ExitGate exit)
    {
        // Вход в магазин
        Console.WriteLine($"Customer{this.Id} trying to enter the store");
        await entrance.Enter(this);

        // Имитация выбора товаров
        Console.WriteLine($"Customer{this.Id} walks around the store, selects a product(0.5 - 5 sec)");
        await Task.Delay(new Random().Next(500, 5000));

        // Оплата товаров
        Console.WriteLine($"Customer{this.Id} approaches the payment area");
        await checkout.PayIfFrei(this);

        // Выход из магазина
        Console.WriteLine($"Customer{this.Id} has left the store");
        await exit.Exit(this);
    }
}

abstract class IoTDevice
{
    protected IMqttClient mqttClient;

    public IoTDevice(IMqttClient client)
    {
        mqttClient = client;
    }

    protected async Task SendMessageAsync(string topic, string message)
    {
        var mqttMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(Encoding.UTF8.GetBytes(message))
            .Build();
        try
        {
            await mqttClient.PublishAsync(mqttMessage);
        }
        catch (MqttCommunicationException q)
        {
            Console.WriteLine($"MqttCommunicationException:{q.Message}");
        }
    }

    protected void Subscribe(string topic)
    {

    }
}






    //class ExitGate : IoTDevice
    //{
    //    public ExitGate(IMqttClient client) : base(client) 
    //    { Subscribe("store/customers/count"); 
    //        mqttClient.UseApplicationMessageReceivedHandler
    //            (e => 
    //            { 
    //                if (e.ApplicationMessage.Topic == "store/customers/count") 
    //                { 
    //                    var currentCount = int.Parse(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
    //                    Console.WriteLine($"Актуальное количество покупателей в магазине: {currentCount}");
    //        } 
    //    }); 
    //    }
    //    public async Task Exit(Customer customer)
    //    {
    //        await SendMessageAsync("store/exit", $"Покупатель {customer.Id} покинул магазин.");
    //        // Тут может быть логика для уменьшения счетчика покупателей
    //    }

    //}








    class EntranceGate : IoTDevice
{
    public EntranceGate(IMqttClient client) : base(client)
    {
        Console.WriteLine("IoT Device Simulator for EntranceGate is started...");
        //    Subscribe("store/customers/count");
        //mqttClient.ApplicationMessageReceivedAsync += e =>
        //{
        //    Console.WriteLine("Received application message.");
        //    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
        //    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        //    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
        //    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
        //    return Task.CompletedTask;
        //};
        }

    public async Task Enter(Customer customer)
    {
        Console.WriteLine($"EntranceGate is open");
        await SendMessageAsync("store/EntranceGate", $"Customer{customer.Id} entered the store.");


        await SendMessageAsync("store/customers/update", "increment");

        await Task.Delay(500);
    }
}

class Checkout : IoTDevice
{
    private volatile bool isCheckoutAvailable = true;
    public Checkout(IMqttClient client) : base(client)
    {
        Console.WriteLine("IoT Device Checkout is started...");
    }

    public async Task PayIfFrei(Customer customer)
    {
        Console.WriteLine($"PayIfFrei:Processing a new customer{customer.Id}...{{ ");

        while (!isCheckoutAvailable)
        {
            Console.WriteLine($"    Customer{customer.Id} waits for the Checkout to become available...");
            await Task.Delay(1000); // Проверяем доступность каждую секунду
        }
        // Имитация процесса сканирования товаров
        Console.WriteLine("        PayIfFrei: self-service cash desk is free, the user can enter");
            isCheckoutAvailable = false;

          
            await SendMessageAsync("store/Checkout/status", "busy");
            Console.WriteLine($"            PayIfFrei:The customer{customer.Id} pays for the items. Wait a couple seconds");
            // Имитация задержки на сканирование и оплату товаров
            {

                // Логика добавления продуктов в корзину покупателя
                var productListInKundenWagen = new List<ProductInStore>();
                productListInKundenWagen.Add(new ProductInStore { ProductName = "milk", Price = 2.00 });
                productListInKundenWagen.Add(new ProductInStore { ProductName = "bread", Price = 1.50 });

                var slip = new PaymentSlip {
                    UserId = customer.Id,
                    Items = productListInKundenWagen,
                    Total = 3.5,
                    Timestamp = DateTime.Now,
                };

                await SendMessageAsync("store/Checkout/PaymentSlip/json",JsonConvert.SerializeObject(slip));
                
                await Task.Delay(2000); // 2 секунд на процесс
            }
            await SendMessageAsync("store/Checkout/status", "available");
            Console.WriteLine($"    Customer{customer.Id} has paid for the items! The Checkout is available");
            isCheckoutAvailable = true;
        
        Console.WriteLine($"PayIfFrei:Processing customer{customer.Id} is End\n}}");
    }

    
}

public partial class PaymentSlip
{
    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("items")]
    public List<ProductInStore> Items { get; set; }

    [JsonProperty("total")]
    public double Total { get; set; }

    [JsonProperty("timestamp")]
    public DateTimeOffset Timestamp { get; set; }
}

public partial class ProductInStore
{
    [JsonProperty("product")]
    public string ProductName { get; set; }

    [JsonProperty("price")]
    public double Price { get; set; }
}

class ExitGate : IoTDevice
{
    public ExitGate(IMqttClient client) : base(client)
    {
        Console.WriteLine("IoT Device Simulator for ExitGate is started...");
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");
            Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
            Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
            return Task.CompletedTask;
        };
    }

    public async Task Exit(Customer customer)
    {
        await SendMessageAsync("store/ExitGate", $"Customer{customer.Id} has left the store.");


        await SendMessageAsync("store/customers/update", "decrement");

        await Task.Delay(100);
    }
}