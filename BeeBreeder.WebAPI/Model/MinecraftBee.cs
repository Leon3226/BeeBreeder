namespace BeeBreeder.WebAPI.Model
{
    public struct MinecraftBeeIndividual
    {
        public int Generation;
        public bool CanSpawn;
        public bool HasEffect;
        public string Type;
        public string DisplayName;
        public bool IsAnalyzed;
        public bool IsSecret;
        public MinecraftGene Active;
        public MinecraftGene Inactive;
        public string Ident;
        public bool IsNatural;
        public bool IsAlive;
        public int Health;
    }
}