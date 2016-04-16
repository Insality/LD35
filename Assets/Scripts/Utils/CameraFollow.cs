using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _myTransform;
    public int ZCoord = -20;
    public Transform Target;

    void Start()
    {
        _myTransform = transform;
    }

    void Update()
    {
        Vector2 velocity = new Vector3();
        if (Target != null)
        {

            var v = Vector2.SmoothDamp(_myTransform.position, Target.position, ref velocity, 0.05f);
            
            _myTransform.position = new Vector3(v.x, v.y, ZCoord);
        }
    }
}
