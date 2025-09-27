# 🖥️ Equipment Management System

### A Software Engineering Course Project

The **Equipment Management System (EQMS)** is a desktop-based application designed to help our university, University of Cebu - Lapu-Lapu and Mandaue, efficiently track, manage, and maintain their IT equipment and inventory. It provides tools for monitoring asset information, tracking equipment status, and generating reports — ensuring transparency and accountability in resource management.

---

## 📘 Table of Contents
- [Overview](#-overview)
- [Features](#-features)
- [System Architecture](#-system-architecture)
- [Tech Stack](#-tech-stack)
- [Installation](#-installation)
- [Usage](#-usage)
- [Screenshots](#-screenshots)
- [Future Improvements](#-future-improvements)
- [License](#-license)

---

## 📖 Overview
The **Equipment Management System** was developed as part of our **Software Engineering** course to demonstrate proficiency in **system design, database integration, and user interface development**.

The system helps organizations:
- Keep a centralized record of all IT assets.
- Track each equipment’s location, condition, and assigned user.
- Generate reports for auditing and inventory management.

---

## ⚙️ Features
✅ Equipment registration and categorization  
✅ User and admin authentication  
✅ Real-time equipment tracking and status updates  
✅ Search and filter functionality  
✅ Generate and export inventory reports  
✅ Equipment lending and return history  
✅ Maintenance schedule tracking  
✅ Responsive, user-friendly interface  

---

## 🧩 System Architecture
**Client Layer:** User interface for managing and viewing data  
**Application Layer:** Business logic for equipment, users, and transactions  
**Database Layer:** Stores all records and relationships  

```

[ User Interface ]
↓
[ Application Logic / API ]
↓
[ Database (SQL Server) ]

````

---

## 💻 Tech Stack
- **Frontend:** C# WPF
- **Backend:** ADO.NET, SQL
- **Database:** Microsoft SQL Server  
- **Tools:** Visual Studio / VS Code, SSMS, GitHub, Figma
- **Version Control:** Git  

---

## 🧰 Installation

1. **Clone the repository**
```bash
   git clone https://github.com/earlfranciss/Equipment-Management-System.git
   cd Equipment-Management-System
````
2. **Set up the database**

   * Open SQL Server Management Studio
   * Run the provided `.sql` file to create the database and tables

3. **Configure the backend**

   * Update the `appsettings.json` with your SQL connection string
   * Run the project in Visual Studio

4. **Build and run the project**

   * Open the .sln file in Visual Studio
   * Press F5 or click Start Debugging to run the app

---

## 🚀 Usage

* **Admin Login:** Manage users, equipment records, and reports
* **User Login:** View and request equipment, check assigned items
* **Reports:** Export inventory summaries for auditing

---

## 🖼️ Screenshots

* Login Page
  
    <img width="512" height="297" alt="unnamed" src="https://github.com/user-attachments/assets/9c8e126a-af31-4718-ac87-ad9f1ea5a6a0" />
 
* Register Page
   
    <img width="512" height="303" alt="unnamed (1)" src="https://github.com/user-attachments/assets/d730413e-384a-42e6-923a-0b41428adc57" />
 
* Dashboard
   
    <img width="512" height="301" alt="unnamed (2)" src="https://github.com/user-attachments/assets/e9bbba02-c264-421b-bff8-41e66886ae37" />
 
* Reservations
   
    <img width="512" height="303" alt="unnamed (4)" src="https://github.com/user-attachments/assets/8b887c25-a411-442f-99f8-ef992b128a2f" />
 
* Reservation Form
   
    <img width="512" height="307" alt="unnamed (3)" src="https://github.com/user-attachments/assets/008fd935-1e15-4453-911d-2556f47f7b10" />


---

## 🔮 Future Improvements

* Implement barcode or QR code scanning
* Add role-based permissions (e.g., IT staff, department head)
* Integrate email notifications for maintenance schedules
* Cloud deployment for remote access

---


## 📜 License

This project was created for educational purposes under the **Software Engineering** course.

© 2025 Group EQMS – All rights reserved.


