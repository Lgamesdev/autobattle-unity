
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
        _button = GetComponent<Button>();
    }

    public void Attack()
    {
        _button.interactable = false;
        GameManager.Instance.networkManager.Attack(() =>
        {
            _button.interactable = true;
        });
        
    }
}
