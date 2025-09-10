using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    public ObjectPool Pool;

    public void Init(ObjectPool pool)
    {
        Pool = pool;
        OnInit();
    }

    // ������ ��� ��������� ���������� ���� ��������� ��-������
    public abstract void ResetPlatform();

    protected void ReturnToPool()
    {
        if (Pool != null)
            Pool.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }

    // ��� ��� ���. ������������� (���� ����-�� �����)
    protected virtual void OnInit() { }
}
