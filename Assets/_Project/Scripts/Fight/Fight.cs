using System.Collections.Generic;

namespace LGamesDev.Fighting
{
    public class Fight
    {
        public Character character;
        public Character opponent;
        public List<FightAction> actions;

        public override string ToString()
        {
            string result = "[ \n" +
                            "character : " + character.ToString() + "\n" +
                            "opponent : " + opponent.ToString() + "\n" +
                            "actions [ : \n";
            actions.ForEach(action => result += action.ToString());
            result += " ] \n";
            return result;
        }
    }
}