using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController: MonoBehaviour
{
    private static AppController _appController;
    [HideInInspector] public Transform PoolStoreTransform;

    // Calculated constants:
    public static float ScreenWidth;
    public static float ScreenHeight;

    // Clips

    public AnimationClip LittleResizeUp;
    public AnimationClip SpikesUp;

    private void Start()
    {
        _appController = this;
        ScreenWidth = Screen.currentResolution.width;
        ScreenHeight = Screen.currentResolution.height;

#if UNITY_ANDROID || UNITY_IPHONE
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
        PoolStoreTransform = transform;
        DontDestroyOnLoad(gameObject);
        SoundController.SetAudioSettings(true, true);
        SceneManager.LoadScene("game");
    }

    public static AppController GetInstance()
    {
        return _appController;
    }

    
}
