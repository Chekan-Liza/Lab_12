using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Lab_12
{
    interface IInfoClass
    {
        double Sum();
        void Info();
        void Set(double d1, double d2);
    }

    //Тестовый класс, содержащий некоторые конструкции
    class Test : IInfoClass
    {
        double d, f;
        public int M = 850;
        public int m
        {
            get => m;
            set => m = value;
        }
        public Test() { }
        public Test(double d, double f)
        {
            this.d = d;
            this.f = f;
        }
        public double Sum()
        {
            return d + f;
        }
        public void Info()
        {
            Console.WriteLine($@"d = { d } f = { f }");
        }
        public void InfoOut(string str)
        {
            int j = Convert.ToInt32(str);
            j = j * 2;
            Console.WriteLine(j);
        }
        public void Set(int a, int b)
        {
            d = (double)a;
            f = (double)b;
        }
        public void Set(double a, double b)
        {
            d = a;
            f = b;
        }
        public static string h(string s)
        {
            return s + " !";
        }
    }

    class My
    {
        public static string q(string s)
        {
            return s + " !";
        }
    }

    //В данном классе определены методы, использующие рефлексию
    class Reflect
    {
        public static void AllInfo(object obj)
        {
            string text = "";
            Type t = obj.GetType();
            Console.WriteLine("----------Информация о классе----------");
            string path = @"text.txt";

            foreach (MemberInfo mi in t.GetMembers())
            {
                Console.WriteLine(mi.DeclaringType + " " + mi.MemberType + " " + mi.Name);
                text = text + mi.DeclaringType + " " + mi.MemberType + " " + mi.Name + '\n';
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    text = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\n");
        }
        public static void Method_Info<T>(T obj) where T : class
        {
            Type myType = typeof(T);

            Console.WriteLine("----------Методы----------");

            foreach (MethodInfo method in myType.GetMethods())
            {
                string modificator = "";

                if (method.IsPublic)
                    modificator += "Public ";

                Console.Write(modificator + method.ReturnType.Name + " " + method.Name + " (");

                ParameterInfo[] parameters = method.GetParameters();

                for (int i = 0; i < parameters.Length; i++)
                {
                    Console.Write(parameters[i].ParameterType.Name + " " + parameters[i].Name);

                    if (i + 1 < parameters.Length)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine(")");
            }
            Console.WriteLine("\n");
        }

        public static void FieldInfo(object obj)
        {
            Type type = obj.GetType();

            Console.WriteLine("----------Поля----------");

            foreach (FieldInfo field in type.GetFields())
            {
                Console.WriteLine($"{field.FieldType} {field.Name}");
            }

            Console.WriteLine("\n----------Свойства----------");

            foreach (PropertyInfo prop in type.GetProperties())
            {
                Console.WriteLine($"{prop.PropertyType} {prop.Name}");
            }
            Console.WriteLine("\n");
        }

        public static void InterfaceInfo(object obj)
        {

            Type type = obj.GetType();

            Console.WriteLine("----------Реализованные интерфейсы----------");

            foreach (Type i in type.GetInterfaces())
            {
                Console.WriteLine(i.Name);
            }

            Console.WriteLine("\n");
        }

        public static void InfoByParam(object obj, string str)
        {
            Type type = obj.GetType();

            MethodInfo[] methods = type.GetMethods();

            Console.WriteLine("-----------Информация по параметру----------");

            foreach (var item in methods)
            {
                ParameterInfo[] param = item.GetParameters();

                if (param.Length != 0)
                {
                    var parQuerry = from sp in param
                                    where sp.Name.Contains(str)
                                    select sp;

                    if (parQuerry.ToArray().Length != 0)
                    {
                        Console.WriteLine(item.Name);

                        foreach (var _item in param)
                        {
                            Console.WriteLine(_item.Name);
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        public static void Method(object obj, string param)
        {
            string path = @"text.txt";
            string buf = "";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    buf = sr.ReadToEnd();
                }
                Console.WriteLine(buf);

                Type type = obj.GetType();

                //создаем экземпляр класса Program
                Object Obj = Activator.CreateInstance(typeof(Test));
                //получаем методы
                MethodInfo method = type.GetMethod(param);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Test test = new Test(12.0, 3.5);
            Reflect.AllInfo(test);

            Reflect.Method_Info<Test>(test);

            Reflect.FieldInfo(test);

            Reflect.InterfaceInfo(test);

            Reflect.InfoByParam(new List<int>(), "count");

            Reflect.Method(test, "InfoOut");
        }
    }
}
