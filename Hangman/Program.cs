// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


var randomWord = new RandomWord();
string word = randomWord.GetRandomWord();
var game = new Game(word);
Console.SetCursorPosition(0, 0);
Console.WriteLine("The word has " + word.Length + " letters" + " type your guess: ");

var countDown = new CountDown(120);
Task startTimer = Task.Run(() => countDown.Start());

Task startGame = Task.Run(() => {
    while (countDown.TotalTime > 0)
    {
        
        Console.SetCursorPosition(0, 3);
        Console.WriteLine(" ");
        Console.SetCursorPosition(16, 2);
        var letter = Console.ReadLine();
        if(letter.Length > 0)
        {
            game.MakeGuess(letter[0]);
            string displayWord = game.DisplayLettersOrUnderscores();
            Console.SetCursorPosition(5, 5);
            Console.WriteLine("Word: " + displayWord);

            if (!displayWord.Contains('_'))
            {
                Console.Clear();
                Console.WriteLine("you got it!");
                countDown.TotalTime = 0;
                return;
            }
        }
    }
});

Task.WaitAll(startTimer, startGame);

class RandomWord
{
    public string[] Words { get; set; } = new string[] { "hello", "bye" };

    public string GetRandomWord()
    {
        Random random = new Random();
        int index = random.Next(0, Words.Length);

        return Words[index];
    }
}

class Game
{
    string Word = "";
    char[] LettersOrUnderscores { get; }

    public Game(string word)
    {
        Word = word;
        LettersOrUnderscores = new char[word.Length];
        Array.Fill(LettersOrUnderscores, '_');
    }

    public bool MakeGuess(char guess)
    {
        if (Word.Contains(guess))
        {
            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == guess)
                {
                    LettersOrUnderscores[i] = guess;
                }
            }
            return true;
        }
        return false;
    }

    public string DisplayLettersOrUnderscores()
    {
        return String.Join("", LettersOrUnderscores);
    }
}

class CountDown
{
    public int TotalTime { get; set; }

    public CountDown(int totalTime)
    {
        TotalTime = totalTime;
    }

    public Task Start()
    {
        var timer = Task.Run(async () => {
            while (TotalTime >= 0)
            {
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("Time Remaining: " + TotalTime.ToString().PadLeft(2, '0') + " seconds");
                await Task.Delay(1000);
                TotalTime--;
            }
        });
        return timer;
    }
}

