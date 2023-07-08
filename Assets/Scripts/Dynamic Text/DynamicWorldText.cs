using UnityEngine;
using UnityEngine.Pool;

public class DynamicWorldText : MonoBehaviour
{
    public static DynamicWorldText Instance;
    public IObjectPool<DynamicText> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new ObjectPool<DynamicText>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, maxPoolSize);
            }
            return m_Pool;
        }
    }

    [SerializeField] private int maxPoolSize = 50;
    [SerializeField] private DynamicText textPrefab;

    private IObjectPool<DynamicText> m_Pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            ShowActionText(Vector3.up + new Vector3(24, 0, -6), Color.yellow, "+50", 1);
            ShowActionText(Vector3.up + new Vector3(27, 0, 0), Color.green, "+100", 1);
            ShowActionText(Vector3.up + new Vector3(22, 0, 6), Color.red, "+20", 1);
        }
    }
#endif

    public void ShowActionText(Vector3 position, Color color, string text, float animTime)
    {
        DynamicText textObject = Pool.Get();
        textObject.SetValues(color, text);
        textObject.StartAnimation(position, animTime);
    }

    #region Pooling

    DynamicText CreatePooledItem()
    {
        DynamicText textObject = Instantiate(textPrefab);
        return textObject;
    }

    void OnReturnedToPool(DynamicText text)
    {
        text.gameObject.SetActive(false);
    }

    void OnTakeFromPool(DynamicText text)
    {
        text.gameObject.SetActive(true);
    }

    void OnDestroyPoolObject(DynamicText text)
    {
        Destroy(text.gameObject);
    }

    #endregion
}
