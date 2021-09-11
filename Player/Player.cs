using Godot;
using System;

public class Player : KinematicBody2D
{
	const int MAX_SPEED = 80;
	const int acceleration = 500;
	const int FRICTION = 500;
	AnimationPlayer animationPlayer = null;
	AnimationTree animationTree = null;
	AnimationNodeStateMachinePlayback animationState=null;

	Vector2 velocity = new Vector2(0, 0);
	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		var inputVector = new Vector2();
		inputVector.x = Input.GetActionStrength("ui_right")
			- Input.GetActionStrength("ui_left");
		inputVector.y = Input.GetActionStrength("ui_down")
			- Input.GetActionStrength("ui_up");
		inputVector = inputVector.Normalized();
		if (inputVector.Length() > 0)
		{
			animationTree.Set("parameters/Idle/blend_position", inputVector);
			animationTree.Set("parameters/Run/blend_position", inputVector);
			animationState.Travel("Run");
			velocity += inputVector * acceleration * delta;
			velocity = velocity.Clamped(MAX_SPEED);
		}
		else
		{
			animationState.Travel("Idle");
			velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);

		}
		//GD.Print(velocity);
		//velocity = inputVector.Length() > 0 ? inputVector*acceleration*delta : Vector2.Zero;

		velocity = MoveAndSlide(velocity);
	}

	public override void _Ready()
	{
		base._Ready();
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationState = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}
}
