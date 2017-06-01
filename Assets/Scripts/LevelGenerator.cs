using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGenerator : MonoBehaviour {
    public static LevelGenerator instance;
    public Transform levelStartPoint;
    public LevelPieceBasic startPlatformPrefab;
    public LevelPieceBasic endPlatformPrefab;
    public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
    public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();
    public bool shouldFinish = false;
    public float maxGameTime = 60;
    // Use this for initialization
    void Start()
    {
        instance = this;
        shouldFinish = false;
        ShowPiece((LevelPieceBasic)Instantiate(startPlatformPrefab));
        AddStartPiece();
        AddPiece();
        AddPiece();
        AddPiece();
        AddPiece();
    }
    public void ShowPiece(LevelPieceBasic piece)
    {
        piece.transform.SetParent(this.transform, false);
        if (pieces.Count < 1)
            piece.transform.position = new Vector2(
            levelStartPoint.position.x - piece.startPoint.localPosition.x,
            levelStartPoint.position.y - piece.exitPoint.localPosition.y);
        else
            piece.transform.position = new Vector2(
            pieces[pieces.Count - 1].exitPoint.position.x + pieces[pieces.Count -
           1].startPoint.localPosition.x+5f,
            pieces[pieces.Count - 1].exitPoint.position.y - pieces[pieces.Count -
           1].exitPoint.localPosition.y);


        piece.gameObject.GetComponent<Rigidbody2D>();
        piece.enabled = true;
        pieces.Add(piece);
    }
    public void Finish()
    {
        shouldFinish = true;
        ShowPiece((LevelPieceBasic)Instantiate(endPlatformPrefab));
    }
    public void AddPiece()
    {
        int randomIndex = Random.Range(0, levelPrefabs.Count -1);
        LevelPieceBasic piece = (LevelPieceBasic)Instantiate(levelPrefabs[randomIndex]);
        ShowPiece(piece);
    }

    public void AddStartPiece()
    {
        LevelPieceBasic piece = (LevelPieceBasic)Instantiate(levelPrefabs[0]);
        ShowPiece(piece);
    }
    public void RemoveOldestPiece()
    {
        if (pieces.Count > 1)
        {
            LevelPieceBasic oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
    }
}




    /*public static LevelGenerator instance;

    public Transform LevelStartPoint;
    public List<LevelPieceBasic> levelPrefabs = new List<LevelPieceBasic>();
    public List<LevelPieceBasic> pieces = new List<LevelPieceBasic>();

    public LevelPieceBasic startPlatformPrefab;
    public LevelPieceBasic endPlatformPrefab;

    public int maxGameTime = 60;
    public bool shouldFinish = false;

    void Start () {

        ShowPiece((LevelPieceBasic)Instantiate(startPlatformPrefab));

        AddPiece();
        AddPiece();
        AddPiece();
        AddPiece();
        AddPiece();
        AddPiece();
        AddPiece();
        instance = this;
	}
	
    public void ShowPiece(LevelPieceBasic piece)
    {
        piece.transform.SetParent(this.transform, false);

        if (pieces.Count < 1)
        {
            piece.transform.position = new Vector2(
                LevelStartPoint.position.x - piece.startPoint.localPosition.x,
                LevelStartPoint.position.y - piece.startPoint.localPosition.y);
        }
        else
        {
            piece.transform.position = new Vector2(
                pieces[pieces.Count - 1].exitPoint.position.x - pieces[pieces.Count - 1].startPoint.localPosition.x,
                pieces[pieces.Count - 1].exitPoint.position.y - pieces[pieces.Count - 1].startPoint.localPosition.y);
        }
        pieces.Add(piece);
    }

	public void AddPiece()
    {
        int randomIndex = Random.Range(0, levelPrefabs.Count);
        LevelPieceBasic piece = (LevelPieceBasic)Instantiate(levelPrefabs[randomIndex]);
        ShowPiece(piece);
    }

    public void RemoveOldestPiece()
    {
        if(pieces.Count >1 )
        {
            LevelPieceBasic oldestPiece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(oldestPiece.gameObject);
        }
    }

    public void Finish()
    {
        shouldFinish = true;
        ShowPiece((LevelPieceBasic)Instantiate(endPlatformPrefab));
    }*/

