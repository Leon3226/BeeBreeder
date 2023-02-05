namespace BeeBreeder.Property.Model
{
    public class Apiary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ApiaryComputer[]? Computers { get; set; }
        public int[] Mods { get; set; }
    }
}
