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
using System.IO;

namespace WordSearchGenerator_GRITT
{
    public partial class Form1 : Form
    {
        //Declare All Class Level Variables
        public static WordPuzzle myPuzzle; //creates the puzzle object
        private static String titleTextString; //defines the title
        private static String authorTextString; // defines the author
        private static int wordLength = 0; //int to check wordLength is less than height/width
        private static int h = 0; //public static for use with printerDocument
        private static int w = 0; //public static for use with printerDocument
        private static int wordCount = 0; //Counts number of words
        private static String wordCountS = "0"; //String to print out number of words
        private static int validCheck = 0; //Checks form validity to provide errors
        private static String wordLoad; //used in fileLoad (not currently working)
        //These must be public for printing to and passing to work
        public static int otherCounter = 0; //Counts number of options selected    
        public static String[] wordStringList; //Public static fore use with printerDocument
        public static String[] optionSelector; //Allows word direction to be randomized
        
        //The Random number Generator
        Random magicTool = new Random(); //built in visual c# random number generator
        
        public Form1()
        {
            InitializeComponent();
            
            //Sets default options 
            textBox2.Text = "25"; //sets default height
            textBox3.Text = "25"; //sets default width
            titleText.Text = "Sample Puzzle Title"; //sets default title
            authorText.Text = "Sample Puzzle Author"; //sets default author


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Tool Tips created for better UX
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip(); //Initialize tool tips
            ToolTip1.SetToolTip(this.button2, "This will generate a word find based on the parameters you entered");
            ToolTip1.SetToolTip(this.label7, "The Current number of words in your word bank");
            ToolTip1.SetToolTip(this.button1, "Add a word to the word bank");
            ToolTip1.SetToolTip(this.button3, "Remove selected word from the word bank");
            ToolTip1.SetToolTip(this.textBox2, "Set height of the puzzle (in chars)");
            ToolTip1.SetToolTip(this.textBox3, "Set width of the puzzle (in chars)");
            ToolTip1.SetToolTip(this.wordBank, "The list of words currenlty in word bank");

            //Prints out the number of words above the wordList
            label7.Text = wordCountS;
        }
     
        //Method to Add new words to wordBank
        private void wordAdd()
        {
            //Checks if the textbox is empty before adding words (empty words cant be added)
            if (!(String.IsNullOrEmpty(textBox1.Text)))
            {
                String storage = textBox1.Text.ToUpper();
                
                //Adds item to wordBank from storage
                wordBank.Items.Add(storage);

                //Checks to see if current word is the longest entered
                //This is used for bulletproofing
                if (storage.Length > wordLength)
                {
                    //Sets word lenght to the length of the current word
                    wordLength = storage.Length;
                }

                //Once word is adding text box is set to null
                textBox1.Text = null;

                //Updates Current Word Count and Prints
                wordCount = wordBank.Items.Count;
                wordCountS = wordCount.ToString();
                label7.Text = "( " + wordCountS + " )";

            }
            else
            {
                //Lets user know if they didn't enter a word properlly
                MessageBox.Show("You can not enter an empty word");
            }
        }

        //Method to Copy Words from wordBank
        private void copyWordList()
        {
            //Checks to make sure there are words in the word list
            if (wordBank.Items.Count > 0)
            {
                //Makes blank string
                string wordCopy = "";
                foreach (object item in wordBank.Items)    //This is executed for every word in wordBank
                {
                    //Adds word to string and makes a new line
                    wordCopy = wordCopy + item.ToString() + "\r\n";
                }

                //Sets the string equal to the clipBoard
                Clipboard.SetText(wordCopy);
            }
        }

        //Method to Paste Words to wordBank
        private void pasteWordList()
        {
            //Creates string for wordPaste and sets it to the clipboard
            string wordPaste = Clipboard.GetText();

            //Ensures that wordPaste is all upper case
            wordPaste = wordPaste.ToUpper();

            //Splits the wordString into an array of words separating by newLine
            string[] wordPasteLines = wordPaste.Split('\n');

            foreach (string word in wordPasteLines)         //executes wordAdd for every word in the list
            {
                if (!(word.Equals(""))) // Only executes if a word is there
                {
                    wordBank.Items.Add(word.Trim()); //Adds current word to the wordBank
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Checks for blank height/width fields, if blank, they are set to default
            if ((string.IsNullOrEmpty(textBox2.Text)) || (string.IsNullOrEmpty(textBox3.Text)))
            {
                textBox2.Text = "25";
                textBox3.Text = "25";
                h = Convert.ToInt32(textBox2.Text); //converts String to Int
                w = Convert.ToInt32(textBox3.Text); //converts String to Int

            }
            else
            {
                h = Convert.ToInt32(textBox2.Text); //converts String to Int
                w = Convert.ToInt32(textBox3.Text); //converts String to Int
            }
            //Assigns Title and Author
            titleTextString = titleText.Text;
            authorTextString = authorText.Text;

            //Get Words into Array
            wordStringList = new String[wordCount]; //Generate new array of strings with numElements == numWords(wordCount)
            wordBank.Items.CopyTo(wordStringList, 0); //Listbox funtion to copy an item to the stringList

            //Checks to make sure form is accuratly filled out
            if ((wordCount > 0) && (h >= wordLength) && (w >= wordLength) && (h < 34) && (w < 37)  )
            {
                //Set Counts number of options
                if (checkBox2.Checked == true)
                {
                    otherCounter++;
                    validCheck = 1;
                }
                if (checkBox3.Checked == true)
                {
                    otherCounter++;
                    validCheck = 1;
                }
                if (checkBox4.Checked == true)
                {
                    otherCounter++;
                    validCheck = 1;
                }
                if (checkBox5.Checked == true)
                {
                    otherCounter++;
                    validCheck = 1;
                }

                //Sets options in option counter
                optionSelector = new String[otherCounter];
                int index = 0;

                if (checkBox2.Checked == true)
                {
                    optionSelector[index++] = "upDown";
                }
                if (checkBox3.Checked == true)
                {
                    optionSelector[index++] = "leftRight";
                }
                if (checkBox4.Checked == true)
                {
                    optionSelector[index++] = "rightLeft";
                }
                if (checkBox5.Checked == true)
                {
                    optionSelector[index++] = "diag";
                }

                //Creates Puzzle and sends it the data needed to generate a puzzle
                myPuzzle = new WordPuzzle(h, w, optionSelector, wordCount, titleTextString, authorTextString, wordStringList, otherCounter);
            }
            else{
                
                //Error message for too few words
                if(wordCount < 1){
                    MessageBox.Show("You Have Not Entered Enough Words");
                }
                
                //Error message for small height
                else if (h < wordLength)
                {
                    MessageBox.Show("You Height must be larger than your longest word");
                }
                
                //Error message for small width
                else if (w < wordLength)
                {
                    MessageBox.Show("Your Width must be larger than your longest word");
                }
                
                //Error message for large width
                else if (w > 36)
                {
                    MessageBox.Show("Your width is too big to fit on a sheet of paper");
                }
                
                //Error message for large height
                else if (h > 33)
                {
                    MessageBox.Show("Your height is too big to fit on a sheet of paper");
                }
                
                //Error message if something else went terribly wrong
                else
                {
                    MessageBox.Show("Some other error has occured, please contact tech support");
                }
        } 
    }
                           
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Calls the method to add a word to the wordBank
            wordAdd(); 
        }
        private void wordBank_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void label4_Click(object sender, EventArgs e)
        {
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {           
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {            
        }
        private void label6_Click(object sender, EventArgs e)
        {
        }
        private void label5_Click(object sender, EventArgs e)
        {
        }
        
        //Menu strip option to save puzzle
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create Puzzle (One Must Be Created to Save)
            myPuzzle = new WordPuzzle(h, w, optionSelector, wordCount, titleTextString, authorTextString, wordStringList, otherCounter);


            //Launches saveFileDialog
            saveFileDialog1.Filter = "Text Files (*.txt) | *.txt"; //Sets permisable file types
            saveFileDialog1.ShowDialog();
            
            //Sets a file path to the save location
            string path = saveFileDialog1.FileName;

            //Calls the save method from myPuzzle
            String save = Form1.myPuzzle.saveSettingToText();

            //Writes Files
            File.WriteAllText(path, save);
            
        }
        
        //Menu strip option to open existing puzzle
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {     
            //Opens Load File Dialog
            openFileDialog1.ShowDialog();
           //Used to have an XML Loader but it DID NOT work  
        }
        
        //Menu strip option to create new puzzle
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create a launch a new instance of newPuzzle form
            Form1 newPuzzle = new Form1();
            newPuzzle.Show();
        }
        
        //Button to remove word from wordList
        private void button3_Click(object sender, EventArgs e)
        {
            //Checks to make sure words are entered (we dont want a negative word count)
            if (wordCount > 0) 
            {
                //Removes selected item from wordBank (listBox)
                wordBank.Items.Remove(wordBank.SelectedItem);

                //Adjusts and reprint the word count
                wordCount = wordBank.Items.Count;
                wordCountS = wordCount.ToString();
                label7.Text = "( " + wordCountS + " )";
            }
            else
            {
                //If word cannot be removed the user is alerted
                MessageBox.Show("You Have No Words to Remove");
            }
        }
        private void onlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Goes to website for online help/contact information
            System.Diagnostics.Process.Start("http://www.ryangritt.com");
        }
        
        //Shortcut Keys (hot keys
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
            //Hotkey for addWord (enter)
            if ((e.KeyCode == Keys.Enter) && !(String.IsNullOrEmpty(textBox1.Text)))
            {
                wordAdd();
            }

            //Hotkey to close current widnow (CRTL - W)
            if (e.Control && e.KeyCode.ToString() == "W")
            {
                this.Close();
            }
        }
        
        //Close application and check for save.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to exit ? (All Unsaved Changes will be LOST!", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            } 
        }
        
        //Tool menu to show 'about' message box
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developed in 2014 by Ryan Gritt you can find out about this application at ryangritt.com");
        }

        //Change the Selected Word
        private void button4_Click(object sender, EventArgs e)
        {
            //Remove Selected Word
            textBox1.Text = wordBank.SelectedItem.ToString();

            wordBank.Items.Remove(wordBank.SelectedItem);

            //User hits addWordd or enter to make the change


            //DO NOT CHANGE WORD COUNT (the word was essentially replaced)
        }

        //Opens the Help File
        private void readManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Calls the help file as defined with innosetup
            Help.ShowHelp(this, "C:\\Program Files (x86)\\RyanGrittSoftware\\help.chm");
        }

        //Calls Copy Word List
        private void copyWordListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Calls Copy word List Method
            copyWordList();
        }

        //Calls Paste word List
        private void pasteWordListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Calls Paste Word List Method
            pasteWordList();
        }
        
        private void wordSearchGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //var used for debuging 
            validCheck = 1;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //var used for debuging
            validCheck = 1;
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //var used for debuging
            validCheck = 1;
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //var used for debuging
            validCheck = 1;
        }
        private void groupBox4_Enter(object sender, EventArgs e)
        {
        }
        private void titleText_TextChanged(object sender, EventArgs e)
        {    
        }
        private void authorText_TextChanged(object sender, EventArgs e)
        {
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }
        //Mouse Click Events
        private void wordBank_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                //Opens context menu on right click
                case MouseButtons.Right:
                    {
                        contextMenuStrip1.Show(Cursor.Position); //places the menu at the pointer position
                    }
                    break;
            }
        }

        //Context menu word copy
        private void copyWord_Click(object sender, EventArgs e)
        {
            try
            {
                //Copys selected word to clipboard
                Clipboard.SetText(wordBank.SelectedItem.ToString());
            }
            catch (Exception Ex)
            {
                MessageBox.Show("You must select a word");
            }
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            //Shortcut Key to Close Window
            if (e.Control && e.KeyCode.ToString() == "W")
            {
                this.Close();
            }
            
            //Shortcut Key for wordAdd
            if (e.KeyCode == Keys.Enter)
            {
                wordAdd();
            }
        }

        private void pasteWord_Click(object sender, EventArgs e)
        {
            try
            {
                //Sets Textbox to Pasted Word
                textBox1.Text = Clipboard.GetText();

                //Calls wordAdd method
                wordAdd();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is nothing to Paste");
            }
        }

      




    }
}
