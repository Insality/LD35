using System;
using UnityEngine;

public class Hunter : LevelEntity
{

    public Player Target;
    public EnemyState State = EnemyState.Waiting;

    protected override void Start()
    {
        Target = GameController.GetInstance().Player;
    }

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);

        if (GameController.GetInstance().Map.GetCell(Coords).State == CellState.Damage)
        {
            SetState(EnemyState.Die);
            SoundController.PlaySound(SoundType.BossDamage);
        }

        switch (State)
        {
            case EnemyState.Waiting:
                if (Vector2.Distance(Coords, Target.Coords) < 5) {
                    SetState(EnemyState.Attacking);
                }
                break;
            case EnemyState.Attacking:
                if (Vector2.Distance(Coords, Target.Coords) >= 10) {
                    SetState(EnemyState.Waiting);
                }

                var direction = PathUtils.GetDirectionToTarget(Coords, Target.Coords);
                if (GameController.GetInstance().Map.IsCanStep(Coords + Player.GetDirection(direction), direction))
                {
                    Move(direction);
                }
                break;
            case EnemyState.Charge:
                break;
            case EnemyState.Die:
                GameController.GetInstance().Map.DestroyItem(this);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
       
        
    }

    private void Move(MoveDirection dir)
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
        }
    }

    private void SetState(EnemyState state)
    {
        State = state;
    }
}


public enum EnemyState
{
    Waiting = 0,
    Attacking = 1,
    Charge = 2,
    Die = 3
}