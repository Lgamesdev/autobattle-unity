namespace LGamesDev.Fighting
{
    public class FightAction
    {
        public bool PlayerTeam;
        public int Damage;
        public bool CriticalHit;
        public bool Dodged;
        
        public override string ToString()
        {
            return  "fightAction :  [ \n" +
                    "playerTeam : " + PlayerTeam + "\n" +
                    "damage : " + Damage + "\n" +
                    "criticalHit : " + CriticalHit + "\n" +
                    "dodged : " + Dodged + "\n";
        }
    }
}