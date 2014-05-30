//Ryan Gritt
//CS3020
//Final Project: Deliverable 2
//April 23 2014

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
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        //Button to Create a New Form
        private void button1_Click(object sender, EventArgs e)
        {
            //Opens new Form
            Form1 newPuzzle = new Form1();
            newPuzzle.Show();
            
        }

        //on CRTL-W this window is closed
        private void Welcome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "W")
            {
                //Closes current window
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
 
            //Opens file dialog
            openFileDialog1.ShowDialog();
          
            //Used to use an XML load, but that does NOT WORK
            
            //Opens new Form1 Puzzle
            Form1 newPuzzle = new Form1();
            newPuzzle.Show();
         
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Message box for "about" information
            MessageBox.Show("Developed in 2014 by Ryan Gritt you can find out about this application at ryangritt.com");

            
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Opens help file, this file is where innosetup puts help.chm
            Help.ShowHelp(this, "C:\\Program Files (x86)\\RyanGrittSoftware\\help.chm");
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Message box offering "about" information
            MessageBox.Show("Developed in 2014 by Ryan Gritt you can find out about this application at ryangritt.com");
        }
    }
}
