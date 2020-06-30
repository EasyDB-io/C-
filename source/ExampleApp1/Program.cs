
namespace ExampleApp1
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            String database = "";
            String token = "";            

            var db = new EasyDB.DB(database, token);

            db.Put("key", "8");

            String result = db.Get("key");
            Console.WriteLine(result);

            result = db.List();
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
