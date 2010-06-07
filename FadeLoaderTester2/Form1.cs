using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FadeLoaderTester2
{
    public partial class Form1 : Form
    {
        private FadeLoader myFadeLoader;
        private Control controlToFade;
        public Form1()
        {
            InitializeComponent();
            controlToFade = this.tabPage1;
            myFadeLoader = new FadeLoader(controlToFade, this,
                Color.DarkOliveGreen, Color.BlanchedAlmond, 200);
        }

        private void showLoadingControlButton_Click(object sender, EventArgs e)
        {
            myFadeLoader.showLoadingControl();
            this.tabControl1.Show();
        }

        private void shrinkAndDisplayButton_Click(object sender, EventArgs e)
        {
            //this.tabControl1.Hide();
            //controlToFade.Hide();
            //shrink at timer_interval 100,  then fade in display 
            myFadeLoader.shrinkLoader(50);
        }

        private void fadeInButton_Click(object sender, EventArgs e)
        {
            //this.tabControl1.Hide();
            //this.tabPage1.Hide();

            ///////////
            myFadeLoader.fadeInImage(this.tabPage1);
            ///////////
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            tabPage1.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form f2 = new Form2();
            f2.Show();

            //Delay the dispose command
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.WriteLine("iii" + i + " " + j);
                }
            }
            f2.Dispose();
        }






    }
}
