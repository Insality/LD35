using UnityEngine;

// The gameobject shaker for shake effects
// Author: Insality

public class Shaker : MonoBehaviour
{
    public Transform ShakeParentObject;
    public Transform[] Transforms;
    public float ShakePower;

    private float _shakeTime = 0;
    private Transform[] _parents;
     

    public void Shake(float time)
    {
        if (_shakeTime > 0) return;

        _shakeTime = time;
        SaveParents();
    }

    #region Implementation

    private void StopShake()
    {
        ShakeParentObject.localPosition = Vector3.zero;
        _shakeTime = 0;
    }

    private void SaveParents()
    {
        _parents = new Transform[Transforms.Length];
        for (int i = 0; i < _parents.Length; i++)
        {
            _parents[i] = Transforms[i].parent;
        }
    }

    private void LoadParents()
    {
        for (int i = 0; i < Transforms.Length; i++)
        {
            Transforms[i].parent = _parents[i];
        }
    }

    private void Update()
    {
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
            ShakeParentObject.localPosition = new Vector3(Random.Range(-ShakePower, ShakePower),
                Random.Range(-ShakePower, ShakePower), 0);
            if (_shakeTime <= 0)
            {
                StopShake();
                LoadParents();
            }
        }
    }

    #endregion
}
