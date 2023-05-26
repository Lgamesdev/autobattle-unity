using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;

namespace LGamesDev
{
    public interface IFighter
    {
        public abstract string Username { get; set; }
        
        public abstract int Level { get; set; }
        
        public abstract Body Body { get; set; }

        public abstract Stat[] Stats { get; set; }
    }
}