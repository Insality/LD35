
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss: LevelEntity
{

    public int Health;
    public Animation CircleAnimation;
    public Player Target;
    public Map Map;

    public BossAttack Style = BossAttack.Charge;
    public EnemyState State = EnemyState.Waiting;

    private int _chargeCounter = 0;
    public int _patternSwitch = 0;


    protected override void Start()
    {
        base.Start();
        Map = GameController.GetInstance().Map;
        CanStepOn = false;
        Target = GameController.GetInstance().Player;
        Style = BossAttack.Spawn;
    }

    public void SetRandomBossAttack()
    {
        Style = (BossAttack) (Random.Range(0, 3));
        if (Health <= 3)
        {
            Style = BossAttack.Charge;
        }
    }

    public override void OnPlayerEnter(MoveDirection dir)
    {
        base.OnPlayerEnter(dir);
        Damage();
        Move(dir);
        Move(dir);
        GameController.GetInstance().Player.MoveInvert(dir);
    }

    private void Damage()
    {
        SoundController.PlaySound(SoundType.BossDamage);
        Health--;
        if (Health < 0)
        {
            SetState(EnemyState.Die);
        }
    }

    private void SetState(EnemyState state)
    {
        State = state;
        if (state == EnemyState.Waiting)
        {
            _patternSwitch = (Health <= 3 ? 3 : 4);
        }
    }


    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);

        switch (State)
        {
            case EnemyState.Waiting:
                _patternSwitch--;
                if (_patternSwitch <= 0)
                {
                    _patternSwitch = 0;
                }

                if (Vector2.Distance(Coords, Target.Coords) < 5 && _patternSwitch == 0)
                {
                    _chargeCounter = 0;
                    SetRandomBossAttack();
                    SetState(EnemyState.Attacking);
                }
                else
                {
                    _chargeCounter++;
                    if (_chargeCounter > 1)
                    {
                        Move((MoveDirection) Random.Range(0, 4));

                        _chargeCounter = 0;
                    }
                }
                break;
            case EnemyState.Attacking:

                switch (Style)
                {
                    case BossAttack.Spawn:
                        _chargeCounter++;
                        CircleAnimation.Stop();
                        CircleAnimation.Play("SlowlyRotate");
                        if (_chargeCounter >= 3)
                        {
                            if (Vector2.Distance(Coords, Target.Coords) >= 10)
                            {
                                SetState(EnemyState.Waiting);
                            }
                            Map.AddItemInGame(Map.RabbitPrefab,
                                Coords + Player.GetDirection((MoveDirection) Random.Range(0, 4)));
                            _chargeCounter = 0;
                            SoundController.PlaySound(SoundType.RabbitSpawn);
                            SetState(EnemyState.Waiting);
                        }
                        break;
                    case BossAttack.Explo:
                        if (_chargeCounter == 0) SoundController.PlaySound(SoundType.BossCharge2);
                        _chargeCounter++;
                        CircleAnimation.Stop();
                        CircleAnimation.Play("SlowlyRotate");
                        Anim.Stop();
                        Anim.Play("SlowlyRotateLeft");
                        if (_chargeCounter >= 3)
                        {
                            if (Vector2.Distance(Coords, Target.Coords) >= 10)
                            {
                                SetState(EnemyState.Waiting);
                            }

                            if (_chargeCounter == 3)
                            {
                                Map.GetCell(new Vector2(Coords.x + 1, Coords.y + 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 1, Coords.y)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 1, Coords.y - 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x, Coords.y + 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x, Coords.y - 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 1, Coords.y + 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 1, Coords.y)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 1, Coords.y - 1)).SetState(CellState.Warning);
                            }
                            if (_chargeCounter >= 4)
                            {
                                Map.GetCell(new Vector2(Coords.x + 2, Coords.y + 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 2, Coords.y)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 2, Coords.y - 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x, Coords.y + 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x, Coords.y - 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 2, Coords.y + 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 2, Coords.y)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 2, Coords.y - 2)).SetState(CellState.Warning);

                                Map.GetCell(new Vector2(Coords.x - 1, Coords.y + 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 1, Coords.y + 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 1, Coords.y - 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 1, Coords.y - 2)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 2, Coords.y + 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 2, Coords.y + 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x - 2, Coords.y - 1)).SetState(CellState.Warning);
                                Map.GetCell(new Vector2(Coords.x + 2, Coords.y - 1)).SetState(CellState.Warning);
                                _chargeCounter = 0;
                                SetState(EnemyState.Waiting);
                            }

                        }
                        break;

                    case BossAttack.Charge:
                        if (_chargeCounter == 0) SoundController.PlaySound(SoundType.BossCharge1);
                        _chargeCounter++;
                        CircleAnimation.Stop();
                        CircleAnimation.Play("FastRotate");
                        if (_chargeCounter >= 3)
                        {
                            if (Vector2.Distance(Coords, Target.Coords) >= 10)
                            {
                                SetState(EnemyState.Waiting);
                            }

                            var direction = PathUtils.GetDirectionToTarget(Coords, Target.Coords);
                            for (int i = 0; i < 4; i++)
                            {
                                if (GameController.GetInstance()
                                    .Map.IsCanStep(Coords + Player.GetDirection(direction), direction))
                                {
                                    if (Move(direction))
                                    {
                                        break;
                                    }
                                }
                            }
                            _chargeCounter = 0;
                            SetState(EnemyState.Waiting);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case EnemyState.Die:
                SoundController.PlaySound(SoundType.EnemyHit);
                GameController.GetInstance().Map.DestroyItem(this);
                GameController.GetInstance().Player.Win();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool Move(MoveDirection dir)
    {
        GameController.GetInstance().Map.GetCell(Coords).SetState(CellState.Warning);

        if (GameController.GetInstance().Map.IsCanStep(Coords + Player.GetDirection(dir), dir))
        {
            var nextCoords = Coords + (Player.GetDirection(dir));
            Coords = nextCoords;
            SetPos();


            var player = GameController.GetInstance().Player;
            if (player.Coords.x == Coords.x && player.Coords.y == Coords.y)
            {
                player.Damage();
                Coords -= Player.GetDirection(dir);
                SetPos();
                return true;
            }
        }
        return false;
    }
}

public enum BossAttack
{
    Spawn = 0,
    Explo = 1,
    Charge = 2,
}