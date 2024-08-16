namespace DataAccessLayer.Models
{
    public class EventLine
    {
        private int a = 6378137;
        private double e = 0.0818191910428;
        private double e2 = 0.00669438;

        public double B { get; set; }
        public double L { get; set; }
        public double H { get; set; }
        public TimeSpan time { get; set; }

        public double N { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public EventLine(string line)
        {
            string[] tokens = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            time = TimeSpan.Parse(tokens[1]);
            B = float.Parse(tokens[2]);
            L = float.Parse(tokens[3]);
            H = float.Parse(tokens[4]);
            double BR = (B * (Math.PI)) / 180;
            double LR = (L * (Math.PI)) / 180;
            double HR = (H * (Math.PI)) / 180;

            N = a * Math.Pow(1 - e2 * Math.Pow(Math.Sin(Math.Pow(BR, 2)), 2), -0.5);
            X = (N + H) * Math.Cos(BR) * Math.Cos(LR);
            Y = (N + H) * Math.Cos(BR) * Math.Sin(LR);
            Z = (N * (1 - e2) + H) * Math.Sin(BR);

        }

        public EventLine(double B, double L, double Alt, double h)
        {
            if (h != null) {
                double BR = (B * (Math.PI)) / 180;
                double LR = (L * (Math.PI)) / 180;
                double HR = (h * (Math.PI)) / 180;

                N = a * Math.Pow(1 - e2 * Math.Pow(Math.Sin(Math.Pow(BR, 2)), 2), -0.5);
                X = (N + h) * Math.Cos(BR) * Math.Cos(LR);
                Y = (N + h) * Math.Cos(BR) * Math.Sin(LR);
                Z = (N * (1 - e2) + h) * Math.Sin(BR);

                this.H = Alt;
            }
        }
    }
}
