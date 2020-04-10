//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class SelectLevelMenu : MonoBehaviour
//{
//    List<MenuBoutonLevel> buttons = new List<MenuBoutonLevel>();
//    public LevelManager levelManager;
//    public GameObject buttonsPanel;

//    private void Start()
//    {
//        SetAllButtons();
//    }

//    void SetAllButtons()
//    {


//        for (int i = 0; i < buttonsPanel.transform.childCount; i++)
//        {
//            MenuBoutonLevel b = buttonsPanel.transform.GetChild(i).GetComponent<MenuBoutonLevel>();
//            if (b)
//            {
//                buttons.Add(b);
//            }
//        }

//        for (int i = 0; i < LevelManager.instance.GetLevelCount(); i++)
//        {
//            buttons[i].levelToLoad = i;
//        }
//    }

//    public void LoadMyLevel(int myLevel)
//    {
//        myLevel = buttons[myLevel].levelToLoad;
//        levelManager.LoadLevel(myLevel);
//    }

//}
