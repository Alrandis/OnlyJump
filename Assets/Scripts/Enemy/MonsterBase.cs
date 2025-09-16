using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    public ObjectPool Pool { get; private set; }

    private GameObject monsterRoot;

    public void Init(ObjectPool pool)
    {
        Pool = pool;
        monsterRoot = GetRootObject();
        OnInit();
    }

    private GameObject GetRootObject()
    {
        // ���� ������ �� ����� � ����� ����
        if (transform.parent == null || transform.parent.GetComponent<ObjectPool>() == null)
            return gameObject;

        // ���� ���� �������� � ���� ������ ������ ����, ������� �� ObjectPool
        Transform t = transform;
        while (t.parent != null && t.parent.GetComponent<ObjectPool>() == null)
            t = t.parent;

        return t.gameObject;
    }

    /// <summary>
    /// ����� ��������� ������� ��� �������� � ���
    /// </summary>
    public abstract void ResetMonster();

    /// <summary>
    /// ������� � ���
    /// </summary>
    protected void ReturnToPool()
    {
        if (Pool != null)
            Pool.ReturnObject(monsterRoot);
        else
            monsterRoot.SetActive(false);
    }

    /// <summary>
    /// �������������� ������������� (override � �����������)
    /// </summary>
    protected virtual void OnInit() { }

    /// <summary>
    /// ��������� ������� (��������, ��� ������)
    /// </summary>
    public void Activate()
    {
        monsterRoot.SetActive(true);
        ResetMonster();
    }

    /// <summary>
    /// ����������� ������� (��������, ��� ������)
    /// </summary>
    public void Deactivate()
    {
        monsterRoot.SetActive(false);
    }
}
