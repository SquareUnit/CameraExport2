using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CollectibleInfo : MonoBehaviour
{
    public Toggle myToggle;
    public TextMeshProUGUI myText;

    public string collectibleName;
    [TextArea(1,3)]
    public string collectibleDescription;
    public GameObject collectibleToSpawn;
    public bool collectibleEnabled = false;

    public Transform objectTransform;




    private void Awake()
    {
        myToggle = GetComponent<Toggle>();
        myText = GetComponent<TextMeshProUGUI>();

        myToggle.interactable = collectibleEnabled;
    }

    public void Update()
    {
            if (objectTransform.childCount > 0)
        {

            objectTransform.GetChild(0).transform.Rotate(0.02f, 0.01f, 0);
        }
    }


    public void UpdateToggle()
    {
        
        myToggle.interactable = collectibleEnabled;

        myToggle.interactable = collectibleEnabled;
        myText.text = collectibleName;
    }

    public void ShowCollectible()
    {

        if (objectTransform.childCount == 0)
        {
            GameObject thisObject = Instantiate(collectibleToSpawn, objectTransform);
            thisObject.transform.position = objectTransform.transform.position;
            thisObject.transform.localScale = new Vector3(300, 300, 300);

        } else
        {
            foreach (Transform child in objectTransform)
            {
                Destroy(child.gameObject);
            }
            GameObject thisObject = Instantiate(collectibleToSpawn, objectTransform);
            thisObject.transform.position = objectTransform.transform.position;
            thisObject.transform.localScale = new Vector3(300, 300, 300);


        }
    }
}
