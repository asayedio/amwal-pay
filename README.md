# Payment Gateway Solution

## Table of Contents

- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Features Implemented](#features-implemented)
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
  - [Backend (PaymentGateway.API)](#backend-paymentgatewayapi)
    - [Running with .NET CLI](#running-with-net-cli)
    - [Running with Docker](#running-with-docker)
  - [Frontend (payment-client)](#frontend-payment-client)
    - [Running with Angular CLI](#running-with-angular-cli)
- [Usage Instructions](#usage-instructions)
- [Notes](#notes)
- [Conclusion](#conclusion)

---

## Overview

This solution consists of a **Payment Gateway** application that processes payment transactions through a RESTful API and a client application. The project is divided into two main parts:

- **Backend**: A **.NET Core 8** Web API following **Clean Architecture** principles.
- **Frontend**: An **Angular 16** web application that interacts with the backend API.

The application demonstrates secure communication by implementing encryption middleware, user authentication, and data processing without the need for database integration.

---

## Technologies Used

- **Backend**:
  - .NET Core 8
  - C# 10
  - ASP.NET Core Web API
  - Clean Architecture principles
  - AES Encryption
- **Frontend**:
  - Angular 16
  - TypeScript
  - CryptoJS (for encryption)
- **Containerization**:
  - Docker

---

## Project Structure

### Backend (`PaymentGatewaySolution`)

- **PaymentGateway.Domain**: Contains domain entities and core business logic.
- **PaymentGateway.Application**: Contains application services and business use cases.
- **PaymentGateway.API**: Contains API controllers, middleware, and presentation logic.

### Frontend (`payment-client`)

- Angular application with components, services, and routing.
- Implements transaction form and communicates with the backend API.

---

## Features Implemented

- **RESTful API Endpoint**: Receives encrypted payment transaction data and processes it.
- **Encryption Middleware**: Handles encryption and decryption of requests and responses.
- **Transaction Processing**: Generates approval codes and returns transaction responses.
- **User Authentication**: Basic authentication flow in the frontend application.
- **Data Encryption**: All data between the client and server is encrypted.
- **BCD Encoding**: Numeric fields are encoded using Binary-Coded Decimal to minimize data length.
- **Clean Architecture**: Separation of concerns across different layers in the backend.
- **No Database Integration**: The application processes data in-memory without persisting to a database.

---

## Prerequisites

- **Node.js** (v16 or higher)
- **Angular CLI** (v16)
- **.NET SDK** (v8.0)
- **Docker** (optional, for containerization)
- **Git** (optional, for cloning the repository)

---

## Setup Instructions

### Backend (`PaymentGateway.API`)

#### Running with .NET CLI

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/asayedio/amwal-pay.git
   cd cloned-directory
   ```

2. **Navigate to the API Project**:

   ```bash
   cd PaymentGateway.API
   ```

3. **Restore Dependencies**:

   ```bash
   dotnet restore
   ```

4. **Build the Project**:

   ```bash
   dotnet build
   ```

5. **Run the API**:

   ```bash
   dotnet run
   ```

6. **API Endpoint**:

   The API will be running at `https://localhost:5001` or `http://localhost:5000`.

#### Running with Docker

1. **Navigate to the Solution Directory**:

   ```bash
   cd cloned-directory
   ```

2. **Build the Docker Image**:

   ```bash
   docker build -t paymentgatewayapi -f PaymentGateway.API/Dockerfile .
   ```

3. **Run the Docker Container**:

   ```bash
   docker run -d -p 5000:80 -p 5001:443 --name paymentgatewayapi paymentgatewayapi
   ```

4. **API Endpoint**:

   The API will be accessible at `http://localhost:5000` and `https://localhost:5001`.

---

### Frontend (`payment-client`)

#### Running with Angular CLI

1. **Navigate to the Client Project**:

   ```bash
   cd payment-client
   ```

2. **Install Dependencies**:

   ```bash
   npm install
   ```

3. **Run the Application**:

   ```bash
   ng serve
   ```

4. **Access the Application**:

   Open your browser and navigate to `http://localhost:4200`.

5. **Build the Angular Application**:

   ```bash
   ng build --prod
   ```

#### Login Page Credentials:

##### Username: admin

##### Password: 123

---

## Usage Instructions

1. **Access the Frontend Application**:

   Navigate to `http://localhost:4200`.

2. **Transaction Form**:

   - **Fields to Fill**:
     - `ProcessingCode`: e.g., `999000`
     - `SystemTraceNr`: e.g., `36`
     - `FunctionCode`: e.g., `1324`
     - `CardNo`: e.g., `4712345601012222`
     - `CardHolder`: e.g., `Ahmed Mohamed`
     - `AmountTrxn`: e.g., `1000`
     - `CurrencyCode`: e.g., `840`
   - **Submit the Form**: Click the **Submit** button.

3. **Data Encryption**:

   - The client application requests an encryption key from the backend API.
   - Transaction data is encrypted using AES encryption before being sent to the server.
   - Numeric fields are encoded using BCD to minimize data length.

4. **Backend Processing**:

   - The backend middleware decrypts the incoming request.
   - The transaction is processed, generating a 6-digit approval code.
   - A response is created and encrypted before being sent back to the client.

5. **View the Response**:

   - The client application decrypts the response.
   - Transaction response details are displayed, including:
     - `ResponseCode`: `00`
     - `Message`: `Success`
     - `ApprovalCode`: e.g., `123456`
     - `DateTime`: e.g., `202102261200`

---

## Notes

- **Encryption Key Exchange**: In this implementation, the encryption key is generated by the server and provided to the client.
- **HTTPS**: Ensure that both the API and the client application are served over HTTPS in a production environment to secure data in transit.
- **Authentication**: Basic authentication logic is included in the frontend but may need to be connected to a real authentication system.
- **Ports**: Adjust port mappings in Docker commands if the default ports are in use or need to be changed.
- **CORS**: If you encounter cross-origin issues, configure CORS policies in the backend API.

---

## Conclusion

This Payment Gateway solution demonstrates secure communication between an Angular frontend and a .NET Core backend API, following Clean Architecture principles. By implementing encryption middleware and BCD encoding, the application ensures data security and efficiency without relying on database integration.

---

If you have any questions or need further assistance, please feel free to contact the project maintainers.
