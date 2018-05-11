using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugarTool;
using System.Net.Http;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateModel();

            Console.WriteLine("完成");
            Console.Read();
        }

        private static async void sss()
        {
            Parallel.Invoke();


            var ddd = await TaskTest("http://192.168.18.86:85/api/PetSystemApi/StartTreasureHunt?accountNum={0}&nonce=111&sign=1111");

        }

        private static async Task<string> TaskTest(string url)
        {
            var nextDelay = TimeSpan.FromSeconds(1);
            using (var client = new HttpClient())
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        return await client.GetStringAsync($"http://192.168.18.86:85/api/PetSystemApi/StartTreasureHunt?accountNum={i}&nonce=111&sign=1111");
                    }
                    catch (Exception)
                    {
                    }

                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }

                Console.WriteLine("TaskTest" + nextDelay);
                return await client.GetStringAsync(url);
            }
        }


        private static void CreateModel()
        {
            var db = SugarDao.GetInstance("PaGame");
            string[] tabs = new string[] { "Tickets_RewardLog" };
            db.DbFirst.Where(tabs).CreateClassFile(@"C:\Users\Administrator\Desktop\Model");
        }


    }
}
