# ProductPhotos.API

ASP.NET Core 8 Web API для керування товарами та їхніми фотографіями.
База даних: PostgreSQL (Supabase), доступ через Entity Framework Core.
Файли зображень зберігаються в `wwwroot/uploads/products/{productId}/`, у базі лежать метадані.

## Налаштування

1. Створіть проєкт на https://supabase.com та скопіюйте рядок підключення
   (Project Settings -> Database -> Connection string, режим Session pooler).
2. Збережіть його в user-secrets, щоб пароль не потрапив у репозиторій:

```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=...;Port=5432;Database=postgres;Username=postgres.xxxx;Password=...;SSL Mode=Require;Trust Server Certificate=true"
```

3. Міграції:

```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Запуск: `dotnet run`, Swagger за адресою `/swagger`.

## Маршрути

| Метод | Маршрут | Опис |
|---|---|---|
| GET | /api/products | список товарів з фотографіями |
| GET | /api/products/{id} | товар за id |
| POST | /api/products | створити товар (multipart/form-data, поле Photos — кілька файлів) |
| PUT | /api/products/{id} | оновити назву та ціну |
| DELETE | /api/products/{id} | видалити товар і його фото |
| GET | /api/products/{id}/photos | фото товару |
| POST | /api/products/{id}/photos | додати кілька фото (поле files) |
| DELETE | /api/products/{id}/photos/{photoId} | видалити фото |

## Обмеження

- формати: jpg, jpeg, png, gif, webp (перевіряється розширення та сигнатура файлу);
- до 5 МБ на файл, до 5 файлів за запит, до 10 фото на товар.
