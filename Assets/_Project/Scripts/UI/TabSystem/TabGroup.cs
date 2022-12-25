using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    
    //public Sprite tabHover;

    //public PanelGroup panelGroup;

    //public List<GameObject> objectsToSwap;
    public TabButton selectedTab;
    public GameObject objectActive;

    private void Start()
    {
        OnTabSelected(tabButtons.First());
    }

    public void OnTabSelected(TabButton button)
    {
        if (button == selectedTab) return;
        
        if (selectedTab != null)
        {
            selectedTab.background.color = new Color(
                selectedTab.background.color.r + 0.20f, 
                selectedTab.background.color.g + 0.20f,
                selectedTab.background.color.b + 0.20f
            );
            selectedTab.Deselect();
        }

        if (objectActive != null)
        {
            objectActive.SetActive(false);
        }

        selectedTab = button;
        button.background.color = new Color(
            button.background.color.r - 0.20f,
            button.background.color.g - 0.20f,
            button.background.color.b - 0.20f
        );
        
        selectedTab.Select();
        
        button.objectToSwap.SetActive(true);
        objectActive = button.objectToSwap;
    }
}
