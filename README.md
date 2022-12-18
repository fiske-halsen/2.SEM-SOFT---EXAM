# Final Exam Project for SI - Software PBA

## Introduction 
The system described in this report has been developed for **MTOGO A/S**.

This system was designed using domain-driven design principles, which involve focusing on the specific needs and behaviors of the business and its customers.
To improve the flexibility and maintainability of the system, we implemented decoupling of components. This involved separating the various components within the system so that they are independent and can operate independently of each other. 

In order to facilitate communication and integration with other systems and applications, the system makes use of APIs *(Application Programming Interfaces)* and API gateway. APIs allow different systems to exchange information and functionality, and API gateways serve as a centralized point of access and management for these APIs.

Additionally, the system utilizes Kafka, a distributed streaming platform, to handle high volumes of data and enable real-time processing and analysis.

## Architecture
We chose to use a microservice architecture for our application because of its ability to scale independently, its modularity, ease of deployment, flexibility, and improved reliability. Microservices allowed us to use different technologies, made it easier to develop and maintain, could be deployed and updated individually, and the failure of one service did not affect the rest of the application. 

             The following image is an overview of our architectural style
![alt text](https://raw.githubusercontent.com/fiske-halsen/2.SEM-SOFT---EXAM/main/Diagrams/Arkitektur.drawio.png)

### Description of architecture
1. To access our application, the client needs to use the DNS that we have configured for the external IP exposed by the ingress controller in the Azure Kubernetes Service (AKS). 

2. The NGINX Ingress Controller routes external traffic to appropriate services within a Kubernetes cluster, simplifying integration and access management. It allows for building scalable and resilient systems that easily integrate with external systems through Kubernetes.

3. We have implemented a GraphQL gateway in our application to provide a single entry point for all GraphQL requests and to aggregate data from multiple backend services. It has enabled us to decouple the client from the underlying backend services and add functionality such as authentication, rate limiting, and caching

4. We used Kafka in our microservice architecture with a gateway to improving communication and performance between microservices acting as a message broker.


## BPMN diagram
![alt text](https://raw.githubusercontent.com/fiske-halsen/2.SEM-SOFT---EXAM/main/Diagrams/BPMN_DIAGRAM.PNG)

Here is a step-by-step breakdown of the process represented in the BPMN diagram:

* Customer creates an order
* Order is sent to the application
* Payment for the order is validated within the application
  * If the payment is approved, the order is sent to a restaurant for preparation
  * If the payment is rejected, the process ends
* The restaurant can either accept or reject the order
  * If the order is accepted, it is prepared and sent for delivery
  * If the order is rejected, the process ends
* The delivery status is updated and an estimated time of arrival is provided
* The delivery is completed
* Customer provides feedback on the delivery

Note that this process includes a number of roles, such as the customer, the application, and the restaurant, and various actions and occurrences, such as the validation of payment and the preparation and delivery of the order.

## Microservices

#### Payment validator service
The service in our application includes business rules related to payment validation. It checks for acceptable credit cards, vouchers, and other relevant details to ensure that payment is handled correctly.

#### Payment processing service
This service is responsible for applying vouchers and discount codes to orders and processing the payment. It handles all necessary actions to complete the payment for an order.
Email service:

#### Email service
The email service in our application is used to keep the user informed about the status of their order. It provides updates on the order confirmation, restaurant approval, and estimated time of delivery. This allows the user to stay informed and track the progress of their order.
User service:

#### User service
The user service is responsible for handling all aspects of user authentication and management. It stores information for all types of users, including customers, restaurant owners, and delivery personnel. The service also includes the Identity Server 4 middleware, which uses the Open Id Connect and OAuth 2.0 protocols to secure our microservices through the client credentials flow. While it would have been ideal to separate the user service and Identity Server into two separate services, they are currently combined in this service.
Restaurant service:

#### Restaurant service
The restaurant service is responsible for managing all aspects of restaurants within our application, including retrieving restaurant information such as location and menu. It also enables restaurant owners to update their menu and other relevant details. This service plays a central role in handling the management and organization of restaurants within our system

#### Order service
The order service is responsible for managing all orders placed by users, including creation and retrieval of orders for restaurants, customers, and delivery personnel. It plays a central role in handling the processing and organization of orders within our system.

#### Delivery service
The delivery service is responsible for managing all aspects of order delivery, including providing delivery information with a list of orders waiting for pickup at restaurants and enabling users to track their delivery and see the estimated time of arrival. This service plays a crucial role in coordinating and executing deliveries within our system.

#### Feedback service
The feedback service is responsible for managing customer reviews and ratings for restaurants. It allows restaurants to access customer feedback and use it to improve their business, and also enables delivery personnel to see feedback on the deliveries they have completed. This service helps facilitate communication between customers and restaurants, and allows for continuous improvement of service quality.

## GraphQL
We have chosen to use Graphql in our application, since it's a much faster alternative to other communication APIs. This is because Graphql encourages you to cut down on your query requests per call. It allows you to specify exactly what you want from the query, and not unnecessary data. Also what we call over- and underfetching.

Apart from the smaller queries, it also has a single endpoint, which makes it more simple to use. One endpoint with all of our queries and mutations to use for all of our communication with Graphql.

These two reasons have been crucial to our choice of Graphql as our gateway in our application. It's easy to implement and makes communication between gateway and services a simple task.

## Kafka
In our environment, we have implemented Kafka as a real-time data streaming platform to handle the processing of creating orders, accepting orders, and creating deliveries.

To do this, we set up a Kafka cluster consisting of three brokers, which are responsible for maintaining a record of all the messages that are published to the cluster and distributing them to the consumers that are subscribed to the topics. By doing this we were able to handle high volume, high throughput, and low latency data streams.

## Hub Service
In our application, we have implemented [Web Sockets](https://github.com/fiske-halsen/2.SEM-SOFT---EXAM/tree/main/Application/HubService) through the use of the SignalR library and created a HubService. The Hub & Spoke pattern is utilized, with a central hub that clients can connect to using WebSockets. This constant connection between the client and server allows us to send real-time updates to the participants involved in an order flow, ensuring that all parties are promptly notified of any changes or updates. This improves communication and coordination within the system. As an example, when a customer places an order, the designated restaurant is immediately notified about a new order through the use of WebSockets.



# Local setup

**step 1**: Run the ````startscript.sh``` in the project root as shown below 

```
├── Application
├── Diagrams
├── Kubernetes
├── azure-pipelines.yml
├── README.md
├── startscript.sh
```

> * https://localhost:5001 for UserService
> * https://localhost:5002 for RestaurantService
> * https://localhost:5003 for OrderService
> * https://localhost:5004 for FeedbackService
> * https://localhost:5005 for DeliveryService
> * https://localhost:5006 for EmailService
> * https://localhost:5007 for PaymentProcessorService
> * https://localhost:5008 for PaymentValidatorService
> * https://localhost:5009 for Gateway
> * https://localhost:5011 for HubService
