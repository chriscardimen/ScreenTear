using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//this is all made possible by
//https://github.com/craigem/csvMaze

public class MazeLoader : MonoBehaviour
{
    private TextAsset currentMaze;
    public Sprite sprite;
    public GameObject player;
    public float barrierSize;
    public Vector3 offset;


    public void Start()
    {
        NewMaze();
    }

    private List<string> SplitMaze(TextAsset mazeToSplit)
    {
        return mazeToSplit.ToString().Split('\n').ToList();
    }

    public void ChooseRandom()
    {
        MazeStorage storage = GetComponent<MazeStorage>();
        if (storage.mazes.Count > 0)
        {
            int rand = Random.Range(0, storage.mazes.Count);
            currentMaze = storage.mazes[rand];
            storage.mazes.RemoveAt(rand);
            LoadMaze();
        }
    }

    public void NewMaze()
    {
        CleanUpMaze();
        ChooseRandom();
    }

    public void CleanUpMaze()
    {
        if (this.gameObject.transform.childCount > 0)
        {
            DestroyImmediate(this.gameObject.transform.GetChild(0).gameObject);
        }
    }

    public void LoadMaze()
    {
        player.transform.position = new Vector3(1 + offset.x, 1 + offset.y, 0 + offset.z);
        GameObject mazeBounds = new GameObject();
        mazeBounds.name = "MazeBounds";
        this.GetComponent<MazeManager>().SetWin(false);
        mazeBounds.transform.parent = this.gameObject.transform;
        List<string> mazeSplit = SplitMaze(currentMaze);
        int col = 1;
        int row = 1;

        foreach (string csvRow in mazeSplit)
        {
            col = 1;
            foreach (char csvCol in csvRow)
            {
                if (csvCol == '1')
                {
                    //Debug.Log("1");
                    GameObject newBounds = new GameObject();
                    newBounds.gameObject.name = col + " : " + row;
                    newBounds.transform.parent = this.gameObject.transform.GetChild(0);
                    newBounds.transform.localScale = new Vector3(barrierSize, barrierSize, barrierSize);
                    newBounds.GetComponent<Transform>().position = new Vector3(col * barrierSize + offset.x,
                        row * barrierSize + offset.y, 0 + offset.z);
                    newBounds.AddComponent<SpriteRenderer>();
                    newBounds.GetComponent<SpriteRenderer>().color = Color.blue;
                    newBounds.GetComponent<SpriteRenderer>().sprite = sprite;
                    newBounds.AddComponent<BoxCollider2D>();
                    newBounds.AddComponent<Rigidbody2D>();
                    newBounds.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    newBounds.GetComponent<Rigidbody2D>().gravityScale = 0;
                    newBounds.GetComponent<Rigidbody2D>().mass = 10;
                }
                else if (csvCol == '3')
                {
                    //Debug.Log("3");
                    //Solve space
                    GameObject newBounds = new GameObject();
                    newBounds.gameObject.name = col + " : " + row;
                    newBounds.transform.parent = this.gameObject.transform.GetChild(0);
                    newBounds.transform.localScale = new Vector3(barrierSize, barrierSize, barrierSize);
                    newBounds.GetComponent<Transform>().position = new Vector3(col * barrierSize + offset.x,
                        row * barrierSize + offset.y, 0 + offset.z);
                    newBounds.AddComponent<SpriteRenderer>();
                    newBounds.GetComponent<SpriteRenderer>().color = Color.green;
                    newBounds.GetComponent<SpriteRenderer>().sprite = sprite;
                    newBounds.AddComponent<BoxCollider2D>();
                    newBounds.AddComponent<MazeWin>();
                }

                col++;
            }

            row++;
        }
    }
}