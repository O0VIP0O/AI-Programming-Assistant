# 🧠 AI Programming Assistant & Intelligent Learning Platform

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core_MVC-8.0-blue?logo=dotnet)](https://dotnet.microsoft.com/apps/aspnet/mvc)
[![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-8.0-512BD4?logo=nuget)](https://docs.microsoft.com/ef/core/)
[![Microsoft SQL Server](https://img.shields.io/badge/SQL_Server-CC292B?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Ollama Local LLM](https://img.shields.io/badge/Ollama-Qwen2.5--Coder-black?logo=ollama&logoColor=white)](https://ollama.com/)
[![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?logo=tailwind-css&logoColor=white)](https://tailwindcss.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

> An enterprise-grade, **Full-Stack AI-powered Developer Platform** that provides automated line-by-line code explanation, multimodal bug diagnosis, and an adaptive, gamified computer science curriculum powered by **.NET 8**, **Entity Framework Core**, and local **Large Language Models (Qwen2.5-Coder via Ollama)**.

---

## 📌 Table of Contents
- [Overview](#-overview)
- [Key Features](#-key-features)
- [System Architecture](#-system-architecture)
- [Tech Stack](#-tech-stack)
- [Database Schema & Data Model](#-database-schema--data-model)
- [AI Engine & Prompt Engineering](#-ai-engine--prompt-engineering)
- [Project Directory Structure](#-project-directory-structure)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation & Setup](#installation--setup)
- [API Endpoints](#-api-endpoints)
- [SRE, Reliability & Optimization](#-sre-reliability--optimization)
- [Contributing & License](#-contributing--license)

---

## 🚀 Overview

The **AI Programming Assistant** is designed to bridge the gap between complex software engineering concepts and emerging developers. Rather than acting as a simple chatbot wrapper, this system integrates deeply with local generative AI inference pipelines, an enterprise relational database, and an interactive modern IDE-like interface to provide:

1. **Instant, Context-Aware Code Mentorship**: Line-by-line pedagogical breakdown and bug remediation.
2. **Multimodal Analysis**: Ability to inspect code from raw text or uploaded IDE screenshots.
3. **Automated Curriculum Generation**: On-the-fly interactive lessons and customized quizzes cached at the database level for instant retrieval.
4. **Gamified Student Tracking**: Unlockable progression tree across multiple programming languages (Python, C#, JavaScript, C++, etc.).

---

## ✨ Key Features

### 1. 🔍 AI Code Explainer & Reviewer
* **Beginner-Friendly Pedagogical Breakdown**: Analyzes syntax, control flow, and data structures, explaining functionality step-by-step.
* **Bug Detection & Auto-Remediation**: Detects logical flaws, anti-patterns, and syntax errors, outputting a cleaned, corrected snippet.
* **Multimodal Image-to-Code OCR**: Ingests screenshots of code from any IDE or terminal, passing visual embeddings directly to the multimodal model.
* **Side-by-Side Visual Diff**: Modern VS Code-inspired dark UI with syntax highlighting and clear separation between diagnosis and corrected code.

### 2. 🎓 Adaptive Learning & Dynamic Curriculum Engine
* **On-Demand Lesson Generation**: Dynamically prompts a local LLM acting as a *Senior Computer Science Instructor* to generate comprehensive HTML-formatted lessons.
* **Automated Assessment & Quizzes**: Synthesizes 10-question multiple-choice exams with verifiable correct answers formatted as strict JSON.
* **Intelligent Database Caching**: Every generated level is permanently persisted in SQL Server; subsequent requests bypass the LLM entirely, reducing response latency from ~8s to <50ms.

### 3. 📈 Gamified Progression & User State
* **Unlockable Skill Tiers**: Users progress from Level 1 upwards; higher difficulty modules remain locked until passing evaluations.
* **Multi-Language Mastery**: Independent skill level tracking for each programming language per user.
* **Persistent Sessions**: User authentication with secure ASP.NET Core session state.

---

## 🛠️ Tech Stack

| Domain | Technologies & Libraries |
| :--- | :--- |
| **Backend & Framework** | .NET 8.0, C#, ASP.NET Core MVC, Dependency Injection, RESTful APIs |
| **ORM & Database** | Entity Framework Core 8, Microsoft SQL Server, LINQ, Code-First / DbContext |
| **AI & Inference** | Ollama Engine, `qwen2.5-coder:3b`, Multimodal Vision, System Prompt Engineering |
| **Frontend UI/UX** | Razor Views (.cshtml), Tailwind CSS, Bootstrap 5, Lucide Icons, Google Fonts |
| **Client-Side Scripting**| JavaScript (ES6+), Fetch API, DOM Manipulation, JSON Serialization |
| **System Reliability** | `IHttpClientFactory`, Circuit Breaker & Timeout Safeguards (5m), HTTP 503/504 Fallbacks |

---

## 📁 Project Directory Structure

```plaintext
AI Programming Assistant/
│
├── Controllers/
│   ├── Code_Explainer.cs          # Orchestrates AI code review & image analysis
│   ├── Learning.cs                # Handles curriculum generation, exams & caching
│   ├── login_create_account.cs    # User authentication & session initialization
│   ├── landing_page.cs            # Marketing landing page controller
│   └── HomeController.cs          # Student dashboard & language progress
│
├── Models/
│   ├── AiProgrammingAssistantContext.cs # EF Core DbContext definition
│   ├── User.cs                          # Application user entity
│   ├── ProgrammingLanguage.cs           # Supported programming languages
│   ├── UsersProgrammingLanguage.cs      # User level progress per language
│   ├── LearningContent.cs               # Cached HTML lessons and exam JSON
│   ├── CodeExplainer.cs                 # DTO for code/image analysis requests
│   └── ErrorViewModel.cs                # Diagnostic error model
│
├── Views/
│   ├── Code_Explainer/                  # Dark-mode IDE code explainer interface
│   ├── Learning/                        # Interactive curriculum & quiz interface
│   ├── Home/                            # User progress dashboard
│   ├── landing_page/                    # Public entrance page
│   ├── login_create_account/            # Authentication forms
│   └── Shared/                          # Layout and partial validation scripts
│
├── wwwroot/                             # Static assets (Tailwind, Bootstrap, JS, Icons)
├── appsettings.json                     # Database connection strings & logging config
└── Program.cs                           # Service registration, DI, and middleware pipeline
```

---

## ⚙️ Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher.
* [Microsoft SQL Server](https://www.microsoft.com/sql-server) or SQL Server LocalDB.
* [Ollama](https://ollama.com/) installed and running locally.

### Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/YourUsername/AI-Programming-Assistant.git
   cd "AI Programming Assistant/AI Programming Assistant"
   ```

2. **Pull the AI Model via Ollama:**
   ```bash
   ollama pull qwen2.5-coder:3b
   ```
   *(Ensure Ollama is running at `http://localhost:11434`)*

3. **Configure the Database Connection:**
   Open `appsettings.json` and adjust your SQL Server connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=AiProgrammingAssistantDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. **Apply Migrations / Initialize Database:**
   ```bash
   dotnet ef database update
   ```

5. **Build and Run the Application:**
   ```bash
   dotnet run
   ```
   Navigate to `https://localhost:7124` or `http://localhost:5124` in your browser.

---

## 🌐 API Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/Code_Explainer/getcurrentExplanation_currentFixedCode` | Submits code text or base64 screenshot for AI analysis and auto-fix. |
| `POST` | `/Learning/GetContent` | Retrieves or generates a specific level lesson and exam for a language. |
| `POST` | `/Learning/UpdateProgress` | Updates user progression upon passing exam requirements. |
| `POST` | `/login_create_account/login_create_account` | Validates credentials and initializes user session. |
| `POST` | `/login_create_account/Register` | Registers a new account and initializes default skill tracking. |

---

## 🛡️ SRE, Reliability & Optimization

* **In-Database Lesson Caching**: Drastically reduces GPU/CPU utilization by querying SQL Server first before dispatching requests to Ollama.
* **Fault-Tolerant HTTP Client**: Utilizes `IHttpClientFactory` to prevent socket exhaustion and implements a 5-minute timeout window with HTTP 503/504 fallback handling.
* **Strict Type Safety**: Relies on C# strong typing and Entity validations to guarantee zero unexpected runtime crashes during payload serialization.

---

## 📄 License
This project is open-source under the [MIT License](LICENSE).
