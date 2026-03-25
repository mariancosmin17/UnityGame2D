using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    [SerializeField] private bool isFinalDoor;
    private UiManager uiManager;

    private void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.tag=="Player")
        {

            if (isFinalDoor)
            {
                if (uiManager != null)
                {
                    uiManager.GameWon();
                }
                return;
            }

            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);

            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);

            }

        }
    }
}
