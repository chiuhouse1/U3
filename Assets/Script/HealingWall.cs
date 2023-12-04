using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingWall : MonoBehaviour
{
    public TileBoard board;

    //int HealingAmount = 10;
    int[,] healingwall = new int[2, 2];
    public GameObject[] Wall;
    public Sprite[] WallState;


    //    1   2   3   4     0
    //   上  右  下  左    無

    //上面儲存方向

    //下面儲存回合

    int round = 3;      //用於Animation

    public void MakeWall()
    {
        int direction;

        do
        {
            direction = Random.Range(0, 4);
        }
        while ((direction == healingwall[0, 0] && healingwall[0, 1] != 0) || (direction == healingwall[1, 0] && healingwall[1, 1] != 0));

        if (healingwall[0, 1] == 0)
        {
            healingwall[0, 0] = direction;
            healingwall[0, 1] = round;
            Wall[direction].gameObject.SetActive(true);
            Wall[direction].GetComponent<Image>().sprite = WallState[2];
        }
        else if (healingwall[1, 1] == 0)
        {
            healingwall[1, 0] = direction;
            healingwall[1, 1] = round;
            Wall[direction].gameObject.SetActive(true);
            Wall[direction].GetComponent<Image>().sprite = WallState[2];
        }
    }

    public void HealingWallRoundCount(int direction)
    {
        if (direction == healingwall[0, 0])
        {
            healingwall[0, 1]--;
            if (healingwall[0, 1] == 0)
            {
                Wall[direction].gameObject.SetActive(false);

                board.CreateHealingTile();

                MakeWall();
            }
            Wall[direction].GetComponent<Image>().sprite = WallState[healingwall[0, 1] - 1];
        }
        else if (direction == healingwall[1, 0])
        {
            healingwall[1, 1]--;
            if (healingwall[1, 1] == 0)
            {
                Wall[direction].gameObject.SetActive(false);

                board.CreateHealingTile();

                MakeWall();
            }
            Wall[direction].GetComponent<Image>().sprite = WallState[healingwall[1, 1] - 1];
        }
    }
}
