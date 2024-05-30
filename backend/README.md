# Backend DRK Web-App

Run all:

docker-compose up -d


## 1. Postgres Database

docker-compose up -d db

#### DBMS System

docker-compose up -d pgadmin

Port: 15432

Login:
- Email: superadmin@drk.de
- Password: A9UZlRyKmeTxRFfGcPo6M67r


## 2. C# Backend: Sebastian

### Run the app:

docker-compose up -d csharp_backend


### Swagger Interface:

{baseUrl}/swagger/index.html

Login:
- Email: superadmin@drk.de
- Password: SuperAdminPasswort


## To-Do's

- Statistiken
- Auth für GET-Endpunkte (Welche?) + Testen

Nach Fertigstellung:
- Seeding anpassen
- Environment zu Production
- SendMailFromProtocol URL-Pfad anpassen