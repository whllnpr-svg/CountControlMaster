using System;
using UnityEngine;

/// <summary>
/// 游戏事件系统 - 解耦脚本间的依赖关系
/// 使用观察者模式，提高代码可维护性
/// </summary>
public static class GameEvents
{
    // ==================== 玩家事件 ====================

    /// <summary>
    /// 火柴人数量变化事件
    /// 参数：当前数量
    /// </summary>
    public static event Action<int> OnStickmanCountChanged;

    /// <summary>
    /// 玩家移动事件
    /// 参数：新位置
    /// </summary>
    public static event Action<Vector3> OnPlayerMoved;

    /// <summary>
    /// 玩家编队完成事件
    /// </summary>
    public static event Action OnFormationComplete;

    // ==================== 战斗事件 ====================

    /// <summary>
    /// 战斗开始事件
    /// 参数：敌人管理器
    /// </summary>
    public static event Action<GameObject> OnBattleStart;

    /// <summary>
    /// 战斗结束事件
    /// 参数：是否胜利
    /// </summary>
    public static event Action<bool> OnBattleEnd;

    /// <summary>
    /// 单位被击败事件
    /// 参数：被击败的单位, 是否为玩家单位
    /// </summary>
    public static event Action<GameObject, bool> OnUnitDefeated;

    // ==================== 门系统事件 ====================

    /// <summary>
    /// 通过门事件
    /// 参数：门类型（加法/乘法）, 数值
    /// </summary>
    public static event Action<bool, int> OnGatePassed;

    /// <summary>
    /// 增加单位事件
    /// 参数：增加的数量
    /// </summary>
    public static event Action<int> OnUnitsAdded;

    // ==================== 游戏流程事件 ====================

    /// <summary>
    /// 游戏开始事件
    /// </summary>
    public static event Action OnGameStart;

    /// <summary>
    /// 游戏暂停事件
    /// 参数：是否暂停
    /// </summary>
    public static event Action<bool> OnGamePause;

    /// <summary>
    /// 游戏胜利事件
    /// 参数：最终分数
    /// </summary>
    public static event Action<int> OnGameWin;

    /// <summary>
    /// 游戏失败事件
    /// </summary>
    public static event Action OnGameLose;

    /// <summary>
    /// 到达终点事件
    /// </summary>
    public static event Action OnReachFinish;

    // ==================== 塔楼事件 ====================

    /// <summary>
    /// 塔楼开始构建事件
    /// 参数：总层数
    /// </summary>
    public static event Action<int> OnTowerBuildStart;

    /// <summary>
    /// 塔楼构建进度事件
    /// 参数：当前层, 总层数
    /// </summary>
    public static event Action<int, int> OnTowerBuildProgress;

    /// <summary>
    /// 塔楼构建完成事件
    /// </summary>
    public static event Action OnTowerBuildComplete;

    // ==================== UI事件 ====================

    /// <summary>
    /// 显示消息事件
    /// 参数：消息内容, 持续时间
    /// </summary>
    public static event Action<string, float> OnShowMessage;

    /// <summary>
    /// 更新分数事件
    /// 参数：新分数
    /// </summary>
    public static event Action<int> OnScoreUpdated;

    // ==================== 音效事件 ====================

    /// <summary>
    /// 播放音效事件
    /// 参数：音效名称
    /// </summary>
    public static event Action<string> OnPlaySound;

    /// <summary>
    /// 播放音乐事件
    /// 参数：音乐名称
    /// </summary>
    public static event Action<string> OnPlayMusic;

    // ==================== 事件触发方法 ====================

    // 玩家事件
    public static void StickmanCountChanged(int count)
    {
        OnStickmanCountChanged?.Invoke(count);
        LogEvent($"火柴人数量: {count}");
    }

    public static void PlayerMoved(Vector3 position)
    {
        OnPlayerMoved?.Invoke(position);
    }

    public static void FormationComplete()
    {
        OnFormationComplete?.Invoke();
        LogEvent("编队完成");
    }

    // 战斗事件
    public static void BattleStart(GameObject enemy)
    {
        OnBattleStart?.Invoke(enemy);
        LogEvent("战斗开始");
    }

    public static void BattleEnd(bool victory)
    {
        OnBattleEnd?.Invoke(victory);
        LogEvent($"战斗结束: {(victory ? "胜利" : "失败")}");
    }

    public static void UnitDefeated(GameObject unit, bool isPlayer)
    {
        OnUnitDefeated?.Invoke(unit, isPlayer);
    }

    // 门系统事件
    public static void GatePassed(bool isMultiply, int value)
    {
        OnGatePassed?.Invoke(isMultiply, value);
        LogEvent($"通过门: {(isMultiply ? "x" : "+")}{value}");
    }

    public static void UnitsAdded(int count)
    {
        OnUnitsAdded?.Invoke(count);
        LogEvent($"增加单位: {count}");
    }

    // 游戏流程事件
    public static void GameStart()
    {
        OnGameStart?.Invoke();
        LogEvent("游戏开始");
    }

    public static void GamePause(bool isPaused)
    {
        OnGamePause?.Invoke(isPaused);
        LogEvent($"游戏{(isPaused ? "暂停" : "继续")}");
    }

    public static void GameWin(int score)
    {
        OnGameWin?.Invoke(score);
        LogEvent($"游戏胜利! 分数: {score}");
    }

    public static void GameLose()
    {
        OnGameLose?.Invoke();
        LogEvent("游戏失败");
    }

    public static void ReachFinish()
    {
        OnReachFinish?.Invoke();
        LogEvent("到达终点");
    }

    // 塔楼事件
    public static void TowerBuildStart(int totalLayers)
    {
        OnTowerBuildStart?.Invoke(totalLayers);
        LogEvent($"开始建塔: {totalLayers}层");
    }

    public static void TowerBuildProgress(int currentLayer, int totalLayers)
    {
        OnTowerBuildProgress?.Invoke(currentLayer, totalLayers);
    }

    public static void TowerBuildComplete()
    {
        OnTowerBuildComplete?.Invoke();
        LogEvent("塔楼建造完成");
    }

    // UI事件
    public static void ShowMessage(string message, float duration = 2f)
    {
        OnShowMessage?.Invoke(message, duration);
    }

    public static void ScoreUpdated(int score)
    {
        OnScoreUpdated?.Invoke(score);
    }

    // 音效事件
    public static void PlaySound(string soundName)
    {
        OnPlaySound?.Invoke(soundName);
    }

    public static void PlayMusic(string musicName)
    {
        OnPlayMusic?.Invoke(musicName);
    }

    // ==================== 调试功能 ====================

    private static bool enableEventLogging = false;

    public static void SetEventLogging(bool enable)
    {
        enableEventLogging = enable;
    }

    private static void LogEvent(string eventName)
    {
        if (enableEventLogging)
        {
            Debug.Log($"[事件] {eventName}");
        }
    }

    /// <summary>
    /// 清空所有事件监听器（场景切换时调用）
    /// </summary>
    public static void ClearAllEvents()
    {
        OnStickmanCountChanged = null;
        OnPlayerMoved = null;
        OnFormationComplete = null;
        OnBattleStart = null;
        OnBattleEnd = null;
        OnUnitDefeated = null;
        OnGatePassed = null;
        OnUnitsAdded = null;
        OnGameStart = null;
        OnGamePause = null;
        OnGameWin = null;
        OnGameLose = null;
        OnReachFinish = null;
        OnTowerBuildStart = null;
        OnTowerBuildProgress = null;
        OnTowerBuildComplete = null;
        OnShowMessage = null;
        OnScoreUpdated = null;
        OnPlaySound = null;
        OnPlayMusic = null;

        Debug.Log("所有事件已清空");
    }
}

/* ==================== 使用示例 ====================

// 1. 发送事件（在 PlayerManager.cs 中）
void OnTriggerEnter(Collider other)
{
    if (other.tag == "gate")
    {
        GateManager gate = other.GetComponent<GateManager>();
        GameEvents.GatePassed(gate.multiply, gate.randomNumber);

        // 增加单位后
        GameEvents.StickmanCountChanged(numberOfStickmans);
        GameEvents.UnitsAdded(gate.randomNumber);
    }
}

// 2. 监听事件（在 UIManager.cs 中）
void OnEnable()
{
    // 订阅事件
    GameEvents.OnStickmanCountChanged += UpdateStickmanCountUI;
    GameEvents.OnGatePassed += ShowGateEffect;
    GameEvents.OnGameWin += ShowWinScreen;
}

void OnDisable()
{
    // 取消订阅（重要！避免内存泄漏）
    GameEvents.OnStickmanCountChanged -= UpdateStickmanCountUI;
    GameEvents.OnGatePassed -= ShowGateEffect;
    GameEvents.OnGameWin -= ShowWinScreen;
}

void UpdateStickmanCountUI(int count)
{
    countText.text = count.ToString();
}

void ShowGateEffect(bool isMultiply, int value)
{
    string message = isMultiply ? $"x{value}" : $"+{value}";
    ShowFloatingText(message);
}

void ShowWinScreen(int score)
{
    winPanel.SetActive(true);
    scoreText.text = $"Score: {score}";
}

// 3. 监听事件（在 AudioManager.cs 中）
void OnEnable()
{
    GameEvents.OnGatePassed += (isMultiply, value) => PlaySound("gate_pass");
    GameEvents.OnBattleStart += (enemy) => PlaySound("battle_start");
    GameEvents.OnGameWin += (score) => PlayMusic("victory");
}

// 4. 监听事件（在 EffectManager.cs 中）
void OnEnable()
{
    GameEvents.OnUnitDefeated += (unit, isPlayer) =>
    {
        Vector3 pos = unit.transform.position;
        SpawnBloodEffect(pos);
    };
}

// ==================== 优点 ====================

1. 解耦：脚本之间不需要直接引用
2. 可扩展：轻松添加新的事件监听器
3. 维护性：修改一个模块不影响其他模块
4. 调试：启用日志可以追踪所有事件
5. 性能：事件只在有监听器时才执行

// ==================== 注意事项 ====================

1. 必须在 OnDisable 中取消订阅，避免内存泄漏
2. 场景切换时调用 ClearAllEvents()
3. 避免在事件处理中执行耗时操作
4. 可以使用 Lambda 表达式简化代码

*/
