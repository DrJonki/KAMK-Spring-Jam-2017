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
    
    public Collider glassCollider;
    public float armRotationSpeed = 10f;
    public float movementSpeed = 2f;
    
	void Start ()
    {
        m_cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
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

            if (Input.GetKey(KeyCode.A))
            {
                m_state = State.LookingAtEnemy;
                pos.x -= offset;
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_state = State.LookingAway;
                pos.x += offset;
            }

            transform.position = pos;
            detectReach();
        }
    }

    private void armRotation()
    {
        Transform pivot = transform.FindChild("ArmPivot");
        Vector3 targetDir = m_cameraController.mousePoint() - pivot.position;

        pivot.rotation = Quaternion.Slerp(
            pivot.rotation,
            Quaternion.Euler(0, 0, -90) * Quaternion.LookRotation(targetDir, -Vector3.forward),
            Time.deltaTime * armRotationSpeed
        );
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

    public bool lookingAtEnemy()
    {
        return m_state == State.LookingAtEnemy;
    }

    public bool atGlass()
    {
        return m_reachedGlass;
    }
}
