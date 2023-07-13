# Web API для бронирования номеров в отеле
## Назначение проектов в решении:
    Entities — сущности моделей базы данных и исключений
    Contracts — интерфейсы репозиториев и логгера
    LoggerService — реализация сервиса для логгирования
    Repository — реализация репозиториев, конфигурация сущностей и контекст, наследуемый от DbContext
    Service.Contracts — интерфейсы сервисов
    Service — реализауия сервисов
    HotelBooking — основной проект, содержащий Program.cs и методы расширения
    HotelBooking.Presentation — реализация уровеня представления (контроллеры)
    Shared — содержит Data Transfer Objects
## Окружение и запуск проекта:
    База данных — Microsoft SQL Server, строка подключения в appsettings.json
    В корне репозитория хранятся коллекции запросов (Postman/Insomnia)
    Присутствует Swagger-документация
    Для запуска необходимо провести миграцию (Add-Migration, Update-Database) для добавления стартовых значений в БД
