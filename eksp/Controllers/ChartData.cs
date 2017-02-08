namespace eksp.Controllers
{
    internal class ChartData
    {
        public ChartData(string label, double value1)
        {
            this.Label = label;
            this.Value1 = value1;
        }

        public ChartData(string label, double value1, double value2)
        {
            this.Label = label;
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }

    }
}