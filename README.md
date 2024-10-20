# PassionProject - Car Showroom Management System

## Overview

PassionProject is a car showroom management system designed to manage cars, owners, and staff members. It provides APIs for creating, reading, updating, and deleting (CRUD) data related to the cars, owners, and staff of the showroom. The project makes use of ASP.NET Core for API development and follows a service-oriented architecture to handle the core logic.

## Features

- **Car Management:** CRUD operations for cars, including details such as make, model, year, and associated owner.
- **Owner Management:** CRUD operations for owners, including first name, last name, and contact information.
- **Staff Management:** CRUD operations for staff members, including first name, last name, position, and managed cars.
- **Relationship Management:** 
  - Many-to-many relationship between cars and staff members.
  - One-to-many relationship between owners and cars.

## Project Structure
### Controllers/

- CarsAPIController.cs
- OwnerController.cs
- StaffAPIController.cs

### Data/
 **Interface/**

- ICarService.cs
- IOwnerService.cs
- IStaffService.cs

**Models/**
- Car.cs
- CarDto.cs
- Owner.cs
- OwnerDto.cs
- Staff.cs
- StaffDto.cs

**Services/**
- CarService.cs
- OwnerService.cs
- StaffService.cs

### API Endpoints

#### Cars API

- **GET** `/api/CarsAPI/List` - Returns a list of all cars.
- **GET** `/api/CarsAPI/{id}` - Returns details of a specific car by ID.
- **POST** `/api/CarsAPI/Add` - Adds a new car.
- **PUT** `/api/CarsAPI/Update/{id}` - Updates the details of a specific car by ID.
- **DELETE** `/api/CarsAPI/Delete/{id}` - Deletes a specific car by ID.

#### Owner API

- **GET** `/api/Owner/List` - Returns a list of all owners.
- **GET** `/api/Owner/{id}` - Returns details of a specific owner by ID.
- **POST** `/api/Owner/Add` - Adds a new owner.
- **PUT** `/api/Owner/{id}` - Updates the details of a specific owner by ID.
- **DELETE** `/api/Owner/{id}` - Deletes a specific owner by ID.

#### Staff API

- **GET** `/api/StaffAPI/List` - Returns a list of all staff members.
- **GET** `/api/StaffAPI/Find/{id}` - Returns details of a specific staff member by ID.
- **POST** `/api/StaffAPI/Add` - Adds a new staff member.
- **PUT** `/api/StaffAPI/Update/{id}` - Updates the details of a specific staff member by ID.
- **DELETE** `/api/StaffAPI/Delete/{id}` - Deletes a specific staff member by ID.

### Technology Stack

- **Backend:** ASP.NET Core 6.0 (Web API)
- **Dependency Injection:** Services (ICarService, IOwnerService, IStaffService)
