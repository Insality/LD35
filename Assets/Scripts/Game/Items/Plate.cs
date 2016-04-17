using UnityEngine;

public class Plate : LevelEntity
{
    public bool IsEnabled = false;

    public override void OnGameBeat(int counter)
    {
        base.OnGameBeat(counter);
        IsEnabled = false;
        foreach (var item in Cell.Map.Items)
        {
            if (item.Coords.x == Coords.x && item.Coords.y == Coords.y)
            {
                IsEnabled = true;
            }
        }

        Graphics.SetSprite(IsEnabled ? "PlateOn" : "PlateOff");
    }

}
