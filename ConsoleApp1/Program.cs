using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        struct MyTask
        {
            public string _id { get; set; }
            public string name { get; set; }
            public DateTime created_date { get; set; }
            public string status { get; set;  }
        }


        static RestAPI restAPI;
        static async Task<Boolean> Principal()
        {
            if(!restAPI.isLogedIn()) { 
                Console.Write("Login: ");
                string username = Console.ReadLine();
                Console.Write("Senha: ");
                string password = Console.ReadLine();

                await restAPI.Auth("http://localhost:3000/login", username, password);
            }

            Console.Write("Digite uma nova tarefa: ");

            MyTask t = new MyTask();
            t.name = Console.ReadLine();
            t.status = "pending";
            t.created_date = DateTime.Now;

            if (t.name.Length > 0)
            {
                await restAPI.Create<MyTask>("http://localhost:3000/tasks", t);
            }

            List<MyTask> list = await restAPI.GetAll<MyTask>("http://localhost:3000/tasks");

            Console.WriteLine("Lista de Tarefas:");
            foreach(MyTask myTask in list)
            {
                Console.WriteLine($"{myTask.name} - {myTask.status} - {myTask.created_date}");
            }
            return true;
        }

        static void Main()
        {
            restAPI = new RestAPI();

            while (true)
            {
                Task<Boolean> x = Principal();
                while (!x.IsCompleted)
                {
                    System.Threading.Thread.Sleep(50);
                }
                if (x.IsFaulted)
                {
                    Console.WriteLine(x.Exception.Message);
                }
            }
        }

    }
}
