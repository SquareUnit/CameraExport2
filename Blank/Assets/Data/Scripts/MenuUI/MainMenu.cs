//Creer par Valentin. 
//Date de création [2019-04-08].
//Modification Valentin le [2019-04-09]
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    //[Valentin 2019-04-08] Start Game button
    public void StartGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    //[Valentin 2019-04-08] Select level button
    public void SelectLevel()
    {
        
    }
    
    //[Valentin 2019-04-08] Quit game button
    public void QuitGame()
    {
        Application.Quit();
    }
}
