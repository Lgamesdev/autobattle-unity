using System.Collections.Generic;

namespace LGamesDev.Fighting
{
    public class Fight
    {
        public bool PlayerWin;
        public Character Character;
        public Character Opponent;
        public List<FightAction> Actions = new();
        public Reward Reward;

        public override string ToString()
        {
            string result = "[ \n" +
                            "character : " + Character.ToString() + "\n" +
                            "opponent : " + Opponent.ToString() + "\n" +
                            "actions [ : \n";
            Actions.ForEach(action => result += action.ToString());
            result += " ] \n" +
                      "reward : " + Reward.ToString();
            return result;
        }
    }
}