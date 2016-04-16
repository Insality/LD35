using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float LifeTime;
    private float _curTime;

    void Update()
    {
        _curTime += Time.deltaTime;
        if (_curTime > LifeTime) Destroy(gameObject);
    }
	
}
