using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();
    public List<AudioClip> audioClipList = new List<AudioClip>();
        
    private void Awake()
    {
        SingletonSetup();
        for (int i = 0; i < audioClipList.Count; i++)
        {
            if(audioClipList[i] != null)
            {
                audioClipDict.Add(audioClipList[i].name, audioClipList[i]);
            }
        }
    }

    void SingletonSetup()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
