using System;

namespace Core.Player
{
    [Serializable]
    public class PlayerConfig
    {
        public string Username;
        public bool CreationDone;
        public bool TutorialDone;

        public override string ToString()
        {
            return "username : " + Username + "\n"
                   + "creationDone : " + CreationDone + "\n"
                   + "tutorialDone : " + TutorialDone + "\n";
        }
    }
}