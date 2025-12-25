using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            // 检查 PlayerManager 的 road 字段指向哪个对象
            PlayerManager pm = FindObjectOfType<PlayerManager>();
            if (pm != null)
            {
                // 使用反射获取 road 字段
                var roadField = typeof(PlayerManager).GetField("road",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

                if (roadField != null)
                {
                    Transform roadTransform = (Transform)roadField.GetValue(pm);
                    if (roadTransform != null)
                    {
                        Debug.Log($"====================================");
                        Debug.Log($"PlayerManager.road 指向: {roadTransform.name}");
                        Debug.Log($"完整路径: {GetGameObjectPath(roadTransform.gameObject)}");
                        Debug.Log($"当前位置: {roadTransform.position}");
                        Debug.Log($"====================================");
                    }
                    else
                    {
                        Debug.LogWarning("PlayerManager.road 字段为空！");
                    }
                }
            }

            // 检查所有顶层对象的位置
            Debug.Log("\n===== 检查主要对象位置 =====");
            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name == "Gates" || obj.name == "Road" || obj.name == "Player")
                {
                    Debug.Log($"{obj.name} 位置: {obj.transform.position}");
                }
            }
            Debug.Log("========================\n");
        }
    }

    string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }
}
