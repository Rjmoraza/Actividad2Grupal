using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    AudioSource music;
    EnemyController currentEnemy;

    [SerializeField]
    EnemyController enemy1, enemy2, enemy3;

    [SerializeField]
    GameObject[] waypoints1, waypoints2, waypoints3;

    // Start is called before the first frame update
    void Awake()
    {
        music = GetComponent<AudioSource>();

        currentEnemy = enemy1;
        enemy2.gameObject.SetActive(false);
        enemy3.gameObject.SetActive(false);

        enemy1.SetWaypoints(waypoints1);
        enemy2.SetWaypoints(waypoints2);
        enemy3.SetWaypoints(waypoints3);
    }

    void Start()
    {
        StartCoroutine(LevelProcess());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LevelProcess()
    {
        yield return new WaitForSeconds(1);
        while(enemy1.IsAlive())
        {
            if(enemy1.IsVisible())
            {
                music.volume = 1;
            }
            yield return null;
        }
        music.volume = 0;
        yield return new WaitForSeconds(10);
        Destroy(enemy1.gameObject);

        enemy2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        while(enemy2.IsAlive())
        {
            if(enemy2.IsVisible())
            {
                music.volume = 1;
            }
            yield return null;
        }
        music.volume = 0;
        yield return new WaitForSeconds(10);
        Destroy(enemy2.gameObject);

        enemy3.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        while(enemy3.IsAlive())
        {
            if(enemy3.IsVisible())
            {
                music.volume = 1;
            }
            yield return null;
        }
        music.volume = 0;
        yield return new WaitForSeconds(10);
        Destroy(enemy3.gameObject);

        print("GAME WIN");
        SceneManager.LoadScene("GameWin");
    }
}
