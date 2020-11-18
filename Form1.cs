using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// Основная рабочая форма. При работе основного кода зависает окно с кнопками и закрыть можно только клавишей стоп.
namespace bot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GlobalKeyboardHook globalKeyboardHook = new GlobalKeyboardHook();
            globalKeyboardHook.HookedKeys.Add(Keys.A);
            globalKeyboardHook.HookedKeys.Add(Keys.F11);
            globalKeyboardHook.KeyUp += new KeyEventHandler(Gkh_KeyUp);
        }

        void Gkh_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Event!");
            Thread.Sleep(5000);
            e.Handled = true;
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            testFunction();
            #region oldThings
            //img1.Save(@"C:\1\img1.png");
            //img2.Save(@"C:\1\img2.png");
            // Console.WriteLine(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            //Thread.Sleep(4000);     
            //string path1 = @"C:\1\b1.png";
            //string path2 = @"C:\1\b2.png";
            //System.Drawing.Bitmap img1 =  PrintWindow(process[0].MainWindowHandle);
            //Thread.Sleep(4000);
            //System.Drawing.Bitmap img2 = PrintWindow(process[0].MainWindowHandle);
            //GetDiff(path1, path2);
            #endregion
        }

        async void testFunction2()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("in");
                 while (true)
                {
                    
                }
                Console.WriteLine("out");
            });
        }
        async void testFunction()
        {
            await Task.Run(() =>
            {
                Process[] process = Process.GetProcessesByName("rf_online.bin");
                
                System.Drawing.Bitmap monsterHPBarTempalte = new System.Drawing.Bitmap(@"C:\1\Template1.png");
                for (int i = 0; i < 100000; i++)
                {
                    Console.WriteLine(i); //WorkWithImages.BringProcessToFrontAndCaptureWindow(process);
                    
                    var img1 = WorkWithImages.BringProcessToFrontAndCaptureD3DWindow(process);
                    Thread.Sleep(500);
                    var img2 = WorkWithImages.BringProcessToFrontAndCaptureD3DWindow(process);
                    var differenceAtImages = WorkWithImages.GetDiffInTwoImages(img1, img2);
                    var arrayOfCountours = WorkWithImages.FindCountoursAtImage(differenceAtImages);

                    var coordinatesForNewCursorPosition = WorkWithImages.GetBiggestCountourCoordinates(arrayOfCountours);
                    var gameWindowCoordinates = NativeMethodsForWindow.GetAbsoluteClientRect(process[0].MainWindowHandle); // Find offset 
                    
                    var x = coordinatesForNewCursorPosition.X + gameWindowCoordinates.X;
                    var y = coordinatesForNewCursorPosition.Y + gameWindowCoordinates.Y;
                    //Cursor.Position = new System.Drawing.Point(x, y);
                    Input.SmoothMouseMove(x, y, 0);
                    
                    Thread.Sleep(900);
                    //CharachterControl.TryToAttackMob();
                    
                    if (GetCursor.IsCursorRed())
                    {
                        CharachterControl.TryToAttackMob();
                    }

                    if (WorkWithImages.IsImageMatchWithTemplate(Direct3DCapture.CaptureWindow(process[0].MainWindowHandle), monsterHPBarTempalte))
                    {
                        var counter = 0;
                        CharachterControl.AttackMobAndWait(1);
                        CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.F1));
                        try
                        {
                            while (WorkWithImages.IsImageMatchWithTemplate(Direct3DCapture.CaptureWindow(process[0].MainWindowHandle), monsterHPBarTempalte))
                            {
                                CharachterControl.AttackMobAndWait(1);
                                Thread.Sleep(1010);
                                counter++;
                                if ((counter % 6) == 0)
                                {
                                    CharachterControl.AttackMobAndWait(100);
                                    CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.F2));
                                    CharachterControl.AttackMobAndWait(100);
                                }

                                if ((counter % 13) == 0)
                                {
                                    CharachterControl.AttackMobAndWait(100);
                                    CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.F1));
                                    CharachterControl.AttackMobAndWait(100);
                                }

                            }
                        }
                        finally
                        {
                            CharachterControl.GetLoot();
                            CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.Escape));
                        }
                    }
                }
            }
            );
        }



        private void Button2_Click(object sender, EventArgs e)
        {
            var newForm = new Form2();
            newForm.Show();
        }

    }
}
