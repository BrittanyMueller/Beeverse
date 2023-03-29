using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
  // TODO create limits for x/z movement, rotate limits
  // but allow full rotation on x axis

  public float maxCameraSpeed = 300f;
  public float maxRotateSpeed = 75f;

  public float minFOV = 3;
  public float maxFOV = 40;
  private float _targetFOV;

  [SerializeField]
  private CinemachineVirtualCamera virtualCamera;

  void Start() { _targetFOV = 20; }
  private void Update() {
    RotateCamera();
    MoveCamera();
    ZoomCamera();
  }

  private void MoveCamera() {
    // Reset move direction before next input
    var input = new Vector3(0, 0, 0);

    if (Input.GetKey(KeyCode.W))
      input.z = 1f; // Move forward
    if (Input.GetKey(KeyCode.A))
      input.x = -1f; // Move left
    if (Input.GetKey(KeyCode.S))
      input.z = -1f; // Move back
    if (Input.GetKey(KeyCode.D))
      input.x = 1f; // Move right

    // Adjust camera speed based on FOV
    var cameraSpeed = maxCameraSpeed * _targetFOV / maxFOV;

    // Move direction relative to camera position
    var moveDirection = transform.forward * input.z + transform.right * input.x;
    moveDirection.y = 0; // Lock y-axis when moving
    transform.position +=
        moveDirection.normalized * (cameraSpeed * Time.deltaTime);
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
      transform.eulerAngles +=
          new Vector3(rotateDirection * rotateSpeed * Time.deltaTime, 0, 0);
    } else {
      transform.eulerAngles +=
          new Vector3(0, -rotateDirection * rotateSpeed * Time.deltaTime, 0);
    }
  }

  private void ZoomCamera() {

    // TODO adjust zoom amount based on current FOV (zoom feels slow when
    // near/at maxFOV)
    
    // Disable zoom when scrolling UI elements
    if (EventSystem.current.IsPointerOverGameObject())
      return;

    if (Input.mouseScrollDelta.y > 0) {
      // Positive scrolling forward (zoom in)
      _targetFOV -= 1;
    }
    if (Input.mouseScrollDelta.y < 0) {
      // Negative indicates scrolling backward (zoom out)
      _targetFOV += 1;
    }
    // Check that desired FOV does not exceed limits
    if (_targetFOV > maxFOV)
      _targetFOV = maxFOV;
    else if (_targetFOV < minFOV)
      _targetFOV = minFOV;
    virtualCamera.m_Lens.FieldOfView = _targetFOV;
  }
}
