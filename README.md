# DRK Web - API

## Run the App:

docker-compose up -d db

docker-compose up -d csharp_backend


Login:
- Email: superadmin@drk.de
- Password: SuperAdminPasswort


## Swagger Interface:

{baseUrl}/swagger/index.html


# To-Dos

- Authorization
- Services: Aktionen aus Protokollen, Messages (Password Change Required, Protocol reviewed,...), ...
- Unit & Integration Tests
- Compose: depends-on
          db:
             condition: service_healthy