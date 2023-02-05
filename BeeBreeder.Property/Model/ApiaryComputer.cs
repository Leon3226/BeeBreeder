namespace BeeBreeder.Property.Model
{
    public class ApiaryComputer
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ApiaryId { get; set; }

        [NonSerialized]
        public string UserId;
    }
}
