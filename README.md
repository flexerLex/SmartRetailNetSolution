---

# SmartRetailNet

**Visualizing retail store operations with IoT devices and behavioral modeling**

### Preliminary Solution Structure:

1. **Backend (Server side)**: ASP.NET Core Web API project responsible for business logic, data management, and communication with IoT devices via MQTT.
    
2. **Frontend (Client part)**: Blazor application to visualize the store and display information from IoT devices in real time, using SignalR to dynamically update the interface.
    
3. **MQTT Broker**: An external component, such as Mosquitto, to exchange messages between IoT devices and the server side.

### IoT Device Integration and Messaging

SmartRetailNet utilizes MQTT for real-time communication between the server and various IoT devices scattered throughout the retail environment.

#### MQTT Topics and Message Formats

To facilitate the development and ensure consistency across the solution - formats for each device type:

- **Access Control Systems (Swinging Electric Doors):**
  - **Topic**: `access_control/door1/status`
  - **Message Format**: 
    ```json
    { 
      "door_id": "door1", 
      "status": "open", 
      "timestamp": "2024-04-02T10:15:00" 
    }
    ```
- **Smart Fridges and Shelves:**
  - **Topic**: `smart_fridge/shelf1/inventory`
  - **Message Format**: 
    ```json
    { 
      "shelf_id": "shelf1", 
      "product": "milk", 
      "quantity": 3, 
      "weight": 1500, 
      "timestamp": "2024-04-02T10:30:00" 
    }
    ```
- **Product Recognition Cameras:**
  - **Topic**: `shelf_camera/shelf1/detection`
  - **Message Format**: 
    ```json
    { 
      "shelf_id": "shelf1", 
      "detected_items": ["apple", "banana"], 
      "timestamp": "2024-04-02T11:00:00" 
    }
    ```
- **Smart Price Tags:**
  - **Topic**: `smart_price_tags/product123/price`
  - **Message Format**: 
    ```json
    { 
      "product_id": "product123", 
      "price": 2.99, 
      "discount": 0.10, 
      "timestamp": "2024-04-02T11:30:00" 
    }
    ```
- **Baskets with Scanners:**
  - **Topic**: `basket_scanner/user1/purchases`
  - **Message Format**: 
    ```json
    { 
      "user_id": "user1", 
      "items": [
        {"product": "bread", "price": 1.50}, 
        {"product": "milk", "price": 2.00}
      ], 
      "total": 3.50, 
      "timestamp": "2024-04-02T12:00:00" 
    }
    ```
- **Climate Control Systems:**
  - **Topic**: `climate_control/store1/environment`
  - **Message Format**: 
    ```json
    { 
      "store_id": "store1", 
      "temperature": 25, 
      "humidity": 60, 
      "timestamp": "2024-04-02T12:30:00" 
    }
    ```



Чтобы реализовать логику ожидания клиента у кассы, пока она не освободится, мы можем использовать концепции асинхронного программирования вместе с некоторой логикой состояния для объекта кассы (Checkout). Предположим, у нас есть переменная состояния isCheckoutAvailable в классе Checkout, которая указывает, занята касса или нет. Мы можем использовать эту переменную для контроля доступа к кассе и реализации ожидания в случае, если касса занята.Давайте расширим наш класс Checkout:class Checkout : IoTDevice
{
    private volatile bool isCheckoutAvailable = true;

    public Checkout(IMqttClient client) : base(client) { }

    public async Task Pay(Customer customer)
    {
        // Ожидание, пока касса не освободится
        while (!isCheckoutAvailable)
        {
            Console.WriteLine($"Покупатель {customer.Id} ожидает освобождения кассы...");
            await Task.Delay(1000); // Проверяем доступность каждую секунду
        }

        // Касса теперь занята
        isCheckoutAvailable = false;
        Console.WriteLine($"Покупатель {customer.Id} начинает процесс оплаты.");

        // Имитация процесса оплаты
        await Task.Delay(5000); // Допустим, процесс занимает 5 секунд

        // Оплата завершена, касса снова доступна
        isCheckoutAvailable = true;
        Console.WriteLine($"Покупатель {customer.Id} завершил оплату и освободил кассу.");

        // Отправляем сообщение о завершении оплаты
        await SendMessageAsync("store/checkout/status", $"Покупатель {customer.Id} оплачивает товары и покидает кассу.");
    }
}
В этом примере, когда метод Pay вызывается, сначала проверяется, доступна ли касса. Если касса занята, метод ожидает (благодаря await Task.Delay(1000);), проверяя состояние isCheckoutAvailable каждую секунду, пока касса не освободится. Как только касса становится доступной, покупатель начинает процесс оплаты, и переменная isCheckoutAvailable устанавливается в false до завершения процесса.Этот подход позволяет эффективно управлять доступом к кассе и обеспечивает правильное взаимодействие покупателей с кассой самообслуживания, даже если требуется ожидание.Для реализации системы, где могут быть очереди и более сложные взаимодействия, возможно, потребуется ввести дополнительные механизмы управления состоянием и очередностью доступа, например, используя очереди сообщений или событийную модель для координации действий между различными компонентами системы.

