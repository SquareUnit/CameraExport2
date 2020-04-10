using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundRelay : MonoBehaviour
{
    [HideInInspector] public AudioSource[] audioSourceArray;
    [HideInInspector] public AudioSource audioSourceOneShot;
    [HideInInspector] public AudioSource audioSourceLooping;

    public enum SetMaster { AvatarVolume, EnemyVolume, IngredientsVolume, AmbiantVolume, MusicVolume }
    public SetMaster masterUsed;

    public AudioMixer masterMixer;
    [Range(-20.0f, 30.0f)] public float dbCorection = 0.0f;

    public List<ComplexClip> complexClipList = new List<ComplexClip>();
    [HideInInspector] int chosenClip;
    [HideInInspector] AudioClip lastChosenClip;

    [System.Serializable]
    public class ComplexClip
    {
        public string name;
        public AudioClip[] clipArray;
        public bool isLooping;
        [Range(0.0f, 1.0f)] public float volumeCorrection = 1.0f;
        [Range(0.0f, 100.0f)] public int chanceToBeMute = 0;
        [Range(1, 20)] public int nbOfRandomPass = 1;
        [HideInInspector] public int lastPlayedClip;

        public ComplexClip()
        {
            clipArray = new AudioClip[1];
            lastPlayedClip = -1;
            nbOfRandomPass = 1;
        }
    }

    public void Start()
    {
        audioSourceArray = new AudioSource[2];
        audioSourceArray = GetComponentsInChildren<AudioSource>();
        audioSourceOneShot = audioSourceArray[0];
        audioSourceLooping = audioSourceArray[1];
        audioSourceLooping.loop = true;
        SetMasterLevel();
    }

    public void PlayAudioClip(int complexClipID)
    {
        //Debug.Log("Is playing sound number : " + complexClipID);
        ComplexClip complexClip = complexClipList[complexClipID];
        AudioClip[] clipArray = complexClip.clipArray;

        if (clipArray.Length > 1)
        {
            AudioClip selectedClip = SelectClipAtRandom(clipArray, complexClip);
            audioSourceOneShot.volume = complexClip.volumeCorrection;
            audioSourceOneShot.PlayOneShot(selectedClip);
        }
        else if (clipArray.Length == 1)
        {
            AudioClip selectedClip = clipArray[0];
            if (!complexClip.isLooping)
            {
                audioSourceOneShot.volume = complexClip.volumeCorrection;
                audioSourceOneShot.PlayOneShot(selectedClip);
            }
            else
            {
                audioSourceLooping.volume = complexClip.volumeCorrection;
                audioSourceLooping.clip = selectedClip;
                audioSourceLooping.Play();
            }
        }
        else if (clipArray.Length == 0)
        {
            Debug.Log("There is no sound assigned in the complex clip you try to play sound from");
        }
    }

      public void PlayAudioClip(AnimationEvent animationEvent)
    {
        //Debug.Log("Is playing sound number : " + complexClipID);
        ComplexClip complexClip = complexClipList[animationEvent.intParameter];
        AudioClip[] clipArray = complexClip.clipArray;

        if (clipArray.Length > 1)
        {
            AudioClip selectedClip = SelectClipAtRandom(clipArray, complexClip);
            audioSourceOneShot.volume = complexClip.volumeCorrection;
            audioSourceOneShot.PlayOneShot(selectedClip);
        }
        else if (clipArray.Length == 1)
        {
            AudioClip selectedClip = clipArray[0];
            if (!complexClip.isLooping)
            {
                audioSourceOneShot.volume = complexClip.volumeCorrection;
                audioSourceOneShot.PlayOneShot(selectedClip);
            }
            else
            {
                audioSourceLooping.volume = complexClip.volumeCorrection;
                audioSourceLooping.clip = selectedClip;
                audioSourceLooping.Play();
            }
        }
        else if (clipArray.Length == 0)
        {
            Debug.Log("There is no sound assigned in the complex clip you try to play sound from");
        }
    }

    public AudioClip SelectClipAtRandom(AudioClip[] clipArray, ComplexClip complexClip)
    {
        AudioClip selectedClip;
        // Roll X times to try to avoid getting the same sfx twice. Else, play same as last one played.
        for (int i = 0; i <= complexClip.nbOfRandomPass - 1; i++)
        {
            chosenClip = Random.Range(0, clipArray.Length);
            if (chosenClip != complexClip.lastPlayedClip)
            {
                complexClip.lastPlayedClip = chosenClip;
                break;
            }
        }

        // Percentage chance that no sound will be played at all sintead of the selected clip.
        if(Random.Range(0, 100) < complexClip.chanceToBeMute)
        {
            selectedClip = null;
        }
        else
        {
            selectedClip = clipArray[chosenClip];
        }
        return selectedClip;
    }

    public void StopLoopingAudio()
    {
        audioSourceLooping.clip = null;
    }

    private void SetMasterLevel()
    {
        string enumVal = masterUsed.ToString();
        /*Debug.Log(enumVal);*/
        //masterMixer.SetFloat(enumVal, dbCorection);
    }

    private void OnValidate()
    {
        foreach(ComplexClip i in complexClipList)
        {
            i.volumeCorrection = (float)System.Math.Round(i.volumeCorrection, 2);
        }
        SetMasterLevel();
    }
}

