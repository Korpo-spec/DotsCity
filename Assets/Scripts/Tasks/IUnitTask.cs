namespace DefaultNamespace
{
    public interface IUnitTask
    {
        
        public void Execute(UnitWorker worker);
        public bool IsComplete { get; protected set; }
    }
}