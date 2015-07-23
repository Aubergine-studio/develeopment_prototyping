using UnityEngine;

public class TransformDebuger : MonoBehaviour
{
    private GameObject _gameObject;
    private Transform _worldTransform;

    // Use this for initialization
    private void Start()
    {
        _worldTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(name + " transform: " + _worldTransform.position);
    }
}