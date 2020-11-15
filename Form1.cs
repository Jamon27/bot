using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
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
            Thread.Sleep(5000);
            e.Handled = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Process[] process = Process.GetProcessesByName("rf_online.bin");
            System.Drawing.Bitmap monsterHPBarTempalte = new System.Drawing.Bitmap(@"C:\1\Template1.png");
            for (int i = 0; i < 100000; i++)
            {
                var img1 = BringProcessToFrontAndCaptureWindow(process);
                Thread.Sleep(500);
                var img2 = BringProcessToFrontAndCaptureWindow(process);

                var differenceAtImages = GetDiffInTwoImages(img1, img2);
                var arrayOfCountours = FindCountoursAtImg(differenceAtImages);

                var newCursorPosition = GetBiggestCountourCoordinates(arrayOfCountours);
                System.Drawing.Rectangle gameWindowCoordinates = NativeMethods.GetAbsoluteClientRect(process[0].MainWindowHandle);
                //Cursor.Position = new System.Drawing.Point(newCursorPosition.X + gameWindowCoordinates.X, newCursorPosition.Y + gameWindowCoordinates.Y);

                Input.SmoothMouseMove(newCursorPosition.X + gameWindowCoordinates.X, newCursorPosition.Y + gameWindowCoordinates.Y);

                Thread.Sleep(900);
                //CharachterControl.TryToAttackMob();

                //GetCursor.IsCursorRed();

                if (GetCursor.IsCursorRed())
                {
                    CharachterControl.TryToAttackMob();
                }

                if (IsMatchWithTemplate(Direct3DCapture.CaptureWindow(process[0].MainWindowHandle), monsterHPBarTempalte))
                {
                    var k = 0;
                    CharachterControl.AttackMob(1);
                    CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.F1));
                    try
                    {
                        while (IsMatchWithTemplate(Direct3DCapture.CaptureWindow(process[0].MainWindowHandle), monsterHPBarTempalte))
                        {
                            CharachterControl.AttackMob(1);
                            Thread.Sleep(1010);
                            k++;
                            if ((k % 6) == 0)
                            {
                                CharachterControl.AttackMob(100);
                                CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.F2));
                                CharachterControl.AttackMob(100);
                            }

                            if ((k % 13) == 0)
                            {
                                CharachterControl.AttackMob(100);
                                CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.F1));
                                CharachterControl.AttackMob(100);
                            }

                        }
                    }
                    finally
                    {
                        CharachterControl.GetLoot();
                        CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.Escape));
                    }

                    var img11 = BringProcessToFrontAndCaptureWindow(process);
                    Thread.Sleep(500);
                    var img22 = BringProcessToFrontAndCaptureWindow(process);

                    var differenceAtImages1 = GetDiffInTwoImagesWithCustomBorders(img11, img22,300,500);
                    var arrayOfCountours1 = FindCountoursAtImg(differenceAtImages1);

                    var newCursorPosition1 = GetBiggestCountourCoordinates(arrayOfCountours1);
                    System.Drawing.Rectangle gameWindowCoordinates1 = NativeMethods.GetAbsoluteClientRect(process[0].MainWindowHandle);
                    //Cursor.Position = new System.Drawing.Point(newCursorPosition.X + gameWindowCoordinates.X, newCursorPosition.Y + gameWindowCoordinates.Y);

                    Input.SmoothMouseMove(newCursorPosition1.X + gameWindowCoordinates.X, newCursorPosition1.Y + gameWindowCoordinates.Y);

                    Thread.Sleep(900);
                    //CharachterControl.TryToAttackMob();

                    //GetCursor.IsCursorRed();

                    if (GetCursor.IsCursorRed())
                    {
                        CharachterControl.TryToAttackMob();
                    }
                }
            }
            Thread.Sleep(1000);
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


        System.Drawing.Bitmap BringProcessToFrontAndCaptureWindow(Process[] process)
        {
            WorkWithProcess.BringProcessWindowToFront(process[0]);
            var capturedImage = Direct3DCapture.CaptureWindow(process[0].MainWindowHandle);
            return capturedImage;
        }


        static void GetDiffAndPrintImg(string mainImage, string templateImage)
        {
            Mat img1 = new Mat(mainImage);
            Mat img2 = new Mat(templateImage);
            Mat differenceBetweenImages = new Mat();
            Cv2.Absdiff(img1, img2, differenceBetweenImages);

            // Get the mask if difference greater than threshold
            Mat mask = new Mat(img1.Size(), MatType.CV_8UC1);
            int threshold = 80;  // 0
            Vec3b vectorOfColorsDifference;
            int curDifferenceLvl;

            for (int j = 0; j < differenceBetweenImages.Rows; ++j)
            {
                for (int i = 0; i < differenceBetweenImages.Cols; ++i)
                {
                    vectorOfColorsDifference = differenceBetweenImages.At<Vec3b>(j, i);
                    curDifferenceLvl = (vectorOfColorsDifference[0] + vectorOfColorsDifference[1] + vectorOfColorsDifference[2]);
                    if (curDifferenceLvl > threshold)
                    {
                        mask.Set<int>(j, i, 255);
                    }
                }
            }


            Mat res = new Mat();

            Cv2.BitwiseAnd(img2, img2, res, mask);
            Cv2.Threshold(res, res, 50, 255, ThresholdTypes.Binary);
            Cv2.CvtColor(res, res, ColorConversionCodes.BGR2GRAY);
            Cv2.FindContours(res, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            int biggestContourNo = 0;
            int ContourLength = 0;
            for (int i = 0; i < contours[i].Length; i++)
            {
                if (ContourLength < contours[i].Length)
                {
                    biggestContourNo = i;
                    ContourLength = contours[i].Length;
                }
            }


            var biggestContourRect = Cv2.BoundingRect(contours[biggestContourNo]);

            Cv2.CvtColor(res, res, ColorConversionCodes.GRAY2BGR);

            Cv2.Rectangle(res,
                new OpenCvSharp.Point(biggestContourRect.X - 10, biggestContourRect.Y - 10),
               new OpenCvSharp.Point(biggestContourRect.X + biggestContourRect.Width + 10, biggestContourRect.Y + biggestContourRect.Height + 10),
              new Scalar(0, 255, 0), 2);

            //Console.WriteLine(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            //Cv2.ImShow("res", res);
            //Cv2.WaitKey();
        }

        Mat GetDiffInTwoImages(System.Drawing.Bitmap firstState, System.Drawing.Bitmap secondState)
        {
            Mat img1 = firstState.ToMat();
            Mat img2 = secondState.ToMat();
            Mat differenceBetweenImages = new Mat();
            Cv2.Absdiff(img1, img2, differenceBetweenImages);

            // Get the mask if difference greater than threshold
            Mat mask = new Mat(img1.Size(), MatType.CV_8UC1);
            int threshold = 70;
            Vec3b vectorOfColorsDifference;
            int curDifferenceLvl;


            var ab = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Parallel.For(60, differenceBetweenImages.Rows - 200,
                   j =>
                   {
                       Parallel.For(30, differenceBetweenImages.Cols - 30,
                            i =>
                            {
                                vectorOfColorsDifference = differenceBetweenImages.At<Vec3b>(j, i);
                                curDifferenceLvl = (vectorOfColorsDifference[0] + vectorOfColorsDifference[1] + vectorOfColorsDifference[2]);
                                if (curDifferenceLvl > threshold)
                                {
                                    mask.Set<int>(j, i, 255);
                                }
                            });
                   });

            ab = DateTimeOffset.Now.ToUnixTimeMilliseconds() - ab;
            //label1.Text = ab.ToString();

            Mat res = new Mat();

            Cv2.BitwiseAnd(img2, img2, res, mask);
            Cv2.Threshold(res, res, 50, 255, ThresholdTypes.Binary);
            Cv2.CvtColor(res, res, ColorConversionCodes.BGR2GRAY);

            #region debug ImShow("res", res)
            //Cv2.ImShow("res", res);
            //Cv2.WaitKey(); 
            #endregion
            return res;


        }


        Mat GetDiffInTwoImagesWithCustomBorders(System.Drawing.Bitmap firstState, System.Drawing.Bitmap secondState, int xBorders, int yBorders)
        {
            Mat img1 = firstState.ToMat();
            Mat img2 = secondState.ToMat();
            Mat differenceBetweenImages = new Mat();
            Cv2.Absdiff(img1, img2, differenceBetweenImages);

            // Get the mask if difference greater than threshold
            Mat mask = new Mat(img1.Size(), MatType.CV_8UC1);
            int threshold = 70;
            Vec3b vectorOfColorsDifference;
            int curDifferenceLvl;


            var ab = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Parallel.For(xBorders, differenceBetweenImages.Rows - xBorders,
                   j =>
                   {
                       Parallel.For(yBorders, differenceBetweenImages.Cols - yBorders,
                            i =>
                            {
                                vectorOfColorsDifference = differenceBetweenImages.At<Vec3b>(j, i);
                                curDifferenceLvl = (vectorOfColorsDifference[0] + vectorOfColorsDifference[1] + vectorOfColorsDifference[2]);
                                if (curDifferenceLvl > threshold)
                                {
                                    mask.Set<int>(j, i, 255);
                                }
                            });
                   });

            ab = DateTimeOffset.Now.ToUnixTimeMilliseconds() - ab;
            //label1.Text = ab.ToString();

            Mat res = new Mat();

            Cv2.BitwiseAnd(img2, img2, res, mask);
            Cv2.Threshold(res, res, 50, 255, ThresholdTypes.Binary);
            Cv2.CvtColor(res, res, ColorConversionCodes.BGR2GRAY);

            #region debug ImShow("res", res)
            //Cv2.ImShow("res", res);
            //Cv2.WaitKey(); 
            #endregion
            return res;


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

        static void RandomDelay(int delayInMilliseconds)
        {
            if (delayInMilliseconds < 5)
            {
                delayInMilliseconds = 5;
            }
            var rand = new Random();
            int percentsFromDelay = delayInMilliseconds / 100 * 20; // +-20%
            int randomDelay = rand.Next(-delayInMilliseconds / percentsFromDelay, delayInMilliseconds / percentsFromDelay);

            Thread.Sleep(delayInMilliseconds + randomDelay);
        } // min delay = 5 сломано

        static void PressRandomKey()
        {
            string[] keys = { "-", "=", "d", "q", "e", "x" };
            var rand = new Random();
            int mistakePercent = 7;
            if (rand.Next(1, 100) <= mistakePercent)
            {
                RandomDelay(300);
                SendKeys.Send(keys[rand.Next(0, keys.Length - 1)]);
            }
        }

        Point GetBiggestCountourCoordinates(OpenCvSharp.Point[][] pointsOfCountours)
        {
            Point mobCoordinate = new Point(622, 401);


            if (pointsOfCountours.Length > 0)
            {
                var biggestContour = pointsOfCountours[GetNoOfBiggestContour(pointsOfCountours)];
                var sortPointsOfContour = biggestContour.OrderByDescending(p => p.X).ToList();
                var arrayOfPoints = sortPointsOfContour.ToArray();
                mobCoordinate = arrayOfPoints[arrayOfPoints.Length / 2];
            }
            return mobCoordinate;

        }



        OpenCvSharp.Point[][] FindCountoursAtImg(Mat image)
        {
            Cv2.FindContours(image, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            return contours;
        }




        int GetNoOfBiggestContour(OpenCvSharp.Point[][] contours)
        {
            int biggestContourNo = 0;
            int ContourLength = 0;
            int count = 0;
            foreach (var contour in contours)
            {

                if (ContourLength < contours[count].Length)
                {
                    biggestContourNo = count;
                    ContourLength = contours[count].Length;
                }
                count++;
            }
            return biggestContourNo;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var newForm = new Form2();
            newForm.Show();
        }


    }
}
