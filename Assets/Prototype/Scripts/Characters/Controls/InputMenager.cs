using System;
using UnityEngine;

/// <summary>
///
/// </summary>
public enum InputSource
{
    KeyboardAndMouse,
    GamePad1,
    GamePad2,
    GamePad3,
    GamePad4
}

public enum AnalogStick
{
    Left,
    Right
}

/// <summary>
///
/// </summary>
[Serializable]
public class Analog
{
    public InputSource InputSource { get; set; }

    public AnalogStick AnalogStick { get; private set; }

    public bool InverVertical = true;

    public float Horizontal { get; private set; }

    public float Vertical { get; private set; }

    public Vector3 Reading
    {
        get
        {
            return (Vertical * Vector3.forward) + (Horizontal * Vector3.right);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public Analog(AnalogStick analogStick)
    {
        Horizontal = Vertical = 0;
        AnalogStick = analogStick;
    }

    /// <summary>
    ///
    /// </summary>
    public void GetInputs()
    {
        Horizontal = Input.GetAxis("Horizontal" + AnalogStick + InputSource);
        Vertical = Input.GetAxis("Vertical" + AnalogStick + InputSource);

        if (InverVertical) Vertical *= -1;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="directionSpeed"></param>
    /// <param name="rootTransform"></param>
    /// <param name="cameraTransform"></param>
    /// <param name="directionOut"></param>
    /// <param name="speedOut"></param>
    public void AnalogToWorldSpace(float directionSpeed, Transform rootTransform, Transform cameraTransform,
        ref float directionOut, ref float speedOut, bool isPivoting, ref float angleOut)
    {
        var rootDirection = rootTransform.forward;
        var stickDirection = new Vector3(Horizontal, 0, Vertical);
        speedOut = stickDirection.sqrMagnitude;
        var cameraDirection = cameraTransform.forward;
        cameraDirection.y = 0;
        var referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);

        var moveDirection = referentialShift * stickDirection;
        var axisSign = Vector3.Cross(moveDirection, rootDirection);

        //Debug.DrawRay(new Vector3(rootTransform.position._starUpVerticalAngle, rootTransform.position._startUpHorizontalAngle + 2f, rootTransform.position.z), stickDirection, Color.blue);
        //Debug.DrawRay(new Vector3(rootTransform.position._starUpVerticalAngle, rootTransform.position._startUpHorizontalAngle + 2f, rootTransform.position.z), axisSign, Color.red);
        //Debug.DrawRay(new Vector3(rootTransform.position._starUpVerticalAngle, rootTransform.position._startUpHorizontalAngle + 2f, rootTransform.position.z), rootDirection, Color.magenta);

        var angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y > 0 ? -1f : 1f);
        if (!isPivoting)
        {
            angleOut = angleRootToMove;
        }

        angleRootToMove /= 180;
        directionOut = angleRootToMove * directionSpeed;
    }

    public override string ToString()
    {
        return "Horizontal: " + Horizontal + ", Vertical: " + Vertical;
    }
}

/// <summary>
///
/// </summary>
public abstract class InputMenager : MonoBehaviour
{
    #region Konfiguraca

    protected InputSource InputSource;

    public InputSource Input
    {
        get { return InputSource; }
        set
        {
            InputSource = value;
            LeftAnalog.InputSource = value;
            RightAnalog.InputSource = value;
        }
    }

    protected CharacterController CharacterController;
    protected HashIDs HashIDs;

    #endregion Konfiguraca

    #region Watości wejść

    public Analog LeftAnalog = new Analog(AnalogStick.Left);
    public Analog RightAnalog = new Analog(AnalogStick.Right);
    public float HorizontalLeftAnalog = 0;
    public float VerticalLeftAalog = 0;

    #endregion Watości wejść

    #region Metody wywodzące się z MonoBehaviour UnityEngine

    /// <summary>
    ///
    /// </summary>
    protected void Start()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    #endregion Metody wywodzące się z MonoBehaviour UnityEngine

    #region Metody abstrakcyjne

    /// <summary>
    ///
    /// </summary>
    public abstract void CollectInputs();

    /// <summary>
    ///
    /// </summary>
    public abstract void ControlPhysicsActions();

    /// <summary>
    ///
    /// </summary>
    public abstract void ControlActions();

    #endregion Metody abstrakcyjne

    /// <summary>
    ///
    /// </summary>
    private void FixedUpdate()
    {
        ControlPhysicsActions();
    }

    /// <summary>
    ///
    /// </summary>
    private void Update()
    {
        CollectInputs();
        ControlActions();
    }
}