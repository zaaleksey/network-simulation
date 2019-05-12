using System;
using RandomVariables;

namespace NetworkSimulator
{
    public class InfoNode : Node
    {
        //Отображает текущее время
        private double CurrentTime;

        //Задает текущее время
        public void SetCurrentTime(double time)
        {
            CurrentTime = time;
        }

        //Возвращает текущее время
        public double GetCurrentTime()
        {
            return CurrentTime;
        }

        public override void Receive(Fragment fragment)
        {
            throw new NotImplementedException();
        }

        public override void Send(Fragment fragment, Node node)
        {
            throw new NotImplementedException();
        }

        public override void Route(Fragment fragment)
        {
            throw new NotImplementedException();
        }

        public override void Activate()
        {
            throw new NotImplementedException();
        }
    }
}
