using UnityEngine;

public class PlayerInputMenager : InputMenager
{
    public override void CollectInputs()
    {
        LeftAnalog.GetInputs();
        RightAnalog.GetInputs();
    }

    public override void ControlPhysicsActions()
    {
        (CharacterController as ThirdPersonPlayerController).ThirdPersonMovePhysics(LeftAnalog);
    }

    public override void ControlActions()
    {
        (CharacterController as ThirdPersonPlayerController).ThirdPersonMove(LeftAnalog);
    }

}