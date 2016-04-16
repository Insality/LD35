using UnityEngine;

public class Turret : LevelEntity
{
    public GameObject ProjectilePrefab;

    public int ShootPeriod = 4;
    public MoveDirection Direction = MoveDirection.Right;

    protected override void Start()
    {
        CanStepOn = false;
        Anim.AddClip(AppController.GetInstance().LittleResizeUp, "Resize");
    }

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);
        if (counter%ShootPeriod == 0)
        {
            Shoot();
            Anim.Play("Resize");
        }
    }

    private void Shoot()
    {
        var sc = Coords + Player.GetDirection(Direction);

        var projectile = Cell.Map.AddItem(ProjectilePrefab, sc).GetComponent<TurretProjectile>();
        projectile.Init(Direction);
    }
}
