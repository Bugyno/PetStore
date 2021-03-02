using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;

namespace PetStore
{
    class Variables
    {
        // Common variables
        public string URL = "https://petstore.swagger.io/v2/";
        public object[] methods = new object[] { Method.PATCH, Method.COPY, Method.MERGE };

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
        public string userURLPath = "user/";

        // Pet variables
        public string petURLPath = "pet/";
        public string api_key = "key";
        public int petId = 1;
        public int categoryId = 1;
        public string categoryName = "Dogs";
        public string petName = "doggie";
        public string updatedPetName = "dooggie";
        public string photoUrls = "URL";
        public int tagId = 1;
        public string tagName = "Dog";
        public string petStatus = "Available";

        // Store variables
        public string storeOrderURLPath = "store/order";
        public string storeInventoryURLPath = "user/inventory";
    }

    // Methods to handle tests
    class Methods
    {
        Variables variables = new Variables();

        // Remove object method
        public string removeObject(object urlPath, object removedObject)
        {
            // New instance of Variables and Methods to get access to classes
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest((String)urlPath + (String)removedObject, Method.DELETE);

            //Creating updated testing input data in JSON format
            if (urlPath.ToString().Equals(variables.userURLPath))
            {
                restRequest.AddJsonBody(new
                {
                    username = (String)removedObject
                });
            }

            if (urlPath.ToString().Equals(variables.petURLPath))
            {
                restRequest.AddJsonBody(new
                {
                    orderId = (String)removedObject
                });
            }

            if (urlPath.ToString().Equals(variables.petURLPath))
            {
                restRequest.AddHeader("api_key", variables.api_key);
                restRequest.AddHeader("petId", (String)removedObject);
            }

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            return response;
        }

        // Add object method
        public string addObject(object urlPath)
        {
            // New instance of Variables and Methods to get access to classes
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest((String)urlPath, Method.POST);

            //Creating testing input data in JSON format
            if (urlPath.ToString().Equals(variables.userURLPath))
            {
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
            }

            if (urlPath.ToString().Equals(variables.petURLPath))
            {
                restRequest.AddJsonBody(new
                {
                    id = variables.petId,
                    category = variables.categoryId,
                    variables.categoryName,
                    name = variables.petName,
                    photoUrls = variables.photoUrls,
                    tags = variables.tagId,
                    variables.tagName,
                    status = variables.petStatus
                });
            }

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            return response;
        }

        // Update object method
        public string updateObject(object urlPath, object updatedObject)
        {
            // Creating Client connection and request to get data from server
            RestClient restClient = new RestClient(variables.URL);

            //Creating updated testing input data in JSON format
            if (urlPath.ToString().Equals(variables.userURLPath))
            {
                RestRequest restRequest = new RestRequest(variables.userURLPath + (String)updatedObject, Method.PUT);
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

                return response;
            }
            else if (urlPath.ToString().Equals(variables.petURLPath))
            {
                RestRequest restRequest = new RestRequest(variables.petURLPath, Method.PUT);
                restRequest.AddJsonBody(new
                {
                    id = variables.petId,
                    category = variables.categoryId,
                    variables.categoryName,
                    name = variables.updatedPetName,
                    photoUrls = variables.photoUrls,
                    tags = variables.tagId,
                    variables.tagName,
                    status = variables.petStatus
                });

                // Executing request to server and checking server response to the it
                IRestResponse restResponse = restClient.Execute(restRequest);

                // Extracting output data from received response
                string response = restResponse.Content;

                return response;
            }
            else return "Something is wrong";
        }

        public string getUserInfo(object user)
        {
            // Creating Client connection and request to get data from server
            RestClient restClient = new RestClient(variables.URL);
            RestRequest restRequest = new RestRequest(variables.userURLPath + (String)user, Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            return response;
        }

        // Json to string serialization
        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include,
                        DefaultValueHandling = DefaultValueHandling.Include
                    };

                    jsonTextWriter.Formatting = Formatting.None;
                    jsonTextWriter.QuoteChar = '"';

                    serializer.Serialize(jsonTextWriter, obj);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }
    }
}
