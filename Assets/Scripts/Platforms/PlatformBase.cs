using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    public ObjectPool Pool { get; private set; }
    protected GameObject platformRoot;

    protected Vector3 initialPosition;
    protected Quaternion initialRotation;
    protected Vector3 initialScale;

    protected virtual void Awake()
    {
        platformRoot = GetRootObject();

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;
    }

    public void Init(ObjectPool pool)
    {
        Pool = pool;
        OnInit();
    }

    private GameObject GetRootObject()
    {
        if (transform.parent == null || transform.parent.GetComponent<ObjectPool>() == null)
            return gameObject;

        Transform t = transform;
        while (t.parent != null && t.parent.GetComponent<ObjectPool>() == null)
            t = t.parent;

        return t.gameObject;
    }

    public virtual void ResetPlatform()
    {
        StopAllCoroutines();

        transform.localRotation = initialRotation;
        transform.localScale = initialScale;
    }

    protected void ReturnToPool()
    {
        StopAllCoroutines();
        if (Pool != null)
            Pool.ReturnObject(platformRoot);
        else
            platformRoot.SetActive(false);
    }

    public void Activate()
    {
        platformRoot.SetActive(true);
    }

    public void Deactivate()
    {
        platformRoot.SetActive(false);
    }

    protected virtual void OnInit() { }
}
