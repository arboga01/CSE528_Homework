using UnityEngine;
using TMPro; // Only needed if you want to change text via code

public class GameWinManager : MonoBehaviour
{
    public GameObject winPanel; 
    public string enemyTag = "Enemy";
    private bool hasWon = false;

    void Update()
    {
      
        if (hasWon) return;

   
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);


        if (enemies.Length == 0)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        hasWon = true;
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

       
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

       
    }
}