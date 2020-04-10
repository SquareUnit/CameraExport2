using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamScript : MonoBehaviour
{
    private static SteamScript s_instance;
    private static SteamScript Instance
    {
        get
        {
            return s_instance ?? new GameObject("SteamScript").AddComponent<SteamScript>();
        }
    }


    private bool m_bInitialized;
    public static bool Initialized
    {
        get
        {
            return Instance.m_bInitialized;
        }
    }

    private void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
        DontDestroyOnLoad(gameObject);


        try
        {
            if (SteamAPI.RestartAppIfNecessary((AppId_t)1174921))
            {
                Application.Quit();
                return;
            }
        }
        catch (System.DllNotFoundException e)
        {
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

            Application.Quit();
            return;
        }
        m_bInitialized = SteamAPI.Init();
        if (!m_bInitialized)
        {
            Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);

            return;
        }

    }

    private void OnEnable()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
    }

    private void OnDestroy()
    {
        if (s_instance != this)
        {
            return;
        }
        s_instance = null;
        if (!m_bInitialized)
        {
            return;
        }

        SteamAPI.Shutdown();
    }
}

