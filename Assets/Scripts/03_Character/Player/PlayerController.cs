using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public bool MoveFlag = true;

	public GameObject m_CameraPivot;
	public GameObject m_CharacterPivot;
	public const float speed = 11.5f;

	RaycastHit floorHit;
	//hold
	private Vector3 m_CamPivot = new Vector3(1,0,1);
	private Vector3 m_CameraDefultPos;

	// Use this for initialization
	void Start () {
		m_CameraDefultPos = m_CameraPivot.transform.localPosition;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (MoveFlag) { 
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= Time.fixedDeltaTime;

            moveDirection *= speed;

            moveDirection += transform.position;

            transform.position = moveDirection;

			Vector3 CamForward = Vector3.Scale(m_CameraPivot.transform.forward,m_CamPivot).normalized;
			Vector3 Movement = Input.GetAxis("Vertical") * CamForward + Input.GetAxis("Horizontal") * m_CameraPivot.transform.right;

			Vector3 InvTranDirMovement = transform.InverseTransformDirection(Movement);

			if(InvTranDirMovement != Vector3.zero)
			{
				m_CharacterPivot.transform.localRotation = Quaternion.Lerp(m_CharacterPivot.transform.localRotation, Quaternion.LookRotation(transform.InverseTransformDirection(Movement),Vector3.up),0.2f);
			}

			Vector3 rayPosition = transform.position;
			Vector3 rayPositionDir = transform.InverseTransformDirection(0,-1,0);
			if(Physics.Raycast(rayPosition,rayPositionDir,out floorHit, 300))
			{
				Vector3 ChangePosition = new Vector3(0,floorHit.point.y,0);
				m_CharacterPivot.transform.localPosition = ChangePosition;
				if(m_CameraDefultPos != null){
					m_CameraPivot.transform.localPosition = m_CameraDefultPos + ChangePosition;
				}
			}
			


        }

    }
}
