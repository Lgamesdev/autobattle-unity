using System.Collections.Generic;

namespace LGamesDev.Fighting
{
    public class Fight
    {
        public Character Character;
        public IFighter Opponent;
        public List<FightAction> Actions = new();
        public Reward Reward;
        public FightType FightType;

        public override string ToString()
        {
            string result = "[ \n" +
                            "character : " + Character + "\n" +
                            "opponent : " + Opponent + "\n" +
                            "actions [ : \n";
            Actions.ForEach(action => result += action.ToString());
            result += " ] \n" +
                      "reward : " + Reward;
            return result;
        }
    }
}