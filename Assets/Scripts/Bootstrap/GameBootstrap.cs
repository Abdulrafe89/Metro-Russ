using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private void Start()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            var managerGO = new GameObject("GameManager");
            managerGO.AddComponent<GameManager>();
        }

        if (FindObjectOfType<WorldGenerator>() == null)
        {
            var worldGO = new GameObject("WorldGenerator");
            worldGO.AddComponent<WorldGenerator>();
        }

        if (FindObjectOfType<RunnerController>() == null)
        {
            CreatePlayer();
        }

        if (FindObjectOfType<FollowCamera>() == null)
        {
            CreateCamera();
        }

        if (FindObjectOfType<UIManager>() == null)
        {
            CreateHud();
        }

        GameManager.Instance.Initialize();
        GameManager.Instance.StartGame();
    }

    private void CreatePlayer()
    {
        var playerGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerGO.name = "Player";
        playerGO.tag = "Player";
        playerGO.transform.position = new Vector3(0f, 1.1f, 0f);
        playerGO.transform.localScale = new Vector3(0.9f, 1.1f, 0.9f);

        var controller = playerGO.AddComponent<RunnerController>();
        controller.Initialize();

        var collider = playerGO.GetComponent<CapsuleCollider>();
        collider.center = new Vector3(0f, 0.1f, 0f);
        collider.height = 2.2f;

        var rb = playerGO.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = playerGO.AddComponent<Rigidbody>();
        }

        rb.mass = 1.25f;
        rb.drag = 0.1f;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void CreateCamera()
    {
        GameObject cameraGO = new GameObject("Main Camera");
        cameraGO.tag = "MainCamera";
        var camera = cameraGO.AddComponent<Camera>();
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 150f;
        camera.fieldOfView = 60f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.18f, 0.17f, 0.25f, 1f);

        cameraGO.AddComponent<AudioListener>();
        var follow = cameraGO.AddComponent<FollowCamera>();
        follow.target = GameObject.FindGameObjectWithTag("Player")?.transform;
        cameraGO.transform.position = new Vector3(0f, 5.2f, -12f);
    }

    private void CreateHud()
    {
        var canvasGO = new GameObject("GameCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var hud = canvasGO.AddComponent<UIManager>();
        hud.CreateDefaultLayout();
    }
}
