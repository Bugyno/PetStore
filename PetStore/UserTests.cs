using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.IO;
using System.Text;

namespace PetStore
{
    [TestClass]
    public class addNewUser
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Successfull adding new user -> expecting response 200
        [TestMethod]
        public void addUser()
        {
            string response = methods.addObject(variables.userURLPath);
            // Verifiying reponse
            if (!response.Contains("200"))
            {
                Assert.Fail(response);
            }
        }
    }

    [TestClass]
    public class updateUser
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Successfull updating user -> expecting response 200
        [TestMethod]
        public void updateExistingUserPassword()
        {
            string response = methods.updateObject(variables.userURLPath, variables.username);

            // Verifiying reponse
            if (!response.Contains("200"))
            {
                Assert.Fail(response);
            }
        }
    }

    [TestClass]
    public class removeUser
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Remove existing user -> expacting result 200
        [TestMethod]
        public void removeExistingUser()
        {
            // Calling remove method and storing response
            string response = methods.removeObject(variables.userURLPath, variables.username);
            
            // Verifying reponse
            if (!response.Contains("200"))
            {
                Assert.Fail(response);
            }
        }
    }

    [TestClass]
    public class getInfoAboutUser
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();

        // Expecting result: User not found. Response 404
        [TestMethod]
        public void getUserInfo()
        {
            // Creating Client connection and request to get data from server
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest(variables.userURLPath + variables.username, Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            // Verifiying reponse
            if (!response.Contains("User not found"))
            {
                Assert.Fail(response + "User exists.");
            }
        }

        // Expecting result: Invalid username supplied. Response 400
        [TestMethod]
        public void getUnknownUserInfo()
        {
            // Creating Client connection and request to get data from server
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest(variables.userURLPath + variables.nonExistingUserName, Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            // Verifiying reponse
            if (!response.Contains("Invalid username supplied"))
            {
                Assert.Fail(response + "User exists.");
            }
        }
    }

    [TestClass]
    public class logInUser
    {
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        //Methods methods = new Methods();

        // Expecting result: Invalid password. Response 400
        [TestMethod]
        public void InvalidlogInUser()
        {
            // Creating valid user
            addNewUser user = new addNewUser();
            user.addUser();

            // Creating Client connection and request to get data from server
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest(variables.userURLPath + "login", Method.GET);

            // Send input data
            restRequest.AddHeader("username", variables.username);
            restRequest.AddHeader("password", variables.invalidPassword);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            // Verifiying reponse
            if (!response.Contains("Invalid username/password supplied"))
            {
                Assert.Fail(response + "Correct username/password.");
            }
        }
    }

    [TestClass]
    public class getMethodNotAllowedResponse
    {
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();

        // Check for not allowed methods for user opeations
        [TestMethod]
        public void geNotAllowed()
        {
            for (int i = 0; i < variables.methods.GetLength(0); i++)
            {
                // Creating Client connection and request to get data from server
                RestClient restClient = new RestClient(variables.URL);
                RestRequest restRequest = new RestRequest(variables.userURLPath, (Method)variables.methods[i]);

                // Executing request to server and checking server response to the it
                IRestResponse restResponse = restClient.Execute(restRequest);

                // Extracting output data from received response
                string response = restResponse.Content;

                // Verifiying reponse
                if (!response.Contains("405"))
                {
                    Assert.Fail(response + "Method " + variables.methods[i].ToString() + " allowed.");
                }
            }
        }
    }

    [TestClass]
    public class checkPOSTandGET
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Remove existing user -> expacting result 200
        [TestMethod]
        public void compareRecords()
        {
            // New instance of Variables and Methods to get access to classes
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest(variables.userURLPath, Method.POST);

            restRequest.AddJsonBody(new
            {
                id = variables.id,
                username = variables.username,
                firstName = variables.firstName,
                lastName = variables.lastName,
                email = variables.email,
                password = variables.password,
                phone = variables.phone,
                userStatus = variables.userStatus
            });

            string postResponse = methods.Serialize(restRequest);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            string getResponse = methods.getUserInfo(variables.username);

            if (!postResponse.Equals(getResponse))
            {
                Assert.Fail(postResponse + getResponse + " Records are not the same.");
            }
        }
    }
}