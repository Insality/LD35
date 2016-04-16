using UnityEngine;

public class CameraRotationFollow : MonoBehaviour {
    public Transform Target;

    private Transform _myTransform;

    void Start()
    {
        _myTransform = transform;
    }

    private void Update() {
        _myTransform.localRotation = Quaternion.Slerp(_myTransform.localRotation, Target.localRotation, 0.2f);
//        var tmpPos = Target.localPosition/2;
//        tmpPos.z = -10;
//        _myTransform.localPosition = tmpPos;
    }
}