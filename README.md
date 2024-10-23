# ManufacturingManagementSystem API

RESTful API для системы управления производством.

## Технологии

- .NET 6
- Entity Framework Core
- PostgreSQL
- Swagger
- XUnit
- JWT аутентификация

## Запуск проекта

### Инструкция по запуску

1. **Скачайте архив или клонируйте репозиторий**

   ```bash
   git clone https://github.com/aevdev/ManufacturingManagementSystem.git

2. **Перейдите в директорию проекта**

3. **Настройте строку подключения**
    
  > [!IMPORTANT]
  > **Для этого у вас должен быть [скачан](https://www.postgresql.org/download/) и установлен PostgreSQL**
    
  - После того как вы настроили PostgreSQL, перейдите в папку `/ManufacturingManagementSystem.`
  - Откройте файл appsetings.json c помощью любого рекдактора кода.
  - В строке
    > "DefaultConnection": "Host=localhost;Database=ManufacturingDB;Username=postgres;Password=PUT_YOUR_PASSWORD_HERE"

    Измените строку идущую после Password= на пароль, который вы использовали во время первичной настройки PostgreSQL.
    Либо вставьте свою строку подключения.
  

4. **Запустите проект**

    Для запуска проекта вернитесь в корневую папку и запустите файл `run_project.bat`.
    Подождите пока завершатся все тесты и приложение не будет запущено.
    В результате вы должны будете увидеть подобную строчку:
   ```bash
   info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:Ваш_порт
   
6.  **Протестируйте функционал**
   
    Если приложение запущено успешно, откройте браузер и откройте **Swagger UI**:
   ``http://localhost:Ваш_порт/swagger``

### Авторизация во время тестирования

  На случай если у вас возникнет вопрос как авторизоваться для тестирования методов.
  1. Зарегистрируйтесь используя `/api/Auth/regiter`
  2. Затем воспользуйтесь `/api/Auth/login`

  В блоке Responses вы должны будете увидеть следующее (только свою строку):
  ```
    {
      "token":     "eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdHVzZXIxIiwiZXhwIjoxNzI5NjU1NDU3LCJpc3MiOiJNYW51ZmFjdHVyaW5nTWFuYWdlbWVudFN5c3RlbSIsImF1ZCI6ImxvY2FsaG9zdCJ9.E6SUhpNDcl9ayjm5vNjKzxc_jQ62b7Ai7EEk-5RRExFkczkP2jRa7OqsvA5HNoZ0"
    }
  ```
  Скопируйте строку поля `"token"` (без кавычек)
  
  3.  Нажав на кнопку `Authorize`, в открывшемся окне впишите `Bearer {Вставьте_сюда_скопированную_строку}`
  4.  Нажмите `Aythorize`

  Теперь все API требующие авторизации должны работать без проблем.
    
