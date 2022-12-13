function changeDirectory {
cd ..
cd ..
cd ..
cd ..
}

cd Application
docker-compose up -d

cd DeliveryService/bin/debug/net6.0
start DeliveryService.exe
changeDirectory

cd EmailService/bin/debug/net6.0
start EmailService.exe
changeDirectory

cd FeedbackService/bin/debug/net6.0
start FeedbackService.exe
changeDirectory

cd GraphqlDemo/bin/debug/net6.0
start GraphqlDemo.exe
changeDirectory

cd HubService/bin/debug/net6.0
start HubService.exe
changeDirectory

cd OrderService/bin/debug/net6.0
start OrderService.exe
changeDirectory

cd PaymentProcessorService/bin/debug/net6.0
start PaymentProcessorService.exe
changeDirectory

cd PaymentValidatorService/bin/debug/net6.0
start PaymentValidatorService.exe
changeDirectory

cd RestaurantService/bin/debug/net6.0
start RestaurantService.exe
changeDirectory

cd UserService/bin/debug/net6.0
start UserService.exe
changeDirectory