using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;

namespace _184863Unit3Summative
{
    public partial class MainWindow : Window
    {
        string currentWord, tempWord, lastWord, tiles;

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            tiles = txtInput.Text;
            MessageBox.Show(tiles, "Your Tiles:");
            while (tiles.IndexOf(" ") != -1)
            {
                tiles = tiles.Remove(tiles.IndexOf(" "), 1);
            }
            int blankTiles = 7 - tiles.Length;
            System.Net.WebClient wc = new System.Net.WebClient();
            Stream download = wc.OpenRead("http://darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt");
            StreamReader sr = new StreamReader(download);
            StreamWriter sw = new StreamWriter("scoringWords.txt");
            for (int i = 0; i < tiles.Length; i++)
            {
                int removeLetter = badLetters.IndexOf(tiles.Substring(i, 1));
                if (removeLetter != -1)
                {
                    badLetters = badLetters.Remove(removeLetter, 1);
                }
            }
            while (!sr.EndOfStream)
            {
                int counter = 0;
                currentWord = sr.ReadLine().ToUpper();
                tempWord = currentWord;
                if (currentWord.Length < 8 && currentWord != lastWord)
                {
                    for (int i = 0; i < badLetters.Length; i++)
                    {
                        if (currentWord.Contains(badLetters.Substring(i, 1)))
                        {
                            counter++;
                        }
                        if (counter > blankTiles)
                        {
                            i = 26;
                        }
                    }
                    if (counter <= blankTiles)
                    {
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            string currentLetter = tiles.Substring(i, 1);
                            int removeLetter = tempWord.IndexOf(currentLetter);
                            if (removeLetter > -1)
                            {
                                tempWord = tempWord.Remove(removeLetter, 1);
                            }
                        }
                        if (tempWord.Length == blankTiles || tempWord.Length == 0)
                        {
                            sw.WriteLine(currentWord);
                            lastWord = currentWord;
                        }
                    }
                }
            }
            sr.Close();
            sw.Close();
            StreamReader sr2 = new StreamReader("scoringWords.txt");
            string tempString = "";
            int maxPoints = 0;
            string topWord = "";
            while (!sr2.EndOfStream)
            {
                currentWord = sr2.ReadLine();
                int totalPoints = 0;
                for (int i = 0; i < currentWord.Length; i++)
                {
                    if (!badLetters.Contains(currentWord.ToCharArray()[i]))
                    {
                        ScrabbleLetter sl = new ScrabbleLetter(currentWord.ToCharArray()[i]);
                        totalPoints += sl.Points;
                    }
                }
                if (totalPoints > maxPoints)
                {
                    maxPoints = totalPoints;
                    topWord = currentWord;
                }
                tempString += currentWord + " ";
            }
            txtOutput.Text = "Scoring Words:\n\n" + tempString + "\n\nHigh Scoring Word is " + topWord.Substring(0, 1) + topWord.Substring(1).ToLower() + " with a score of " + maxPoints + "\n\n" + "Your Tiles:\n\n" + tiles;
        }

        string badLetters = "ETAOINSHRDLCUMWFGYPBVKJXQZ";//SORT FROM MOST COMMON TO LEAST
        public MainWindow()
        {
            InitializeComponent();
            ScrabbleGame sg = new ScrabbleGame();
            tiles = sg.drawInitialTiles();
            tiles = "ACK TLR";
            MessageBox.Show(tiles, "Your Tiles:");
            while (tiles.IndexOf(" ") != -1)
            {
                tiles = tiles.Remove(tiles.IndexOf(" "), 1);
            }
            int blankTiles = 7 - tiles.Length;
            System.Net.WebClient wc = new System.Net.WebClient();
            Stream download = wc.OpenRead("http://darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt");
            StreamReader sr = new StreamReader(download);
            StreamWriter sw = new StreamWriter("scoringWords.txt");
            for (int i = 0; i < tiles.Length; i++)
            {
                int removeLetter = badLetters.IndexOf(tiles.Substring(i, 1));
                if (removeLetter != -1)
                {
                    badLetters = badLetters.Remove(removeLetter, 1);
                }
            }
            while (!sr.EndOfStream)
            {
                int counter = 0;
                currentWord = sr.ReadLine().ToUpper();
                tempWord = currentWord;
                if (currentWord.Length < 8 && currentWord != lastWord)
                {
                    for (int i = 0; i < badLetters.Length; i++)
                    {
                        if (currentWord.Contains(badLetters.Substring(i, 1)))
                        {
                            counter++;
                        }
                        if (counter > blankTiles)
                        {
                            i = 26;
                        }
                    }
                    if (counter <= blankTiles)
                    {
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            string currentLetter = tiles.Substring(i, 1);
                            int removeLetter = tempWord.IndexOf(currentLetter);
                            if (removeLetter > -1)
                            {
                                tempWord = tempWord.Remove(removeLetter, 1);
                            }
                        }
                        if (tempWord.Length == blankTiles || tempWord.Length == 0)
                        {
                            sw.WriteLine(currentWord);
                            lastWord = currentWord;
                        }
                    }
                }
            }
            sr.Close();
            sw.Close();
            StreamReader sr2 = new StreamReader("scoringWords.txt");
            string tempString = "";
            int maxPoints = 0;
            string topWord = "";
            while (!sr2.EndOfStream)
            {
                currentWord = sr2.ReadLine();
                int totalPoints = 0;
                for (int i = 0; i < currentWord.Length; i++)
                {
                    if (!badLetters.Contains(currentWord.ToCharArray()[i]))
                    {
                        ScrabbleLetter sl = new ScrabbleLetter(currentWord.ToCharArray()[i]);
                        totalPoints += sl.Points;
                    }
                }
                if (totalPoints > maxPoints)
                {
                    maxPoints = totalPoints;
                    topWord = currentWord.Substring(0, 1) + currentWord.Substring(1).ToLower();
                }
                else if (totalPoints == maxPoints)
                {
                    topWord += " or " + currentWord.Substring(0, 1) + currentWord.Substring(1).ToLower();
                }
                tempString += currentWord + " ";
            }
            MessageBox.Show(tempString + "\n\nHigh Scoring Word is " + topWord + " with a score of " + maxPoints + "\n\n" + "Your Tiles:\n\n" + tiles, "Scoring Words");
            sr2.Close();
        }
    }
}
