Пример вывода псевдо-логов в консоль:
- магазин открылся 
- в магазин через iot-устройство входа вошел покупатель1. 
-в магазин через iot-устройство входа вошел покупатель2
- покупатель один занял кассу самообслуживания и совершает сканирование товара 
- вошел 3 покупатель 
- покупатель 2 стоит в очереди на кассу самообслуживания 
- вошлел покупатель 4
- покупатель 1 сканирует чек на iot устройстве выхода из магазина 
- покупатель 2 занял кассу
- покупатель 1 покинул магазин



Давайте создадим консольное приложение на C#, которое будет подписываться на все топики, используемые предыдущими имитаторами IoT устройств, чтобы отображать логи действий в магазине в реальном времени.

Для начала убедитесь, что у вас установлена библиотека MQTTnet. Если нет, добавьте её через NuGet Package Manager:

```
Install-Package MQTTnet -Version 3.1.1
```

Теперь вот полный код приложения:

```csharp
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace StoreMonitoringApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Создание и конфигурация MQTT клиента
            var mqttFactory = new MqttFactory();
            var mqttClient = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883) // Адрес и порт MQTT брокера
                .Build();

            // Подключение к MQTT брокеру
            await mqttClient.ConnectAsync(options);

            // Подписка на топики
            await mqttClient.SubscribeAsync(new MQTTnet.Client.Subscribing.MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter("store/#") // Подписываемся на все топики, связанные с магазином
                .Build());

            Console.WriteLine("Monitoring Store Activity...");

            // Обработчик получения сообщений
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                // Выводим сообщение в консоль в зависимости от топика
                switch (topic)
                {
                    case "store/entrance":
                        Console.WriteLine($"- через iot-устройство входа в магазин {payload}");
                        break;
                    case "store/checkout/status":
                        Console.WriteLine($"- {payload}");
                        break;
                    default:
                        Console.WriteLine($"Received message on topic {topic}: {payload}");
                        break;
                }
            });

            // Бесконечный цикл, чтобы приложение продолжало работать
            while (true)
            {
                Task.Delay(1000).Wait(); // Чтобы избежать завершения программы
            }
        }
    }
}
```

### Как это работает:

- **Подключение к MQTT брокеру**: Приложение устанавливает соединение с MQTT брокером (в этом примере используется `localhost`).
- **Подписка на топики**: Приложение подписывается на все топики, начинающиеся с `store/`, что позволяет ему получать сообщения от всех IoT устройств магазина.
- **Обработка сообщений**: Для каждого полученного сообщения определяется его топик, и в зависимости от этого выводится соответствующее сообщение в консоль. Это позволяет в реальном времени отслеживать действия покупателей и статус кассы самообслуживания.

Полученное приложение демонстрирует возможность мониторинга действий в магазине в реальном времени, используя MQTT для обмена сообщениями.




---
Теперь напиши имитацию iot устройства как отдельного консольного приложения, имитирующего кассу самообслуживания - куда клиенты приходят перед тем как покинуть магазин, чтобы отсканировать каждый товар, и оплатив и получив чек для благополучного выхода из магазина (реализуй проверки на нужные топики и сообщения например о статусе занятой кассы самообслуживания - тогда клиенту нужно встать в очередь и ждать) 




Давайте создадим имитацию IoT устройства кассы самообслуживания. Это приложение будет имитировать процесс сканирования и оплаты товаров покупателями. Касса будет отправлять сообщения о своем статусе (занята/свободна), а также об обработке покупок покупателей.

### Шаги реализации:

1. **Установите библиотеку MQTTnet**, если ещё не установлена.

   ```
   Install-Package MQTTnet -Version 3.1.1
   ```

2. **Реализация консольного приложения**:

Создайте новый проект консольного приложения в Visual Studio и добавьте следующий код:

```csharp
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SelfCheckoutSimulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            var mqttClient = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883) // Используйте адрес вашего MQTT брокера
                .Build();

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            Console.WriteLine("Self-Checkout Simulator started...");

            bool isCheckoutAvailable = true; // Переменная, указывающая на доступность кассы

            while (true)
            {
                if (isCheckoutAvailable)
                {
                    // Имитация процесса сканирования товаров
                    Console.WriteLine("Processing a new customer...");
                    isCheckoutAvailable = false;

                    // Отправляем сообщение о занятости кассы
                    var busyMessage = new MqttApplicationMessageBuilder()
                        .WithTopic("store/checkout/status")
                        .WithPayload(Encoding.UTF8.GetBytes("{\"status\": \"busy\"}"))
                        .Build();
                    await mqttClient.PublishAsync(busyMessage, CancellationToken.None);

                    // Имитация задержки на сканирование и оплату товаров
                    await Task.Delay(10000); // 10 секунд на процесс

                    // Отправляем сообщение о завершении обработки покупателя и освобождении кассы
                    var availableMessage = new MqttApplicationMessageBuilder()
                        .WithTopic("store/checkout/status")
                        .WithPayload(Encoding.UTF8.GetBytes("{\"status\": \"available\"}"))
                        .Build();
                    await mqttClient.PublishAsync(availableMessage, CancellationToken.None);

                    Console.WriteLine("Customer processed, checkout is now available.");
                    isCheckoutAvailable = true;

                    // Пауза перед следующим клиентом
                    await Task.Delay(5000);
                }
            }
        }
    }
}
```

В этом примере симулятор кассы самообслуживания работает следующим образом:

- Приложение подключается к MQTT брокеру (в этом примере используется `localhost`).
- В цикле приложение имитирует процесс обслуживания покупателя, который занимает около 10 секунд. Во время этого процесса касса считается занятой, и она отправляет соответствующее сообщение о своем статусе в топик `store/checkout/status`.
- После обработки покупателя касса отправляет сообщение об освобождении, что означает, что следующий покупатель может подойти к кассе.
- Между обработками покупателей введена задержка в 5 секунд для имитации времени ожидания следующего клиента.

Этот простой симулятор можно расширить и дополнить более сложной логикой, например, управлением очередями покупателей и интеграцией с другими системами магазина для комплексной симуляции розничной торговли.



---
1. Хорошо, в таком случае напиши консольное приложение на c# . Net которое имитировало бы работу iot устройства входа в магазин и отправляло бы сообщения на брокер при каждом входе покупателя в магазин(как будет представлен покупатель в контексте приложения, и имитация его повеления(функций)) и выхода из него(покупатель может выйти после оплаты товаров, а может ничего так и не купить)



Давайте разработаем простое консольное приложение на C#, которое имитирует работу IoT-устройства входа и выхода в магазин. Покупатели будут имитироваться через случайные интервалы времени, при этом для каждого события входа и выхода будет отправляться сообщение на MQTT брокер.

### Шаги реализации:

1. **Установите библиотеку MQTTnet**: Для работы с MQTT в проекте .NET нужно добавить библиотеку MQTTnet. Это можно сделать через NuGet Package Manager.

   ```
   Install-Package MQTTnet -Version 3.1.1
   ```

2. **Разработка имитатора устройства**:

Создайте новый проект консольного приложения в Visual Studio и добавьте следующий код:

```csharp
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace IoTDeviceSimulator
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var mqttFactory = new MqttFactory();
            using var mqttClient = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("адрес_вашего_MQTT_брокера", 1883) // Например, localhost и 1883
                .Build();

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            Console.WriteLine("IoT Device Simulator for Entrance and Exit started...");

            // Имитация работы устройства
            var random = new Random();

            while (true)
            {
                var customerAction = random.Next(2) == 0 ? "entered" : "exited";
                var customerId = random.Next(1000, 9999); // Генерация случайного ID покупателя
                var message = $"{{\"customerId\": \"{customerId}\", \"action\": \"{customerAction}\", \"timestamp\": \"{DateTime.Now}\"}}";

                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("store/entrance")
                    .WithPayload(Encoding.UTF8.GetBytes(message))
                    .WithExactlyOnceQoS()
                    .Build();

                await mqttClient.PublishAsync(mqttMessage, CancellationToken.None);

                Console.WriteLine($"Published: {message}");

                // Пауза перед следующим действием покупателя
                await Task.Delay(random.Next(5000, 10000));
            }
        }
    }
}
```

Замените `"адрес_вашего_MQTT_брокера"` на актуальный адрес вашего MQTT брокера. Если вы используете Mosquitto на локальной машине, адрес будет `localhost`, а порт по умолчанию `1883`.

### Как это работает:

- Приложение устанавливает соединение с MQTT брокером.
- В бесконечном цикле приложение генерирует события входа или выхода покупателей в магазин с использованием случайных значений. Для каждого события создается JSON-сообщение, содержащее ID покупателя, действие (`entered` или `exited`) и временную метку.
- Сообщение публикуется в топик `"store/entrance"` на MQTT брокере.
- Интервал между событиями также генерируется случайным образом (от 5 до 10 секунд), что имитирует различные интервалы времени между посещениями покупателей.

Этот пример имитирует работу IoT-устройства для отслеживания входа и выхода покупателей в магазин, отправляя соответствующие данные на MQTT брокер.
