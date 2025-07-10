# TestcontainersExamples

This repository contains a collection of practical examples demonstrating how to use [Testcontainers](https://testcontainers.com/) with .NET projects. The examples cover a variety of testing strategies and scenarios, including database isolation, API testing, and end-to-end testing with Selenium.

## Repository Structure

- **0 Test isolation strategies/**  
  Examples of different test isolation strategies using Testcontainers, implemented with both xUnit and NUnit. Includes per-test, per-class, per-collection, per-assembly, and per-run isolation patterns for database-backed tests.

- **1 API Tests/**  
  Demonstrates how to use Testcontainers to spin up required services and databases for integration and API tests.

- **2 Selenium examples/**  
  End-to-end testing examples using Selenium WebDriver, including:
  - A sample Angular frontend (`seleniumexample.portal.client`)
  - A .NET backend API (`SeleniumExample.Portal.Server`)
  - Employees service
  - Dockerfiles for running services in containers
  - Selenium-based automated UI tests

## What is Testcontainers?

Testcontainers is a .NET library that supports JUnit-like integration tests by providing lightweight, throwaway instances of common databases, Selenium web browsers, or anything else that can run in a Docker container.

## How to Use

Each folder contains its own solution and project files. You can explore each example independently.  
To run the tests, ensure you have Docker installed and running on your machine.
