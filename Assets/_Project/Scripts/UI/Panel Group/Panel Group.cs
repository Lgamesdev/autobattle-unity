using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LGamesDev.UI
{
    public class PanelGroup : MonoBehaviour
    {
        public GameObject[] panels;

        public TabGroup tabGroup;

        public int panelIndex;

        private void Awake()
        {
            ShowCurrentPanel();
        }

        void ShowCurrentPanel()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (i == panelIndex)
                {
                    panels[i].gameObject.SetActive(true);
                }
                else
                {
                    panels[i].gameObject.SetActive(false);
                }
            }
        }

        public void SetPageIndex(int index)
        {
            panelIndex = index;
            ShowCurrentPanel();
        }
    }
}
