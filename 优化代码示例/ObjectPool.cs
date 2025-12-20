using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用对象池管理器 - 优化性能，避免频繁实例化和销毁
/// 使用方法：
/// 1. 将此脚本挂载到空物体上
/// 2. 设置 prefab 和 initialSize
/// 3. 使用 Get() 获取对象，使用 Return() 归还对象
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("对象池配置")]
    [Tooltip("要池化的预制件")]
    public GameObject prefab;

    [Tooltip("初始池大小")]
    public int initialSize = 20;

    [Tooltip("是否自动扩展")]
    public bool autoExpand = true;

    [Tooltip("最大池大小（0为无限制）")]
    public int maxSize = 0;

    // 对象池队列
    private Queue<GameObject> pool = new Queue<GameObject>();

    // 已激活的对象集合（用于追踪）
    private HashSet<GameObject> activeObjects = new HashSet<GameObject>();

    // 统计信息
    private int totalCreated = 0;
    private int totalReused = 0;

    void Start()
    {
        InitializePool();
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    private void InitializePool()
    {
        if (prefab == null)
        {
            Debug.LogError("ObjectPool: Prefab 未设置！");
            return;
        }

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }

        Debug.Log($"对象池初始化完成: {prefab.name}, 初始大小: {initialSize}");
    }

    /// <summary>
    /// 创建新对象并加入池中
    /// </summary>
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Enqueue(obj);
        totalCreated++;
        return obj;
    }

    /// <summary>
    /// 从对象池获取对象
    /// </summary>
    /// <param name="position">生成位置</param>
    /// <param name="rotation">生成旋转</param>
    /// <returns>池化的游戏对象</returns>
    public GameObject Get(Vector3 position = default, Quaternion rotation = default)
    {
        GameObject obj;

        // 如果池中有可用对象
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            totalReused++;
        }
        // 如果允许自动扩展且未达到最大限制
        else if (autoExpand && (maxSize == 0 || totalCreated < maxSize))
        {
            obj = CreateNewObject();
            Debug.LogWarning($"对象池扩展: {prefab.name}, 当前总数: {totalCreated}");
        }
        else
        {
            Debug.LogError($"对象池已满且不允许扩展: {prefab.name}");
            return null;
        }

        // 设置位置和旋转
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // 追踪激活的对象
        activeObjects.Add(obj);

        return obj;
    }

    /// <summary>
    /// 归还对象到池中
    /// </summary>
    /// <param name="obj">要归还的对象</param>
    public void Return(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("尝试归还空对象到对象池");
            return;
        }

        // 检查是否属于此池
        if (!activeObjects.Contains(obj))
        {
            Debug.LogWarning($"对象 {obj.name} 不属于此对象池");
            return;
        }

        // 重置对象状态
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // 移除追踪并加入池
        activeObjects.Remove(obj);
        pool.Enqueue(obj);
    }

    /// <summary>
    /// 归还所有激活的对象
    /// </summary>
    public void ReturnAll()
    {
        // 创建副本避免在迭代时修改集合
        List<GameObject> objectsToReturn = new List<GameObject>(activeObjects);

        foreach (GameObject obj in objectsToReturn)
        {
            Return(obj);
        }

        Debug.Log($"归还了 {objectsToReturn.Count} 个对象到池中");
    }

    /// <summary>
    /// 获取对象池统计信息
    /// </summary>
    public void PrintStats()
    {
        Debug.Log($"=== 对象池统计 [{prefab.name}] ===");
        Debug.Log($"总创建数: {totalCreated}");
        Debug.Log($"复用次数: {totalReused}");
        Debug.Log($"池中可用: {pool.Count}");
        Debug.Log($"正在使用: {activeObjects.Count}");
        Debug.Log($"复用率: {(totalReused / (float)(totalCreated + totalReused) * 100):F2}%");
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear()
    {
        ReturnAll();

        while (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            Destroy(obj);
        }

        totalCreated = 0;
        totalReused = 0;

        Debug.Log($"对象池已清空: {prefab.name}");
    }

    void OnDestroy()
    {
        PrintStats();
    }

#if UNITY_EDITOR
    /// <summary>
    /// Inspector中显示调试信息
    /// </summary>
    [Header("调试信息（运行时）")]
    [SerializeField] private int debugPoolCount;
    [SerializeField] private int debugActiveCount;
    [SerializeField] private int debugTotalCreated;

    void Update()
    {
        debugPoolCount = pool.Count;
        debugActiveCount = activeObjects.Count;
        debugTotalCreated = totalCreated;
    }
#endif
}

/* 使用示例：

// 1. 在场景中创建对象池
GameObject poolObj = new GameObject("StickmanPool");
ObjectPool pool = poolObj.AddComponent<ObjectPool>();
pool.prefab = stickmanPrefab;
pool.initialSize = 50;

// 2. 获取对象
GameObject stickman = pool.Get(spawnPosition, Quaternion.identity);

// 3. 归还对象（而不是销毁）
pool.Return(stickman);

// 4. 查看统计
pool.PrintStats();

*/
