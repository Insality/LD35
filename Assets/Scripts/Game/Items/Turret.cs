using UnityEngine;

public class Turret : LevelEntity
{
    public GameObject ProjectilePrefab;

    public int ShootPeriod = 4;
    public MoveDirection Direction = MoveDirection.Right;

    void Start()
    {
        CanStepOn = false;
    }

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);
        if (counter%ShootPeriod == 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var sc = Coords + Player.GetDirection(Direction);
        TurretProjectile projectile = Instantiate(ProjectilePrefab).GetComponent<TurretProjectile>();

        projectile.MyTransform.parent = MyTransform.parent;
        projectile.Coords = sc;
        projectile.SetPosInstantly();
        projectile.Init(Direction);

        Cell.Map.Items.Add(projectile);
    }
}
