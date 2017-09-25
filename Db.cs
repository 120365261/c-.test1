using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Db : IDisposable
    {
        string token = null;
        public Db()
        {
            token = DateTime.Now.Ticks.ToString();
            Console.WriteLine("created");
        }
        public void Dispose()
        {
            Console.WriteLine("disposed");

            var logger = NLog.LogManager.GetLogger("file1");
            logger.Info("disposed");
        }

        public void SayHello()
        {
            Console.WriteLine("hello:" + token);
        }
    }
}
