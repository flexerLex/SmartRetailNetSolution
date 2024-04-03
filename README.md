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
