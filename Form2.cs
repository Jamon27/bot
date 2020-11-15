using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;
namespace bot
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(2000);

            //CharachterControl.TryToAttackMob();
            int x = 0;
            int y = 1000;
            int currentX = Convert.ToInt32(Input.GetCursorPosition().X);
            int currentY = Convert.ToInt32(Input.GetCursorPosition().X);

            while ((currentX != x) | (currentY != y))  {
                int xOffset = 0;
                int yOffset = 0;

                if (currentX != x)
                {
                    
                    if( x - currentX > 0)
                    {
                        xOffset = 1;
                    }
                    else
                    {
                        xOffset = -1;
                    }
                }

                if (currentY != y)
                {
                    
                    if (y - currentY > 0)
                    {
                        yOffset = 1;
                    }
                    else
                    {
                        yOffset = -1;
                    }
                }

                int aX = currentX + xOffset;
                int aY = currentY + yOffset;
                Input.SetCursorPos(aX, aY);

                Thread.Sleep(3);

                currentX = Convert.ToInt32(Input.GetCursorPosition().X);
                currentY = Convert.ToInt32(Input.GetCursorPosition().Y);
            }

            //Input.mouse_event(Input.MOUSEEVENTF_ABSOLUTE | Input.MOUSEEVENTF_MOVE, 2000, 20000, 0, 0);
        }

        Boolean IsMatchWithTemplate(System.Drawing.Bitmap monsterRef, System.Drawing.Bitmap monsterTemplate)
        {
            Mat reference = monsterRef.ToMat();
            Mat template = monsterTemplate.ToMat();
            Mat result = new Mat(reference.Rows - template.Rows + 1, reference.Cols - template.Cols + 1, MatType.CV_32FC1);
            {
                //Convert input images to gray
                Mat gref = reference.CvtColor(ColorConversionCodes.BGR2GRAY);
                Mat gtpl = template.CvtColor(ColorConversionCodes.BGR2GRAY);

                double threshold = 0.7;
                Cv2.MatchTemplate(gref, gtpl, result, TemplateMatchModes.CCoeffNormed);
                Cv2.Threshold(result, result, threshold, 1.0, ThresholdTypes.Tozero);
                Cv2.MinMaxLoc(result, out _, out double maxval, out _, out _);

                if (maxval >= threshold)
                {
                    return true;

                    #region Отрисовка найденого сходства (дебажная функция)
                    /*
                    //Setup the rectangle to draw
                    Rect r = new Rect(new Point(maxloc.X, maxloc.Y), new Size(template.Width, template.Height));
                    Console.WriteLine($"MinVal={minval.ToString()} MaxVal={maxval.ToString()} MinLoc={minloc.ToString()} MaxLoc={maxloc.ToString()} Rect={r.ToString()}");
                    //Draw a rectangle of the matching area
                    Cv2.Rectangle(reference, r, Scalar.LimeGreen, 2);

                    //Fill in the result Mat so you don't find the same area again in the MinMaxLoc
                    //Rect outRect;
                    //Cv2.FloodFill(result, maxloc, new Scalar(0), out outRect, new Scalar(0.1), new Scalar(1.0), FloodFillFlags.Link4);

                    Cv2.ImShow("Matches", reference);
                    Cv2.WaitKey();
                    */
                    #endregion 

                }
                else
                {
                    return false;
                }
            }
        }
        public void BringProcessWindowToFront(Process proc)
        {
            if (proc == null)
                return;
            IntPtr handle = proc.MainWindowHandle;
            int i = 0;

            while (!NativeMethods.IsWindowInForeground(handle))
            {
                if (i == 0)
                {
                    // Initial sleep if target window is not in foreground - just to let things settle
                    Thread.Sleep(1);
                }

                if (NativeMethods.IsIconic(handle))
                {
                    // Minimized so send restore
                    NativeMethods.ShowWindow(handle, NativeMethods.WindowShowStyle.Restore);
                }
                else
                {
                    // Already Maximized or Restored so just bring to front
                    NativeMethods.SetForegroundWindow(handle);
                }
                Thread.Sleep(1);

                // Check if the target process main window is now in the foreground
                if (NativeMethods.IsWindowInForeground(handle))
                {
                    // Leave enough time for screen to redraw
                    Thread.Sleep(5);
                    return;
                }

                // Prevent an infinite loop
                if (i > 620)
                {
                    throw new Exception("Could not set process window to the foreground");
                }
                i++;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            GlobalKeyboardHook gkh = new GlobalKeyboardHook();
            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.B);
            gkh.KeyUp += new KeyEventHandler(Gkh_KeyUp);
        }

        void Gkh_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("AAAA");
            e.Handled = true;
        }

    }
}
