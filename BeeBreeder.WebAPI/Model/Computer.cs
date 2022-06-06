﻿namespace BeeBreeder.WebAPI.Model
{
    public class Computer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Identifier { get; set; }
        public string OpenComputersId { get; set; }
        public bool Active { get; set; }
    }
}
