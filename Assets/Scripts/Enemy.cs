using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public Text wordText;
    private int typeIndex;
    [HideInInspector]
    public string word = "really long word";

    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        word = GameManager.instance.RandomWord();
        wordText.text = word;

        GameManager.instance.enemies.Add(this);
        target = Waypoints.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        waypointIndex++;

        if (waypointIndex >= Waypoints.points.Length)
        {
            GameManager.instance.enemies.Remove(this);
            GameManager.instance.RemoveHealth(word.Length);
            if(GameManager.instance.IsActiveEnemy(this))
            {
                GameManager.instance.hasActiveEnemy = false;
            }

            Destroy(gameObject);
        }
        else
        {
            target = Waypoints.points[waypointIndex];
        }
    }

    public void RemoveLetter()
    {
        wordText.text = wordText.text.Remove(0, 1);
        wordText.color = Color.red;
    }

    public char GetNextLetter()
    {
        return word[typeIndex];
    }

    public void TypeLetter()
    {
        typeIndex++;
        RemoveLetter();
    }

    public bool WordTyped()
    {
        bool wordTyped = (typeIndex >= word.Length);

        return wordTyped;
    }
}
