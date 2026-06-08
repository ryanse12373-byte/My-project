using UnityEngine;

public class SpectatorManager : MonoBehaviour
{
    public bool isSpectate;
    [SerializeField] private GameObject player;
    [SerializeField] private FreeCam freeCam;
    [SerializeField] private CameraHolder cameraHolder;
    [SerializeField] private PlayerCam playerCam;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private GameObject staminaUi;
    [SerializeField] private GameObject SpawningUi;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSpectate = !isSpectate;
            print("Louis Bonafous");
            Spectate();
        }
    }

    void Spectate()
    {
        cameraHolder.enabled = !isSpectate;
        player.SetActive(!isSpectate);
        freeCam.enabled = isSpectate;
        playerCam.enabled = !isSpectate;
        staminaUi.SetActive(!isSpectate);
        SpawningUi.SetActive(isSpectate);

        if (!isSpectate)
        {
            player.transform.position = Camera.main.transform.position;
            Camera.main.transform.position = cameraHolder.transform.position;
        }
    }
}
