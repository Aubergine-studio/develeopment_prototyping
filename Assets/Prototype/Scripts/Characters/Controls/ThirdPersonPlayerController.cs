using UnityEngine;

[RequireComponent(typeof(PlayerInputMenager))]
public class ThirdPersonPlayerController : CharacterController
{
    public Transform PlayerCamera { get; private set; }

    public int PlayerNumber { get; private set; }

    public AnimatorStateInfo AnimatorStateInfo { get; protected set; }

    public AnimatorTransitionInfo AnimatorTransitionInfo { get; protected set; }


    /// <summary>
    /// Wywoływana przy stworzeniu obiektu gracza.
    ///     - Rejestruje gracza
    /// </summary>
    private new void Start()
    {
        base.Start();
        PlayerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
        HashIDs = FindObjectOfType<HashIDs>();

        var split = name.Split('_');
        PlayerNumber = int.Parse(split[1]);
        PlayerManager.Instance.RegisterPlayer(gameObject);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="leftAnalog"></param>
    public void ThirdPersonMove(Analog leftAnalog)
    {
        AnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorTransitionInfo = Animator.GetAnimatorTransitionInfo(0);
        Direction = Angle = 0;

        var isInPivot = IsInPivot();
        //Debug.Log(isInPivot);
        leftAnalog.AnalogToWorldSpace(DirectionSpeed, transform, PlayerCamera, ref Direction, ref Speed, isInPivot, ref Angle);

        Animator.SetFloat(HashIDs.SpeedFloat, Speed, SpeedDampTime, Time.deltaTime);
        Animator.SetFloat(HashIDs.DirectionFloat, Direction, DirectionDampTime, Time.deltaTime);

        if (Speed > LocomationThreshold && !isInPivot)
            Animator.SetFloat(HashIDs.AngleFloat, Angle);
        //Debug.Log(Direction.ToString());
        if (!(Speed < LocomationThreshold) || !(Mathf.Abs(leftAnalog.Horizontal) < 0.05)) return;

        Animator.SetFloat(HashIDs.AngleFloat, 0);
        Animator.SetFloat(HashIDs.DirectionFloat, 0);
    }

    public void ThirdPersonMovePhysics(Analog leftAnalog)
    {
        if ((AnimatorStateInfo.fullPathHash == HashIDs.LocomotionState) &&
            (Direction > 0 && leftAnalog.Horizontal > 0) || (Direction < 0 && leftAnalog.Horizontal < 0))
        {
            var rotationAmount = Vector3.Lerp(Vector3.zero,
                new Vector3(0f, RotationDegreePerSeckend * (leftAnalog.Horizontal < 0f ? -1f : 1f), 0f),
                Mathf.Abs(leftAnalog.Horizontal));
            var deltaRatation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            Transform.rotation = Transform.rotation * deltaRatation;
        }
    }

    /// <summary>
    /// Poruszanie się postaci wykorzystywane w projektach 2D.
    /// </summary>
    /// <param name="direction2D">
    /// Kierunek:
    ///     - Prawo
    ///     - Lewo
    /// </param>
    /// <param name="spead">
    /// Prędkość z jaką porusza się postać.
    /// </param>
    public void Move2D(float spead)
    {
        if (Rigidbody != null)
            Rigidbody.velocity = new Vector3((MaxSpead * spead), Rigidbody.velocity.y);
        else
            Rigidbody2D.velocity = new Vector3((MaxSpead * spead), Rigidbody2D.velocity.y);
    }

    public bool IsInLocomotion()
    {
        if (HashIDs == null) return false;
        return AnimatorStateInfo.nameHash == HashIDs.LocomotionState;
    }

    public bool IsInPivot()
    {
        if (HashIDs == null) return false;
        return AnimatorStateInfo.nameHash == HashIDs.LocomotionPivotLeftState
            || AnimatorStateInfo.nameHash == HashIDs.LocomotionPivotRightState
            || AnimatorTransitionInfo.nameHash == HashIDs.LocomotionPivotTransitionLeftState
            || AnimatorTransitionInfo.nameHash == HashIDs.LocomotionPivotTransitionRightState;
    }
}