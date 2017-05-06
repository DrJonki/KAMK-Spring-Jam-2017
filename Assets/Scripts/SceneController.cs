using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    private PlayerController m_playerController;
    private EnemyController m_enemyController;
    
	void Start ()
    {
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        m_enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();
    }
	
	void Update ()
    {
		if (m_enemyController.lookingAtPlayer() && m_playerController.lookingAtEnemy())
        {
            Application.Quit();
        }
	}
}
