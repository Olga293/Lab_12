using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;


//1. Для изучения.NET Reflection API допишите класс Рефлектор, который
//будет содержать методы выполняющие следующие действия:
//a.выводит всё содержимое класса в текстовый файл (принимает
//в качестве параметра имя класса);
//b.извлекает все общедоступные публичные методы класса
//(принимает в качестве параметра имя класса);
//c.получает информацию о полях и свойствах класса;
//d.получает все реализованные классом интерфейсы;
//e.выводит по имени класса имена методов, которые содержат
//заданный(пользователем) тип параметра(имя класса
//передается в качестве аргумента);
//f.вызывает некоторый метод класса, при этом значения для его
//параметров необходимо прочитать из текстового файла(имя
//класса и имя метода передаются в качестве аргументов).
//Продемонстрируйте работу «Рефлектора» для исследования типов на
//созданных вами классах не менее двух(предыдущие лабораторные работы) и
//стандартных классах.Net.


namespace lab_12_1_
{
    abstract class GeneralInfo
    {
        public string Title { get; set; }
        public string Country { get; set; }
        public int Year { get; set; }
        public int Pages { get; set; }
        public string Cover { get; set; }
        public double Price { get; set; }
    }
    class Book : GeneralInfo
    {
        public Book() { }
        public Book(string title, string country, int year, int pages, string cover, double price)
        {
            Title = title;
            Country = country;
            Year = year;
            Pages = pages;
            Cover = cover;
            Price = price;
        }
        public override string ToString()
        {
            return "~~~~~~~~~~Information about book~~~~~~~~~~\nTitle: " + Title + "\nYear: " + Year + "\nPages: " + Pages + "\nCountry: " + Country + "\nPrice: " + Price;
        }
    }

    interface IShow1
    {
        void Show(string str);
    }

    class SchoolBook : GeneralInfo, IShow1
    {
        public SchoolBook() { }
        public string TitleOfSchoolBook { get; set; }
        public SchoolBook(string title, string country, int year, int pages, string cover)
        {
            TitleOfSchoolBook = title;
            Country = country;
            Year = year;
            Pages = pages;
            Cover = cover;
        }
        public override string ToString()
        {
            return "~~~~~~~~~~Information about school book~~~~~~~~~~\nTitle: " + TitleOfSchoolBook + "\nYear: " + Year + "\nPages: " + Pages + "\nCountry: " + Country;
        }
        public void Show(string str)
        {
            Console.WriteLine("Done: " + str);
        }
    }

    public class Reflector
    {
        public void ClassData(Type NameOfClass, StreamWriter write)   //содержимое класса
        {
            MemberInfo[] data = NameOfClass.GetMembers();   //все члены типа в виде массива объектов MemberInfo
            write.WriteLine("Data in class" + NameOfClass + ":");
            foreach (var x in data)
            {
                write.WriteLine(x.MemberType + "-----" + x.Name);
            }
            write.Write("\n\n");
        }

        public void PublicMethods(Type NameOfClass, StreamWriter write) // публичные методы класса
        {
            MethodInfo[] data = NameOfClass.GetMethods();
            //MethodInfo[] content = NameOfClass.GetMethods(BindingFlags.Public);   //только публичные
            write.WriteLine("Public methods in class " + NameOfClass + ":");
            foreach (var x in data)
            {
                write.WriteLine(x.Name + "-----" + x.ReturnType.Name);
            }
            write.Write("\n\n");
        }

        public void FieldsProperties(Type NameOfClass, StreamWriter write) //информация о полях и свойствах класса
        {
            FieldInfo[] fields = NameOfClass.GetFields();
            write.WriteLine("Fields in class " + NameOfClass + ":");
            foreach (var x in fields)
            {
                write.WriteLine(x.Name);
            }
            write.Write("\n\n");

            write.WriteLine($"Properties in class " + NameOfClass + ":");
            PropertyInfo[] properties = NameOfClass.GetProperties();
            foreach (var x in properties)
            {
                write.WriteLine(x.Name);
            }
            write.Write("\n\n");
        }
        public void Interfaces(Type NameOfClass, StreamWriter write) // реализованные классом интерфейсы
        {
            var data = NameOfClass.GetInterfaces();
            write.WriteLine("Realized class interfaces " + NameOfClass + ":");
            foreach (var x in data)
            {
                write.WriteLine(x.Name);
            }
            write.Write("\n\n");
        }

        public void TypeOfParameter(Type NameOfClass, string param, StreamWriter write) // имя метода с заданным типом параметра
        {
            MethodInfo[] data = NameOfClass.GetMethods();
            write.WriteLine("Methods in class " + NameOfClass + ", which contain a parameter of type " + param + ":");
            foreach (var x in data)
            {
                foreach (var y in x.GetParameters())
                {
                    if (y.ParameterType.Name == param)
                        write.WriteLine(x.Name);
                }
            }
            write.Write("\n\n");
        }

        public void FileParameter(Type NameOfClass, string method) // вызвать некоторый метод класса и прочитать его параметры из файла
        {
            StreamReader read = new StreamReader(@"C:\Users\Olga\OneDrive\Documents\BSTU\2_course\1_semester\OOP\lab_12\Lab_12\read.txt", Encoding.GetEncoding(1251));
            string str = read.ReadLine();
            read.Close();
            MethodInfo content = NameOfClass.GetMethod(method);
            object obj = Activator.CreateInstance(NameOfClass);
            object newmathod = content.Invoke(obj, new object[] { str });
            Console.WriteLine(newmathod);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StreamWriter write = new StreamWriter(@"C:\Users\Olga\OneDrive\Documents\BSTU\2_course\1_semester\OOP\lab_12\Lab_12\write.txt", false);
                Type classBook = typeof(Book);
                Type classSchoolBook = typeof(SchoolBook);

                Reflector reflector = new Reflector();
                reflector.ClassData(classBook, write);
                reflector.ClassData(classSchoolBook, write);

                reflector.PublicMethods(classBook, write);
                reflector.PublicMethods(classSchoolBook, write);

                reflector.FieldsProperties(classBook, write);
                reflector.FieldsProperties(classSchoolBook, write);

                reflector.Interfaces(classSchoolBook, write);

                reflector.TypeOfParameter(classBook, "String", write);
                reflector.TypeOfParameter(classSchoolBook, "String", write);

                reflector.FileParameter(classSchoolBook, "Show");

                write.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " \n" + ex.TargetSite);
            }
        }
    }
}
