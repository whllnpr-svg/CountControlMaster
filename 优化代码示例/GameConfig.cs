using UnityEngine;

/// <summary>
/// 游戏配置数据 - ScriptableObject
/// 用于集中管理游戏参数，方便调整和平衡
///
/// 创建方法：
/// 右键 → Create → Config → Game Config
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game Config", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("=== 玩家设置 ===")]
    [Tooltip("玩家移动速度")]
    [Range(1f, 20f)]
    public float playerSpeed = 5f;

    [Tooltip("道路移动速度")]
    [Range(1f, 20f)]
    public float roadSpeed = 5f;

    [Tooltip("初始火柴人数量")]
    [Range(1, 50)]
    public int startingStickmanCount = 10;

    [Tooltip("最大火柴人数量（性能限制）")]
    [Range(50, 500)]
    public int maxStickmanCount = 200;

    [Header("=== 移动范围设置 ===")]
    [Tooltip("少量人数时的移动范围")]
    public float smallGroupMoveRange = 1.1f;

    [Tooltip("大量人数时的移动范围")]
    public float largeGroupMoveRange = 0.7f;

    [Tooltip("切换移动范围的人数阈值")]
    [Range(20, 100)]
    public int moveRangeThreshold = 50;

    [Header("=== 编队设置 ===")]
    [Tooltip("编队距离系数（控制队伍紧密度）")]
    [Range(0.1f, 3f)]
    public float distanceFactor = 1f;

    [Tooltip("螺旋角度半径")]
    [Range(0.1f, 5f)]
    public float radius = 1f;

    [Tooltip("编队动画时间")]
    [Range(0.1f, 2f)]
    public float formationAnimationDuration = 0.5f;

    [Header("=== 门系统设置 ===")]
    [Tooltip("加法门最小值")]
    [Range(5, 50)]
    public int minAddGate = 10;

    [Tooltip("加法门最大值")]
    [Range(20, 200)]
    public int maxAddGate = 100;

    [Tooltip("乘法门最小倍数")]
    [Range(1, 3)]
    public int minMultiplyGate = 1;

    [Tooltip("乘法门最大倍数")]
    [Range(2, 5)]
    public int maxMultiplyGate = 3;

    [Tooltip("加法门数值必须为偶数")]
    public bool addGateMustBeEven = true;

    [Header("=== 敌人设置 ===")]
    [Tooltip("敌人最小数量")]
    [Range(10, 50)]
    public int minEnemyCount = 20;

    [Tooltip("敌人最大数量")]
    [Range(50, 200)]
    public int maxEnemyCount = 120;

    [Tooltip("敌人生成随机种子（0为随机）")]
    public int enemySeed = 0;

    [Header("=== 对战设置 ===")]
    [Tooltip("攻击范围")]
    [Range(0.5f, 5f)]
    public float attackRange = 1.5f;

    [Tooltip("对战速度（每秒消耗人数）")]
    [Range(1, 100)]
    public int battleSpeed = 30;

    [Tooltip("攻击时玩家移动速度")]
    [Range(0.1f, 10f)]
    public float attackMoveSpeed = 2f;

    [Tooltip("攻击时道路速度")]
    [Range(0f, 5f)]
    public float attackRoadSpeed = 0f;

    [Header("=== 摄像机设置 ===")]
    [Tooltip("小队伍摄像机偏移")]
    public Vector3 smallGroupCameraOffset = new Vector3(0, 5, -4);

    [Tooltip("大队伍摄像机偏移")]
    public Vector3 largeGroupCameraOffset = new Vector3(0, 8, -8);

    [Tooltip("摄像机切换人数阈值")]
    [Range(20, 100)]
    public int cameraThreshold = 50;

    [Tooltip("摄像机平滑速度")]
    [Range(0.1f, 10f)]
    public float cameraSmoothSpeed = 2f;

    [Header("=== 塔楼设置 ===")]
    [Tooltip("每行最大火柴人数")]
    [Range(3, 15)]
    public int maxPlayerPerRow = 7;

    [Tooltip("水平间距")]
    [Range(0f, 3f)]
    public float towerXGap = 1f;

    [Tooltip("垂直间距")]
    [Range(0f, 3f)]
    public float towerYGap = 1f;

    [Tooltip("Y轴偏移")]
    [Range(0f, 20f)]
    public float towerYOffset = 5f;

    [Tooltip("塔楼构建延迟（秒）")]
    [Range(0.05f, 1f)]
    public float towerBuildDelay = 0.2f;

    [Header("=== 粒子效果设置 ===")]
    [Tooltip("血液粒子持续时间")]
    [Range(0.5f, 5f)]
    public float bloodParticleDuration = 1f;

    [Tooltip("血液粒子数量")]
    [Range(10, 100)]
    public int bloodParticleCount = 30;

    [Header("=== 难度设置 ===")]
    [Tooltip("难度系数（影响敌人数量）")]
    [Range(0.5f, 3f)]
    public float difficultyMultiplier = 1f;

    [Tooltip("是否启用难度递增")]
    public bool enableProgressiveDifficulty = false;

    [Tooltip("每关难度增长率")]
    [Range(0f, 0.5f)]
    public float difficultyIncreaseRate = 0.1f;

    [Header("=== 性能设置 ===")]
    [Tooltip("是否启用对象池")]
    public bool enableObjectPooling = true;

    [Tooltip("对象池初始大小")]
    [Range(20, 200)]
    public int objectPoolInitialSize = 50;

    [Tooltip("是否启用优化编队")]
    public bool enableOptimizedFormation = true;

    [Tooltip("目标帧率")]
    [Range(30, 120)]
    public int targetFrameRate = 60;

    [Header("=== 调试设置 ===")]
    [Tooltip("显示调试信息")]
    public bool showDebugInfo = false;

    [Tooltip("显示编队Gizmos")]
    public bool showFormationGizmos = false;

    [Tooltip("显示攻击范围")]
    public bool showAttackRange = false;

    /// <summary>
    /// 根据关卡计算难度
    /// </summary>
    public float GetDifficultyForLevel(int level)
    {
        if (!enableProgressiveDifficulty)
            return difficultyMultiplier;

        return difficultyMultiplier * (1 + (level - 1) * difficultyIncreaseRate);
    }

    /// <summary>
    /// 根据关卡计算敌人数量
    /// </summary>
    public int GetEnemyCountForLevel(int level)
    {
        float difficulty = GetDifficultyForLevel(level);
        int baseCount = Random.Range(minEnemyCount, maxEnemyCount);
        return Mathf.RoundToInt(baseCount * difficulty);
    }

    /// <summary>
    /// 验证配置合法性
    /// </summary>
    public void ValidateConfig()
    {
        if (playerSpeed <= 0) playerSpeed = 5f;
        if (roadSpeed <= 0) roadSpeed = 5f;
        if (distanceFactor <= 0) distanceFactor = 1f;
        if (radius <= 0) radius = 1f;

        if (minAddGate > maxAddGate)
        {
            int temp = minAddGate;
            minAddGate = maxAddGate;
            maxAddGate = temp;
        }

        if (minEnemyCount > maxEnemyCount)
        {
            int temp = minEnemyCount;
            minEnemyCount = maxEnemyCount;
            maxEnemyCount = temp;
        }

        Debug.Log("配置验证完成");
    }

    /// <summary>
    /// 重置为默认值
    /// </summary>
    [ContextMenu("重置为默认值")]
    public void ResetToDefaults()
    {
        playerSpeed = 5f;
        roadSpeed = 5f;
        distanceFactor = 1f;
        radius = 1f;
        minAddGate = 10;
        maxAddGate = 100;
        minEnemyCount = 20;
        maxEnemyCount = 120;

        Debug.Log("已重置为默认配置");
    }

    void OnValidate()
    {
        // 确保最小值不大于最大值
        if (minAddGate > maxAddGate) maxAddGate = minAddGate;
        if (minMultiplyGate > maxMultiplyGate) maxMultiplyGate = minMultiplyGate;
        if (minEnemyCount > maxEnemyCount) maxEnemyCount = minEnemyCount;
    }
}

/* 使用示例：

// 1. 创建配置文件
// 右键 → Create → Config → Game Config

// 2. 在脚本中引用
public class PlayerManager : MonoBehaviour
{
    public GameConfig config;

    void Start()
    {
        playerSpeed = config.playerSpeed;
        DistanceFactor = config.distanceFactor;
        Radius = config.radius;
    }
}

// 3. 在Inspector中拖拽配置文件到 config 字段

// 优点：
// - 集中管理所有参数
// - 可以创建多个配置（简单/困难）
// - 运行时可以切换配置
// - 便于团队协作和平衡调整

*/
