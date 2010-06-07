using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsControlLibrary1;

namespace FadeLoaderTester2
{
    class FadeLoader : IDisposable
    {
        private LoadingControl loader;
        private Control parentControl;
        private Form parentForm;
        //private Control parentForm;
        private Bitmap fadeBMP;
        private float alpha;
        private ColorMatrix fadeColorMatrix;
        private ImageAttributes fadeImageAttributes;
        private Timer fadeTimer;

        public FadeLoader(Control pControl, 
            Form pForm ,
            Color BrushColor, 
            Color backgroundColor,
            int animationSpeed)
        {
            
            parentForm = pForm;
            parentControl = pControl;
            loader = new LoadingControl(BrushColor, backgroundColor, 
                animationSpeed);
            
            //Add Loader Control to Parent control so it shows up when being painted
            //parentControl.Controls.Add(loader);
            pControl.Controls.Add(loader);


            //set offset inside form for loader
            Point posInControl = new Point(pControl.Size.Width / 2 - (loader.Size.Width / 2)
                , pControl.Size.Height / 2 - (loader.Size.Height / 2));

            //get absolute x and yPos in entire Form
            Point posInForm = GetPositionInForm(pControl);

           // posInForm.Offset(posInControl);
            
            loader.Location = new Point(posInControl.X,
                                        posInControl.Y);

            loader.Name = "loader";
            loader.BringToFront();
            loader.Hide();
            

            alpha = 1.0F;
            fadeColorMatrix = new ColorMatrix();

            fadeTimer = new Timer();
            //Add custom event Handler
            fadeTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);

        }

        private Point GetPositionInForm(Control ctrl)
        {
            Point p = ctrl.Location;
            Control parent = ctrl.Parent;
            while (!(parent is Form))
            {
                p.Offset(parent.Location.X, parent.Location.Y);
                parent = parent.Parent;
            }
            return p;
        }


        /// <summary>
        /// Custom event Handler that occurs each time fadeTimer ticks,
        /// The alpha for the fadeBMP is incremented and if it reaches
        /// a certain limit, the timer is stopped and parentControl is 
        /// shown
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            //increment fade value
            alpha = alpha + 0.15F;
            Console.WriteLine("ALpha value currently : " + alpha);
            if (alpha >= 0.7F)
            {
                fadeTimer.Stop();
                Console.WriteLine("Setting groupBox to Visible--->alpha value is : " + alpha);
               
                //Tell parent Control To show itself
                //Replacing the faded in BMP image.
                parentControl.Show();
                ///////////
                //////
                /////  Put in a signal/delegate here
                /////  to delete / dispose FadeLoader?
                //////
                ///////////
                return;
            }

            //Continue fading in BMP image
            drawControlScreen(parentControl);
        }

        private void drawControlScreen(Control aControl)
        {
            //Set up colorMatrix and set new alpha value
            
            fadeColorMatrix.Matrix33 = alpha;
            fadeImageAttributes = new ImageAttributes();
            fadeImageAttributes.SetColorMatrix(fadeColorMatrix);

            using (Graphics myGraphics = parentForm.CreateGraphics())
            {
                //Draw BMP @ Rectangle area (resized to fit), 
                // starting @ x,y in BMP and spanning for Width Height,
                // using Pixel graphics unit and image attribute ia
                Rectangle rectToDrawTo = new Rectangle(aControl.Location.X, aControl.Location.Y,
                                                   aControl.Width, aControl.Height);
                Rectangle rc2 = new Rectangle(0, 0, 200, 300);
                myGraphics.DrawImage(fadeBMP, rc2, 0, 0,
                   fadeBMP.Width, fadeBMP.Height, GraphicsUnit.Pixel, fadeImageAttributes);

            }

        }

        private void setFadeBitmap(Control aControl)
        {
           
            //use using to automatically dispose of myGraphics when scope ends
            using (Graphics myGraphics = aControl.CreateGraphics())
            {
                //If any memory was previously allocated for fadeBMP, release it
                if(fadeBMP != null)
                    fadeBMP.Dispose();

                //Create a new Bitmap the size of the groupBox and draw the contents to it.
                Size s = aControl.Size;
                fadeBMP = new Bitmap(s.Width, s.Height, myGraphics);
                Rectangle Rect = new Rectangle(0, 0, aControl.Width, aControl.Height);
                aControl.DrawToBitmap(fadeBMP, Rect);
            }
            
        }


        public void fadeInImage(Control sourceControlImage)
        {
            //Reset alpha value to start fading in from nothing
            alpha = 0.0F;  
            //grab the image that will fade into the screen
            
            setFadeBitmap(sourceControlImage);

            fadeTimer.Start();

        }


        public void showLoadingControl(){

          
            loader.Show();

        }

        public void shrinkLoader(int shrinkSpeedInterval)
        {
            loader.Show();
            loader.shrink(shrinkSpeedInterval);
            //used to call fadeInImage here 
            
        }

      

        public void Dispose()
        {
            //release custom made timer
            fadeTimer.Dispose();
            //release BMP memory
            fadeBMP.Dispose();
        }


    }
}
