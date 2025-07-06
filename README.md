# Mediva - Graduation Project 🏥

This is the backend of our graduation project **Mediva**, a chronic disease management system.  
I was responsible for implementing the **Authentication & Authorization** part of the system using **ASP.NET Core Web API**.

---

## 🔐 Authentication Features

I built and implemented a complete authentication system including:

- **User Registration** with email and password
- **Login** with JWT (JSON Web Token)
- **Email Confirmation** via SendGrid
- **Password Reset** (Forget & Reset Password APIs)
- **Role-based Authorization** (Admin, Patient, Doctor, etc.)
- **Secure Endpoints** protected with `[Authorize]` attribute
- **Token Validation & Refresh Token Support**

---

## 🛠️ Technologies Used

- ASP.NET Core 8 Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- SendGrid API (for email confirmation & password reset)
- AutoMapper & Data Transfer Objects (DTOs)

---

## 📁 Project Structure

```
Mediva.API/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── ForgetPasswordController.cs
│   └── ResetPasswordController.cs
│
├── DTOs/
│   ├── RegisterDto.cs
│   ├── LoginDto.cs
│   ├── ResetPasswordDto.cs
│   └── EmailConfirmationDto.cs
│
├── Services/
│   └── AuthService.cs
│
├── Helpers/
│   └── JwtTokenGenerator.cs
│
└── Program.cs
```

---

## 📫 API Endpoints (Sample)

| Method | Endpoint                  | Description             |
|--------|---------------------------|-------------------------|
| POST   | `/api/auth/register`      | Register new user       |
| POST   | `/api/auth/login`         | Login & get JWT token   |
| POST   | `/api/auth/confirm-email` | Confirm email via token |
| POST   | `/api/auth/forget-password` | Send reset link       |
| POST   | `/api/auth/reset-password`  | Reset password         |

---

## 👨‍💻 Author

**Backend Developer:** Ghbalat  
Responsible for: Full authentication module, JWT integration, secure role-based access, and email confirmation setup.

---

## 📌 Notes

- Admin roles and users are seeded automatically.
- Swagger UI available for testing endpoints.
- Tokens expire after X minutes and are securely signed.

---

## 🔗 GitHub Repository

> 📍 [GitHub Repo Link]( https://github.com/ElgharibAhmed091/AuthApiGradu.git)
