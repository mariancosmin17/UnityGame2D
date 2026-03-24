using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }
    public void Respawn1()
    {
        transform.position=currentCheckpoint.position;
        playerHealth.Respawn();
        //muta camera la checkpoint room(checkpoint object trebuie pus ca copil al obiectului camera)
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
