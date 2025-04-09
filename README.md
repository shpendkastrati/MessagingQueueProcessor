# Messaging Queue Processor

## Overview
This .NET 8 application simulates processing messages in an Application-to-Person (A2P) platform. It demonstrates:
- An in-memory message queue (using a producer/consumer pattern)
- Support for multiple message types (SMS, Email, Push Notification)
- Asynchronous message processing with simulated external API delays and failures
- Persistence using Entity Framework Core with SQLite to enable recovery
- Structured logging with Serilog and basic API endpoints for monitoring
- Docker containerization for easy local deployment

## Features
- **Message Queue:** In-memory queue implementation with thread-safe operations.
- **API Layer:** REST endpoints for submitting messages, querying message status, and retrieving statistics.
- **Message Processing:** A background hosted service processes messages asynchronously, with retry logic, exponential backoff, and a dead-letter mechanism.
- **Persistence:** Messages are persisted in SQLite via EF Core to enable recovery after application restarts.
- **Logging & Monitoring:** Integrated Serilog for structured logging and a statistics API endpoint.
- **Resilience:** Implements retry policies for failed external API simulations.
- **Docker:** A Dockerfile is provided to containerize the application.

## Setup Instructions

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/shpendkastrati/MessagingQueueProcessor.git
   cd MessagingQueueProcessor
