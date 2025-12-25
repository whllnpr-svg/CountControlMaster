using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GateManager : MonoBehaviour
{
    public TextMeshPro GateNo;
    public int randomNumber;
    public bool multiply;
    void Start()
    {
        if (multiply)
        {
            randomNumber = Random.Range(1, 3);
            GateNo.text = "X" + randomNumber;
            Debug.Log($"门已创建：乘法门 x{randomNumber}");
        }
        else
        {
            randomNumber = Random.Range(10, 100);

            if (randomNumber % 2 != 0)
                randomNumber += 1;

            GateNo.text = randomNumber.ToString();
            Debug.Log($"门已创建：加法门 +{randomNumber}");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"门被触发！碰撞对象：{other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家通过门！");
        }
    }
    
}
