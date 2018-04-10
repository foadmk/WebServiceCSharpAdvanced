using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class RestAPI
    {
        struct AuthResult
        {
            public string token { get; set; }
            public bool auth { get; set; }
        }

        struct AuthParams
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        AuthResult authResult;
        MyClient client;

        public RestAPI()
        {
            authResult = new AuthResult();
            authResult.auth = false;
            client = new MyClient();
        }

        public bool isLogedIn()
        {
            return authResult.auth;
        }

        public async Task<Boolean> Auth(string restUrl, string username, string password)
        {
            AuthParams authParams = new AuthParams();
            authParams.username = username;
            authParams.password = password;

            string myContent = JsonConvert.SerializeObject(authParams);
            try
            { 
                string str = await client.Post(restUrl, myContent);
                AuthResult objs = JsonConvert.DeserializeObject<AuthResult>(str);
                client.SetToken(objs.token);
                authResult = objs;
                return objs.auth;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<T>> GetAll<T>(string restUrl)
        {
            List<T> objs;

            try
            {
                string str = await client.Get(restUrl);
                objs = JsonConvert.DeserializeObject<List<T>>(str);
                return objs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<Boolean> Create<T>(string restUrl, T obj)
        {
            var myContent = JsonConvert.SerializeObject(obj);

            try
            {
                await client.Post(restUrl, myContent);
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }


    }
}
