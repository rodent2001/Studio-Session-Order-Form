# Studio Session Order Form

### v0.2.0 — Basic MVVM refactor

- Added `MainWindowViewModel`
- Moved form state from `MainWindow.xaml.cs` to ViewModel
- Moved validation logic to ViewModel
- Moved price calculation logic to ViewModel
- Moved Save and Reset workflows to ViewModel
- Reduced `MainWindow.xaml.cs` to thin code-behind
- Preserved existing UI behavior

### v0.1.0 — Initial working version

- Basic WPF order form
- Customer, session type and duration selection
- Price per hour, discount and urgent surcharge handling
- Total cost calculation
- Validation warning borders

## Overview

Studio Session Order Form is a small educational WPF application that simulates creating a music studio session.
The app allows users to select session options, apply discounts or urgent booking surcharges, and calculate the final session price.

## Features
- Customer selection
- Session type selection
- Session duration selection
- Automatic hourly rate calculation
- Manual hourly rate input
- Manual discount input
- Urgent session surcharge
- Final price calculation

## Tech Stack
- C#
- WPF
- Visual Studio 2026

## How to Run
1. Open the project in Visual Studio 2026.
2. Build the project.
3. Run the application.

## Screenshots

![Start screen](screenshots/_msof0.png)
![Some input mistake](screenshots/_msof1.png)
![Order saved](screenshots/_msof2.png)
