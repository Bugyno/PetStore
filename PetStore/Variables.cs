using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace PetStore
{
    class Variables
    {
        // Variables
        // Test user variables
        public int id = 1;
        public string username = "Bugyno";
        public string firstName = "Lukas";
        public string lastName = "Bugaj";
        public string email = "email@email.com";
        public string password = "Heslo123";
        public string updatedPassword = "Heslo123*";
        public string invalidPassword = "Heslo 321";
        public string phone = "0900000000";
        public int userStatus = 0;
        public string URL = "https://petstore.swagger.io/v2/";
        public string userURLPath = "user/";
        public object[] methods = new object[] { Method.PATCH, Method.COPY, Method.MERGE};
    }

    class Methods
    {
        // Set up methods
        Variables variables = new Variables();

        // Method for User
        public RestRequest GetNewRestRequestUser()
        {
            Variables variables = new Variables();
            RestRequest request = new RestRequest(variables.username, Method.PUT);
            return request;
        }
    }
}
