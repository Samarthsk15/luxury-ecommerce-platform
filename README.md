# Order and Product Management System

A full-stack e-commerce management app with:

- Vue 3 + Vite SPA frontend
- ASP.NET Core Web API backend
- SQLite database with EF Core
- JWT authentication and role-based authorization
- Product management and order creation features

## Setup

1. Open the workspace in Visual Studio Code.
2. Install backend dependencies:
   ```powershell
   cd "c:\Users\Darshan Kulkarni\Desktop\8th sem\backend\OrderProductApi"
   dotnet restore
   ```
3. Install frontend dependencies:
   ```powershell
   cd "c:\Users\Darshan Kulkarni\Desktop\8th sem\frontend"
   npm install
   ```

## Run

- Start the backend API:
  ```powershell
  cd "c:\Users\Darshan Kulkarni\Desktop\8th sem\backend\OrderProductApi"
  dotnet run
  ```

- Start the frontend app:
  ```powershell
  cd "c:\Users\Darshan Kulkarni\Desktop\8th sem\frontend"
  npm run dev
  ```

## Default credentials

- Admin user: `admin@shop.local`
- Password: `Admin@123`

## Notes

- API listens on `http://localhost:5000`
- Frontend assumes the backend is available at `http://localhost:5000/api`
