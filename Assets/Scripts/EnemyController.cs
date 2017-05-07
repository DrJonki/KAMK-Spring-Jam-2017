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
    private GameObject m_render;
    private PlayerController m_playerController;

    public float turnTime = 1f;
    public float randomTurnTimeMin = 0.5f;
    public float randomTurnTimeMax = 5.0f;
    public Sprite idleSprite2;
    public Sprite turningSprite;
    public Sprite watchingSprite;
    public Sprite idleSprite;

    void Start ()
    {
        m_render = GameObject.Find("Enemy/Sprite");
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        setTexture(idleSprite);
        
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
                    setTexture(idleSprite);
                }
            }

            else if (Random.Range(0f, 100f) < 0.4f)
            {
                setTexture(idleSprite2);
                m_idleTexTimer = 0.5f;
            }
        }

        if (!m_playerController.atGlass() && (m_stateTimer -= Time.deltaTime) <= 0f)
        {
            switch (m_state)
            {
                case State.LookingAtPlayer:
                    {
                        m_state = State.LookingAway;
                        Debug.Log("Looking away");
                        randomizeTurnTime();
                        setTexture(idleSprite);
                        invert();
                        break;
                    }
                case State.LookingAway:
                    {
                        m_state = State.Turning;
                        Debug.Log("Turning");
                        m_stateTimer = turnTime;
                        setTexture(turningSprite);
                        break;
                    }
                case State.Turning:
                    {
                        m_state = State.LookingAtPlayer;
                        Debug.Log("Watching");
                        randomizeTurnTime();
                        setTexture(idleSprite);
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

    private void setTexture(Sprite sprite)
    {
        m_render.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void invert()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void restoreScale()
    {
        Vector3 scale = transform.localScale;
        scale.x = 1f;
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
