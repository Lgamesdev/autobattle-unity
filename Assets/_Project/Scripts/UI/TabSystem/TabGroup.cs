using System.Collections.Generic;
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
        selectedTab.background.color = new Color(
            selectedTab.background.color.r - 0.20f, 
            selectedTab.background.color.g - 0.20f,
            selectedTab.background.color.b - 0.20f
        );
        
        OnTabSelected(selectedTab);
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        if (objectActive != null)
        {
            objectActive.SetActive(false);
        }
        
        selectedTab = button;
        selectedTab.Select();
        
        ResetTabs();
        button.background.color = new Color(
            button.background.color.r - 0.20f, 
            button.background.color.g - 0.20f,
            button.background.color.b - 0.20f
        );
        
        button.objectToSwap.SetActive(true);
        objectActive = button.objectToSwap;

        /*int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
                objectActive = objectsToSwap[i];
            } else {
                objectsToSwap[i].SetActive(false);
            }
        }*/

        /*if (panelGroup != null)
        {
            panelGroup.SetPageIndex(index);
        }*/
    }
    
    /*public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.background.color = new Color(
                button.background.color.r - 0.10f, 
                button.background.color.g - 0.10f,
                button.background.color.b - 0.10f
            );
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }*/

    private void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) continue;
            button.background.color = new Color(
                button.background.color.r + 0.20f, 
                button.background.color.g + 0.20f,
                button.background.color.b + 0.20f
            );
        }
    }
}
