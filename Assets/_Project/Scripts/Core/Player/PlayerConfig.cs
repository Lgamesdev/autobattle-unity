namespace Core.Player
{
    public class PlayerConfig
    {
        public bool CreationDone;
        public bool TutorialDone;

        public override string ToString()
        {
            return "creationDone : " + CreationDone + "\n"
                   + "tutorialDone : " + TutorialDone + "\n";
        }
    }
}