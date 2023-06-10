/* C#, lesson_45  27.05.2023

Task № exam:

Створити додаток «Словники».
Основне завдання проекту: зберігати словники різними мовами і дозволяти користувачеві знаходити переклад потрібного слова або фрази.
Інтерфейс додатку повинен надавати такі можливості:
'- cтворювати словник. Під час створення необхідно вказати тип словника. Наприклад, англо-українськии або україно-англійський.
'- додавати слово і його переклад до вже існуючого словника. Оскільки слово може мати декілька перекладів,
   необхідно дотримуватися можливості створення декількох варіантів перекладу.
'- замінювати слово або його переклад у словнику.
'- видаляти слово або переклад. Якщо слово видаляється, усі його переклади видаляються разом з ним.
   Не можна видалити переклад слова, якщо це останній варіант перекладу.
'- шукати переклад слова.
'- словники повинні зберігатися у файлах.
'- слово і варіанти його перекладів можна експортувати до окремого файлу результату.
'- при старті програми потрібно показувати меню для роботи з програмою. Якщо вибір пункту меню відкриває підменю,
   тоді в ньому потрібно передбачити можливість повернення до попереднього меню.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lesson_45_exam
{
/*************************************************************************************************************************************/
    [Serializable]
    public class Translation                    // клас Переклад - список перекладів слова
    {
        private List<string> words;

        public Translation()                    // конструктор без параметрамів
        {
            words = new List<string>();
        }
        public Translation(string word)         // конструктор з параметрами
        {
            words = new List<string>();
            Adding(word);
        }
        public int GetCount()                   // метод - повертає кількість слів-перекладів в списку
        {
            return words.Count;
        }
        public bool Searching(string word)      // метод - пошук співпадінь в списку слів-перекладів
        {
            if (words.Contains(word))
                return true;
            else return false;
        }
        public void Adding(string word)          // метод - додавання нового перекладу до списку
        {
            words.Add(word);
        }

        public void Editing(string new_word, int index) // метод - редагування слова-перекладу
        {
            words[index] = new_word;
        }

        public void Removing(int index) // метод - видалення слова-перекладу зі списку
        {
            words.RemoveAt(index);
        }

        public override string ToString() // метод - повернення списку слів-перекладів у вигляді string
        {
            if (words.Count == 1)
                return words[0];
            else
            {
                string result = "";
                foreach (var word in words)
                    result = result + word + ", ";
                return result;
            }
        }
    }
/*************************************************************************************************************************************/
    class Program           // основний клас
    {
        // статичний метод - пошук слова в словнику
        public static bool Searching(Dictionary<string, Translation> dictionary, string word)
        {
            if (dictionary.Keys.Contains(word))
                return true;
            else
                return false;
        }
//-------------------------------------------------------------------------------------------------------------------------------------
        // статичний метод - додавання нового слова і його перекладів в словник
        public static void Adding(Dictionary<string, Translation> dictionary, string word, string typeDict)
        {
            int choice;

            Translation translation = new Translation();
            Console.Write($"\nEnter a translation for the word \'{word}\': ");
            do
            {
                string wordTranslation;
                do
                {
                    wordTranslation = Console.ReadLine();					// введення нового перекладу слова
                    if (translation.Searching(wordTranslation) || !Cheking(wordTranslation, typeDict.Substring(4, 3)))	// перевірка перекладу на повторення і на правильність мови введення
                    {
                        Console.WriteLine("Error! There's already such a translation this word... ");
                        Console.WriteLine("or the input language of the word is chosen incorrectly. Try again.");
                        Console.Write($"\nEnter a translation for the word \'{word}\': ");
                    }
                    else break;
                } while (true);

                translation.Adding(wordTranslation);            // додавання нового перекладу до списку

                Console.Write("\nDoes this word have another translation? (1 - yes, other digit - no): ");
                choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 1)
                    Console.Write($"\nEnter another translation for the word \'{word}\': ");
            } while (choice == 1);

            dictionary.Add(word, translation);      // додавання нового слова з перекладами в словник
            Printing(dictionary, word);             // друк нового слова з перекладами
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
        // статичний метод - перевірка правильності вибору мови введення слова або перекладів
        public static bool Cheking(string word, string language)        
        {
            if (language == "ENG")				                        // перевірка латинських літер для англійських слів
            {
                foreach (var letter in word)
                {
                    if (Char.ToLower(letter) < 'a' || Char.ToLower(letter) > 'z')
                    {
                        return false;
                    }
                }
                return true;
            }
            else //if (language == "UKR")                   // перевірка літер кирилицею для українських слів
            {
                foreach (var letter in word)
                {
                    if (Char.ToLower(letter) <= (char)(127))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
//-------------------------------------------------------------------------------------------------------------------------------------
        // статичний метод - редагування слова і/або його перекладів
        public static void Editing(Dictionary<string, Translation> dictionary, string word, string typeDict)
        {
            int choice; string new_word = "", new_translation = ""; int index;

            Console.Write("\nDo you need to edit the word (1) or its translation(s) (other digit)? ");	// вибір - редагувати слово чи переклад
            choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)											// редагування основного слова
            {
                do
                {
                    Console.Write("\nEnter the new version of the word: ");
                    new_word = Console.ReadLine();
                    if (Searching(dictionary, new_word) || !Cheking(new_word, typeDict.Substring(0, 3))) // перевірка слова на повторення в словнику і на правильність мови введення
                    {
                        Console.WriteLine("Error! Such a word is already in the dictionary... ");
                        Console.WriteLine("or the input language of the word is chosen incorrectly. Try again.");
                    }
                    else break;
                } while (true);
            }

            do
            {
                Translation temp = dictionary[word];
                Console.Write("\nDo you need to add new translation (1)\nor edit current translation(s) (other digit) ? "); // редагувати - додати ще один переклад чи редагувати діючі?
                index = Convert.ToInt32(Console.ReadLine());

                if (index == 1)             // додавання ще одного перекладу
                {
                    string wordTranslation;
                    do
                    {
                        if (new_word == "")
                            Console.Write($"\nEnter a new translation for the word \'{word}\': ");
                        else
                            Console.Write($"\nEnter a new translation for the word \'{new_word}\': ");
                        wordTranslation = Console.ReadLine();                   // введення нового перекладу слова
                        if (temp.Searching(wordTranslation) || !Cheking(wordTranslation, typeDict.Substring(4, 3)))  // перевірка перекладу на повторення і на правильність мови введення
                        {
                            Console.WriteLine("Error! There's already such a translation this word... ");
                            Console.WriteLine("or the input language of the word is chosen incorrectly. Try again.");
                        }
                        else break;
                    } while (true);

                    temp.Adding(wordTranslation);            // додавання нового перекладу до списку		
                }
                else
                {
                    if (temp.GetCount() == 1)                       // редагування перекладу якщо міститься лише один переклад слова
                    {
                        do
                        {
                            Console.Write("\nEnter the new version of the translation: ");
                            new_translation = Console.ReadLine();
                            if (temp.Searching(new_translation) || !Cheking(new_translation, typeDict.Substring(4, 3))) // перевірка перекладу на повторення в списку і на правильність мови введення
                            {
                                Console.WriteLine("Error! There's already such a translation this word... ");
                                Console.WriteLine("or the input language of the word is chosen incorrectly. Try again.");
                            }
                            else break;
                        } while (true);

                        temp.Editing(new_translation, 0);
                    }

                    else                                // редагування перекладів якщо міститься більше одного перекладу слова
                    {
                        Console.WriteLine($"\nThe word \'{word}\' has {temp.GetCount()} translations. "); // друк кількості перекладів слова

                        do
                        {
                            do
                            {
                                Console.Write($"\nWhat number of translations do you need to edit? ");  // вибір номера перекладу слова для редагування
                                index = Convert.ToInt32(Console.ReadLine());
                                if (index < 1 || index > dictionary[word].GetCount())
                                {
                                    Console.WriteLine("Error! Try again.");
                                }
                                else break;
                            } while (true);

                            do
                            {
                                Console.Write("\nEnter the new version of the translation: ");      // введення відредагованого перекладу
                                new_translation = Console.ReadLine();
                                if (temp.Searching(new_translation) || !Cheking(new_translation, typeDict.Substring(4, 3))) // перевірка перекладу на повторення в списку і на правильність мови введення
                                {
                                    Console.WriteLine("Error! There's already such a translation this word... ");
                                    Console.WriteLine("or the input language of the word is chosen incorrectly. Try again.");
                                }
                                else break;
                            } while (true);

                            temp.Editing(new_translation, index - 1);   // редагування перекладу в списку по індексу
                            //break;

                            Console.Write("\nDo you need to continue editing the current translation? ('1' for 'yes'): ");
                            index = Convert.ToInt32(Console.ReadLine());
                            if (index != 1)
                                break;
                        } while (true);
                    }
                }

                if (choice == 1)            // редагування в словнику і слова і його перекладів
                {
                    dictionary.Remove(word);
                    dictionary.Add(new_word, temp);
                    word = new_word;
                }
                else                        // редагування в словнику тільки переклади слова
                {
                    dictionary[word] = temp;
                }
                Printing(dictionary, word);	// друк нового слова з перекладами	

                Console.Write("\nDo you need to continue editing? ('1' for 'yes'): ");
                index = Convert.ToInt32(Console.ReadLine());
                if (index != 1)
                    break;
            } while (true);
        }
//-------------------------------------------------------------------------------------------------------------------------------------
        // статичний метод - видалення слова і/або його перекладів зі словника
        public static void Deleting(Dictionary<string, Translation> dictionary, string word)
        {
            int choice;

            Console.Write("\nDo you need to delete the word (1) or its translation(s) (other digit)? ");
            choice = Convert.ToInt32(Console.ReadLine());       // вибір - видалити слово з усіма перекладами чи тільки окремі переклади

            if (choice == 1)				// видалення в цілому слова і його перекладів зі словника
            {
                dictionary.Remove(word);
                Console.WriteLine($"\nThe word \'{word}\' has been removed from the dictionary. ");
            }
            else
            {
                if (dictionary[word].GetCount() == 1)			// якщо в слова лише один переклад - видалення перекладу неможливе
                    Console.WriteLine($"\nThe word \'{word}\' has only one translation.\nIt's not possible to delete the translation.");
                else
                {
                    do
                    {
                        Console.WriteLine($"\nThe word \'{word}\' has {dictionary[word].GetCount()} translations. "); // друк кількості перекладів
                        do
                        {
                            Console.Write("\nWhat number of translations do you need to delete? ");		// вибір номеру перекладу зі списку для видалення
                            int index = Convert.ToInt32(Console.ReadLine());
                            if (index < 1 || index > dictionary[word].GetCount())
                            {
                                Console.WriteLine("Error! Try again.");
                            }
                            else
                            {
                                dictionary[word].Removing(index - 1);		// видалення перекладу зі списку по індексу
                                break;
                            }
                        } while (true);

                        Printing(dictionary, word);                 // друк слова з перекладами після часткового видалення перекладу

                        if (dictionary[word].GetCount() == 1)		// якщо в слова залишився лише один переклад - подальше видалення перекладу неможливе
                        {
                            Console.WriteLine($"\nThe word \'{word}\' already has only one translation.\nFurther deletion of the translation is not possible.");
                            break;
                        }
                        else
                        {
                            Console.Write("\n\nDo you need to continue deleting? ('1' for 'yes'): "); // продовжити видалення?
                            int index = Convert.ToInt32(Console.ReadLine());
                            if (index != 1)
                                break;
                        }
                    } while (true);
                }
            }
        }
//-------------------------------------------------------------------------------------------------------------------------------------
        // статичний метод - серіалізація об'єкту Dictionary і збереження його в бінарний файл
        public static void SerializeDict(string path, Dictionary<string, Translation> dictionary)
        {
            using (FileStream fileDict = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileDict, dictionary);
            }
        }
//-------------------------------------------------------------------------------------------------------------------------------------
        // статичний метод - завантаження з файлу серіалізованого об'єкту Dictionary
        public static Dictionary<string, Translation> Download(string path)
        {
            Dictionary<string, Translation> dictionary = new Dictionary<string, Translation>();
            using (FileStream fileDict = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                dictionary = (Dictionary<string, Translation>)formatter.Deserialize(fileDict);
            }
            return dictionary;
        }
//-------------------------------------------------------------------------------------------------------------------------------------
        // статичний метод - друк слова з його перекладами в консоль
        public static void Printing(Dictionary<string, Translation> dictionary, string word)
        {
            Console.WriteLine("\nWORD\t\tTRANSLATION(S)");
            Console.WriteLine("========================================================");
            Console.WriteLine($"{word}\t\t{dictionary[word]}");
        }
/*************************************************************************************************************************************/
        static void Main(string[] args)
        {
            Console.WriteLine("Program \"C# \"Task Exam \"Dictionary\"\n");
            int index, choice; string fileDict; string typeDict1 = "ENG_UKR", typeDict2 = "UKR_ENG", typeDict;
            string myFile = "MyDictionary.bin";
            Dictionary<string, Translation> dictionary = new Dictionary<string, Translation>();

            do
            {
                do								// меню № 1 - вибір виду словника
                {
                    Console.WriteLine("\nWhat kind of the dictionary do you need to use?\nENG-UKR - 1\nUKR-ENG - 2");
                    Console.Write("\nYour choice: ");
                    index = Convert.ToInt32(Console.ReadLine());
                    if (index != 1 && index != 2)
                        Console.WriteLine("Error! Try again.");
                } while (index != 1 && index != 2);

                if (index == 1)                 // формування назви файлу з якого завантажувати або в який зберігати словник
                    typeDict = typeDict1;
                else
                    typeDict = typeDict2;
                fileDict = typeDict + ".bin";

                if (File.Exists(fileDict))			// завантаження словника з файлу, якщо такий існує
                    dictionary = Download(fileDict);

                do									// меню № 2 - вибір дії, що робити зі словником
                {
                    Console.WriteLine("\nsearch the word in the dictionary - 1");
                    Console.WriteLine("save and close current dictionary - 2");
                    Console.WriteLine("return to the previous menu       - other digit");

                    Console.Write("\nChoose your next action: ");
                    choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:								// шукати слово в словнику
                            int choice2; string word;
                            do
                            {
                                Console.Write("\nEnter the word for searching: ");
                                word = Console.ReadLine();
                                if (!Cheking(word, typeDict.Substring(0, 3)))   // перевірка відповідності введеного слова до виду словника
                                {
                                    Console.WriteLine("Error! The language of the word is incorrect. Try again");
                                }
                                else break;
                            } while (true);

                            if (Searching(dictionary, word))		// якщо слово знайдене в словинку
                            {
                                Printing(dictionary, word);         // друк слова з перекладами

                                Console.WriteLine("\n\nedit   the word or its translation in the dictionary   - 1");	// меню № 3.1 - вибір дії, що робити зі знайденим словом
                                Console.WriteLine("delete the word or its translation from the dictionary - 2");
                                Console.WriteLine("record the word in my own dictionary file              - 3");
                                Console.WriteLine("return to the previous menu                            - other digit");
                                Console.Write("\nChoose your next action: ");
                                choice2 = Convert.ToInt32(Console.ReadLine());
                                switch (choice2)
                                {
                                    case 1:											// редагувати слово в словнику
                                        Editing(dictionary, word, typeDict);
                                        break;
                                    case 2:
                                        Deleting(dictionary, word);					// видалити слово або переклади зі словника
                                        break;
                                    case 3:                                         // записати слово у власний словник
                                        Dictionary<string, Translation> temp = new Dictionary<string, Translation>();
                                        if (File.Exists(myFile))                  // завантаження словника з файлу, якщо такий існує, в тимчасову змінну
                                             temp = Download(myFile);
                                        temp.Add(word, dictionary[word]);         // додати слово в тимчасовий словник
                                        SerializeDict(myFile, temp);
                                        Console.WriteLine($"\nThe word \'{word}\' was saved in your own dictionary file.");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else					            // якщо слово НЕ знайдене в словинку
                            {
                                Console.WriteLine("\nThe dictionary doesn't consist this word.\n");
                                Console.WriteLine("Do you need to add this word in the dictionary? ");
                                Console.WriteLine("\nadd a word to the dictionary - 1");		        // меню № 3.2 - вибір дії, що робити з новим словом
                                Console.WriteLine("return to the previous menu  - other digit");
                                Console.Write("\nYour choice: ");
                                choice2 = Convert.ToInt32(Console.ReadLine());
                                if (choice2 == 1)
                                {
                                    Adding(dictionary, word, typeDict);         // додавання нового слова з перекладами в словник
                                }
                            }
                            break;
                        case 2:
                            SerializeDict(fileDict, dictionary);					// серіалізація об'єкту СЛОВНИК і збереження його в файл
                            Console.WriteLine("\nA dictionary was saved in a file.");
                            break;
                        default:
                            break;
                    }
                } while (choice == 1);

                if (choice == 3)
                {
                    SerializeDict(fileDict, dictionary);	// якщо вихід в меню № 1 - то об'єкт СЛОВНИК також серіалізується і зберігається у файл
                }

            } while (choice != 1 && choice != 2);

//-------------------------------------------------------------------------------------------------------------------------------------
            // перегляд свого власного словника з окремого файлу
            Console.Write("\nDo you want to watch your own dictionary file? (1 - yes, other digit - no): ");
            index = Convert.ToInt32(Console.ReadLine());
            if (index == 1)
            {
                Dictionary<string, Translation> temp = new Dictionary<string, Translation>();
                if (File.Exists(myFile))                  // завантаження словника з файлу, якщо такий існує, в тимчасову змінну
                {
                    temp = Download(myFile);
                    foreach (var word in temp)
                        Printing(temp, word.Key);
                }
                else
                {
                    Console.WriteLine("\nSorry, your own dictionary file is still empty.");
                }
            }
            Console.WriteLine("\n\nThe dictionaries have been closed ...\n\n");    // словники закритий
        }
    }
}
