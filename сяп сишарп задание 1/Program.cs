using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace сяп_сишарп_задание_1
{
    class OutOfRangeSet : Exception
    {
        private string message = "Выход за пределы множества!";
        public override string Message { get => message; }

        public OutOfRangeSet (string message)
        {
            this.message = message;
        }
        
    }
    abstract class Set
    {
        protected int max_value = 0;
        public int Max_value { get => max_value; }
        abstract public void Insert(int elem);
        abstract public void Erase(int key);
        abstract public bool Find(int key);

        public virtual void Insert(int[] mass)
        {
            for (int i = 0; i < mass.Length; i++)
                Insert(mass[i]);
        }

        protected bool isNum(char a)
        {
            return (a >= '0' && a <= '9');
        }

        public virtual void Insert(string str, char separator)
        {
            bool isnumhere = false;
            foreach (char a in str)
                if (isNum(a))
                    isnumhere = true;
                else if (a != separator)
                    throw new ArgumentException("В строке содержатся посторонние элементы!");

            if (!isnumhere)
                throw new ArgumentException("В строке нет циферок!!!");

            string[] words = str.Split(separator);
            int[] numbers = new int[words.Length];
            for (int i = 0; i < words.Length; i++)
                int.TryParse(words[i], out numbers[i]);
            Insert(numbers);
        }

        public virtual string Show()
        {
            string res = "";
            for (int i = 0; i <= max_value; i++)
                if (Find(i))
                    res += i + " ";
            return res.TrimEnd(' ');
        }
    }

    class SimpleSet : Set
    {
        private bool[] set;

        public bool[] Set { get => set; }

        public SimpleSet(int max_value)
        {
            set = new bool[max_value + 1];
            this.max_value = max_value;
        }
        public override void Insert(int elem)
        {
            if (elem > max_value)
                throw new OutOfRangeSet("Введено значение больше максимального!");
            set[elem] = true;
        }

        public override void Erase(int key)
        {
            if (key > max_value)
                throw new OutOfRangeSet("Удаляется значение больше максимального!");
            set[key] = false;
        }

        public override bool Find(int key)
        {
            if (key > max_value)
                return false;
            return set[key];
        }

        public static SimpleSet operator +(SimpleSet a, SimpleSet b)
        {
            int max_value = a.set.Length > b.set.Length ? a.set.Length : b.set.Length;
            SimpleSet result = new SimpleSet(max_value - 1);
            for (int i = 0; i < a.set.Length; i++)
                result.set[i] = a.set[i];
            for (int i = 0; i < b.set.Length; i++)
                result.set[i] = result.set[i] || b.Set[i];
            return result;
        }

        public static SimpleSet operator *(SimpleSet a, SimpleSet b)
        {
            int max_value = a.set.Length < b.set.Length ? a.set.Length : b.set.Length;
            SimpleSet result = new SimpleSet(max_value - 1);
            for (int i = 0; i < result.set.Length; i++)
                result.set[i] = a.set[i];
            for (int i = 0; i < result.Set.Length; i++)
                result.set[i] = result.set[i] && b.Set[i];
            return result;
        }

    }

    class BitSet : Set
    {
        private int[] set;

        public BitSet(int max_value)
        {
            set = new int[(max_value / 32) + 1];
            for (int i = 0; i < set.Length; i++)
                set[i] = 0;
            this.max_value = max_value;
        }

        public override void Insert(int elem)
        {
            if (elem > max_value)
                throw new OutOfRangeSet("Введено значение больше максимального!");
            set[elem / 32] |= 1 << (elem % 32);
        }

        public override void Erase(int key)
        {
            if (key > max_value)
                throw new OutOfRangeSet("Удаляется значение больше максимального!");
            set[key / 32] &= ~(1 << (key % 32));
        }

        public override bool Find(int key)
        {
            if (key > max_value)
                return false;
            return ((set[key / 32] >> (key % 32) & 1) == 1);
        }

        public static BitSet operator +(BitSet a, BitSet b)
        {
            int max_value = a.Max_value > b.Max_value ? a.Max_value : b.Max_value;
            BitSet result = new BitSet(max_value);
            for (int i = 0; i < a.set.Length; i++)
                result.set[i] = a.set[i];
            for (int i = 0; i < b.set.Length; i++)
                result.set[i] = result.set[i] | b.set[i];
            return result;
        }

        public static BitSet operator *(BitSet a, BitSet b)
        {
            int max_value = a.Max_value < b.Max_value ? a.Max_value : b.Max_value;
            BitSet result = new BitSet(max_value);
            for (int i = 0; i < result.set.Length; i++)
                result.set[i] = a.set[i];
            for (int i = 0; i < result.set.Length; i++)
                result.set[i] = result.set[i] & b.set[i];
            return result;
        }
    }

    class MultiSet : Set
    {
        private int[] set;

        public MultiSet(int max_value)
        {
            set = new int[max_value + 1];
            for (int i = 0; i < set.Length; i++)
                set[i] = 0;
            this.max_value = max_value;
        }

        public override void Insert(int elem)
        {
            if (elem > max_value)
                throw new OutOfRangeSet("Введено значение больше максимального!");
            set[elem]++;
        }

        public override void Erase(int key)
        {
            if (key > max_value)
                throw new OutOfRangeSet("Удаляется значение больше максимального!");
            if (Find(key))
                set[key]--;
        }

        public override bool Find(int key)
        {
            if (key > max_value)
                return false;
            return set[key] > 0;
        }

    }

    class Program
    {
        static public void Input(Set my_set, int max_value)
        {
            string answer1 = "";
            Console.WriteLine(@"Введите числа через пробел:");
                while (true)
                {
                    try
                    {
                        my_set.Insert(Console.ReadLine(), ' ');
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    return;
                    }
                    break;
                }
        }
        static public void Delete(Set my_set)
        {
            string answer1;
            int for_delete;
            while (true)
            {
                Console.WriteLine("Введите удаляемый элемент:");
                answer1 = Console.ReadLine();
                try
                {
                    for_delete = Convert.ToInt32(answer1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                break;
            }
            my_set.Erase(for_delete);
            Console.WriteLine("Элемент успешно удалён.");
        }
        static public void Find(Set my_set)
        {
            string answer1;
            int for_find;
            while (true)
            {
                Console.WriteLine("Введите искомый элемент:");
                answer1 = Console.ReadLine();
                try
                {
                    for_find = Convert.ToInt32(answer1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                break;
            }
            if (my_set.Find(for_find))
                Console.WriteLine("Элемент найден.");
            else Console.WriteLine("Такого элемента не было.");
        }
        static public void Dialog()
        {
            string answer1, answer2;
            int max_value;
            while (true)
            {
                Console.WriteLine(@"Выберите тип контейнера:
1 — SimpleSet (упорядоченное множество натуральных чисел без повторений, реализованное с помощью логического массива);
2 — BitSet (упорядоченное множество натуральных чисел без повторений, реализованное с помощью битового массива);
3 — MultiSet (упорядоченное множество натуральных чисел с повторениями, реализованное с помощью логического массива);
4 — Слишком сложно, я выхожу из программы.");

                answer1 = Console.ReadLine();
                if (!(answer1 == "1" || answer1 == "2" || answer1 == "3" || answer1 == "4"))
                    Console.WriteLine("Некорректный ввод!");
                else break;
            }

            while (true)
            {
                Console.WriteLine("Введите максимально возможный элемент:");
                answer2 = Console.ReadLine();
                try
                {
                    max_value = Convert.ToInt32(answer2);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                break;
            }

            Set my_set = null;

            switch(answer1[0])
            {
                case '1': my_set = new SimpleSet(max_value);
                    break;
                case '2': my_set = new BitSet(max_value);
                    break;
                case '3': my_set = new MultiSet(max_value);
                    break;
                case '4': return;
            }

            Input(my_set, max_value);

            while (true)
            {
                Console.WriteLine(@"Выберите действие:
1 — добавить элементы в множество;
2 — удалить элемент из множества;
3 — найти элемент в множестве;
4 — показать множество;
5 — выход.");

                answer1 = Console.ReadLine();
                if (!(answer1 == "1" || answer1 == "2" || answer1 == "3" || answer1 == "4" || answer1 == "5"))
                    Console.WriteLine("Некорректный ввод!");

                switch (answer1[0])
                {
                    case '1':
                        Input(my_set, max_value);
                        break;
                    case '2':
                        Delete(my_set);
                        break;
                    case '3':
                        Find(my_set);
                        break;
                    case '4': Console.WriteLine(my_set.Show());
                        break;
                    case '5': return;
                }

            }

        }

        static public void Test()
        {
            Console.WriteLine("Введите максимальное значение первого множества:");
            int max1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите максимальное значение второго множества:");
            int max2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите первое множество:");
            string str1 = Console.ReadLine();
            Console.WriteLine("Введите второе множество:");
            string str2 = Console.ReadLine();

            SimpleSet simple1 = new SimpleSet(max1);
            simple1.Insert(str1, ' ');
            SimpleSet simple2 = new SimpleSet(max2);
            simple2.Insert(str2, ' ');

            BitSet bit1 = new BitSet(max1);
            bit1.Insert(str1, ' ');
            BitSet bit2 = new BitSet(max2);
            bit2.Insert(str2, ' ');

            BitSet bsplus = bit1 + bit2;
            BitSet bsmult = bit1 * bit2;
            SimpleSet ssplus = simple1 + simple2;
            SimpleSet ssmult = simple1 * simple2;

            Console.WriteLine("Результат объединения первого и второго множества:");
            Console.WriteLine("С помощью SimpleSet: " + ssplus.Show());
            Console.WriteLine("С помощью BitSet: " + bsplus.Show());

            Console.WriteLine("Результат пересечения первого и второго множества:");
            Console.WriteLine("С помощью SimpleSet: " + ssmult.Show());
            Console.WriteLine("С помощью BitSet: " + bsmult.Show());

        }
        static void Main(string[] args)
        {
            string answer1 = "";
            while (true)
            {
                Console.WriteLine(@"1 — Тест операций над множествами;
2 — Режим диалога.");
                answer1 = Console.ReadLine();
                if (!(answer1 == "1" || answer1 == "2"))
                    Console.WriteLine("Некорректный ввод!");
                else break;
            }
            if (answer1 == "2")
            {
                Console.WriteLine("Да начнётся веселье (нет).");
                Dialog();
                Console.WriteLine("Вы вышли.");
            }
            if (answer1 == "1")
            {
                Test();
            }

            Console.ReadKey();
        }
    }
}
