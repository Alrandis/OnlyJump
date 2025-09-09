using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    protected ObjectPool _pool;

    public void Init(ObjectPool pool)
    {
        _pool = pool;
        OnInit();
    }

    // ������ ��� ��������� ���������� ���� ��������� ��-������
    public abstract void ResetPlatform();

    protected void ReturnToPool()
    {
        if (_pool != null)
            _pool.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }

    // ��� ��� ���. ������������� (���� ����-�� �����)
    protected virtual void OnInit() { }
}
