using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectiblesMenu : MonoBehaviour {

    public RectTransform mainPanel;
    public RectTransform collectiblePanel;
    public RectTransform penseePanel;

    public Toggle defaultButtonSelectionCollectible;
    public GameObject defaultCollectibleSelection;
    public VerticalLayoutGroup contentCollectible;
    public bool firstFound = true;


    public void Start()
    {
        if (defaultButtonSelectionCollectible != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButtonSelectionCollectible.gameObject);
        }

        //setup de départ
        firstFound = true;
        mainPanel.gameObject.SetActive(true);
        collectiblePanel.gameObject.SetActive(false);
        penseePanel.gameObject.SetActive(false);
    }

    public void Main()
    {
        mainPanel.gameObject.SetActive(true);
        collectiblePanel.gameObject.SetActive(false);
        penseePanel.gameObject.SetActive(false);
    }


    public void Collectible()
    {
        if (defaultCollectibleSelection == null)
        {
            for (int i = 0; i < contentCollectible.gameObject.transform.childCount; i++)
            {
                if (firstFound == true)
                {
                    if (contentCollectible.gameObject.transform.GetChild(i).name != "???")
                    {
                        defaultCollectibleSelection = contentCollectible.gameObject.transform.GetChild(i).gameObject;
                        EventSystem.current.SetSelectedGameObject(defaultCollectibleSelection);
                        Invoke("chooseEventCollectible", 0.1f);
                        firstFound = false;
                    }
                }
            }
        }


        mainPanel.gameObject.SetActive(false);
        collectiblePanel.gameObject.SetActive(true);
        penseePanel.gameObject.SetActive(false);
    }

    public void chooseEventCollectible()
    {
        Toggle toggleSelection = defaultCollectibleSelection.GetComponent<Toggle>();
        toggleSelection.OnSelect(new BaseEventData(EventSystem.current));
    }
}
