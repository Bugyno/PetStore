using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace PetStore
{
    [TestClass]
    public class addNewUser
    {
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        //Methods methods = new Methods();

        // Successfull adding new user -> expecting response 200
        [TestMethod]
        public void addUser()
        {
            //Creating Client connection 
            RestClient restClient = new RestClient(variables.URL);

            //Creating request to get data from server
            RestRequest restRequest = new RestRequest(variables.userURLPath, Method.POST);

            //Creating testing input data in JSON format
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

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

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
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        //Methods methods = new Methods();

        // Successfull updating new user
        [TestMethod]
        public void updateAUser()
        {
            // Creating Client connection and request to get data from server
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest(variables.userURLPath + variables.username, Method.PUT);

            //Creating updated testing input data in JSON format
            restRequest.AddJsonBody(new
            {
                id = variables.id,
                username = variables.username,
                firstName = variables.firstName,
                lastName = variables.lastName,
                email = variables.email,
                password = variables.updatedPassword,
                phone = variables.phone,
                userStatus = variables.userStatus
            });

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

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
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        //Methods methods = new Methods();

        [TestMethod]
        public void removeAUser()
        {
            // New instance of Variables and Methods to get access to classes
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest(variables.userURLPath + variables.username, Method.DELETE);

            //Creating updated testing input data in JSON format
            restRequest.AddJsonBody(new
            {
                username = variables.username
            });

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

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
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        //Methods methods = new Methods();

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
            RestRequest restRequest = new RestRequest(variables.userURLPath + "user1", Method.GET);

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
}