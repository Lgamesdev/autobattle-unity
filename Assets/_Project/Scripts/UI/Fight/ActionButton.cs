using LGamesDev.Fighting;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.UI
{
    public class ActionButton : MonoBehaviour
    {
        public FightActionType actionType;

        public void OnClick()
        {
            ActionButtons.Instance.Attack(actionType);
        }
    }
}