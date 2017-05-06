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
    private float m_stateTimer = 0f;

    public float turnTime = 1f;
    public float randomTurnTimeMin = 0.5f;
    public float randomTurnTimeMax = 5.0f;
    
    void Start ()
    {
        randomizeTurnTime();
	}
	
	void Update ()
    {
        if ((m_stateTimer -= Time.deltaTime) <= 0f)
        {
            switch (m_state)
            {
                case State.LookingAtPlayer:
                    {
                        m_state = State.LookingAway;
                        Debug.Log("Looking away");
                        randomizeTurnTime();
                        break;
                    }
                case State.LookingAway:
                    {
                        m_state = State.Turning;
                        Debug.Log("Turning");
                        m_stateTimer = turnTime;
                        break;
                    }
                case State.Turning:
                    {
                        m_state = State.LookingAtPlayer;
                        Debug.Log("Watching");
                        randomizeTurnTime();
                        break;
                    }
            }
        }
	}

    private void randomizeTurnTime()
    {
        m_stateTimer = Random.Range(randomTurnTimeMin, randomTurnTimeMax);
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
