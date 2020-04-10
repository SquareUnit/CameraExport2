using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIManager : MonoBehaviour, ISelectHandler
{
    public static UIManager instance;

    //son
    public SoundRelay soundRelay;


    //Canvas et panel de base
    public Canvas canvas;
    public GameObject dialoguePanel;
    public GameObject savePanel;
    public GameObject decalPanel;
    public GameObject pauseMenu;
    public GameObject collectibleMenuPanel;
    public GameObject cinematicUIPanel;
    public GameObject PlayTestMenuPanel;
    public GameObject MainMenuPanel;
    public GameObject nouvellePartiePannel;
    public GameObject splashScreenPanel;


    public GameObject cheatUIPanel;
    public GameObject loadingPanel;


    //hover
    public GameObject[] hoverLvl;
    public GameObject[] hoverMain;


    //camera
    public Camera refCamera;
    public Camera camUI;


    //Perspective
    public Image maskPerspective;
    private float startTime;
    private float t;
    [Range(0.001f, 10.0f)] public float colorLerpSpeed = 10f;
    private Color currentColor;
    private Color desiredColor;
    private Color baseMaskColor = new Color(0, 0, 0, 0);
    private Color addMaskColor = Color.blue;
    private Color subMaskColor = Color.red;
    private Color combinedMaskColor = Color.magenta;

    //SplashScreen
    private bool firstStart = false;

    //Cinematic
     

    //Dialogue
    public GameObject dialogueBox;
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;


    //Save
    public TextMeshProUGUI saveText;

    //decal 
    public TextMeshProUGUI decalText;
    public Image fadeToBlack;
    //private bool isFading = false;
    private Color colorTemp;

    //GameOver
    private TextMeshProUGUI gameOver;

    //loading
    public bool isFading = false;
    public bool isFadingIn = false;
    public bool isFadingOut = false;
    public bool allowFade = true;
    public float fadeState = 0;

    //Menu Collectibe
    public GameObject collectibleMenu;

    public Scrollbar scrollbarCollectible;

    public Toggle[] groupBouttonCollectible = new Toggle[9];



    //Menu pensee

    //GameOver
    public bool gameOverText;
    public float fadeOutSpeed = 1f;
    private bool isFadingPantin = false;
    public float fadeOutSpeedPantin = 2f;
    private bool fadeOut = false;


    //Continue default for pause menu
    public Toggle continueButton;


    //Ajout  MainMenu Interaction

    [SerializeField] private GameObject secondaryPanel;
    [SerializeField] private RectTransform niveauPanel;


    public bool firstClick = true;
    //public Mask[] subGame;
    //public int subGameID = 0;


    //Event system
    public Toggle defaultButtonSelection;
    public Toggle defaultLevelSelection;
    public Toggle nonButton;

    public ToggleGroup mainPanelToggleGroup;
    public ToggleGroup NiveauPanelToggleGroup;

    public Button upButton;


    //Perspetive UI
    /*public Image additivePerspective;
    public Image negativePerspective;*/

    //ghost mode
    private float holdDur = 2f;
    float timer;
    bool cheatOpen = false;

    private void Awake()
    {
        SetupSingleton();
        saveText = saveText.GetComponent<TextMeshProUGUI>();
        decalText = decalPanel.GetComponentInChildren<TextMeshProUGUI>();

        gameOver = fadeToBlack.GetComponentInChildren<TextMeshProUGUI>();

        soundRelay = GetComponent<SoundRelay>();

        StartSetting();

    }

    private void Start()
    {
        defaultButtonSelection = defaultButtonSelection.GetComponent<Toggle>();
        defaultLevelSelection = defaultLevelSelection.GetComponent<Toggle>();

        if (SaveSystem.isPath)
            continueButton.gameObject.SetActive(false);
    }


    public void OnSelect(BaseEventData eventData)
        {
            throw new System.NotImplementedException();
        }


    //start setting pour menu UI
    public void StartSetting ()
    {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayTestMenuPanel.transform.childCount; i++)
        {
            PlayTestMenuPanel.transform.GetChild(i).gameObject.SetActive(false);
        }

        //Splash screen
        PlayTestMenuPanel.SetActive(true);
        PlayTestMenuPanel.transform.GetChild(0).gameObject.SetActive(true);
        splashScreenPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        cheatUIPanel.SetActive(false);
        cheatOpen = false;
        firstStart = false;
    }

    public void Update()
    {

        t = (Time.time - startTime) * colorLerpSpeed;
        if (savePanel.activeInHierarchy)
            saveText.alpha -= fadeOutSpeed * Time.deltaTime;
        if (decalPanel.activeInHierarchy)
            decalText.alpha -= fadeOutSpeed * Time.deltaTime;

        if (allowFade)
        {
            if (isFadingIn)
            {
                colorTemp = fadeToBlack.color;
                colorTemp.a += fadeOutSpeed * Time.deltaTime;
                fadeToBlack.color = colorTemp;
                if (gameOverText)
                    gameOver.alpha += fadeOutSpeed * Time.deltaTime;

                if (fadeToBlack.color.a >= 1f)
                {
                    isFadingIn = false;
                    allowFade = false;
                }
                
            }
            else if(isFadingOut)
            {
                if (fadeToBlack.color.a > 0f)
                {
                    colorTemp = fadeToBlack.color;
                    colorTemp.a -= fadeOutSpeedPantin * Time.deltaTime;
                    fadeToBlack.color = colorTemp;
                }
                else
                {
                    isFadingOut = false;
                    DisableFadeToBlack();
                }
            }

            if (InputsManager.instance.crossPressed && !splashScreenPanel.activeInHierarchy && !cheatUIPanel.activeInHierarchy)
            {
                Niveau();
                cheatOpen = true;
                InputsManager.instance.SetVibration(1f, 1f, true);
            }
   

        }
        if (InputsManager.instance.crouch)
        {
            if (pauseMenu.activeInHierarchy)
                GameManager.instance.Resume();

            else if (collectibleMenu.activeInHierarchy)
            {
                DisableCollectibleMenu();
            }

            if (secondaryPanel.activeInHierarchy)
            {
                secondaryPanel.SetActive(false);
                EventSystem.current.SetSelectedGameObject(defaultButtonSelection.gameObject);
            }

            if (cheatUIPanel.activeInHierarchy)
            {
                cheatUIPanel.SetActive(false);
                if (MainMenuPanel.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(defaultButtonSelection.gameObject);
                }
            }
        }

        //splashscreen

        if (firstStart == false && splashScreenPanel.activeInHierarchy == true)
        {
            if (InputsManager.instance.start || InputsManager.instance.crouch || InputsManager.instance.back || InputsManager.instance.interact) {

                firstStart = true;
                PlayTestMenuPanel.transform.GetChild(0).gameObject.SetActive(true);
                PlayTestMenuPanel.transform.GetChild(1).gameObject.SetActive(true);
                SaveData continueMaybe = SaveSystem.LoadGame();
                if (continueMaybe == null)
                {
                    PlayTestMenuPanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                }
                splashScreenPanel.SetActive(false);
                secondaryPanel.gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(defaultButtonSelection.gameObject);

            }

        }

        if (isFadingPantin)
        {
            if (fadeToBlack.color.a <= 1f && !fadeOut)
            {
                colorTemp = fadeToBlack.color;
                colorTemp.a += fadeOutSpeedPantin * Time.deltaTime;
                fadeToBlack.color = colorTemp;
            }
            else
            {
                fadeOut = true;
                if (fadeToBlack.color.a > 0f)
                {
                    colorTemp = fadeToBlack.color;
                    colorTemp.a -= fadeOutSpeedPantin * Time.deltaTime;
                    fadeToBlack.color = colorTemp;
                }
                else
                {
                    DisableFadeToBlack();
                    fadeOut = false;
                }
            }
        }
    }
    //mask des perspectives in game
    public void CheckPerspectiveInputs()
    {
        if (InputsManager.instance.triggerLeftDown)
        {
            startTime = Time.time;
            currentColor = maskPerspective.color;
            desiredColor = subMaskColor;
        }
        if (!InputsManager.instance.triggerLeft)
        {
            startTime = Time.time;
            desiredColor = subMaskColor;
        }
    }

    public void LerpPerspectiveMaskColor()
    {
        if (desiredColor != currentColor)
        {
            maskPerspective.color = Color.Lerp(currentColor, desiredColor, t);
        }
    }


    public void EnableCinematicPanel()
    {
        cinematicUIPanel.SetActive(true);
    }

    public void DisableCinematicPanel()
    {
        cinematicUIPanel.SetActive(false);
    }

    public void EnableSavePanel()
    {
        savePanel.SetActive(true);
        Invoke("DisableSavePanel", 2f);
    }

    private void DisableSavePanel()
    {
        savePanel.SetActive(false);
    }

    public void EnableDecalPanel()
    {
        decalText.text = GameManager.instance.spcLvlMan.collectibleCollected + "/" + GameManager.instance.spcLvlMan.totalCollectible;
        decalPanel.SetActive(true);
        Invoke("DisableDecalPanel", 2f);
    }

    private void DisableDecalPanel()
    {
        decalText.alpha = 1f;
        decalPanel.SetActive(false);
    }

    public void EnableGameOver()
    {
        gameOverText = true;
        EnableFadeToBlack();
    }

    public void EnableFadeIn()
    {
        fadeToBlack.gameObject.SetActive(true);
        isFadingIn = true;
        allowFade = true;
    }

    public void EnableFadeToBlack()
    {
        fadeToBlack.gameObject.SetActive(true);
        isFadingIn = true;
        allowFade = true;
        Invoke("EnableFadeOut", 3f);
    }

    public void EnableFadeToBlack(float timer)
    {
        fadeToBlack.gameObject.SetActive(true);
        isFadingIn = true;
        allowFade = true;
        Invoke("EnablefadeOut", timer);
    }

    public void EnableFadeToBlackPatin()
    {
        fadeToBlack.gameObject.SetActive(true);
        isFadingPantin = true;
    }

    public void EnableFadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
        allowFade = true;
    }

    public void EnableFadeOut(float timer)
    {
        isFadingIn = false;
        isFadingOut = true;
        allowFade = true;
        Invoke("DisableFadeToBlack", timer);
    }

    private void DisableFadeToBlack()
    {
        isFading = false;
        allowFade = true;
        isFadingIn = false;
        isFadingOut = false;
        isFadingPantin = false;
        fadeToBlack.color = new Vector4(0, 0, 0, 0);
        gameOverText = false;
        gameOver.alpha = 0f;
        loadingPanel.SetActive(false);
        fadeToBlack.gameObject.SetActive(false);
    }

    public void PauseFadeToBlack()
    {
        isFading = false;
        isFadingIn = false;
        isFadingOut = false;
    }

    public void ResumeFadeToBlack(bool fadingOfTypeIn)
    {
        if (fadingOfTypeIn)
            isFadingIn = true;
        else
            isFadingOut = true;
        isFading = true;
    }

    public void EnablePauseMenu()
    {
        //UI cam following player Cam
        camUI.gameObject.transform.position = GameManager.instance.currentCamera.transform.position;
        camUI.gameObject.transform.rotation = GameManager.instance.currentCamera.transform.rotation;
        pauseMenu.SetActive(true);
        
        //continueButton par defaut
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
    }

    public void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void EnableCollectibleMenu()
    {
        DisablePauseMenu();
        collectibleMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(upButton.gameObject);
    }

    public void BackNouvelle()
    {
        nouvellePartiePannel.SetActive(false);
        secondaryPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(defaultButtonSelection.gameObject);

    }

    public void DisableCollectibleMenu()
    {
        collectibleMenu.SetActive(false);
        EnablePauseMenu();
    }


    public void EnableLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void DisableLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }


    public void ToggleMainMenuPanel()
    {
        canvas.gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void ToggleCheatPanel()
    {
        cheatUIPanel.SetActive(!cheatUIPanel.activeInHierarchy);
    }

    private void SetupSingleton()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }



    public void Charger()
    {
        if (firstClick)
        {
            firstClick = false;
            ChargerSuite();
        }
        else
            Invoke("ChargerSuite", 0.9f);

        if (defaultButtonSelection != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultLevelSelection.gameObject);
        }
    }

    public void ChargerSuite()
    {
        for (int i = 0; i < secondaryPanel.transform.childCount; i++)
        {
            secondaryPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
        secondaryPanel.transform.GetChild(0).gameObject.SetActive(true);

    }


    public void Niveau()
    {
        if (!cheatUIPanel.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < secondaryPanel.transform.childCount; i++)
            {
                secondaryPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
            cheatUIPanel.gameObject.SetActive(true);
            nouvellePartiePannel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(defaultLevelSelection.gameObject);
        }
    }

    public void Credit()
    {
        if (firstClick)
        {
            firstClick = false;
            CreditSuite();
        }
        else
            Invoke("CreditSuite", 0.9f);
    }

    public void CreditSuite()
    {
        for (int i = 0; i < secondaryPanel.transform.childCount; i++)
        {
            secondaryPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
        secondaryPanel.transform.GetChild(2).gameObject.SetActive(true);
    }

    public void NewGame()
    {
        //BOOL IS PLACEHOLDER
        bool savedGame = true;
        if (savedGame) {
            secondaryPanel.SetActive(true);
            nouvellePartiePannel.SetActive(true);
            cheatUIPanel.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(nonButton.gameObject);
        }
        else GameManager.instance.LoadLevel1();
    }
    public void ScrollUp()
    {
        if (scrollbarCollectible.value > 0)
        {
            scrollbarCollectible.value += 0.2f;
        }
    }

    public void ScrollDown()
    {
        if (scrollbarCollectible.value > 0)
        {
            scrollbarCollectible.value -= 0.2f;
        }        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
