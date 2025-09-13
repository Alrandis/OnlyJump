using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    public ObjectPool Pool { get; private set; }

    // ��������� �������� ������ ���������
    private GameObject platformRoot;

    public void Init(ObjectPool pool)
    {
        Pool = pool;
        platformRoot = GetRootObject();
        OnInit();
    }

    // �������� ������ ���������
    private GameObject GetRootObject()
    {
        // ���� ������ �� ����� � ����� ����
        if (transform.parent == null || transform.parent.GetComponent<ObjectPool>() == null)
            return gameObject;

        // ���� ���� �������� � ���� ������ ������ ����, ������� �� PoolManager
        Transform t = transform;
        while (t.parent != null && t.parent.GetComponent<ObjectPool>() == null)
            t = t.parent;

        return t.gameObject;
    }

    // ���������� ��������� ���������
    public abstract void ResetPlatform();

    // ������� � ���
    protected void ReturnToPool()
    {
        if (Pool != null)
            Pool.ReturnObject(platformRoot);
        else
            platformRoot.SetActive(false);
    }

    // ��� ��� �������������� �������������
    protected virtual void OnInit() { }

    // ��������� ���������
    public void Activate()
    {
        platformRoot.SetActive(true);
    }

    // ����������� ���������
    public void Deactivate()
    {
        platformRoot.SetActive(false);
    }
}
