//Ryan Gritt
//CS3020
//Final Project: Deliverable 3
//May 7th 2014

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WordSearchGenerator_GRITT
{
    public class WordPuzzle
    {
        //Declare All Class Level Variables
        //Many are public static for use with saveXML
        private static String title;
        private static String author;
        public static char[,] wordFindSolution;
        public static char[,] wordFindBase; //Array of characters that is the basic array for puzzle
        private static int height; //public static for use with printerDocument
        private static int width; //public static for use with printerDocument


        private static int wordCount = 0; //Counts number of words
        private static int optionCount = 0; //Counts number of options selected
        private static String[] wordStringList; //Public static fore use with printerDocument
        private static String[] optionSelector; //Allows word direction to be randomized
        
        Random magicTool = new Random(); //built in visual c# random number generator

        //Method to return height
        public int getHeight()
        {
            return height;
        }

        //Method to return width
        public int getWidth()
        {
            return width;
        }

        //Method to Return puzzle title
        public string getTitle()
        {
            return title;
        }

        //Method to Return puzzle author
        public string getAuthor()
        {
            return author;
        }

        //Main cunstructor to create puzzle
        public WordPuzzle(int h, int w, String[] optionSelector, int wordCount, String titleTextString, String authorTextString, String[] wordStringList, int optionCount)
        {
            //Setting and intializing all variables
            optionCount = optionCount; //this seems to fix some errors, not sure why?
            height = h;
            width = w;
            title = titleTextString;
            author = authorTextString;
            wordFindBase = new char[h, w]; //Intializes wordFindBase to user defined dimensions
            wordFindSolution = new char[h, w]; //Intializaes solution

            //Nested for loop that will fill the array with null this helps me detrime which spots are 'available'
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    wordFindBase[i, j] = '0'; // i is the x dim and j is the y dim
                }
            }
            //Nested for loop that will fill the array with null this helps me detrime which spots are 'available'
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    wordFindSolution[i, j] = ' '; // i is the x dim and j is the y dim
                }
            }
            
            
                //Set Options for Difficulty
                /* Booleans are used here to choose options, while they are currently not used in my program, the idea is that
                 * I could make the logic more sound and simple by using booleans instead of strings so I added them for program 
                 * expandability
                 */

                //Populate Word Find

                //Flag used to determine when a word has actually been recorded to wordFindBase
                bool writeTrue = false;


                //foreach loop that will execute for every word entered in wordList
                foreach (String word in wordStringList)
                {


                    //Set Random Starting Point
                    int x = magicTool.Next(0, h); //magicTool is random declared above
                    int y = magicTool.Next(0, w);

                    //Raondmoize Word Direction
                    int randomizer = magicTool.Next(0, optionCount); //creates a random number between 0 and the number of options available

                    //Do While loop that will execute until a word is actually written to the puzzle
                    do
                    {
                        writeTrue = false;
                        if (optionSelector[randomizer].Equals("upDown")) //executes if upDown option was randomly selected 
                        {
                            if (upDownCheck(x, y, word) == true) //calls upDown Checker which will see if the word can be written to random pos.
                            {
                                upDownWrite(x, y, word); //if check is OK this will write this.word to wordFindBase
                                writeTrue = true; //Sets flag so do while knows the word was written
                            }
                            else
                            {
                                //Check failed so new random positon is generated
                                x = magicTool.Next(0, h);
                                y = magicTool.Next(0, w);
                            }

                        }
                        else if (optionSelector[randomizer].Equals("leftRight")) //executes if leftRight option was randomly selected
                        {
                            if (leftRightCheck(x, y, word) == true) //calls leftRight checker to see if word can be written
                            {
                                leftRightWrite(x, y, word); //Check is OK, word is written using leftRight
                                writeTrue = true; //flag is set for do-while
                            }
                            else
                            {
                                //Check Failed - position is re-randomized
                                x = magicTool.Next(0, h);
                                y = magicTool.Next(0, w);
                            }
                        }
                        else if (optionSelector[randomizer].Equals("rightLeft")) //executes if rightLeft option was randomly selected
                        {
                            if (rightLeftCheck(x, y, word) == true) //calls rightLeft checker to see if word can be written
                            {
                                rightLeftWrite(x, y, word); //Check is OK, word is written using rightLeft
                                writeTrue = true; //flag is set for do-while
                            }
                            else
                            {
                                //Check Failed, position is re-randomized
                                x = magicTool.Next(0, h);
                                y = magicTool.Next(0, w);
                            }
                        }
                        else if (optionSelector[randomizer].Equals("diag")) // executes if diagonal option was randomly selected
                        {
                            if (diagCheck(x, y, word) == true) //calls diagonal checker to see if word can be written (does it fit)
                            {
                                diagWrite(x, y, word); //Check is OK, word is written using diag
                                writeTrue = true; // flag is set for do-while
                            }
                            else
                            {
                                //Check Failed, position is re-randomized
                                x = magicTool.Next(0, h);
                                y = magicTool.Next(0, w);
                            }
                        }
                        else
                        {
               
                        }
                    }
                    while (writeTrue == false); //Checks the writeTrue flag
                }

                //Calls method that will randomly fill in letters to wordFindBase
                wordFillerInnner();

                //Opens window to preivew/print the puzzle
                puzzlePreview previewWindow = new puzzlePreview();
                previewWindow.Show();
            }

            
        
        //Method to check words top to bottom
        private bool upDownCheck(int x, int y, String word)
        {
            //Create Flag For Return Value
            bool check = true;

            //Convert passed word to Character Array for checking
            char[] letters = new char[word.Length];
            letters = word.ToCharArray();

            //Beging of try/catch to check for exception errors (word is placed outside of array)
            try
            {

                for (int i = 0; i < word.Length; i++) //for loop going through every letter in current word
                {
                    //if-statment that will check if all the spaces in the current word are empty
                    if ((wordFindBase[x + i, y].Equals('0')) || (wordFindBase[x + i, y].Equals(letters[i])))
                    {
                        //nothing will execute, flag will stay as 'true' and method will return 'true'
                    }
                    else
                    {

                        //if the above statement was false, the check flag will be set to false and method will return false
                        check = false;
                    }
                }


            }
            //if exception occurs flag is set to false
            catch (Exception e)
            {
                check = false;
            }


            //Return Boolean (true if OK false if not)
            return check;
        }

        //Method to check words left to right
        private bool leftRightCheck(int x, int y, String word)
        {
            //Create Flag For Return Value
            bool check = true;

            //Convert passed word to Character Array for checking
            char[] letters = new char[word.Length];
            letters = word.ToCharArray();

            //Beging of try/catch to check for exception errors (word is placed outside of array)
            try
            {

                for (int i = 0; i < word.Length; i++) //for loop going through every letter in current word
                {
                    if ((wordFindBase[x, y + i].Equals('0')) || (wordFindBase[x, y + i].Equals(letters[i]))) //Checks if the needed positions are empty
                    {
                        //flag stays true
                    }
                    else
                    {
                        //check failed, flag is set to false
                        check = false;
                    }
                }


            }
            //will catch exections, when a word is placed outside of array
            catch (Exception e)
            {
                check = false;
            }
            return check;
        }

        //Method to fill in the Word Find with random letters 
        private void wordFillerInnner()
        {
            //Nested for loop that goes through the two dimensional array, filling in random characters
            for (int i = 0; i < height; i++) //x dimension
            {
                for (int j = 0; j < width; j++) //y dimension
                {
                    if(wordFindBase[i,j].Equals('0') == true){ //only inserts a raondom character if the position is empty (see write methods)
                        //Char array populated with all characters except vowels, I did this to avoid created accidental words
                        char[] letters = {  'B', 'C', 'D',  'F', 'G', 'H',  'J', 'K', 'L', 'M', 'N',  'P', 'Q', 'R', 'S', 'T',  'V', 'W', 'X', 'Y', 'Z' }; 
                        //Using random variable, this assignes a character from above char array 'letters'
                        wordFindBase[i, j] = letters[magicTool.Next(0, 20)];
                    }
                }
            }
        }
        
        //Method to Check word right to left
        private bool rightLeftCheck(int x, int y, String word)
        {
            //Create Flag For Return Value
            bool check = true;

            //Convert passed word to Character Array for checking
            char[] letters = new char[word.Length];
            letters = word.ToCharArray();


            //Beging of try/catch to check for exception errors (word is placed outside of array)
            try
            {

                //for loop that goes through every character in this.word
                for (int i = 0; i < word.Length; i++)
                {
                    if ((wordFindBase[x, y - i].Equals('0')) || (wordFindBase[x, y - i].Equals(letters[i]))) //Checks if needed positions are empty
                    {
                        //flag stays true
                    }
                    else
                    {
                        //check failed, flag is set to false
                        check = false;
                    }
                }


            }

            //catches exceptions, when the word is placed outside of array
            catch (Exception e)
            {
                check = false;
            }

            //returns check value
            return check;
        }

        //Method to check words diagonally
        private bool diagCheck(int x, int y, String word)
        {
            //Create Flag For Return Value
            bool check = true;

            //Convert passed word to Character Array for checking
            char[] letters = word.ToCharArray();

            //Beging of try/catch to check for exception errors (word is placed outside of array)
            try
            {
                //for loop to go through every letter in current word
                for (int i = 0; i < word.Length; i++)
                {
                    if ((wordFindBase[x - i, y + i].Equals('0')) || (wordFindBase[x - i, y + i].Equals(letters[i])))
                    {
                        //flag stays set at true 
                    }
                    else
                    {
                        //check failed, check is set to false
                        check = false;
                    }
                }


            }

            //catches all exception errors, whenever word is placed outside of array
            catch (Exception e)
            {
                check = false;
            }

            //returns check value
            return check;
        }

        //Method to write words to upDown
        private void upDownWrite(int x, int y, String word)
        {
            //converts current word to char array
            char[] letters = word.ToCharArray();

            //Goes through every character writing each one
            for (int i = 0; i < word.Length; i++)
            {
                //Writes current character to wordFindBase and wordFindSolution moving the up one each time
                wordFindBase[x + i, y] = letters[i];
                wordFindSolution[x + i, y] = letters[i];
            }
        }

        //Method to Write words left to right
        private void leftRightWrite(int x, int y, String word)
        {
            //converts current word to char array
            char[] letters = word.ToCharArray();

            //Goes through every character writing each one
            for (int i = 0; i < word.Length; i++)
            {
                //Writes current character to wordFindBase and wordFindSolution moving to the right each time
                wordFindBase[x, y + i] = letters[i];
                wordFindSolution[x, y + i] = letters[i];
            }
        }

        //Method to write words right to left
        private void rightLeftWrite(int x, int y, String word)
        {
            //converts current word to char array
            char[] letters = word.ToCharArray();

            //Goes through every character writing each one
            for (int i = 0; i < word.Length; i++)
            {
                //Writes current character to wordFindBase and wordFindSolution moving to the left each time (makes word backwards)
                wordFindBase[x, y - i] = letters[i];
                wordFindSolution[x, y - i] = letters[i];
            }
        }
        //File Open
        public String openSettingFronText()
        {
            String openFile = null;



            return openFile;

        }
        //File Save
        public String saveSettingToText()
        {
            //Creates string that will containt file save
            String saveFile = null;

            //Creates string building which will serialize info
            var saveFileStringBuilder = new StringBuilder();

            //Write each of all the words on a new line in string
            foreach (string word in Form1.wordStringList)
            {
                saveFileStringBuilder.AppendLine(word);
            }

            //Separtes data with a new line and "***"
            saveFileStringBuilder.AppendLine("\n***\n");

            //Writes height on new line
            saveFileStringBuilder.AppendLine(height.ToString());
            
            //Writes width on new line
            saveFileStringBuilder.AppendLine(width.ToString());

            //Separates data with a new line and "***"
            saveFileStringBuilder.AppendLine("\n***\n");

            //Creates a string variables for writing arrays
            string tempLine = "";

            //Nested For-Loop stepping through the array
            for (int i = 0; i < height; i++)
            {
                tempLine = "";//Sets tempLine back to ''
                for (int j = 0; j < width; j++)
                {
                    //Adds current char in array to "tempLine"
                    tempLine = tempLine + wordFindBase[i, j];
                }
                //Writes tempLine to stringBuilder
                saveFileStringBuilder.AppendLine(tempLine);
            }

            //Separates data with a new line and "***"
            saveFileStringBuilder.AppendLine("\n***\n");

            //Nested for loop to print char array
            for (int i = 0; i < height; i++)
            {
                tempLine = ""; //sets tempLine back to ''

                for (int j = 0; j < width; j++)
                {
                    //Writes current char in array to tempLIne
                    tempLine = tempLine + wordFindSolution[i, j];
                }

                //Writes templine to the string builder
                saveFileStringBuilder.AppendLine(tempLine);
            }

            //Sets templine back to ''
            tempLine = "";

            //Sets string builder to the returned string
            saveFile = saveFileStringBuilder.ToString();

            //returns saveFile string
            return saveFile;

        }

        //Method to copy ASCII to clipboard
        public static void copyToClipboard()
        {
            //Intialized string used in copying
            String clip = "";

            //nested for loop writing every character to ASCII
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //Adds current char in array to the string
                    clip = clip + wordFindBase[i, j].ToString();
                }
                clip = clip + "\r\n"; //ends line and starts a new line
            }

            Clipboard.SetText(clip); //copies the current line to the clipboard
        }

        //This method could be used to write word find to a text file
        public static void saveToFile(String path)
        {
            //System function to write to file
            System.IO.StreamWriter file = new System.IO.StreamWriter(@path);
            
            //Writes Text Header Information
            file.WriteLine(title);
            file.WriteLine(author);
            file.WriteLine("");
            file.WriteLine("");

            //nested for loop writing every character to text file
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    file.Write(wordFindBase[i, j].ToString());

                }
                file.WriteLine();
            }
            file.Close(); //close file
        }
        //Method to write words diagonally
        private void diagWrite(int x, int y, String word)
        {
            //converts current word to char array
            char[] letters = word.ToCharArray();

            //Goes through every character writing each one
            for (int i = 0; i < word.Length; i++)
            {
                //Writes current character to wordFindBase and wordFindSolution moving up and right each time (think slope)
                wordFindBase[x - i, y + i] = letters[i];
                wordFindSolution[x - i, y + i] = letters[i];
            }
        }
    }
    }

