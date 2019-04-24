/*
 * Name: Riley de Gans
 * Date: April 22nd, 2019
 * Description:  A program that cheats at scrabble using an online dictionary and some inefficient logic.
 */
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
        string currentWord, tempWord, lastWord, tiles, tempString, topWord, scoringLetters; //Declare string variables
        int maxPoints = 0;//Declare int variables
        string badLetters = "ETAOINSHRDLCUMWFGYPBVKJXQZ";//THe alphabet sorted by frequency in the english language to reduce run time by searching for the most common restricted letters
        public MainWindow()
        {
            InitializeComponent();
            ScrabbleGame sg = new ScrabbleGame();//Create the scrabble game class and draw your tiles
            tiles = sg.drawInitialTiles();
            tiles = "DSOPPE ";
            MessageBox.Show(tiles, "Your Tiles:");
            while (tiles.IndexOf(" ") != -1)//Check each index of the string for a blank space, remove it and count it as a blank tile
            {
                tiles = tiles.Remove(tiles.IndexOf(" "), 1);
            }
            int blankTiles = 7 - tiles.Length;
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();//Read the dictionary
                Stream download = wc.OpenRead("http://darcy.rsgc.on.ca/ACES/ICS4U/SourceCode/Words.txt");
                StreamReader sr = new StreamReader(download);
                StreamWriter sw = new StreamWriter("scoringWords.txt");
                for (int i = 0; i < tiles.Length; i++)//Take out the tiles from the alphabet
                {
                    int removeLetter = badLetters.IndexOf(tiles.Substring(i, 1));
                    if (removeLetter != -1)
                    {
                        badLetters = badLetters.Remove(removeLetter, 1);
                    }
                }
                while (!sr.EndOfStream)//Read through the whole stream
                {
                    scoringLetters = "";
                    int counter = 0;
                    currentWord = sr.ReadLine().ToUpper();
                    tempWord = currentWord;

                    //Logic sorted from most vague to most specific

                    if (currentWord.Length < 8 && currentWord != lastWord)//Only read words if they are 7 or less characters and if they are not duplicates (in the case of the headers)
                    {
                        for (int i = 0; i < badLetters.Length; i++)//Check if the current word contains any of the restricted letters
                        {
                            if (currentWord.Contains(badLetters.Substring(i, 1)))
                            {
                                counter++;//Count the number of restricted letters
                            }
                            if (counter > blankTiles)//If there are too many restricted letters
                            {
                                i = 26;//Cut the loop short to save time
                            }
                        }
                        if (counter <= blankTiles)//If the # of restricted letters is less than or equal to the # of blank tiles continue selection, in the case that there are no blank tiles there can be no restricted letters
                        {
                            for (int i = 0; i < tiles.Length; i++)//For each tile
                            {
                                string currentLetter = tiles.Substring(i, 1);
                                int removeLetter = tempWord.IndexOf(currentLetter);//Get the first index of the letter in the word
                                if (removeLetter > -1)
                                {
                                    scoringLetters += tempWord.Substring(removeLetter, 1);//Record the valid letters to be scored
                                    tempWord = tempWord.Remove(removeLetter, 1);//Take the tile out of the word
                                }
                            }
                            if (tempWord.Length <= blankTiles)//If the number of letters left over is less than or equal to the # of blank tiles they are replaced with blank tiles, if there are no blank tiles at this point, length should be zero
                            {
                                lastWord = currentWord;//Set the last word to the current word to eliminate repetition of words
                                tempString += currentWord + " ";
                                int totalPoints = 0;
                                for (int i = 0; i < scoringLetters.Length; i++)//Read each character of the string
                                {
                                        ScrabbleLetter sl = new ScrabbleLetter(scoringLetters.ToCharArray()[i]);//Get the point value for the character
                                        totalPoints += sl.Points;//Increase the total points of the word
                                }
                                if (totalPoints > maxPoints)//If the current word has more points
                                {
                                    maxPoints = totalPoints;//Set best word and best points to the current values
                                    topWord = currentWord.Substring(0, 1) + currentWord.Substring(1).ToLower();
                                }
                                else if (totalPoints == maxPoints)//If the points are the same 
                                {
                                    topWord += " or " + currentWord.Substring(0, 1) + currentWord.Substring(1).ToLower();//Add a new option for best points and best word 
                                }
                            }
                        }
                    }
                }
                sr.Close();
                sw.Close();
                MessageBox.Show(tempString + "\n\nHigh Scoring Word is " + topWord + " with a score of " + maxPoints + "\n\n" + "Your Tiles:\n\n" + tiles, "Scoring Words");//Print the output
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Error Encountered\n" + ex);//Account for errors
            }
        }
    }
}
