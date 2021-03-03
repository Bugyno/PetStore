using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace PetStore
{
    [TestClass]
    public class addNewUser
    {
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Successfull adding new user -> expecting response 200
        [TestMethod]
        public void addUser()
        {
            // Calling method addObject() from methods class to create new User
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
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Successfull updating user -> expecting response 200
        [TestMethod]
        public void updateExistingUserPassword()
        {
            // Calling method updateObject() from methods class to update a user
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
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Remove existing user -> expacting result 200
        [TestMethod]
        public void removeExistingUser()
        {
            // Calling method removeObject() from methods class to remove a user
            string response = methods.removeObject(variables.userURLPath, variables.username);
            
            // Verifying reponse
            if (!response.Contains("200"))
            {
                Assert.Fail(response);
            }
        }
    }

    [TestClass]
    public class getInfoAboutUnknownUser
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Expecting result: User not found. Response 404
        [TestMethod]
        public void getUnknownUserInfo()
        {
            // Calling method getUserInfo() from methods class to get info about a user
            string response = methods.getUserInfo(variables.username.ToUpper());

            // Verifiying reponse
            if (!response.Contains("User not found"))
            {
                Assert.Fail(response);
            }
        }
    }
    
    [TestClass]
    public class getInfoAboutKnownUser
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Expecting result: Info about user exists. Reponse 200
        [TestMethod]
        public void getKnownUserInfo()
        {
            // Calling method getUserInfo() from methods class to get info about a user
            string response = methods.getUserInfo(variables.username);

            // Verifiying reponse
            if (!response.Contains(variables.password))
            {
                Assert.Fail(response);
            }
        }
    }

    [TestClass]
    public class getMethodNotAllowedResponse
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();

        // Check for not allowed methods for user opeations -> expacting response 405
        [TestMethod]
        public void getNotAllowed()
        {
            // Loop "for" to test all methods stored in variable "methods"
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
        // New instance of Variables and Methods to get access to classes
        Variables variables = new Variables();
        Methods methods = new Methods();

        // Comparing records. Expected responses are equal. Records are the same. Precondition: User has to be created.
        [TestMethod]
        public void compareRecords()
        {
            // Storing responses to compare
            string postResponse = methods.Serialize((new
            {
                id=variables.id,
                username=variables.username,
                firstName=variables.firstName,
                lastName=variables.lastName,
                email=variables.email,
                password=variables.password,
                phone=variables.phone,
                userStatus=variables.userStatus
            }));
            string getResponse = methods.getUserInfo(variables.username);

            // Verifying response
            if (!postResponse.Equals(getResponse))
            {
                Assert.Fail(postResponse + getResponse + " Records are not the same.");
            }
        }
    }

    [TestClass]
    public class getInvalidURL
    {
        // New instance of Variables to get access to the class
        Variables variables = new Variables();

        // Check for invalid URL response -> expected 404
        [TestMethod]
        public void invalidURL()
        {
            // Creating Client connection and request to get data from server
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest("invalidPath", Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            // Verifiying reponse
            if (!response.Contains("404"))
            {
                Assert.Fail(response);
            }
        }
    }
}