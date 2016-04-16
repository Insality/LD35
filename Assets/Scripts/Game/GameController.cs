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


    void Start()
    {
        _gameController = this;
        Map.LoadMap();
        Player.SetPos((int)Map.SpawnPoint.x, (int)Map.SpawnPoint.y);

        _isRunning = true;
        StartCoroutine(StartBeating());
    }

    private IEnumerator StartBeating()
    {
        yield return new WaitForSeconds(1f);
        _beatCounter = 0;
        while (_isRunning)
        {
            GameBeat();
            yield return new WaitForSeconds(60/_bpm);
        }
    }

    private void GameBeat()
    {
        _beatCounter++;

        Map.StayEvent(Player.Coords);

        if (_beatCounter%4 == 0)
        {
            SoundController.PlaySound(SoundType.TestKick2);
        }
        else
        {
            SoundController.PlaySound(SoundType.TestKick);
        }

        if (GameStep != null)
        {
            GameStep(_beatCounter);
        }
    }

    public static GameController GetInstance() {
        return _gameController;
    }

}
