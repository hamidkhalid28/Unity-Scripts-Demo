using UnityEngine;
using CnControls;
using System.Collections;
using YounGenTech.HealthScript;

public class PlayerController : MonoBehaviour 
{
	public float speed = 100;		//Speed by which our character will move.

	private bool isCharacterControllerAttached = false;
	private CharacterController character_controller = null;
	private Rigidbody rigid_body = null;
	private SimpleJoystick MovementJoystick;
	private float jumpForce = 50;

	public bool isBusy;


	// Use this for initialization
	void Awake () 
	{
		if(!MovementJoystick)
		{
			MovementJoystick = GameObject.FindGameObjectWithTag(Tags.Joystick).GetComponent<SimpleJoystick>();
		}


		if(gameObject.GetComponent<CharacterController>() != null)
		{
			isCharacterControllerAttached = true;
			character_controller = gameObject.GetComponent<CharacterController>();
		}
		else
		{
			isCharacterControllerAttached = false;
			rigid_body = GetComponentInChildren<Rigidbody>();
		}
	
	}
	
//	 Update is called once per frame
//	void Update () 
//	{
//
//	}

	void Update()
	{
		if(SharedVariables.isGamePlay)
		{
			if(MovementJoystick.isJoystickAlive && !isBusy)
			{
				float x = CnInputManager.GetAxis("Horizontal");
				float y = CnInputManager.GetAxis("Vertical");


				movePlayer(x,y);	//Function which will perform the duty of moving character.
				turnPlayer(x,y);	//Function which will perform the duty of turning character.
			}
		}
	}	

	void movePlayer(float x,float y)
	{

		if(isCharacterControllerAttached)
		{
			movePlayerThroughCharacterController(x,y);
		}
		else
		{
			movePlayerThroughRigidBody(x,y);
		}
	}

#region RIGIDBODY

	void turnPlayer(float x,float y)
	{
		Vector3 rotate_vector = new Vector3(transform.eulerAngles.x,(Mathf.Atan2(x,y) * Mathf.Rad2Deg)-90,transform.eulerAngles.z);
		transform.eulerAngles  = rotate_vector;

	}

	void movePlayerThroughRigidBody(float x,float y)
	{

//		rigid_body.MovePosition(transform.position - transform.forward * speed * Time.fixedDeltaTime );

		transform.Translate(-Vector3.forward * speed * Time.deltaTime);

	}
		

#endregion




/********************************************* Character Controller ************************************************/

	void movePlayerThroughCharacterController(float x, float y)
	{
		Vector3 move_vector = new Vector3(x,0,y);
		move_vector.y = 0;
		move_vector.Normalize();	//prepared the vector to move player.
		move_vector = -move_vector;
		
		character_controller.Move(move_vector * Time.deltaTime * speed);		//Moving Character.
	}

	public void Revive()
	{
		GetComponent<Health> ().Heal (new HealthEvent (gameObject, GetComponent<Health> ().MaxValue));
	}

	public void Jump()
	{
		rigid_body.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
	}
}
