using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
  // TODO create limits for x/z movement, rotate limits
  // but allow full rotation on x axis

  public float maxCameraSpeed = 300f;
  private float maxRotateSpeed = 75f;

  public float minFOV = 3;
  public float maxFOV = 40;

  public float minXRotation = 360 - 60;  // Note: this is because angles or stored [0,360]
  public float maxXRotation = 50;

  public float minXTravel = 100;
  public float maxXTravel = 900;
  public float minZTravel = 0;
  public float maxZTravel = 750;

  private float _targetFOV;

  [SerializeField]
  private CinemachineVirtualCamera virtualCamera;

  private void Start() {
    _targetFOV = 20;
  }
  private void Update() {
    RotateCamera();
    MoveCamera();
    ZoomCamera();
    RecentreCamera();
  }

  private void MoveCamera() {
    // Reset move direction before next input
    var input = new Vector3(0, 0, 0);

    if (Input.GetKey(KeyCode.W))
      input.z = 1f;  // Move forward
    if (Input.GetKey(KeyCode.A))
      input.x = -1f;  // Move left
    if (Input.GetKey(KeyCode.S))
      input.z = -1f;  // Move back
    if (Input.GetKey(KeyCode.D))
      input.x = 1f;  // Move right

    // Adjust camera speed based on FOV
    var cameraSpeed = maxCameraSpeed * _targetFOV / maxFOV;

    // Move direction relative to camera position
    var moveDirection = transform.forward * input.z + transform.right * input.x;
    moveDirection.y = 0;  // Lock y-axis when moving

    // Prevent the camera from moving off the terrain
    var newPosition =
        transform.position + moveDirection.normalized * (cameraSpeed * Time.deltaTime);
    if (newPosition.x < minXTravel || newPosition.x > maxXTravel || newPosition.z < minZTravel ||
        newPosition.z > maxZTravel) {
      return;
    }
    transform.position = newPosition;
  }

  private void RotateCamera() {
    var rotateDirection = 0f;
    if (Input.GetKey(KeyCode.Q))
      rotateDirection = 1f;
    if (Input.GetKey(KeyCode.E))
      rotateDirection = -1f;
    
    // Adjust rotate speed relative to how zoomed out
    var rotateSpeed = maxRotateSpeed * _targetFOV / maxFOV;
    if (Input.GetMouseButton(1)) {
      var newAngle = transform.eulerAngles.x + rotateDirection * rotateSpeed * Time.deltaTime;
      if (!(newAngle < maxXRotation || newAngle > minXRotation)) {
        return;
      }
      transform.eulerAngles += new Vector3(rotateDirection * rotateSpeed * Time.deltaTime, 0, 0);
    } else {
      transform.eulerAngles += new Vector3(0, -rotateDirection * rotateSpeed * Time.deltaTime, 0);
    }
  }

  private void ZoomCamera() {

    var zoomAmount = 2;
    if (_targetFOV > 15) {
      // Adjust zoom amount to decrement faster when far away
      zoomAmount = 5;
    }

    // Disable zoom when scrolling UI elements
    if (EventSystem.current.IsPointerOverGameObject())
      return;

    if (Input.mouseScrollDelta.y > 0) {
      // Positive scrolling forward (zoom in)
      _targetFOV -= zoomAmount;
    }
    if (Input.mouseScrollDelta.y < 0) {
      // Negative indicates scrolling backward (zoom out)
      _targetFOV += zoomAmount;
    }
    // Check that desired FOV does not exceed limits
    if (_targetFOV > maxFOV)
      _targetFOV = maxFOV;
    else if (_targetFOV < minFOV)
      _targetFOV = minFOV;
    virtualCamera.m_Lens.FieldOfView = _targetFOV;
  }

  private void RecentreCamera() {
    if (Input.GetKey(KeyCode.Space)) {
      transform.eulerAngles = new Vector3(30, 0, 0);
      transform.position = new Vector3(500, 250, 80);;
    }
  }
}
