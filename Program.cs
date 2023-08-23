using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;

namespace ConsoleApp2;
public class CaesarCipher
{
    const string RussianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    private string CodeEncode(string text, int k)
    {
        int letterQty;
        string fullalphabet;
        fullalphabet = RussianAlphabet + RussianAlphabet.ToLower();
        letterQty = fullalphabet.Length;
        var result = "";
        for (int i = 0; i < text.Length; i++)
        {
            var symbol = text[i];
            var temp = fullalphabet.IndexOf(symbol);
            if (temp < 0)
            {
                result += symbol.ToString();
            }
            else
            {
                var Index = (letterQty + temp + k) % letterQty;
                result += fullalphabet[Index];
            }
        }

        return result;
    }
    public string Encrypt(string Message, int key)
        => CodeEncode(Message, key);

    public string Decrypt(string encryptedMessage, int key)
        => CodeEncode(encryptedMessage, -key);
}

class Program
{
    static void Main(string[] args)
    {
        
        using (ApplicationContext db = new ApplicationContext())
        {
            var step = 3;
            var cipher = new CaesarCipher();
            string text;
            int quit = 0;
            while (quit == 0)
            {
                Console.Write("Укажите, что вы хотите сделать(зашифровать, расшифровать, выйти): ");
                string action = Console.ReadLine();
                if (action == "зашифровать")
                {
                    Console.Write("Введите русское слово, которое вы хотите зашифровать(ключ = 3): ");
                    text = Console.ReadLine();
                    var encryptedText = cipher.Encrypt(text, step);
                    Word slovo = new Word {WordCons = encryptedText};
                    db.Words.Add(slovo);
                    db.SaveChanges();
                    var Words = db.Words.ToList();
                    Console.WriteLine("Ваше слово добавлено в базу данных");
                }
                if (action == "расшифровать")
                {
                    Console.Write("Укажите индекс слова, которое вы хотите расшифровать: ");
                    int u = int.Parse(Console.ReadLine());
                    List<Word> selected = db.Words.Where(x => x.Id == u).ToList();
                    
                    foreach (Word a in selected)
                    {
                        text = $"{a.WordCons}";
                        var cryptedtext = cipher.Decrypt(text, step);
                        Console.WriteLine($"{a.Id} " + cryptedtext);
                    }
                }
                if (action == "выйти"){ 
                    quit = 1;
                }
            }
        }
    }
}
