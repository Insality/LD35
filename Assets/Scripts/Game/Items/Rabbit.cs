using UnityEngine;
using Random = UnityEngine.Random;

public class Rabbit: LevelEntity
{
    public Player Target;
    public EnemyState State = EnemyState.Waiting;

    private int _chargeCounter = 0;

    protected override void Start()
    {
        base.Start();
        CanStepOn = false;
        Target = GameController.GetInstance().Player;
        Anim.AddClip(AppController.GetInstance().LittleResizeUp, "charge");
    }

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);

        switch (State)
        {
            case EnemyState.Waiting:
                if (Vector2.Distance(Coords, Target.Coords) < 5)
                {
                    SetState(EnemyState.Charge);
                }
                break;
            case EnemyState.Attacking:
                if (Vector2.Distance(Coords, Target.Coords) >= 10)
                {
                    SetState(EnemyState.Waiting);
                }

                var jumpCount = Random.Range(2, 4);
                var direction = PathUtils.GetDirectionToTarget(Coords, Target.Coords);

                Graphics.gameObject.transform.localRotation = Quaternion.LookRotation(Vector3.forward, Player.GetDirection(direction));

                for (int i = 0; i < jumpCount; i++)
                {
                    if (GameController.GetInstance().Map.IsCanStep(Coords + Player.GetDirection(direction), direction))
                    {
                        Move(direction);
                    }
                }
                SetState(EnemyState.Charge);
                break;
            case EnemyState.Charge:
                _chargeCounter++;
                Anim.Play("charge");
                if (_chargeCounter >= 2)
                {
                    SetState(EnemyState.Attacking);
                    _chargeCounter = 0;
                }
                break;
            case EnemyState.Die:
                SoundController.PlaySound(SoundType.BossDamage);
                GameController.GetInstance().Map.DestroyItem(this);
                break;
        }
    }

    private void Move(MoveDirection dir)
    {
        GameController.GetInstance().Map.GetCell(Coords).SetState(CellState.Warning);

        var nextCoords = Coords + (Player.GetDirection(dir));
        Coords = nextCoords;
        SetPos();
        

        var player = GameController.GetInstance().Player;
        if (player.Coords.x == Coords.x && player.Coords.y == Coords.y)
        {
            player.Damage();
            Coords -= Player.GetDirection(dir);
            SetPos();
        }
    }

    public override void OnPlayerEnter(MoveDirection dir)
    {
        base.OnPlayerEnter(dir);
        SetState(EnemyState.Die);
    }

    private void SetState(EnemyState state)
    {
        State = state;
    }
}
