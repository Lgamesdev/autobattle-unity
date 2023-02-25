
using LGamesDev;
using LGamesDev.Fighting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AttackButton : MonoBehaviour
{
    private Button _button;
    
    private void Start()
    {
        FightManager.Instance.ActionsComplete += () => _button.interactable = true;
        
        _button = GetComponent<Button>();
        _button.interactable = false;
    }

    public void Attack()
    {
        GameManager.Instance.networkManager.Attack();
        _button.interactable = false;
    }
}
