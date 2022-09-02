namespace LGamesDev.Fighting
{
    public class FightAction
    {
        public bool playerTeam;
        public int damage;
        public bool critialHit;
        public bool dodged;
        
        public override string ToString()
        {
            return  "fightAction :  [ \n" +
                    "playerTeam : " + playerTeam + "\n" +
                    "damage : " + damage + "\n" +
                    "criticalHit : " + critialHit + "\n" +
                    "dodged : " + dodged + "\n";
        }
    }
}