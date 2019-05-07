namespace NetworkSimulator
{
    //Класс для требования
    public abstract class Demand
    {
        public long ID { get; set; }
        //Время создания требования
        public double TimeGeneration { get; set; }
    }
}
