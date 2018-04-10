using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class MyClient
    {
        HttpClient client;

        public MyClient()
        {
            client = new HttpClient();
        }

        public void SetToken(string token)
        {
            client.DefaultRequestHeaders.Add("x-access-token", token);
        }

        Uri GetUri(string Url)
        {
            return new Uri(string.Format(Url, string.Empty));
        }

        ByteArrayContent ToByteArrayContent(string myContent, string headerType = "application/json")
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(headerType);
            return byteContent;
        }

        Exception ErrorHandler(string error)
        {
            return new Exception(error);
        }

        public async Task<String> Get(string restUrl)
        {
            var response = await client.GetAsync(GetUri(restUrl));

            if (response.IsSuccessStatusCode)
            {               
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw ErrorHandler($"Falha na chamada GET ao endereço: {restUrl} - StatusCode: {response.StatusCode}");
            }
        }
        public async Task<String> Post(string restUrl, string myContent, string headerType = "application/json")
        {
            HttpResponseMessage response = await client.PostAsync(GetUri(restUrl), ToByteArrayContent(myContent, headerType));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw ErrorHandler($"Falha na chamada POST ao endereço: {restUrl} - StatusCode: {response.StatusCode}");
            }
        }


    }
}
