
using Com.JinYiWei.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Com.IFlyDog.ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Snowflake.Instance().GetId());
            //TestIdWorker();
            Console.WriteLine(DateTime.Now.AddSeconds(100));
            Console.WriteLine(Convert.ToInt32("JH17021101113".Substring("JH17021101113".Length-3))+1);
            //Task.Run(() =>
            //{
            //    for(int i=0;i<=100;i++)
            //    AutoNumber.Instance().CKNumber(1,"JH") ;
            //});
            //Task.Run(() =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //        AutoNumber.Instance().CKNumber(1, "JH");
            //});
            //Task.Run(() =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //        Console.WriteLine(AutoNumber.Instance().CKNumber(1, "JH"));
            //});

            //Task.Run(() =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //        Console.WriteLine(AutoNumber.Instance().CKNumber(1,"TH"));
            //});
            //Task.Run(() =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //        Console.WriteLine(AutoNumber.Instance().CKNumber(1, "TH"));
            //});
            //Task.Run(() =>
            //{
            //    for (int i = 0; i <= 100; i++)
            //        Console.WriteLine(AutoNumber.Instance().CKNumber(1, "TH"));
            //});



            Console.ReadLine();
        }

        private static void TestIdWorker()
        {
            HashSet<long> set = new HashSet<long>();
            IdWorker idWorker1 = SingleIdWork.Instance(2,1);
            Console.WriteLine(idWorker1.nextId());
            IdWorker idWorker2 = SingleIdWork.Instance(1,1);
            Console.WriteLine(idWorker2.nextId());
            Thread t1 = new Thread(() => DoTestIdWoker(idWorker1, set));
            Thread t2 = new Thread(() => DoTestIdWoker(idWorker2, set));
            t1.IsBackground = true;
            t2.IsBackground = true;

            t1.Start();
            t2.Start();
            try
            {
                Thread.Sleep(30000);
                t1.Abort();
                t2.Abort();
            }
            catch (Exception e)
            {
            }

            Console.WriteLine("done");
        }

        private static void DoTestIdWoker(IdWorker idWorker, HashSet<long> set)
        {
            while (true)
            {
                long id = idWorker.nextId();
                if (!set.Add(id))
                {
                    Console.WriteLine("duplicate:" + id);
                }

                Thread.Sleep(1);
            }
        }
    }
}
