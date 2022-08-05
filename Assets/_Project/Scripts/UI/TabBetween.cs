using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class TabBetween : MonoBehaviour
    {
        public List<Selectable> elements; // add UI elements in inspector in desired tabbing order
        private int index;

        private void Start()
        {
            index = -1; // always leave at -1 initially
            //elements[0].Select(); // uncomment to have focus on first element in the list
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                for (var i = 0; i < elements.Count; i++)
                    if (elements[i].gameObject.Equals(EventSystem.current.currentSelectedGameObject))
                    {
                        index = i;
                        break;
                    }

                if (Input.GetKey(KeyCode.LeftShift))
                    index = index > 0 ? --index : index = elements.Count - 1;
                else
                    index = index < elements.Count - 1 ? ++index : 0;
                elements[index].Select();
            }
        }
    }
}