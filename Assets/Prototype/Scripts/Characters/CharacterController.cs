using UnityEngine;

/// <summary>
/// Enum determinujący kierunek poruszania się w projektach 2D.
/// </summary>
public enum Direction2D
{
    Left,
    Right
}

/// <summary>
///
/// </summary>
public class CharacterController : MonoBehaviour
{
    #region Referencje na komponenty własne.

    // Ciało fizyczne 3D
    public Rigidbody Rigidbody { get; private set; }

    // Ciało fizyczne 2D
    public Rigidbody2D Rigidbody2D { get; private set; }

    //  Transformacja postaci
    public Transform Transform { get; private set; }

    public Animator Animator { get; private set; }

    #endregion Referencje na komponenty własne.

    #region Referencje na komponenty obce

    public HashIDs HashIDs { get; protected set; }

    #endregion Referencje na komponenty obce

    #region Statystyki ruchu postaci

    public float MaxSpead = 5.5f;
    protected float Speed;
    protected float Angle;

    protected float Direction = 0.0f;

    [SerializeField]
    protected float DirectionDampTime = 0.25f;

    [SerializeField]
    protected float SpeedDampTime = 0.05f;

    [SerializeField]
    protected float DirectionSpeed = 1.5f;

    [SerializeField]
    protected float RotationDegreePerSeckend = 120f;

    [SerializeField]
    protected float LocomationThreshold = 0.05f;

    #endregion Statystyki ruchu postaci

    protected void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Transform = GetComponent<Transform>();
        Animator = GetComponent<Animator>();
    }
}