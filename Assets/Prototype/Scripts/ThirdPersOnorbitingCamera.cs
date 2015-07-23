using UnityEngine;

[ExecuteInEditMode]
public class ThirdPersOnorbitingCamera : MonoBehaviour
{
    #region Referencje na komponenty obce

    private ThirdPersonPlayerController _playerController;
    private PlayerInputMenager _playerInputMenager;

    #endregion Referencje na komponenty obce

    #region Referencje na komponenty własne.

    private Transform _cameraTransform;

    #endregion Referencje na komponenty własne.

    #region Parametry ruchu kamery.

    [SerializeField]
    private Vector3 _cameraOffset = Vector3.zero;

    [SerializeField]
    private Transform _playerTransform;

    [SerializeField]
    private float _maxX = 70f;

    [SerializeField]
    private float _minX = 45f;

    [SerializeField]
    private float _cameraReturnSpeed = 2;

    [SerializeField]
    private float _cameraRotarionSpeed = 2;

    [SerializeField]
    private float _starUpVerticalAngle = 15f;

    [SerializeField]
    private float _startUpHorizontalAngle = 0;

    #endregion Parametry ruchu kamery.

    private void Start()
    {
        var playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _playerController = playerGameObject.GetComponent<ThirdPersonPlayerController>();
        _playerInputMenager = playerGameObject.GetComponent<PlayerInputMenager>();
        _playerTransform = playerGameObject.transform;

        _cameraTransform = GameObject.FindGameObjectWithTag("PlayerCamera").gameObject.transform;
        _cameraTransform.localPosition = Vector3.zero + _cameraOffset;

        _playerInputMenager.RightAnalog.InverVertical = false;
    }

    private void Update()
    {
        if (!Application.isPlaying)
            _cameraTransform.localPosition = Vector3.zero + _cameraOffset;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var cameraSpeed = 0;
        if (Mathf.Abs(_playerInputMenager.RightAnalog.Horizontal) < 0.1
            && _playerController.IsInLocomotion()
            && _playerInputMenager.LeftAnalog.Vertical >= 0)
        {
            var playerEulerAngles = _playerTransform.transform.rotation.eulerAngles;
            _starUpVerticalAngle = playerEulerAngles.x;
            _startUpHorizontalAngle = playerEulerAngles.y;
            cameraSpeed = 0;
        }
        else
        {
            _starUpVerticalAngle += _playerInputMenager.RightAnalog.Vertical;
            _startUpHorizontalAngle += _playerInputMenager.RightAnalog.Horizontal;
            cameraSpeed = 1;
        }
        _starUpVerticalAngle = Mathf.Clamp(_starUpVerticalAngle, _minX, _maxX);

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.Euler(_starUpVerticalAngle, _startUpHorizontalAngle, 0),
            (cameraSpeed == 0 ? _cameraReturnSpeed : _cameraRotarionSpeed) * Time.deltaTime);

        transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + _cameraOffset.y, _playerTransform.position.z);
    }
}