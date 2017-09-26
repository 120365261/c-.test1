using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    public enum EDbs
    {
        MiniAccount,
        zDCData,
        zErpMini,
        ZStatisData
    }

    public class TestService
    {
        public string InstanceMethod(string name, int num1)
        {
            return name + "---" + num1;
        }

        private string InstanceMethod()
        {
            return "私有实例方法";
        }

        public static int StaticMethod()
        {
            return 1;
        }
        private static int StaticMethod(int a = 11111)
        {
            return a;
        }
    }

    public class IocContainer
    {
        static IocContainer()
        {
            Program.InitIoc(

        }
        public static IContainer Container { get; set; }
        public static T Resolve<T>()
        {
            var obj = Container.Resolve<T>();
            return obj;
        }
    }
    public enum EOrderField
    {
        A,
        B,
        C
    }
    class Program
    {
        public EOrderField a { set; get; }

        public void MyTestMethod()
        {
            Console.WriteLine(a.ToString());
            return;
        }
        private static IContainer Container { get; set; }
        static void Main(string[] args)
        {


            Console.WriteLine("都不会输出什么来。。。");
            Debug.WriteLine("会出来吗？？");
            //Debug.WriteLine("会出来吗？？
            //TestInvoke();
            //NlogTest();
            InitIoc();

            ResolveTest();

            ResolveTest();

            Task.Run(() =>
            {
                Console.WriteLine("------------1");
                ResolveTest();
            }).Wait();

            Console.WriteLine("------------2");
            using (var scope = Container.BeginLifetimeScope())
            {
                var db = scope.Resolve<Db>();
                db.SayHello();
            }

            Console.ReadLine();
        }

        public static void InitIoc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Db>().AsSelf().InstancePerLifetimeScope();
            Container = builder.Build();
            IocContainer.Container = Container;
        }

        private static void ResolveTest()
        {
            var db = Container.Resolve<Db>();
            db.SayHello();
        }

        private static void NlogTest()
        {
            var logger = NLog.LogManager.GetLogger("file1");
            for (int i = 0; i < 10; i++)
            {
                logger.Info(i.ToString());
            }
            Console.WriteLine("OK");
        }

        private static void TestInvoke()
        {
            string assemblyName = "Test.exe";
            string className = typeof(TestService).FullName;
            string staticMethodName = typeof(TestService).GetMethod("StaticMethod").Name;
            string instanceMethodName = typeof(TestService).GetMethod("InstanceMethod").Name;

            Invoke(assemblyName, className, staticMethodName);
            Invoke(assemblyName, className, instanceMethodName, DateTime.Now.Millisecond, 520);
        }

        private static void Invoke(string assemblyName, string classFullName, string methodFullName, params object[] inputParames)
        {
            var assembly = Assembly.LoadFrom(assemblyName);
            var cls = assembly.GetType(classFullName);
            var method = cls.GetMethod(methodFullName);

            object obj = null;
            if (!method.IsStatic)
            {
                obj = assembly.CreateInstance(classFullName);
            }

            var methodParams = method.GetParameters();
            if (methodParams.Length > 0 && inputParames != null)
            {
                var loop_i = 0;
                foreach (var param in methodParams)
                {
                    if (inputParames.Length >= loop_i + 1)
                    {
                        inputParames[loop_i] = Convert.ChangeType(inputParames[loop_i], param.ParameterType);
                        loop_i++;
                    }
                }
            }

            var result = method.Invoke(obj, inputParames);

            Console.WriteLine(result);
        }
    }
}
