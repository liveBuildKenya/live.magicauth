# WebAuth Server

Password‑less & Passkey‑first Authentication

_A lightweight, extensible solution enabling WebAuthn workflows._

---

## Table of Contents

- [WebAuth Server](#webauth-server)
  - [Table of Contents](#table-of-contents)
  - [🌟 Features](#-features)
  - [🏛 Architecture](#-architecture)
  - [🛠 Tech Stack](#-tech-stack)
  - [🚀 Getting Started](#-getting-started)
    - [Prerequisites](#prerequisites)
    - [Clone \& Installation](#clone--installation)
  - [🤝 Contributing](#-contributing)
  - [📝 License](#-license)

---

## 🌟 Features

- **WebAuthn / FIDO2**: Full support for passkeys (biometric & hardware tokens)
- **Clean Architecture**: Separation of concerns, generic contracts, and maintainable code
- **Extensible**: Plug in new authentication flows or custom storage providers  

---

## 🏛 Architecture

```
Client (Razor Pages)
   ↓  (REST / JSON)
Backend/API (ASP.NET Core)
├─ Endpoints
├─ Application (Use Cases / Services)
├─ Domain (Entities, Value Objects)
├─ Infrastructure
│  ├─ Persistence (EF Core / Migrations)
│  ├─ WebAuthn FIDO2 Library
└─ Cross‑Cutting (Logging, Config, DI)
```

---

## 🛠 Tech Stack

- **Backend**: .NET 9  
- **ORM**: Entity Framework Core  
- **Front‑End**: Pages (for default templates)  
- **Auth Libraries**: [Fido2.NET](https://github.com/passwordless-lib/fido2-net-lib) for WebAuthn  
- **Database**: PostgreSQL (via EF Core)

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)  
- PostgreSQL instance 

### Clone & Installation

```bash
git clone https://github.com/liveBuildKenya/live.magicauth.git
cd live.magicauth

# Backend
cd src/Live.MagicAuth
dotnet restore

### Database Migrations (optional)

> _Tip: To generate SQL script for review or deployment pipelines:_
> ```bash
> dotnet ef migrations script --output ./deployments/migration.sql
> ```

### Run

```bash
# Launch server
cd src/Live.MagicAuth
dotnet run

The API will listen on `https://localhost:5166` by default.

---

## 🔑 Usage

### WebAuthn / Passkeys

1. **Begin Registration**  
   `POST /api/webauthn/register/begin`  
2. **Complete Registration**  
   `POST /api/webauthn/register/complete`  
3. **Begin Authentication**  
   `POST /api/webauthn/authenticate/begin`  
4. **Complete Authentication**  
   `POST /api/webauthn/authenticate/complete`

---

### Magic Links & One‑Time‑Use Tokens

1. **Request Link**  
   `POST /api/auth/magiclink`  
   ```json
   { "email": "user@example.com" }
   ```
2. **Validate Token**  
   `GET /api/auth/magiclink/callback?token=<TOKEN>`

---

## 🤝 Contributing

1. Fork this repo  
2. Create your feature branch (`git checkout -b feature/foo`)  
3. Commit your changes (`git commit -am 'Add foo'`)  
4. Push to the branch (`git push origin feature/foo`)  
5. Open a Pull Request  

Please follow our [code style guide](CONTRIBUTING.md) and add tests for all new functionality.

---

## 📝 License

This project is licensed under the **MIT License**. See [LICENSE](LICENSE) for details.
