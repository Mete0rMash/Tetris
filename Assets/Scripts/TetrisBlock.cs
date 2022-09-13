using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    private float fallTime = 0.8f;
    public static int w = 10;
    public static int h = 23;
    public static Transform[,] grid = new Transform[w, h];
    public static bool addScore = false;


    private void Awake()
    {
        if (!IsValid())
        {
            StartCoroutine("Restart");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0);

            if (!IsValid()) transform.position -= new Vector3(-1, 0);
        }

        else if (Input.GetKeyDown(KeyCode.D)) 
        {
            transform.position += new Vector3(1, 0);

            if (!IsValid()) transform.position -= new Vector3(1, 0);
        }

        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

            if (!IsValid()) transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }

        if(Time.time - previousTime > (Input.GetKey(KeyCode.S) ? fallTime/10:fallTime))
        {
            transform.position += new Vector3(0, -1);

            if (!IsValid())
            { 
                transform.position -= new Vector3(0, -1);
                AddToGrid();

                CheckForLines();

                this.enabled = false;
                FindObjectOfType<SpawnBlock>().NewBlock();
            }

            previousTime = Time.time;
        }
    } 

    private void CheckForLines()
    {
        for (int i = h-1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    private bool HasLine(int i)
    {
        for (int j = 0; j < w; j++)
        {
            if (grid[j, i] == null) return false;
        } 
        addScore = true;
        fallTime -= 0.2f;
        return true;
    }

    private void DeleteLine(int i)
    {
        for (int j = 0; j < w; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    private void RowDown(int i)
    {
        for (int y = i; y < h; y++)
        {
            for (int j = 0; j < w; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1);
                }
            }
        }
    }

    private void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;
        }
    }

    private bool IsValid()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if(roundX < 0 || roundX >= w || roundY < 0 || roundY >= h) return false;

            if (grid[roundX, roundY] != null) return false;
        }
        return true;
    }
    private IEnumerator Restart()
    {
        
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
    }
}
