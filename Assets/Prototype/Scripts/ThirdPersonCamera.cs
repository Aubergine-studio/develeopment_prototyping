using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    // Śledzona postać.
    [SerializeField]
    private ThirdPersonPlayerController _follow;

    #region Parametry ruchu kamery

    // Odległość od postaci śledzonej
    [SerializeField]
    public float DistanceAway = 5f;

    // Odległość nad postacią śledzoną
    [SerializeField]
    public float DistanceUp = 2f;

    // Gładkość przejścia między starą a nową pozycją kamery
    [SerializeField]
    public float Smooth = 3f;

    // Szybkość zmiany gładkości ruchu kamery
    [SerializeField]
    private float _cameraSmoothDampTime = 0.1f;

    // Szybkość zmiany kierunku widzenia.
    [SerializeField]
    private float _lookDirectionDampTime = 0.1f;

    #region Prywatne parametry ruchu kamery

    // Pozycja ceu
    private Vector3 _targetPosition;

    private Vector3 _loockDirection;
    private Vector3 _curentLookDirection;
    private Vector3 _vecocityCameraSmooth = Vector3.zero;
    private Vector3 _velocityLookDirection = Vector3.zero;

    #endregion Prywatne parametry ruchu kamery

    #endregion Parametry ruchu kamery

    private Analog _leftAnalog;
    private Analog _rightAnalog;

    #region Funkcje UnityEngine

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (_follow == null)
            _follow = player.GetComponent<ThirdPersonPlayerController>();

        _loockDirection = _curentLookDirection = _follow.Transform.forward;

        var pim = player.GetComponent<PlayerInputMenager>();
        _leftAnalog = pim.LeftAnalog;
        _rightAnalog = pim.RightAnalog;
    }

    private void LateUpdate()
    {
        var characterOffset = _follow.Transform.position + (DistanceUp * _follow.Transform.up);

        if (_follow.IsInLocomotion() && !_follow.IsInPivot())
        {
            _loockDirection = Vector3.Lerp(
                _follow.Transform.right * (_leftAnalog.Horizontal < 0 ? 1f : -1f),
                _follow.Transform.forward * (_leftAnalog.Vertical < 0 ? -1f : 1f),
                Mathf.Abs(Vector3.Dot(transform.forward, _follow.Transform.forward))
                );
            //Debug.Log(_loockDirection.ToString());
            //Debug.DrawRay(transform.position, _loockDirection, Color.green);

            _curentLookDirection = Vector3.Normalize(characterOffset - transform.position);
            _curentLookDirection.y = 0;
            //Debug.DrawRay(transform.position, _curentLookDirection, Color.green);

            _curentLookDirection = Vector3.SmoothDamp(_curentLookDirection, _loockDirection, ref _velocityLookDirection, _lookDirectionDampTime);
        }

        _targetPosition = characterOffset + _follow.Transform.up * DistanceUp - Vector3.Normalize(_curentLookDirection) * DistanceAway;
        CompensateForWalls(characterOffset, ref _targetPosition);
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _vecocityCameraSmooth, _cameraSmoothDampTime);
        //transform.rotation = Quaternion.Euler(0, 10, 0);
        transform.LookAt(_follow.Transform);
    }

    #endregion Funkcje UnityEngine

    #region Funkcje własne

    private void CompensateForWalls(Vector3 fromObjec, ref Vector3 toTarget)
    {
        //Debug.DrawLine(fromObjec, toTarget, Color.cyan);
        RaycastHit wallHit;
        if (!Physics.Linecast(fromObjec, toTarget, out wallHit)) return;
        //Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
        toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
    }

    #endregion Funkcje własne
}