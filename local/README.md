Скрипты на bash, запускающие локально в докере сервисы, необходимые для разработки (учитывая временное отсутствие дев-тест среды):
- Seq для записи логов основного приложения, после запуска логи будут доступны из браузера по адресу http://localhost/#/events
- RabbitMQ используется в качестве транспорта для брокера сообщений MassTransit, его веб-интерфейс будет доступен локально по адресу http://localhost:15672/ логин и пароль в /local/deployment/rabbit-mq/rabbitmq.conf
- PostgreSQL используется в качестве базы данных, схема называется Relocan, а база Dev, строка соединения доступка в appsettings.json в корне проектов Web и Worker

Все вышеперечисленные сервисы запускаются в одной сети (relocan-dev-net) в локальном докере, т.е. они будут видеть друг друга. Подробнее можно посмотреть в файлах docker-compose.yml

Сами скрипты:
- start-dev-environment.sh стартует все сервисы, настройки берет из файлов в соответствующей директории
- stop-dev-environment.sh останавливает все сервисы
- clear-dev-environment.sh удаляет все контейнеры из докера
