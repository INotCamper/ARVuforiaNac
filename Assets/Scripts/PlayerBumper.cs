using UnityEngine;
using UnityEngine.UI;

public class PlayerBumper : MonoBehaviour
{
    public Text pointsText;
    private int points;

    private void Awake()
    {
        pointsText.text = "0";
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Note"))
        {
            Destroy(collision.gameObject);
            points += 10;
            pointsText.text = points.ToString();
        }
    }
}
