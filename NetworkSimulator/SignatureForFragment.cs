namespace NetworkSimulator
{
    public class SignatureForFragment
    {
        //Фрагмент-родитель
        public Fragment ParentFragment { get; set; }

        //Номер фрагмента среди фрагментов, которые были получены при делении
        public int SubID { get; set; }

        //Идентификатор дивайдера на котором произошло деление фрагмента,
        //0 если деления не было (т.е. фрагмент является требованием)
        public int ForkNodeID { get; set; }

        public SignatureForFragment(Fragment ParentFragment, int SubID, int ForkNodeID)
        {
            this.ParentFragment = ParentFragment;
            this.SubID = SubID;
            this.ForkNodeID = ForkNodeID;
        }
    }
}
