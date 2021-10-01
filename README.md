# Feedback
## Database Design
### Collections
![alt text](https://i.imgur.com/DyCuMn0.png)
### Indices
To ensure that we don't have two feedbaks with same UserId and SessionId, I created the following index:

Unique_UserId_SessionId
![alt text](https://i.imgur.com/JiX5ttJ.png)

## Api documentation
For that we have swagger. If you start the service a new window will pop-up with a swagger instance that will detail the routes and the payload.
![alt text](https://i.imgur.com/Kb0srvM.png)
![alt text](https://i.imgur.com/BN23Qu9.png)

## Run the project and tests
It's needed to have .Net Core 5 and MongoDB installed. To start the project you can use Visual Studio and IIS Express. By default it points to a local instance of MondoDB (mongodb://localhost:27017) and the database name is Feedback.

To run the tests just click on "Run all tests" in Visual Studio. Tests are splitted in different projects, one fore each layer of the service and another one for integration tests.
We have a total of 18 tests that will ensure that the service works as expected.
![alt text](https://i.imgur.com/j2iAsAj.png)
