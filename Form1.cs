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
            Console.WriteLine("Event!");
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
                var arrayOfCountours = FindCountoursAtImage(differenceAtImages);

                var coordinatesForNewCursorPosition = GetBiggestCountourCoordinates(arrayOfCountours);
                var gameWindowCoordinates = NativeMethodsForDirect3D.GetAbsoluteClientRect(process[0].MainWindowHandle); // Find offset 


                var x = coordinatesForNewCursorPosition.X + gameWindowCoordinates.X;
                var y = coordinatesForNewCursorPosition.Y + gameWindowCoordinates.Y;
                //Cursor.Position = new System.Drawing.Point(x, y);
                Input.SmoothMouseMove(x, y, 2);

                Thread.Sleep(900);
                //CharachterControl.TryToAttackMob();

                if (GetCursor.IsCursorRed())
                {
                    CharachterControl.TryToAttackMob();
                }

                if (IsMatchWithTemplate(Direct3DCapture.CaptureWindow(process[0].MainWindowHandle), monsterHPBarTempalte))
                {
                    var counter = 0;
                    CharachterControl.AttackMobAndWait(1);
                    CharachterControl.PressKeyBoardButton(Convert.ToByte(Keys.F1));
                    try
                    {
                        while (IsMatchWithTemplate(Direct3DCapture.CaptureWindow(process[0].MainWindowHandle), monsterHPBarTempalte))
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

                    //var img11 = BringProcessToFrontAndCaptureWindow(process);
                    //Thread.Sleep(500);
                    //var img22 = BringProcessToFrontAndCaptureWindow(process);

                    //var differenceAtImages1 = GetDiffInTwoImagesWithCustomBorders(img11, img22,300,500);
                    //var arrayOfCountours1 = FindCountoursAtImage(differenceAtImages1);

                    //var newCursorPosition1 = GetBiggestCountourCoordinates(arrayOfCountours1);
                    //System.Drawing.Rectangle gameWindowCoordinates1 = NativeMethods.GetAbsoluteClientRect(process[0].MainWindowHandle);
                    ////Cursor.Position = new System.Drawing.Point(newCursorPosition.X + gameWindowCoordinates.X, newCursorPosition.Y + gameWindowCoordinates.Y);

                    //Input.SmoothMouseMove(newCursorPosition1.X + gameWindowCoordinates.X, newCursorPosition1.Y + gameWindowCoordinates.Y, 2);

                    //Thread.Sleep(900);
                    ////CharachterControl.TryToAttackMob();

                    ////GetCursor.IsCursorRed();

                    //if (GetCursor.IsCursorRed())
                    //{
                    //    CharachterControl.TryToAttackMob();
                    //}
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

        OpenCvSharp.Mat GetDiffInTwoImages(System.Drawing.Bitmap firstState, System.Drawing.Bitmap secondState)
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


            Mat result = new Mat();

            Cv2.BitwiseAnd(img2, img2, result, mask);
            Cv2.Threshold(result, result, 50, 255, ThresholdTypes.Binary);
            Cv2.CvtColor(result, result, ColorConversionCodes.BGR2GRAY);

            #region debug ImShow("res", res)
            //Cv2.ImShow("res", res);
            //Cv2.WaitKey(); 
            #endregion
            return result;
        }

        OpenCvSharp.Point[][] FindCountoursAtImage(Mat image)
        {
            Cv2.FindContours(image, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            return contours;
        }

        Mat GetDiffInTwoImagesWithCustomBorders(System.Drawing.Bitmap firstState, System.Drawing.Bitmap secondState, int xBorder, int yBorder)
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

            Parallel.For(xBorder, differenceBetweenImages.Rows - xBorder,
                   j =>
                   {
                       Parallel.For(yBorder, differenceBetweenImages.Cols - yBorder,
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
                }
                else
                {
                    return false;
                }
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
