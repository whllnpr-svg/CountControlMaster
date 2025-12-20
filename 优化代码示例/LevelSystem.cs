using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 关卡系统 - 管理多个关卡的数据和进度
/// </summary>
[System.Serializable]
public class LevelData
{
    [Header("基本信息")]
    public int levelNumber;
    public string levelName;
    public string sceneName;

    [Header("难度设置")]
    [Range(0.5f, 3f)]
    public float difficultyMultiplier = 1f;

    [Header("起始配置")]
    public int startStickmanCount = 10;
    public int minEnemyCount = 30;
    public int maxEnemyCount = 80;

    [Header("奖励")]
    public int scoreMultiplier = 1;
    public int coinReward = 100;

    [Header("解锁条件")]
    public bool isLocked = true;
    public int requiredLevel = 0;  // 需要完成的前置关卡

    [Header("统计")]
    public int bestScore = 0;
    public int playCount = 0;
    public bool completed = false;
    public float bestTime = float.MaxValue;
}

/// <summary>
/// 关卡管理器
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("关卡列表")]
    public List<LevelData> levels = new List<LevelData>();

    [Header("当前关卡")]
    public int currentLevel = 0;

    [Header("玩家进度")]
    public int unlockedLevel = 1;  // 最高解锁关卡
    public int totalScore = 0;
    public int totalCoins = 0;

    private float levelStartTime;

    void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 监听游戏事件
        GameEvents.OnGameStart += OnLevelStart;
        GameEvents.OnGameWin += OnLevelComplete;
        GameEvents.OnGameLose += OnLevelFailed;
    }

    void OnDestroy()
    {
        GameEvents.OnGameStart -= OnLevelStart;
        GameEvents.OnGameWin -= OnLevelComplete;
        GameEvents.OnGameLose -= OnLevelFailed;
    }

    /// <summary>
    /// 加载指定关卡
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError($"关卡索引越界: {levelIndex}");
            return;
        }

        LevelData level = levels[levelIndex];

        // 检查是否解锁
        if (level.isLocked && levelIndex > unlockedLevel)
        {
            Debug.LogWarning($"关卡 {levelIndex} 未解锁");
            GameEvents.ShowMessage("关卡未解锁", 2f);
            return;
        }

        currentLevel = levelIndex;
        Debug.Log($"加载关卡 {levelIndex}: {level.levelName}");

        // 加载场景
        if (!string.IsNullOrEmpty(level.sceneName))
        {
            SceneManager.LoadScene(level.sceneName);
        }
        else
        {
            SceneManager.LoadScene("Level");  // 默认场景
        }
    }

    /// <summary>
    /// 加载下一关
    /// </summary>
    public void LoadNextLevel()
    {
        int nextLevel = currentLevel + 1;

        if (nextLevel >= levels.Count)
        {
            Debug.Log("已完成所有关卡！");
            GameEvents.ShowMessage("恭喜通关！", 3f);
            // 返回主菜单或显示完成界面
            SceneManager.LoadScene("Menu");
            return;
        }

        LoadLevel(nextLevel);
    }

    /// <summary>
    /// 重新开始当前关卡
    /// </summary>
    public void RestartLevel()
    {
        LoadLevel(currentLevel);
    }

    /// <summary>
    /// 返回关卡选择
    /// </summary>
    public void ReturnToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    /// <summary>
    /// 关卡开始回调
    /// </summary>
    private void OnLevelStart()
    {
        levelStartTime = Time.time;
        LevelData level = GetCurrentLevelData();
        if (level != null)
        {
            level.playCount++;
        }
    }

    /// <summary>
    /// 关卡完成回调
    /// </summary>
    private void OnLevelComplete(int score)
    {
        float levelTime = Time.time - levelStartTime;
        LevelData level = GetCurrentLevelData();

        if (level != null)
        {
            // 更新统计
            level.completed = true;

            // 更新最佳分数
            if (score > level.bestScore)
            {
                level.bestScore = score;
                Debug.Log($"新纪录！分数: {score}");
            }

            // 更新最快时间
            if (levelTime < level.bestTime)
            {
                level.bestTime = levelTime;
                Debug.Log($"新纪录！时间: {levelTime:F2}秒");
            }

            // 计算奖励
            int earnedCoins = level.coinReward;
            totalCoins += earnedCoins;
            totalScore += score;

            // 解锁下一关
            int nextLevel = currentLevel + 1;
            if (nextLevel < levels.Count && nextLevel > unlockedLevel)
            {
                unlockedLevel = nextLevel;
                levels[nextLevel].isLocked = false;
                Debug.Log($"解锁关卡 {nextLevel}: {levels[nextLevel].levelName}");
            }

            // 保存进度
            SaveProgress();

            // 显示完成信息
            Debug.Log($"关卡完成！分数: {score}, 金币: {earnedCoins}, 时间: {levelTime:F2}秒");
        }
    }

    /// <summary>
    /// 关卡失败回调
    /// </summary>
    private void OnLevelFailed()
    {
        Debug.Log("关卡失败");
        // 可以在这里添加失败处理逻辑
    }

    /// <summary>
    /// 获取当前关卡数据
    /// </summary>
    public LevelData GetCurrentLevelData()
    {
        if (currentLevel >= 0 && currentLevel < levels.Count)
        {
            return levels[currentLevel];
        }
        return null;
    }

    /// <summary>
    /// 获取关卡数据
    /// </summary>
    public LevelData GetLevelData(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            return levels[levelIndex];
        }
        return null;
    }

    /// <summary>
    /// 检查关卡是否解锁
    /// </summary>
    public bool IsLevelUnlocked(int levelIndex)
    {
        if (levelIndex >= levels.Count) return false;
        return levelIndex <= unlockedLevel;
    }

    /// <summary>
    /// 保存进度
    /// </summary>
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
        PlayerPrefs.SetInt("TotalScore", totalScore);
        PlayerPrefs.SetInt("TotalCoins", totalCoins);

        // 保存每个关卡的数据
        for (int i = 0; i < levels.Count; i++)
        {
            LevelData level = levels[i];
            PlayerPrefs.SetInt($"Level_{i}_BestScore", level.bestScore);
            PlayerPrefs.SetFloat($"Level_{i}_BestTime", level.bestTime);
            PlayerPrefs.SetInt($"Level_{i}_PlayCount", level.playCount);
            PlayerPrefs.SetInt($"Level_{i}_Completed", level.completed ? 1 : 0);
        }

        PlayerPrefs.Save();
        Debug.Log("进度已保存");
    }

    /// <summary>
    /// 加载进度
    /// </summary>
    public void LoadProgress()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // 加载每个关卡的数据
        for (int i = 0; i < levels.Count; i++)
        {
            LevelData level = levels[i];
            level.bestScore = PlayerPrefs.GetInt($"Level_{i}_BestScore", 0);
            level.bestTime = PlayerPrefs.GetFloat($"Level_{i}_BestTime", float.MaxValue);
            level.playCount = PlayerPrefs.GetInt($"Level_{i}_PlayCount", 0);
            level.completed = PlayerPrefs.GetInt($"Level_{i}_Completed", 0) == 1;

            // 设置解锁状态
            level.isLocked = i > unlockedLevel;
        }

        Debug.Log($"进度已加载，解锁到关卡 {unlockedLevel}");
    }

    /// <summary>
    /// 重置所有进度
    /// </summary>
    [ContextMenu("重置进度")]
    public void ResetProgress()
    {
        unlockedLevel = 1;
        totalScore = 0;
        totalCoins = 0;

        foreach (LevelData level in levels)
        {
            level.bestScore = 0;
            level.bestTime = float.MaxValue;
            level.playCount = 0;
            level.completed = false;
            level.isLocked = level.levelNumber > 1;
        }

        SaveProgress();
        Debug.Log("进度已重置");
    }

    /// <summary>
    /// 解锁所有关卡（调试用）
    /// </summary>
    [ContextMenu("解锁所有关卡")]
    public void UnlockAllLevels()
    {
        unlockedLevel = levels.Count;
        foreach (LevelData level in levels)
        {
            level.isLocked = false;
        }
        SaveProgress();
        Debug.Log("已解锁所有关卡");
    }

    /// <summary>
    /// 创建默认关卡
    /// </summary>
    [ContextMenu("创建默认关卡")]
    public void CreateDefaultLevels()
    {
        levels.Clear();

        for (int i = 1; i <= 10; i++)
        {
            LevelData level = new LevelData
            {
                levelNumber = i,
                levelName = $"关卡 {i}",
                sceneName = "Level",
                difficultyMultiplier = 1 + (i - 1) * 0.15f,
                startStickmanCount = 10,
                minEnemyCount = 20 + i * 5,
                maxEnemyCount = 60 + i * 10,
                scoreMultiplier = i,
                coinReward = 100 * i,
                isLocked = i > 1,
                requiredLevel = i - 1
            };

            levels.Add(level);
        }

        Debug.Log($"创建了 {levels.Count} 个默认关卡");
    }

    /// <summary>
    /// 获取关卡统计信息
    /// </summary>
    public string GetLevelStats(int levelIndex)
    {
        LevelData level = GetLevelData(levelIndex);
        if (level == null) return "";

        return $"关卡 {level.levelNumber}: {level.levelName}\n" +
               $"最佳分数: {level.bestScore}\n" +
               $"最快时间: {(level.bestTime < float.MaxValue ? level.bestTime.ToString("F2") + "秒" : "未完成")}\n" +
               $"游玩次数: {level.playCount}\n" +
               $"状态: {(level.completed ? "已完成" : "未完成")}";
    }
}

/* ==================== 使用示例 ====================

// 1. 在场景中创建 LevelManager
GameObject managerObj = new GameObject("LevelManager");
LevelManager manager = managerObj.AddComponent<LevelManager>();

// 2. 在 Inspector 中配置关卡列表，或使用代码创建
manager.CreateDefaultLevels();

// 3. 在菜单中加载关卡
public class LevelSelectUI : MonoBehaviour
{
    public void OnLevelButtonClick(int levelIndex)
    {
        LevelManager.Instance.LoadLevel(levelIndex);
    }

    public void OnNextLevelClick()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    public void OnRestartClick()
    {
        LevelManager.Instance.RestartLevel();
    }
}

// 4. 显示关卡信息
public class LevelInfoUI : MonoBehaviour
{
    public Text levelNameText;
    public Text statsText;

    void Start()
    {
        LevelData level = LevelManager.Instance.GetCurrentLevelData();
        if (level != null)
        {
            levelNameText.text = level.levelName;
            statsText.text = LevelManager.Instance.GetLevelStats(level.levelNumber - 1);
        }
    }
}

// 5. 在 PlayerManager 中应用关卡配置
void Start()
{
    LevelData level = LevelManager.Instance.GetCurrentLevelData();
    if (level != null)
    {
        numberOfStickmans = level.startStickmanCount;
        // 应用其他配置...
    }
}

// 6. 在 enemyManager 中使用关卡难度
void Start()
{
    LevelData level = LevelManager.Instance.GetCurrentLevelData();
    if (level != null)
    {
        int enemyCount = Random.Range(level.minEnemyCount, level.maxEnemyCount);
        enemyCount = Mathf.RoundToInt(enemyCount * level.difficultyMultiplier);
        // 生成敌人...
    }
}

*/
