using System;
using System.Collections.Generic;
using System.Collections;
using LiteDB;

namespace LiteC
{
    class Program
    {
        public class Text
        {
            public int Id { get; set; }
            public string text { get; set; }
        }

        public class Numb
        {
            public int Id { get; set; }
            public int number { get; set; }
        }

        public class Bkey
        {
            public int Id { get; set; }
            public byte[] bkey { get; set; }
        }

        public class MList
        {
            public int Id { get; set; }
            public List<String> List { get; set; }
        }

        public class MHset
        {
            public int Id { get; set; }
            public int Size { get; set; }
            public Object[] Set { get; set; }
            public MHset()
            {
                Size = 0;
                Set = new object[Size];
            }
            public MHset(HashSet<Object> Input)
            {
                int fullSize = 0;
                int currentSize = 0;
                var denum_size = Input.GetEnumerator();
                while (denum_size.MoveNext())
                {
                    fullSize++;
                }
                Size = fullSize;
                Set = new object[Size];
                foreach (var i in Input)
                {
                    currentSize++;
                    Set[currentSize - 1] = i;
                }
            }
        }

        public class MHash
        {
            public int Id { get; set; }
            public string[] Keys { get; set; }
            public Object[] Value { get; set; }
            public int Size { get; set; }
            public MHash(int x)
            {
                Size = x;
                Keys = new string[Size];
                Value = new object[Size];
            }

            public MHash(MHash x)
            {
                Size = x.Size;
                Keys = new string[Size];
                Value = new object[Size];
                for (int i = 0; i < Size; ++i)
                {
                    Keys[i] = x.Keys[i];
                    Value[i] = x.Value[i];
                }
            }
            public MHash(Hashtable Input)
            {
                int fullSize = 0;
                int currentSize = 0;
                IDictionaryEnumerator denum = Input.GetEnumerator();
                IDictionaryEnumerator denum_size = Input.GetEnumerator();
                DictionaryEntry dictionaryEntry;

                while (denum_size.MoveNext())
                {
                    fullSize++;
                }
                Size = fullSize;
                Keys = new string[Size];
                Value = new object[Size];

                while (denum.MoveNext())
                {
                    currentSize++;
                    dictionaryEntry = (DictionaryEntry)denum.Current;
                    Keys[currentSize - 1] = dictionaryEntry.Key.ToString();
                    Value[currentSize - 1] = dictionaryEntry.Value;
                }
            }
        }
        public class Car
        {
            public double Price { get; set; }
            public String Name { get; set; }
            public DateTime Date { get; set; }
        }
        static void Main(string[] args)
        {
            using (var dataBase = new LiteDatabase(@"C:\DB\LDB1V.db"))
            {
                Text text = new Text { text = "Gorelov R.A., 1 Var, LiteDB" };
                dataBase.GetCollection<Text>("textkey").Insert(text);
                var textResult = dataBase.GetCollection<Text>("textkey").FindOne(textkey => textkey != null);

                Console.WriteLine("Выводим простые значения по ключу:");
                Console.WriteLine("KEY: Text, Value = " + textResult.text + ";");


                Numb numb = new Numb { number = 2281488 };
                dataBase.GetCollection<Numb>("numberkey").Insert(numb);
                var numberResult = dataBase.GetCollection<Numb>("numberkey").FindOne(numberkey => numberkey != null);

                Console.WriteLine("KEY: Number, Value = " + numberResult.number + ";"); 
                Console.WriteLine();


                byte[] bt = { 5, 6, 7, 8 };
                Bkey bkey = new Bkey { bkey = bt };
                dataBase.GetCollection<Bkey>("bytekey").Insert(bkey);
                var massiveResult = dataBase.GetCollection<Bkey>("bytekey").FindOne(bytekey => bytekey != null);

                Console.WriteLine("Выводим массив байт по ключу:");
                Console.Write("KEY: ByteKey, Value =  ");
                foreach (byte bte in massiveResult.bkey)
                    Console.Write(bte + " ");
                Console.Write(";");
                Console.WriteLine();
                Console.WriteLine();

                List<string> list = new List<string>();
                list.Add("Some");
                list.Add("Random");
                list.Add("Text");
                MList mlist = new MList { List = list };
                dataBase.GetCollection<MList>("listkey").Insert(mlist);
                var listResult = dataBase.GetCollection<MList>("listkey").FindOne(listkey => listkey != null);

                Console.WriteLine("Выводим элементы динамического списка List оператором foreach:");
                Console.Write("KEY: List, Value = ");
                foreach (string res in listResult.List)
                    Console.Write(res + "; ");
                Console.WriteLine();
                Console.WriteLine();


                HashSet<Object> hashset = new HashSet<Object>();
                hashset.Add("string");
                hashset.Add(22.33);
                hashset.Add(111);
                MHset mhset = new MHset(hashset);
                dataBase.GetCollection<MHset>("setkey").Insert(mhset);
                var setResult = dataBase.GetCollection<MHset>("setkey").FindOne(setkey => setkey != null);

                Console.WriteLine("Выводим элементы несортированного множества Set оператором for:");
                Console.Write("KEY: Set, Value = ");
                for (int i = 0; i < setResult.Size; i++)
                    Console.Write(setResult.Set[i] + "; ");
                Console.WriteLine();
                Console.WriteLine();


                Hashtable hashtable = new Hashtable();
                hashtable.Add("String", "string");
                hashtable.Add("Float", 22.33);
                hashtable.Add("Int", 111);
                MHash mhash = new MHash(hashtable);
                dataBase.GetCollection<MHash>("hashtablekey").Insert(mhash);
                var hashtableResult = dataBase.GetCollection<MHash>("hashtablekey").FindOne(hashtablekey => hashtablekey != null);

                Console.WriteLine("Выводим элементы пары 'ключ-значение' Hashtable:");
                Console.WriteLine("KEY: Hashtable, Value:");
                for (int i = 0; i < hashtableResult.Size; i++)
                    Console.WriteLine("KEY: '" + hashtableResult.Keys[i] + "' Value = '" + hashtableResult.Value[i] + "';");
                Console.WriteLine();


                dataBase.GetCollection<Car>("Car").Insert(new Car { Price = 123456.789, Name = "Lexus", Date = DateTime.Now });
                var carResult = dataBase.GetCollection<Car>("Car").FindOne(Car => Car != null);

                Console.WriteLine("Выводим объекты пользовательского класса:");
                Console.WriteLine("KEY: Car, Value: Price = " + carResult.Price + "; Name = " + carResult.Name + "; Date = " + carResult.Date);
                Console.WriteLine();
                Console.WriteLine("Для завершения нажмите любую кнопку");
                Console.ReadKey();
            }
        }
    }
}