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

    public void NewGame()       //��s����
    {
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void Fullpage()      //�L�k�ʼu�ɰ���
    {
        board.enabled = false;
        board.FullPageStart();
        Invoke("NewGame", 1);
    }
}
