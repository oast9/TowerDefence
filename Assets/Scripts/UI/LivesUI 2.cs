using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
   public Text livesPlayerText;
   public Text livesEnemyText;
    void Update()
    {
        livesPlayerText.text = PlayerStats.LivesPlayer.ToString();
        livesEnemyText.text = PlayerStats.LivesEnemy.ToString();
    }
}
