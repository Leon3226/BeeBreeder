namespace BeeBreeder.Property.Model
{
    public class ComputerBindRequest
    {
        public string ComputerIdentifier { get; set; }
        public string ConfirmCode { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool Failed { get; set; }
        public bool Resolved { get; set; }
        public TimeSpan TimeValid { get; set; }
        public bool IsExpired => Created + TimeValid > DateTime.UtcNow;
    }
}
