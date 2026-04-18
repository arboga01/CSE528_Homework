using UnityEngine;
using TMPro; 

public class EnemyCounterScript : MonoBehaviour
{
    public TextMeshProUGUI counterText; 

    void Update()
    {
        
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

      
        counterText.text = "Enemies Left: " + enemyCount;
    }
}