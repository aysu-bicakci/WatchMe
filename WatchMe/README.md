# Watch Me: Online Movie and TV Show Archive

## Overview
**Watch Me** is an online platform where users can create a personalized archive of movies and TV shows they have watched. The application allows users to mark content as watched, express their preferences by liking or disliking it, and leave comments. Profiles display the user's watched, liked, disliked, and commented content. The project is built using .NET MVC and integrates modern API technologies to enhance user experience.

## Key Features

- **User Profiles**:
  - View watched, liked, disliked, and commented content.
  - Edit or delete comments and preferences.

- **Movie and TV Show Browsing**:
  - Explore content categorized by genre.
  - Navigate to genre-specific pages to see related content.

- **Guest Access**:
  - Guests can browse content but cannot like, comment, or view a profile.

- **Interactive Features**:
  - Mark movies and TV shows as watched.
  - Indicate likes or dislikes for watched content.

- **API Integrations**:
  - Fetch content details using TMDb API.
  - Use SOAP and gRPC protocols for advanced communication between services.

## Technologies and Architecture

### Backend
- Framework: .NET MVC
- APIs: REST, SOAP, gRPC
- Database: PostgreSQL

### APIs and Integrations
- **TMDb API** for fetching movie and TV show details.
- **SOAP API** for fetching detailed movie data.

### Tools and Libraries
- **Version Control**: Git
- **Development Environment**: Visual Studio Code
- **UI/UX**: HTML, CSS, JavaScript (Razor views)

## Installation

### Prerequisites
Ensure the following are installed:
- .NET 8+
- PostgreSQL

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/Dilara-Selin/WatchMe.git
   ```
2. Navigate to the project directory:
   ```bash
   cd WatchMe
   ```
3. Set up the PostgreSQL database:
   - Create a database named `watch_me`.
   - Run the provided migration scripts located in the `database/migrations` folder.
4. Build and run the .NET MVC application:
   ```bash
   dotnet build
   dotnet run
   ```

### SOAP Service Setup
1. Ensure the SOAP API runs on port `5003`.
2. Test the connection by fetching sample data via `http://localhost:5003/wsdl`.

## Usage

1. Register or log in to the application.
2. Browse the home page to discover movies and TV shows.
3. Click on a genre to see content within that category.
4. Mark content as "watched" and express your preferences by liking or disliking it.
5. Leave comments and view your activity on your profile page.

## Customization

- **Add Genres**: Update the `genres` list in the database or TMDb API configurations.
- **API Endpoints**: Modify or extend controllers and services for additional functionality.
- **UI Enhancements**: Customize Razor views for improved design and usability.


## Contributing

We welcome contributions to improve this project! Here are some ways you can contribute:
- Report bugs and suggest features via GitHub Issues.
- Submit pull requests for new features, bug fixes, or improvements.
- Improve documentation to help others understand and use the project.



