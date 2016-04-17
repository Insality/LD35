using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameGUIController GameGuiController;
    public Map Map;
    public Player Player;
    public Action<int> GameStep;

    private float _bpm = 130;
    private int _beatCounter;
    private bool _isRunning;
    private static GameController _gameController;
    public float TimeLeft;


    void Start()
    {
        TimeLeft = 180;
        _gameController = this;
        Map.LoadMap();
        Player.SetPos((int)Map.SpawnPoint.x, (int)Map.SpawnPoint.y);
        transform.localPosition = new Vector3(1800, 2000, -20);

        GameGuiController.ScreenEffectController.ShineEffect.intensity = 0.3f;
        GameGuiController.ScreenEffectController.ShineEffect.enabled = true;
    }

    private void StartGame()
    {
        _isRunning = true;
        GameGuiController.HideStartScreen();
        GameGuiController.TimerText.gameObject.SetActive(true);
        GameGuiController.ScreenEffectController.ShineEffect.enabled = false;
        StartCoroutine(StartBeating());
    }

    private IEnumerator StartBeating()
    {
        yield return new WaitForSeconds(0.5f);
        _beatCounter = 0;
        SoundController.PlayMusic(MusicType.GameMusic);
        TimeLeft = 180;
        yield return new WaitForSeconds(1.84f);
        StartCoroutine(TimerRefresh());
        while (_isRunning)
        {
            GameBeat();
            yield return new WaitForSeconds(60/_bpm);
        }
    }

    public void StopGame()
    {
        _isRunning = false;
    }

    private IEnumerator TimerRefresh()
    {
        while (true)
        {
            GameGuiController.TimerText.SetText(string.Format("{0:0.00}", TimeLeft));
            yield return new WaitForSeconds(0.06f);   
        }
    }

    private void GameBeat()
    {
        _beatCounter++;

        if (_beatCounter == 1)
        {
            ShowHint("3");
        }
        if (_beatCounter == 2) {
            ShowHint("2");
        }
        if (_beatCounter == 3) {
            ShowHint("GO\n\nARROWS OR WASD TO MOVE");
        }



        Map.StayEvent(Player.Coords);

        if (GameStep != null)
        {
            GameStep(_beatCounter);
        }
    }

    public static GameController GetInstance() {
        return _gameController;
    }

    public void ShowHint(string hintText)
    {
        GameGuiController.ShotHint(hintText);
    }

    public void HideHint()
    {
        GameGuiController.HideHint();
    }

    public void ShowHintTime(string text, float time)
    {
        ShowHint(text);
        Tween.DelayAction(time, HideHint);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Player.Lose();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Player.Level = 5;
            Player.Upgrade();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (_isRunning)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft < 0)
            {
                Player.Lose();
            }
        }
    }
}
