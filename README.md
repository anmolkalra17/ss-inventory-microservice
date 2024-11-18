# Inventory Microservice

This is a micro service made for Scalable Services assignment. The service is responsible to handle the inventory in an e-commerce application. It helps to perform the creation of products in an inventory, updating the products, deleting the products as well as fetching the products (either all products or by product ID).

It is build with .net and uses MongoDB at the database side.

This service is capable to communicate with another service that is responsible to create orders. Once an order is created, a message is sent in the RabbitMQ queue and the quantity of the item is decremented from the stock as per the number of items consumed in the order.


## Installation

Use the [dotnet](https://dotnet.microsoft.com/en-us/download) framework to build and run this micro service.

Once installed, install the following packages to add the dependencies:
1. RabbitMQ.Client v6.2.1
2. Newtonsoft.JSON
3. MongoDB.Driver
4. Microsoft.AspNetCore.OpenApi
5. Swashbuckle.AspNetCore

You can run the below command followed by the package name to add it in your project.

Eg:

```bash
dotnet add package RabbitMQ.Client --version 6.2.1
```

## Usage

Once all the packages are installed, you can simply run the micro service by using this command:

```bash
dotnet run
```

## Endpoints

This micro service has the following endpoints:

host = localhost:{port}

1. ```GET host/api/inventory/products```: Returns back all the products in the inventory database
2. ```POST host/api/inventory/products```: Add a new product in the inventory database
3. ```GET host/api/inventory/products/{id}```: Returns a specific product that matches the supplied id from the inventory database
4. ```PUT host/api/inventory/products/{id}```: Update a specific product that matched with the supplied id in the inventory database using the JSON Body
5. ```DELETE host/api/inventory/products/{id}```: Deletes a specific product that matches with the supplied id from the inventory database


## Product Schema

```id```: Unique identifier for the product

```name```: Name of the product 

```quantity```: Number of items in stock

```price```: Price of the product

```description```: Description of the product

```available```: Whether the product is available

```createdAt```: Automatically added timestamp when the document is created

```updatedAt```: Automatically added timestamp when the document is updated

## Docker Image URL
This micro service has been containerized and the docker image url can be found [here](https://hub.docker.com/r/itsmeakay/ss-microservice-inventory).
