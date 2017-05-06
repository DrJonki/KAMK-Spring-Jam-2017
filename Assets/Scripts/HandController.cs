using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    private PlayerController m_playerController;
    private CameraController m_cameraController;
    private float m_activateCounter = 2f;
    private GameObject m_hand;
    private Vector3 m_initialPos;

    public float movementSpeed = 10f;
    
	void Start ()
    {
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        m_cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        m_hand = transform.FindChild("Hand").gameObject;
        m_initialPos = transform.position;
    }
	
	void Update ()
    {
	    if (m_playerController.atGlass() && m_cameraController.isZoom() && (m_activateCounter -= Time.deltaTime) <= 0f)
        {
            m_hand.SetActive(true);

            Vector3 pos = Vector3.Slerp(transform.position, m_cameraController.mousePoint(), Time.deltaTime);
            pos.z = transform.position.z;
            transform.position = pos;
        }
        else
        {
            m_hand.SetActive(false);
            transform.position = m_initialPos;
        }
	}
}
