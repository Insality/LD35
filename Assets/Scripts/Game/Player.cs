
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : BaseEntity {

    public Map Map;
    public int Level;
    public int Health;

    private float _canStepTimeZone = 0.1f;
    private float _canStepTimer = 0f;
    private bool _isCanStep = false;

    protected override void Start()
    {
        GameController.GetInstance().GameStep += GameStep;
        Level = 0;
        RefreshGraphics();
        Health = Constants.PlayerHp;
    }

    private void GameStep(int i)
    {
        if (i > 0 && Level < 3)
        {
            Upgrade();
        }

        _canStepTimer = _canStepTimeZone;

        if (Map.Cells[(int) Coords.x, (int) Coords.y].State == CellState.Damage)
        {
            Damage();
        }
    }

    public void Damage()
    {
        if (Health >= 0)
        {
            Health--;

            SoundController.PlaySound(SoundType.PlayerDamage);
            GameController.GetInstance().GameGuiController.ScreenEffectController.ShineScreen(-0.4f, 0.2f);
            GameController.GetInstance().GameGuiController.Shaker.Shake(0.25f);
            GameController.GetInstance().GameGuiController.SetPlayerHealth(Health);
        }
        else
        {
            SoundController.PlaySound(SoundType.Lose);
            Tween.DelayAction(0.5f, ()=> SceneManager.LoadScene("game"));
            
        }
        GameController.GetInstance().GameGuiController.PermanentShaker.ShakePower = 3 - Health;
    }

    protected override void Update()
    {
        base.Update();
        if (_canStepTimer > 0) {
            _canStepTimer -= Time.deltaTime;
            if (_canStepTimer <= 0) {
                _canStepTimer = 0;
                _isCanStep = true;
            }
        }

//        if (_isCanStep)
//        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                TryMove(MoveDirection.Up);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                TryMove(MoveDirection.Right);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                TryMove(MoveDirection.Down);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TryMove(MoveDirection.Left);
            }
//        }
    }

    private void TryMove(MoveDirection dir)
    {
        var delta = GetDirection(dir);

        if (Map.IsCanStep(Coords+delta, dir)) {
           Move(dir);
        } else {
//           Coords -= delta;
        }
    }

    public void Move(MoveDirection dir)
    {
        Coords += GetDirection(dir);
        SetPos();
        Map.MoveFromEvent(Coords - GetDirection(dir), dir);
        Map.MoveToEvent(Coords, dir);
        _isCanStep = false;
        SoundController.PlaySound(SoundType.PlayerMove);
    }

    public void MoveInvert(MoveDirection dir) {
        if (Map.IsCanStep(Coords - GetDirection(dir), dir))
        {
            Coords -= GetDirection(dir);
            SetPos();
            _isCanStep = false;
        }
    }

    public void Upgrade()
    {
        if (Level < 6)
        {
            Level++;
            RefreshGraphics();
            if (Level > 3)
            {
                SoundController.PlaySound(SoundType.Upgrade);
            }

            GameController.GetInstance().GameGuiController.Shaker.Shake(0.1f);
            GameController.GetInstance().GameGuiController.ScreenEffectController.ShineScreen(0.1f, 0.1f);
        }
    }

    private void RefreshGraphics()
    {
        Graphics.gameObject.SetActive(Level != 0);
        if (Level > 0)
        {
            Graphics.SetSprite("Player" + Level);
        }
    }

    public static Vector2 GetDirection(MoveDirection dir)
    {
        switch (dir)
        {
            case MoveDirection.Up:
                return new Vector2(0, 1);
            case MoveDirection.Right:
                return new Vector2(1, 0);
                case MoveDirection.Down:
                return new Vector2(0, -1);
            case MoveDirection.Left:
                return new Vector2(-1, 0);
            default:
                throw new ArgumentOutOfRangeException("dir");
        }
    }
}

public enum MoveDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}
