using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private enum State
    {
        LookingAway,
        Turning,
        LookingAtPlayer,
    }
    private State m_state = State.LookingAway;
    private float m_turnCounter = 0f;

    public float randomTurnTimeMin = 0.5f;
    public float randomTurnTimeMax = 5.0f;
    
    void Start ()
    {
        randomizeTurnTime();
	}
	
	void Update ()
    {
        if (turning())
        {

        }

        if ((m_turnCounter -= Time.deltaTime) <= 0f)
        {
            m_state = State.Turning;
        }
	}

    private void randomizeTurnTime()
    {
        m_turnCounter = Random.Range(randomTurnTimeMin, randomTurnTimeMax);
    }

    public bool lookingAtPlayer()
    {
        return m_state == State.LookingAtPlayer;
    }

    public bool turning()
    {
        return m_state == State.Turning;
    }
}
