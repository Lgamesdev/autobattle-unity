namespace LGamesDev.Fighting
{
    public class FightAction
    {
        public bool PlayerTeam;
        public FightActionType ActionType;
        public int Damage;
        public bool CriticalHit;
        public bool Dodged;
        public int EnergyGained;
        
        public override string ToString()
        {
            return  "fightAction :  [ \n" +
                    "playerTeam : " + PlayerTeam + "\n" +
                    "actionType : " + ActionType + "\n" +
                    "damage : " + Damage + "\n" +
                    "criticalHit : " + CriticalHit + "\n" +
                    "dodged : " + Dodged + "\n" +
                    "energyGained : " + EnergyGained + "\n";
        }
    }
}