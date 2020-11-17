using OpenCvSharp;

namespace bot
{
    class FindAndBuildRectangleForDifference
    {
        static void f(Mat result)
        {
            var res = result;
            Cv2.FindContours(res, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchyIndexes, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

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

            var biggestContourRect = Cv2.BoundingRect(contours[biggestContourNo]);

            Cv2.CvtColor(res, res, ColorConversionCodes.GRAY2BGR);

            Cv2.Rectangle(res,
                new OpenCvSharp.Point(biggestContourRect.X - 10, biggestContourRect.Y - 10),
                new OpenCvSharp.Point(biggestContourRect.X + biggestContourRect.Width + 10, biggestContourRect.Y + biggestContourRect.Height + 10),
                new Scalar(0, 255, 0), 2);
        }
    }
}
