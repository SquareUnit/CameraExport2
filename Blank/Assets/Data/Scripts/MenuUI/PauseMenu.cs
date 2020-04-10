using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class PauseMenu : MonoBehaviour
{
    //[valentin 2019-04-05] Jeu en pause ?
    public static bool gameIsPaused = false;
    //[valentin 2019-04-05] Canvas du menu pause.
    public GameObject pauseMenuUI;
    public MenuBouton pauseMenu;

    //[Valentin 2019-04-08] The Rewired Player et Player ID
    private Player player;
    public int playerID = 0;



    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
    }
    private void Start()
    {
        pauseMenuUI.SetActive(false);
        pauseMenu = GetComponent<MenuBouton>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("pause"))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;        
        
    }

    public void LoadMainMenu(string nomMainMenu)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomMainMenu);
    }
}
