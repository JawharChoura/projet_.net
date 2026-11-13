# MonResto

MonResto est une application de gestion et de commande de restaurant composée de deux projets .NET :

- **API** : API REST ASP.NET Core avec authentification JWT, Entity Framework Core, SQL Server et Swagger.
- **Front** : interface cliente Blazor WebAssembly pour consulter les articles, gérer le panier et passer des commandes.

## Fonctionnalités

- Inscription et connexion des utilisateurs.
- Gestion des catégories et des articles.
- Consultation du menu.
- Gestion du panier.
- Création et consultation des commandes.
- Fonctionnalités d'administration pour les articles, les catégories et les commandes.
- Documentation interactive de l'API avec Swagger.

## Technologies

- .NET 9
- ASP.NET Core Web API
- Blazor WebAssembly
- Entity Framework Core 9
- SQL Server / LocalDB
- ASP.NET Core Identity
- JWT
- Bootstrap

## Prérequis

Installer les outils suivants :

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server ou SQL Server LocalDB
- Visual Studio 2022 ou un éditeur compatible .NET

Vérifier l'installation du SDK :

```bash
dotnet --version
```

## Configuration

La chaîne de connexion et les paramètres JWT se trouvent dans [API/appsettings.json](API/appsettings.json).

Par défaut, l'API utilise la base LocalDB suivante :

```text
Server=(localdb)\MSSQLLocalDB;Database=MonRestoDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true
```

Avant une utilisation en production :

- remplacer la chaîne de connexion par une valeur adaptée à l'environnement ;
- définir la clé JWT via une variable d'environnement ou un gestionnaire de secrets ;
- remplacer les valeurs de développement présentes dans la configuration ;
- vérifier les origines autorisées par CORS.

## Installation et lancement

Depuis la racine du dépôt, restaurer les dépendances :

```bash
dotnet restore API/API.csproj
dotnet restore Front/Front.csproj
```

### 1. Lancer l'API

```bash
dotnet run --project API/API.csproj --launch-profile http
```

L'API est disponible à l'adresse `http://localhost:5162`.

Swagger est accessible ici :

<http://localhost:5162/swagger>

Les migrations Entity Framework Core sont appliquées automatiquement au démarrage de l'API.

### 2. Lancer le frontend

Dans un second terminal :

```bash
dotnet run --project Front/Front.csproj --launch-profile http
```

Le frontend est disponible à l'adresse `http://localhost:5122`.

Le frontend utilise l'API configurée dans [Front/Program.cs](Front/Program.cs). Si l'API est exécutée sur une autre adresse, mettre à jour la valeur `BaseAddress`.

## Structure du projet

```text
.
├── API/
│   ├── Controllers/       # Contrôleurs REST
│   ├── Data/              # Contexte Entity Framework Core
│   ├── DTOs/              # Objets de transfert
│   ├── Migrations/        # Migrations de base de données
│   ├── Models/            # Entités métier
│   └── Respositories/     # Interfaces et implémentations des dépôts
└── Front/
    ├── Models/            # Modèles utilisés par l'interface
    ├── Pages/             # Pages Blazor
    ├── Services/          # Services d'appel à l'API
    └── Layout/            # Layout et navigation
```

## API principale

Les contrôleurs sont accessibles sous le préfixe `api` :

- `api/Account` : inscription et connexion ;
- `api/Articles` : consultation et gestion des articles ;
- `api/Categories` : consultation et gestion des catégories ;
- `api/Cart` : gestion du panier ;
- `api/Orders` : création et gestion des commandes.

Les routes protégées nécessitent un jeton JWT dans l'en-tête :

```text
Authorization: Bearer <token>
```

## Développement

Compiler les deux projets :

```bash
dotnet build API/API.csproj
dotnet build Front/Front.csproj
```

Pour créer une nouvelle migration :

```bash
dotnet ef migrations add NomDeLaMigration --project API/API.csproj
```

Puis appliquer manuellement la base si nécessaire :

```bash
dotnet ef database update --project API/API.csproj
```

## Sécurité

Les valeurs de configuration incluses dans le dépôt sont destinées au développement local. Ne pas utiliser de clé JWT, de mot de passe ou de chaîne de connexion de développement en production. Utiliser plutôt les variables d'environnement, les User Secrets ou un coffre de secrets.
