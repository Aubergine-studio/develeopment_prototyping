using UnityEngine;

/// <summary>
///
/// </summary>
public class HashIDs : MonoBehaviour
{
    #region Statusy Animacji

    public int LocomotionState { get; private set; }

    public int LocomotionPivotLeftState { get; private set; }

    public int LocomotionPivotRightState { get; private set; }

    #endregion Statusy Animacji

    #region Statusy przejść

    public int LocomotionPivotTransitionLeftState { get; private set; }

    public int LocomotionPivotTransitionRightState { get; private set; }

    #endregion Statusy przejść

    #region Parametry animacji

    public int SpeedFloat { get; private set; }

    public int DirectionFloat { get; private set; }

    public int AngleFloat { get; private set; }

    #endregion Parametry animacji

    // Use this for initialization
    private void Awake()
    {
        #region Statusy Animacji

        LocomotionState = Animator.StringToHash("Base Layer.Locomation");
        LocomotionPivotLeftState = Animator.StringToHash("Base Layer.LocomotionPivotLeft");
        LocomotionPivotRightState = Animator.StringToHash("Base Layer.LocomotionPivotRight");

        #endregion Statusy Animacji

        #region Statusy przejść

        LocomotionPivotTransitionLeftState = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.LocomotionPivotLeft");
        LocomotionPivotTransitionRightState = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.LocomotionPivotRight");

        #endregion Statusy przejść

        #region Parametry animacji

        SpeedFloat = Animator.StringToHash("Speed");
        DirectionFloat = Animator.StringToHash("Direction");
        AngleFloat = Animator.StringToHash("Angle");

        #endregion Parametry animacji
    }
}