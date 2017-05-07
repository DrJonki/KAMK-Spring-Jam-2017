using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private enum State
    {
        LookingAtEnemy,
        LookingAway,
    }
    private State m_state = State.LookingAway;
    private bool m_reachedGlass = false;
    private CameraController m_cameraController;
    private SpriteRenderer m_render;
    private float m_textureSwitchTimer = 0f;
    private int m_currentTexture = 0;
    
    public Collider glassCollider;
    public float armRotationSpeed = 10f;
    public float movementSpeed = 2f;
    public bool armsFollowMouse = false;
    public Sprite idleTexture;
    public Sprite[] sneakTextures = new Sprite[3];
    public float textureSwitchTime = 0.1f;
    
	void Start ()
    {
        m_cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        m_render = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
        setTexture(idleTexture);
        m_textureSwitchTimer = textureSwitchTime;
	}
	
	void Update ()
    {
        movement();
        armRotation();
	}
    
    private void movement()
    {
        if (!m_reachedGlass)
        {
            Vector3 pos = transform.position;
            float offset = Time.deltaTime * movementSpeed;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (m_state == State.LookingAway)
                {
                    setTexture(sneakTextures[0]);
                    m_textureSwitchTimer = textureSwitchTime;
                }
                else if ((m_textureSwitchTimer -= Time.deltaTime) <= 0f)
                {
                    if (++m_currentTexture >= sneakTextures.Length)
                        m_currentTexture = 0;

                    setTexture(sneakTextures[m_currentTexture]);
                    m_textureSwitchTimer = textureSwitchTime;
                }

                m_state = State.LookingAtEnemy;
                pos.x -= offset;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                m_state = State.LookingAway;
                setTexture(idleTexture);
                pos.x += offset / 2;
            }

            transform.position = pos;
            detectReach();
        }
    }

    private void armRotation()
    {
        if (armsFollowMouse)
        {
            Transform pivot = transform.FindChild("ArmPivot");
            Vector3 targetDir = m_cameraController.mousePoint() - pivot.position;

            pivot.rotation = Quaternion.Slerp(
                pivot.rotation,
                Quaternion.Euler(0, 0, -90) * Quaternion.LookRotation(targetDir, -Vector3.forward),
                Time.deltaTime * armRotationSpeed
            );
        }
    }

    private void detectReach()
    {
        GameObject arm = transform.FindChild("ArmPivot/Arm").gameObject;
        
        foreach (Collider coll in Physics.OverlapSphere(arm.transform.position, arm.GetComponent<SphereCollider>().radius * arm.transform.localScale.x))
        {
            if (coll.gameObject.name == "Glass")
            {
                m_cameraController.setZoom(true);
                m_reachedGlass = true;
            }
        }
    }

    private void setTexture(Sprite tex)
    {
        m_render.sprite = tex;
    }

    public bool lookingAtEnemy()
    {
        return m_state == State.LookingAtEnemy;
    }

    public bool atGlass()
    {
        return m_reachedGlass;
    }
}
