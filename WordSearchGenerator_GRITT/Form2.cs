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
    public partial class puzzlePreview : Form
    {
        //Declare Important Variables
        public static String path;
        int pager = 1;
        int wordListLength = Form1.wordStringList.Length;
        public static WordPuzzle myPuzzle;
        public puzzlePreview()
        {
            InitializeComponent();
        }
        
        //Open Print Dialog when Print is Called **FROM CHRISES PRESENTATION**
        private void button1_Click(object sender, EventArgs e)
        {
            this.printDialog1.Document = this.printDocument1;

            DialogResult dr = this.printDialog1.ShowDialog(this);

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

       

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Save Option in File Menu
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open Save Dialog
            saveFileDialog1.Filter = "Text Files (*.txt) | *.txt";
            saveFileDialog1.ShowDialog();

            //Makes save directory variable
            string path = saveFileDialog1.FileName;

            //Calls saving method
            String save = Form1.myPuzzle.saveSettingToText();

            //Writes File
            File.WriteAllText(path,save);
        }

        //Open Option in File Menu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //Im Trying to use the XML Serializer to save and open data
            openFileDialog1.ShowDialog();
        }

        //New Option in File Menu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Creates and Displays newPuzzle window
            Form1 newPuzzle = new Form1();
            newPuzzle.Show();
        }

        //Online Help Menu  - Launches a webpage
        private void onlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ryangritt.com");
        }

        private void readManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Opens help file as defined in innosetup
            Help.ShowHelp(this, "C:\\Program Files (x86)\\RyanGrittSoftware\\help.chm");
            
        }

        //About message box (will eventually contain more info)
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developed in 2014 by Ryan Gritt you can find out about this application at ryangritt.com");

        }

        //Hotkeys are stored here
        private void puzzlePreview_KeyDown(object sender, KeyEventArgs e)
        {
            //Shortcut key for closing window
            if (e.Control && e.KeyCode.ToString() == "W")
            {
                this.Close();
            }
        }

        private void printPreviewControl1_Click(object sender, EventArgs e)
        {
            
        }

        //Print Puzzle
        //Creation of PrintDocument1 as taught in class
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Creation of needed fonts
            Font smallFont = new Font(FontFamily.GenericMonospace, 15);
            Font bigFont = new Font(FontFamily.GenericMonospace, 25, FontStyle.Bold);
           
            //Declaration of Printing Strings and sizing
            String text;
            SizeF size;

            //forLoop to assign word find to string
            for (int i = 0; i < Form1.myPuzzle.getHeight(); i++)
            {
                for (int j = 0; j < Form1.myPuzzle.getWidth(); j++)
                {
                    text = WordPuzzle.wordFindBase[i,j].ToString();
                    size = e.Graphics.MeasureString(text, smallFont);

                    //Prints first page -- the actual word find
                    if (pager == 1)                                                                   
                    {
                        //Draw String for Title
                        e.Graphics.DrawString(Form1.myPuzzle.getTitle(), bigFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
                        e.Graphics.DrawString(Form1.myPuzzle.getAuthor(), smallFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + (2) * size.Height);

                        //Draw String for puzzle
                        e.Graphics.DrawString(String.Format("{0}",text), smallFont, Brushes.Black, e.MarginBounds.Left + (j) * size.Width, e.MarginBounds.Top + (i + 4) * size.Height);
                    }
                }
            }
            //prints second page -- the list of words to find
            if (pager == 2)                   
            {
                //Declare title text
                text = "Word List";

                //Setting size for title
                size = e.Graphics.MeasureString(text, bigFont);

                //Draw String for word list title
                e.Graphics.DrawString(text, bigFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);

                //For loop for word list generation
                for (int i = 0; i < wordListLength; i++)
                {
                    //setting text to word in word list
                    text = Form1.wordStringList[i];

                    //set size for current word
                    size = e.Graphics.MeasureString(text, smallFont);

                    //Draw string for current word in word list
                    e.Graphics.DrawString(text, smallFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + (i + 2) * size.Height);
                }
            }
            //Either prints second page or ends printing
            if (pager != 2)                   
                e.HasMorePages = true;
            else e.HasMorePages = false;
            pager++; //increment number of pages
        }

        //Start printing document at 1
        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            pager = 1;
        }

        //On click preview is changed to the puzzle
        private void button2_Click(object sender, EventArgs e)
        {
            printPreviewControl1.Document = printDocument1;
        }

        //On click the puzzle is saved to a .txt file
        private void button3_Click(object sender, EventArgs e)
        {
            //Open Save Dialog
            saveFileDialog1.Filter = "Text Files (*.txt) | *.txt";
            saveFileDialog1.ShowDialog();

            //Makes save directory variable
            string path = saveFileDialog1.FileName;
            
            //Call Save Method
            WordPuzzle.saveToFile(path);         
            
        }

        //Generation of the print document for the solutions
        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Declare Fonts
            Font smallFont = new Font(FontFamily.GenericMonospace, 15);
            Font bigFont = new Font(FontFamily.GenericMonospace, 25, FontStyle.Bold);
            Font solutionFont = new Font(FontFamily.GenericMonospace, 15, FontStyle.Bold);

            //Declare text and size
            String text;
            SizeF size;
            
                //Loop printing the puzzle without solutions
                for (int k = 0; k < Form1.myPuzzle.getHeight(); k++)
                {
                    for (int l = 0; l < Form1.myPuzzle.getWidth(); l++)
                    {
                        text = WordPuzzle.wordFindBase[k, l].ToString();
                        size = e.Graphics.MeasureString(text, smallFont);


                        e.Graphics.DrawString(String.Format("{0}", text), smallFont, Brushes.Black, e.MarginBounds.Left + (l) * size.Width, e.MarginBounds.Top + (k + 4) * size.Height);

                    }
                }
                //Loop that highlights solution in red and bold, this prints an array without the other random characters filled in
                for (int i = 0; i < Form1.myPuzzle.getHeight(); i++)
                {
                    for (int j = 0; j < Form1.myPuzzle.getWidth(); j++)
                    {
                        text = WordPuzzle.wordFindSolution[i, j].ToString();
                        size = e.Graphics.MeasureString(text, smallFont);


                        //Draw String for Title
                        e.Graphics.DrawString(Form1.myPuzzle.getTitle(), bigFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
                        e.Graphics.DrawString(Form1.myPuzzle.getAuthor(), smallFont, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + (2) * size.Height);

                        e.Graphics.DrawString(String.Format("{0}", text), solutionFont, Brushes.Red, e.MarginBounds.Left + (j) * size.Width, e.MarginBounds.Top + (i + 4) * size.Height);

                    }
                e.HasMorePages = false;


            }
        }
        //Sets starting page at 1 for document2
        private void printDocument2_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            pager = 1;
        }
        //Prints the solutions **CODE IS FROM CHRIS'S Presentaiton**
        private void button4_Click(object sender, EventArgs e)
        {
            this.printDialog1.Document = this.printDocument2;

            DialogResult dr = this.printDialog1.ShowDialog(this);

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                this.printDocument2.Print();
            }
        }

        //Generates a print preview for the solutions.
        private void button5_Click(object sender, EventArgs e)
        {
            printPreviewControl1.Document = printDocument2;
        }

        private void printPuzzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.printDialog1.Document = this.printDocument1;

            DialogResult dr = this.printDialog1.ShowDialog(this);

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

        private void printSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.printDialog1.Document = this.printDocument2;

            DialogResult dr = this.printDialog1.ShowDialog(this);

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                this.printDocument2.Print();
            }
        }

        private void puzzlePreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.otherCounter = 0;
            Form1.optionSelector = null;
            

        }

        private void puzzlePreview_Load(object sender, EventArgs e)
        {

        }

        private void viewSolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Displays solutions in print preview
            printPreviewControl1.Document = printDocument2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Copies word puzzle to clipboard
            WordPuzzle.copyToClipboard();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            //Copies word puzzle to clipboard
            WordPuzzle.copyToClipboard();
        }

        private void viewPuzzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Displays puzzle in Print Preview
            printPreviewControl1.Document = printDocument1;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

   
        }
    }

