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
    private float m_idleTexTimer = 0.5f;

    public float turnTime = 1f;
    public float randomTurnTimeMin = 0.5f;
    public float randomTurnTimeMax = 5.0f;
    public Texture idleTexture;
    public Texture idleTexture2;
    public Texture turningTexture;
    public Texture watchingTexture;

    void Start ()
    {
        randomizeTurnTime();
	}
	
	void Update ()
    {
        if (m_state == State.LookingAway)
        {
            if (m_idleTexTimer > 0f)
            {
                if ((m_idleTexTimer -= Time.deltaTime) <= 0f)
                {
                    setTexture(idleTexture);
                }
            }

            else if (Random.Range(0f, 100f) < 0.4f)
            {
                setTexture(idleTexture2);
                m_idleTexTimer = 0.5f;
            }
        }

        if ((m_stateTimer -= Time.deltaTime) <= 0f)
        {
            switch (m_state)
            {
                case State.LookingAtPlayer:
                    {
                        m_state = State.LookingAway;
                        Debug.Log("Looking away");
                        randomizeTurnTime();
                        setTexture(idleTexture);
                        invert();
                        break;
                    }
                case State.LookingAway:
                    {
                        m_state = State.Turning;
                        Debug.Log("Turning");
                        m_stateTimer = turnTime;
                        setTexture(turningTexture);
                        break;
                    }
                case State.Turning:
                    {
                        m_state = State.LookingAtPlayer;
                        Debug.Log("Watching");
                        randomizeTurnTime();
                        setTexture(idleTexture);
                        invert();
                        break;
                    }
            }
        }
	}

    private void randomizeTurnTime()
    {
        m_stateTimer = Random.Range(randomTurnTimeMin, randomTurnTimeMax);
    }

    private void setTexture(Texture tex)
    {
        GetComponent<MeshRenderer>().material.mainTexture = tex;
    }

    private void invert()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
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
