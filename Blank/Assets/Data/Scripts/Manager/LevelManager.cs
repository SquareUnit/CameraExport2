// Script Owner : Felix Desrosiers - 25/04/2019
// Last Modification : Felix

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; 
    public Scene currentScene;
    public int currentLvl;
    public bool levelDesignOnly = false;
    public bool firstLoad = true;
    public Image image;
    public bool busy = false;
    public bool canStartCoroutine = true;

    int cpt = 0;

    public enum LevelToLoad { Null, level1, level2, level3, level4, level5, level6, level7, level8, level9 };
    Dictionary<LevelToLoad, LevelsInfos> levelDictionary = new Dictionary<LevelToLoad, LevelsInfos>();
    public List<LevelsInfos> levels = new List<LevelsInfos>(9);

    [System.Serializable]
    public class LevelsInfos
    {
        public string levelName;
        public LevelToLoad level;
        public Scene sceneLA, sceneLD;
        public bool spawnAtStart;
    }

    private void Awake()
    {
        SingletonSetup();

        foreach (LevelsInfos i in levels)
        {
            if(i.level != LevelToLoad.Null)
                levelDictionary.Add(i.level, i);
        }
    }

    /// <summary> Call to load the desired LA and LD scene in order, then . Currently not cleaning whats leaving behind, I think. Loading a scene makes it active by default </summary>
    /// <param name="desiredLevel"> The level you wish to lead</param>
    /// <param name="levelDesignOnly"> If true, will only load the level designer blocking</param>
    public void LoadLevel(LevelToLoad lvl, bool levelDesignOnly)
    {
        GameManager.instance.levelLoaded = false;
        if (UIManager.instance != null)
            UIManager.instance.EnableFadeIn();
        if (!levelDesignOnly)
        {
            StartCoroutine(LoadSceneDual(levelDictionary[lvl].spawnAtStart, levelDictionary[lvl].sceneLA, levelDictionary[lvl].sceneLD));
            currentScene = SceneManager.GetActiveScene();
            currentLvl = (int)lvl;
        }
        else
        {
            StartCoroutine(LoadSceneLD(levelDictionary[lvl].spawnAtStart, levelDictionary[lvl].sceneLD));
            currentScene = SceneManager.GetActiveScene();
            currentLvl = (int)lvl;
        }
    }

    IEnumerator LoadSceneDual(bool avatarSpawnAtStart, Scene sceneLA, Scene sceneLD)
    {
        busy = true;
        while (UIManager.instance.isFadingIn)
            yield return new WaitForEndOfFrame();

        if (canStartCoroutine)
        {
            canStartCoroutine = false;
            StartCoroutine(LoadingCoroutine());
        }
        
        ///Logic avant load
        AsyncOperation asyncScene;
        asyncScene = SceneManager.LoadSceneAsync(sceneLA.handle, LoadSceneMode.Single);
        while (!asyncScene.isDone)
            yield return new WaitForEndOfFrame();
        ///Logic entre
        asyncScene = SceneManager.LoadSceneAsync(sceneLD.handle, LoadSceneMode.Additive);
        while (!asyncScene.isDone)
            yield return new WaitForEndOfFrame();
        ///Logic apres load
        if (firstLoad)
        {
            GameManager.instance.FirstLoad();
            firstLoad = false;
        }
        else
        {
            GameManager.instance.OnLevelLoad();
        }
        
        while (!GameManager.instance.levelLoaded)
        {
            cpt++;
            yield return new WaitForEndOfFrame();
        }
        UIManager.instance.EnableFadeOut();
        canStartCoroutine = true;
        busy = false;
        UIManager.instance.DisableLoadingPanel();
    }

    IEnumerator LoadSceneLD(bool avatarSpawnAtStart, Scene sceneLD)
    {
        busy = true;
        ///Logic avant load
        AsyncOperation asyncScene;
        asyncScene = SceneManager.LoadSceneAsync(sceneLD.handle, LoadSceneMode.Single);
        while (!asyncScene.isDone)
            yield return new WaitForEndOfFrame();
        ///Logic apres load
        busy = false;
    }

    IEnumerator LoadingCoroutine()
    {
        UIManager.instance.EnableLoadingPanel();
        while (busy)
            yield return new WaitForEndOfFrame();
    }

    private void SingletonSetup()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}