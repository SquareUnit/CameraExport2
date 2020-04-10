// Créé par: Jonathan G. Mann, Felix Desrosiers-Dorval
// En date du: 03-04-19
// Modifié par: Felix Desrosiers-Dorval, David Babin
// En date du: 17-04-2019, 21-05-2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    #region Managers
    [HideInInspector] public static GameManager instance;
    [HideInInspector] public CameraManager cameraManager;
    [HideInInspector] public EventManager eventManager;
    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public DialogueManager dialogueManager;
    /*[HideInInspector]*/ public VideoPlayerManager videoPlayerManager;
    public VideoPlayerManager videoPlayerManagerDeath;

    public SoundRelay soundRelay;
    public MusicPlayer musicPlayer;
    #endregion

    #region VARIABLE SCRIPTS
    public Character modelAvatar;
    public Character currentAvatar;
    public float avatarHeight;
    public SpawnPointLocation spawnPoint;
    public PoolingManager fxPool, sfxPool;
    private SaveData saveData;
    /*[HideInInspector]*/
    public Vector3 checkpointLocation;
    public Vector3 checkpointRotation;
    [HideInInspector] public SpecificLevelManager spcLvlMan;
    public PlayerCamera currentCamera;
    public bool levelLoaded = false;
    public bool pantinCamIsOff = false;
    public CheatMenu cheatMenu;
    public CollectibleInfo[] collectibleInfos;
    public bool[] collectibleDecals;
    [HideInInspector] public int debugTimer;
    public RefAvatar refAvatar;
    public RefCamera refCam;
    private bool gameIsPaused = false;
    //[HideInInspector] public UnityEvent ToggleRD;
    public bool disableRDOnStart = true;
    [HideInInspector] public bool playerCamOn = false;
    public bool GestaltCompleted = false;
    public bool canPlaySound = true;
    public bool characterIsDead = false;
    public enum langues { english, francais};
    public langues langue;
    #endregion

    #region Events
    [HideInInspector]
    public UnityEvent callAdditiveStart = new UnityEvent();
    [HideInInspector]
    public UnityEvent callAdditiveEnd = new UnityEvent();
    [HideInInspector]
    public UnityEvent callSubstractiveStart = new UnityEvent();
    [HideInInspector]
    public UnityEvent callSubstractiveEnd = new UnityEvent();
    [HideInInspector]
    public UnityEvent callReset = new UnityEvent();
    #endregion

    private void Awake()
    {
        langue = langues.francais;
        SingletonSetup();
        collectibleDecals = new bool[20];
        refAvatar.GetComponent<RefAvatar>();
        refCam.GetComponent<RefCamera>();
        if(videoPlayerManager != null)
            videoPlayerManager = videoPlayerManager.GetComponent<VideoPlayerManager>();
    }

    private void Start()
    {
        cheatMenu = GetComponent<CheatMenu>();
        Application.targetFrameRate = 60;
        Scene startScene = SceneManager.GetActiveScene();
        QualitySettings.vSyncCount = 0;
        soundRelay = GetComponent<SoundRelay>();
        

        if (startScene.name != "MainMenu")
        {
            UIManager.instance.PlayTestMenuPanel.gameObject.SetActive(false);
            Invoke("FirstLoad", 1f);
        }
        else
        {
            videoPlayerManager.myVP.enabled = true;
            if (musicPlayer != null)
            {
                Invoke("PlayMainMenuMusic", 2f);
            }
        }
        if (disableRDOnStart)
        {
            InputsManager.instance.ToggleRD.Invoke();
            disableRDOnStart = false;
        }
    }

    public void PlayMainMenuMusic()
    {
        musicPlayer.PlayMusicTrack(musicPlayer.musicTrack01);
    }

    private void Update()
    {
        if (Time.timeScale != 0f)
        {
            if (instance.spcLvlMan != null)
            {
                if (instance.spcLvlMan.perspAddUnlocked || instance.spcLvlMan.perspSubsUnlocked)
                {
                    CallPerspectives();
                }
                debugTimer++;
            }
        }

        if (InputsManager.instance.start)
        {
            if (!gameIsPaused || UIManager.instance.pauseMenu.activeInHierarchy)
                Pause();
            else
                Resume();
        }
        if (disableRDOnStart)
        {
            StartCoroutine("WaitFor");
            disableRDOnStart = false;
        }
    }

    public IEnumerator WaitFor()
    {
        InputsManager.instance.ToggleRD.Invoke();
        yield return new WaitForSeconds(1);
    }

    public void FirstLoad()
    {
        spcLvlMan.Init();
        currentAvatar = Instantiate(modelAvatar, spcLvlMan.playerStart, spcLvlMan.playerStartRotation);
        avatarHeight = currentAvatar.GetComponent<CharacterController>().height;
        disableRDOnStart = true;
        SaveSystem.GenerateDefaultSaveFile();
        Invoke("ResetLevelLoaded", 7f);
    }

    public void OnLevelLoad()
    {
        spcLvlMan.OnChangeLevel();
        currentAvatar.tr.position = spcLvlMan.playerStart;
        currentAvatar.tr.rotation = spcLvlMan.playerStartRotation;
        disableRDOnStart = true;
        SaveGame();
        Invoke("ResetLevelLoaded", 7f);
    }

    public void ResetLevelLoaded()
    {
        levelLoaded = true;
    }

    // Unity events
    private void CallPerspectives()
    {
        if (currentAvatar != null && currentAvatar.stateMachine.currentState != currentAvatar.stateMachine.death)
        {
            if (instance.spcLvlMan.perspAddUnlocked)
            {
                if (InputsManager.instance.triggerRightDown) callAdditiveStart.Invoke();
                if (InputsManager.instance.triggerRightUp) callAdditiveEnd.Invoke();
            }

            if (instance.spcLvlMan.perspSubsUnlocked)
            {
                if (InputsManager.instance.triggerLeftDown) callSubstractiveStart.Invoke();
                if (InputsManager.instance.triggerLeftUp) callSubstractiveEnd.Invoke();
            }
        }
    }

    public void CallResetOnDeath()
    {
        callReset.Invoke();
    }

    private void SingletonSetup()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void RespawnAtCheckpoint()
    {
        currentAvatar.controller.enabled = false;
        currentAvatar.tr.position = checkpointLocation;
        currentAvatar.tr.eulerAngles = checkpointRotation;
        refCam.ResetCamera();
        currentAvatar.controller.enabled = true;
        characterIsDead = false;
    }

    #region SCENETOLOAD
    public void LoadLevel1()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level1, false);
    }
    public void LoadLevel2()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level2, false);
    }
    public void LoadLevel3()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level3, false);
    }
    public void LoadLevel4()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level4, false);
    }
    public void LoadLevel5()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level5, false);
    }
    public void LoadLevel6()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level6, false);
    }
    public void LoadLevel7()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level7, false);
    }
    public void LoadLevel8()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level8, false);
    }
    public void LoadLevel9()
    {
        LevelManager.instance.LoadLevel(LevelManager.LevelToLoad.level9, false);
    }
    public void LoadMenu()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    /// <summary> Pause le jeu et active le menu pause </summary>
    public void Pause()
    {
        if (!UIManager.instance.MainMenuPanel.activeInHierarchy)
        {
            Debug.Log("Pause");
            Time.timeScale = 0f;
            gameIsPaused = true;
            UIManager.instance.EnablePauseMenu();
        }
    }

    /// <summary> Resume le jeu, desactive le menu pause </summary>
    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        UIManager.instance.DisablePauseMenu();
    }

    public void Exit()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(LevelManager.instance, instance);
        UIManager.instance.EnableSavePanel();
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        int levelToLoad = data.currentLevel <= 0 ? 1 : data.currentLevel;
        UIManager.instance.MainMenuPanel.SetActive(false);
        Invoke("LoadLevel" + levelToLoad.ToString(), 1f);
    }
}