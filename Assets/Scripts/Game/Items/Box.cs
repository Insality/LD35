public class Box : LevelEntity {
    protected override void Start()
    {
        base.Start();
        CanStepOn = false;
    }

    public override void OnPlayerEnter(MoveDirection dir)
    {
        base.OnPlayerEnter(dir);
        var map = GameController.GetInstance().Map;
        if (map.IsCanStep(Coords + Player.GetDirection(dir), dir))
        {
            Move(dir);
        }
    }

    private void Move(MoveDirection dir)
    {
        var map = GameController.GetInstance().Map;
        var nextCoords = Coords + (Player.GetDirection(dir));
        map.MoveToEvent(nextCoords, dir);
        map.MoveFromEvent(Coords, dir);

        Coords = nextCoords;
        var canStep = true;
        foreach (var item in map.Items)
        {
            if (item.Coords.x == Coords.x && item.Coords.y == Coords.y)
            {
                if (!item.CanStepOn && item != this)
                {
                    canStep = false;
                }
            }
        }
        if (canStep)
        {
            SetPos();
        }
        else
        {
//            GameController.GetInstance().Player.MoveInvert(dir);
            Coords -= Player.GetDirection(dir);
        }
    }
}
