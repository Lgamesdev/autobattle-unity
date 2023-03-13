using System.ComponentModel;
using Newtonsoft.Json;

namespace LGamesDev.Core.Player
{
    
    public class PlayerOptions
    {
        public float MusicVolume = 0.5f;
        
        public float EffectsVolume = 0.5f;

        public int FightSpeed = 1;
        
        public override string ToString()
        {
            return "musicVolume : " + MusicVolume + "\n"
                   + "effectsVolume : " + EffectsVolume + "\n"
                   + "fightSpeed : " + FightSpeed + "\n";
        }
    }
}