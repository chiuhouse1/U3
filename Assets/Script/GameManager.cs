using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileBoard board;
    public int Level = 0;

    private void Start()
    {
        NewGame();
        board.CreateEnemy(Level);
    }

    public void NewGame()       //刷新版面
    {
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void Fullpage()      //無法動彈時執行
    {
        board.enabled = false;
        board.FullPageStart();
        Invoke("NewGame", 1);
    }
}
