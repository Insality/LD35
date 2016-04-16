using UnityEngine;

public class PathUtils  {

    public static MoveDirection GetDirectionToTarget(Vector2 from, Vector2 target)
    {
        var result = MoveDirection.Right;
        var dX = target.x - from.x;
        var dY = target.y - from.y;

        if (Mathf.Abs(dX) >= Mathf.Abs(dY))
        {
            if (dX < 0) return MoveDirection.Left;
            return MoveDirection.Right;
        }
        if (dY < 0) return MoveDirection.Down;
        return MoveDirection.Up;
    }

}
