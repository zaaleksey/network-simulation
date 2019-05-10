namespace NetworkSimulator
{
    public abstract class Buffer
    {
        //Помещаем фрагмент в буффер
        public abstract void Put(Fragment fragment);

        //Берет фрагмент из буффера
        public abstract Fragment Take();

        //Число элементов в буффере
        public abstract int Count();

        //Проверка буффера на пустоту
        public abstract bool IsEmpty();
    
    }
}
