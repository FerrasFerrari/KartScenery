using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Objeto que a c�mera deve seguir.")]
    public Transform target;

    [Tooltip("Offset da c�mera em rela��o ao alvo (em espa�o local do alvo).")]
    public Vector3 offset = new Vector3(0, 5, -10);

    [Tooltip("Velocidade de suaviza��o da posi��o da c�mera.")]
    public float smoothSpeed = 5f;

    [Tooltip("Velocidade de suaviza��o da rota��o da c�mera.")]
    public float rotationSmoothSpeed = 5f;

    [Tooltip("�ndice do jogador (0 = topo, 1 = baixo).")]
    public int cameraIndex = 0;

    private Camera cam;
    private Vector3 currentVelocity;

    private void Start()
    {
        cam = GetComponent<Camera>();
        ConfigureViewport();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Define a posi��o desejada com base na rota��o Y do alvo
        Quaternion flatRotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
        Vector3 desiredPosition = target.position + flatRotation * offset;

        // Suaviza a posi��o com SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / smoothSpeed);

        // Suaviza a rota��o para olhar o alvo
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }

    private void ConfigureViewport()
    {
        if (cam == null) return;

        if (cameraIndex == 0)
        {
            cam.rect = new Rect(0f, 0.5f, 1f, 0.5f); // topo
        }
        else if (cameraIndex == 1)
        {
            cam.rect = new Rect(0f, 0f, 1f, 0.5f); // baixo
        }
        else
        {
            Debug.LogWarning($"[CameraFollow] �ndice de c�mera {cameraIndex} n�o � suportado para splitscreen de 2 jogadores.");
        }
    }
}