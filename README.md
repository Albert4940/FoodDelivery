
# FoodDelivery

## Overview
Application allow users that order the food online.
Has an ASP.NET MVC website for frontoffice using, an API for order processing and a MAUI desktop application for backoffice task, a mobile version for inprove user experience,
Creating a cohesive and versatile solution for efficient operations.

![Screen cast](https://github.com/Albert4940/FoodDelivery/blob/master/media/ScreenApp1.png)

## Technologies Used

- **Git,GitHub**: Utilized for version control and collaborative code management, ensuring efficient tracking of changes and serving as a secure backup.
- **GitHub Project**:Implemented for organized project management, using issues to streamline workflow and monitor progress.
- **ASP.NET MVC**: Used for building the user interface and providing a dynamic user experience.
- **SQLite Database**:  Integrated for managing and storing data within the website application, particularly handling cart information efficiently.
- **ASP.NET API-based Controller**: Employed for handling various functionalities including order processing, user information, and payment information styling.
- **MSSQLLocalDB**: Utilized as the local database management system, being a lightweight version of SQL Server, for efficient data management in the development environment.
- **Entity framework**:  Utilized for data access and manipulation, enhancing the efficiency and scalability of the application's database operations.
- **Azure**: Utilized for deployment, providing a reliable and scalable platform for hosting the application in a cloud environment.
- **BCrypt.Net-Next:**: Used this package for password encryption with salt generation.
- **PayPal**: Integrated for enabling secure and convenient online payment processing within the application.

## Challenges

| Challenge Description                         | Solution                                                                                         |
|-----------------------------------------------|--------------------------------------------------------------------------------------------------|
| Error: The instance of entity type 'Cart' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked| Detaching the DbContext instance after each method execution help prevent conflicts related to tracking instances. |
|Get a specific error message from api not only a simple BadRequest|replace this code : throw new Exception($"{response.StatusCode.ToString()} - {response.ReasonPhrase}"); by this :var errorContent = await response.Content.ReadAsStringAsync();|

## Future Features

- **Translate Language**: Implement a language translation feature, enhancing accessibility and allowing users to view content in their preferred language..
- **Dark Mode**: Implement a night mode feature to enhance user experience in low-light environments.
- **Add Other Payment Method**: Plan to integrate Other payment method like credit card and stripe.
- **Notification**: Send notification by email or phone number when order is delivered
