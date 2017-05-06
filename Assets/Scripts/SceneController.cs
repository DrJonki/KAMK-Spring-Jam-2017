using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    private PlayerController m_playerController;
    private EnemyController m_enemyController;
    private Text m_timeTextComponent;
    private float m_timer = 0f;
    
	void Start ()
    {
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        m_enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
        m_timeTextComponent = GameObject.Find("TimeText").GetComponent<Text>();
    }
	
	void Update ()
    {
		if (m_enemyController.lookingAtPlayer() && m_playerController.lookingAtEnemy() && !m_playerController.atGlass())
        {
            Debug.Log("Defeat");
            SceneManager.LoadScene("FailScene");
        }

        m_timeTextComponent.text = "Time: " + (m_timer += Time.deltaTime).ToString("0.0");
	}
}
