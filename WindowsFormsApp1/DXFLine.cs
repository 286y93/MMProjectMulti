using System;

namespace WindowsFormsApp1
{
    /// <summary>
    /// DXF 線段資料結構
    /// </summary>
    public class DXFLine
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public double Length
        {
            get
            {
                double dx = X2 - X1;
                double dy = Y2 - Y1;
                return Math.Sqrt(dx * dx + dy * dy);
            }
        }

        public DXFLine(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override string ToString()
        {
            return $"({X1:F2}, {Y1:F2}) -> ({X2:F2}, {Y2:F2}), 長度: {Length:F2}";
        }
    }
}
