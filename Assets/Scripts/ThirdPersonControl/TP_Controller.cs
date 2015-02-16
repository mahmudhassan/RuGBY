using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour
{
	public static CharacterController CharacterController;
	public static TP_Controller Instance;

	//public bool ClimbEnabled { get; set; }

	void Awake() 
	{
		if (networkView.isMine)
		{
			CharacterController = GetComponent("CharacterController") as CharacterController;
		}
		Instance = this;
	}
	
	void Update() 
	{
		if (networkView.isMine)
		{
			if (Camera.main == null)
				return;
			
			GetLocomotionInput();
			HandleActionInput();
			
			TP_Motor.Instance.UpdateMotor();
		}
		
	}

	
	void GetLocomotionInput()
	{
		if (TP_Animator.Instance.MoveDirection == TP_Animator.Direction.Locked)
			return;
		var deadzone = 0.1f;
		
		TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
		TP_Motor.Instance.MoveVector = Vector3.zero;
		
		if (Input.GetAxis("Vertical") > deadzone || Input.GetAxis("Vertical") < -deadzone)
			TP_Motor.Instance.MoveVector += new Vector3(0, 0, Input.GetAxis("Vertical"));
		
		if (Input.GetAxis("Horizontal") > deadzone || Input.GetAxis("Horizontal") < -deadzone)
			TP_Motor.Instance.MoveVector += new Vector3(Input.GetAxis("Horizontal"),0, 0);
		
		TP_Animator.Instance.DetermineCurrentMoveDirection();
	}
	
	void HandleActionInput()
	{
		if (Input.GetButton("Jump"))
		{
			//if (ClimbEnabled)
			//	Climb();
			//else
				Jump();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			Use();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Run();
		}

		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			Walk();
		}

		if (Input.GetMouseButtonDown(0))
		{
			Attack();
		}

		if (Input.GetMouseButtonDown(1))
		{
			if (TP_Animator.Instance.State == TP_Animator.CharacterState.Attacking)
				SmashAttack();
			else if (TP_Animator.Instance.State != TP_Animator.CharacterState.Defending)
				Defend();
		}

		if (Input.GetMouseButtonUp (1) && TP_Animator.Instance.IsDefending) 
		{
			EndDefend();
		} 

		if (Input.GetKeyDown(KeyCode.F1)) 
		{
			Reset();
		}

		if (Input.GetKeyDown(KeyCode.F2)) 
		{
			Die();
		}

	}

	void Use()
	{
		TP_Animator.Instance.Use();
	}
	
	public void Jump()
	{
		TP_Motor.Instance.Jump();
		TP_Animator.Instance.Jump();
	}

	//void Climb()
	//{
		//TP_Animator.Instance.Climb();
	//}

	void Run()
	{
		if (TP_Animator.Instance.MoveDirection == TP_Animator.Direction.Stationary ||
			TP_Animator.Instance.MoveDirection == TP_Animator.Direction.Locked)
			return;
		TP_Animator.Instance.Run();
	}

	public void Walk()
	{
		TP_Animator.Instance.Walk();
	}

	void Attack()
	{
		Debug.Log ("Made it to attack statement");
		if (TP_Animator.Instance.ComboCounter >= 3 || !CharacterController.isGrounded)
			return;
		if (!TP_Animator.Instance.EndAttack && !TP_Animator.Instance.IsSmashing) 
		{
			TP_Animator.Instance.IsAttacking = true;
			TP_Animator.Instance.Attack();
			Debug.Log("Attack Occured");
		}
	}

	void SmashAttack()
	{
		if (!TP_Animator.Instance.EndAttack && 
		    TP_Animator.Instance.State == TP_Animator.CharacterState.Attacking &&
		    !TP_Animator.Instance.IsSmashing)
		{
			TP_Animator.Instance.IsSmashing = true;
			TP_Animator.Instance.IsAttacking = true;
			TP_Animator.Instance.SmashAttack();
			Debug.Log("Smash Occured");
		}
	}

	void Defend()
	{
		if (!CharacterController.isGrounded)
			return;
		TP_Animator.Instance.Defend();
	}

	void EndDefend()
	{
		TP_Animator.Instance.EndDefend();
	}

	public void Reset()
	{
		TP_Animator.Instance.Reset();
	}

	public void Die()
	{
		TP_Animator.Instance.Die();
	}
}