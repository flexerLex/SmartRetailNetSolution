---

# SmartRetailNet

**Visualizing retail store operations with IoT devices and behavioral modeling**

### Preliminary Solution Structure:

1. **Backend (Server side)**: ASP.NET Core Web API project responsible for business logic, data management, and communication with IoT devices via MQTT.
    
2. **Frontend (Client part)**: Blazor application to visualize the store and display information from IoT devices in real time, using SignalR to dynamically update the interface.
    
3. **MQTT Broker**: An external component, such as Mosquitto, to exchange messages between IoT devices and the server side.

### IoT Device Integration and Messaging

SmartRetailNet utilizes MQTT for real-time communication between the server and various IoT devices scattered throughout the retail environment.

#### output from the console application "" which simulates the operation of 3 iot devices:
1)EntranceGate
2)Checkout
3)ExitGate

on the example of 5 customers in a store:

```
Simulation-Work-IOT-Device-Store is started
IoT Device Simulator for EntranceGate is started...
IoT Device Checkout is started...
IoT Device Simulator for ExitGate is started...
Store Monitoring Service is running...
Customer1 trying to enter the store
EntranceGate is open
Customer2 trying to enter the store
EntranceGate is open
Customer3 trying to enter the store
EntranceGate is open
Customer4 trying to enter the store
EntranceGate is open
Customer5 trying to enter the store
EntranceGate is open
Customer4 walks around the store, selects a product(0.5 - 5 sec)
Customer5 walks around the store, selects a product(0.5 - 5 sec)
Customer1 walks around the store, selects a product(0.5 - 5 sec)
Customer2 walks around the store, selects a product(0.5 - 5 sec)
Customer3 walks around the store, selects a product(0.5 - 5 sec)
Customer3 approaches the payment area
PayIfFrei:Processing a new customer3...{
        PayIfFrei: self-service cash desk is free, the user can enter
            PayIfFrei:The customer3 pays for the items. Wait a couple seconds
Customer5 approaches the payment area
PayIfFrei:Processing a new customer5...{
    Customer5 waits for the Checkout to become available...
    Customer5 waits for the Checkout to become available...
Customer2 approaches the payment area
PayIfFrei:Processing a new customer2...{
    Customer2 waits for the Checkout to become available...
    Customer3 has paid for the items! The Checkout is available
PayIfFrei:Processing customer3 is End
}
Customer3 has left the store
Customer4 approaches the payment area
PayIfFrei:Processing a new customer4...{
        PayIfFrei: self-service cash desk is free, the user can enter
            PayIfFrei:The customer4 pays for the items. Wait a couple seconds
    Customer5 waits for the Checkout to become available...
Customer1 approaches the payment area
PayIfFrei:Processing a new customer1...{
    Customer1 waits for the Checkout to become available...
    Customer2 waits for the Checkout to become available...
    Customer5 waits for the Checkout to become available...
    Customer2 waits for the Checkout to become available...
    Customer1 waits for the Checkout to become available...
    Customer4 has paid for the items! The Checkout is available
PayIfFrei:Processing customer4 is End
}
Customer4 has left the store
        PayIfFrei: self-service cash desk is free, the user can enter
            PayIfFrei:The customer5 pays for the items. Wait a couple seconds
    Customer1 waits for the Checkout to become available...
    Customer2 waits for the Checkout to become available...
    Customer2 waits for the Checkout to become available...
    Customer1 waits for the Checkout to become available...
    Customer5 has paid for the items! The Checkout is available
PayIfFrei:Processing customer5 is End
}
Customer5 has left the store
        PayIfFrei: self-service cash desk is free, the user can enter
        PayIfFrei: self-service cash desk is free, the user can enter
            PayIfFrei:The customer1 pays for the items. Wait a couple seconds
            PayIfFrei:The customer2 pays for the items. Wait a couple seconds
    Customer2 has paid for the items! The Checkout is available
PayIfFrei:Processing customer2 is End
}
Customer2 has left the store
    Customer1 has paid for the items! The Checkout is available
PayIfFrei:Processing customer1 is End
}
Customer1 has left the store
```





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

