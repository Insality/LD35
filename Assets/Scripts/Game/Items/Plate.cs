using UnityEngine;

public class Plate : LevelEntity
{
    private bool _isEnabled = false;

    public override void OnPlayerEnter(MoveDirection dir)
    {
        base.OnPlayerEnter(dir);
        if (!_isEnabled) { 
            _isEnabled = true;
            Graphics.SetSprite("PlateOn");
        }
    }

    public override void OnPlayerLeave()
    {
        base.OnPlayerLeave();
        if (_isEnabled) {
            _isEnabled = false;
            Graphics.SetSprite("PlateOff");
        }
    }

}
