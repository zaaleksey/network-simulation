namespace NetworkSimulator
{
    //Фрагмент в сети с делением и слиянием требований
    public class Fragment : Demand
    {
        //Сигнатура фрагмента
        public SignatureForFragment Sigma { get; set; }

        public Fragment(double TimeGeneration, long ID, SignatureForFragment Sigma)
        {
            this.TimeGeneration = TimeGeneration;
            this.ID = ID;
            this.Sigma = Sigma;
            NumberOfParts = 1;

        }

        //Число частей на которое был поделен фрагмент
        public int NumberOfParts { get; set; }
        //Время поступления
        public double TimeArrivale { get; set; }
        //Время начала обслуживания
        public double TimeStartService { get; set; }
        //Время завершения обслуживания
        public double TimeLeave { get; set; }
        //Общее время пребывания в сети
        public double TotalTime { get; set; }
    }
}
