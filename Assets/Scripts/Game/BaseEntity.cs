using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    private Transform _cachedTransform;
    public Vector2 Coords;
    private Vector3 _targetPos;

    public Transform MyTransform
    {
        get{
            if (_cachedTransform == null)
            {
                _cachedTransform = transform;
            }
            return _cachedTransform;
        }
    }
    public tk2dSprite Graphics;


    public void SetPos()
    {
        SetPos((int)Coords.x, (int)Coords.y);
    }

    public void SetPos(int i, int j)
    {
        Coords = new Vector2(i, j);
        var z = MyTransform.localPosition.z;
        _targetPos  = new Vector3(i * Constants.CellSize + (i * Constants.CellGap), j * Constants.CellSize + (j * Constants.CellGap), z);
    }

    public void SetPosInstantly()
    {
        SetPos();
        MyTransform.localPosition = _targetPos;
    }

    protected virtual void Update()
    {
        Vector3 velocity = new Vector3();
        MyTransform.localPosition = Vector3.SmoothDamp(MyTransform.localPosition, _targetPos, ref velocity, 0.05f);
    }

}
