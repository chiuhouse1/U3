using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Define
{

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}

public class TileBoard : MonoBehaviour
{
    public GameManager GameManager;
    public PopBoxController popBoxController;
    public HPBarController hPBarController;
    public TextMeshProUGUI roundtext;
    public TextMeshProUGUI showlevel;
    public Attack attack;

    public int playerHP;
    public int Sword;
    public int Ax;
    public int Spear;
    public int Bow;
    int swordcount;
    int axcount;
    int spearcount;
    int bowcount;
    int bonus = 1;
    int movetimes = 3;

    int round;

    public Tile tilePrefab;
    public Enemy enemyPrefab;
    public WeaponState[] weaponStates;
    public EnemyState[] enemyStates;

    public GameObject board;
    public GameObject FullBoardPopScreen;
    public Slider PlayerHP;
    public Slider PlayerHPAni;
    public Slider MonsterArmor;
    public Slider MonsterHP;
    public Slider MonsterHPAni;

    TileGrid grid;
    public GameObject EnemySpawnPoints;
    List<Tile> tiles;
    Enemy enemy;
    public bool waiting;

    public float Touch_buffer;

    bool point = false;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);

        PlayerHP.maxValue = playerHP;
        PlayerHP.value = playerHP;
        PlayerHPAni.maxValue = playerHP;
        PlayerHPAni.value = playerHP;
    }

    public void ClearBoard()        //清空版面
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    public void CreateTile()        //生成方塊
    {
        int newtile = Random.Range(0, 4);
        if (newtile != 0)
        {
            newtile = 1;
        }
        else
        {
            newtile = 2;
        }

        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(weaponStates[newtile - 1]);
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    public void CreateEnemy(int level)        //生成敵人
    {
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }
        Enemy monster = Instantiate(enemyPrefab, EnemySpawnPoints.transform);
        monster.SetState(enemyStates[level], MonsterHP, MonsterHPAni, MonsterArmor);
        enemy = monster.GetComponent<Enemy>();
        round = enemy.round;
        roundtext.text = round.ToString();
        showlevel.text = "Level     " + (level + 1);
    }

    // 记录手指触碰位置
    private Vector2 S_Pos = new Vector2();

    private void Update()
    {
        if (!waiting)
        {
            MouseInput();   // 检测鼠标输入

            MobileInput();  // 检测触摸输入
        }
    }

    private GameObject MouseClick()
    {
        // 将鼠标位置转换为2D世界坐标
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 通过射线检测点击位置
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        // 检测是否击中物体
        if (hit.collider != null)
        {
            // 输出击中物体的名称
            Debug.Log("Clicked on: " + hit.collider.gameObject.name);

            return hit.collider.gameObject;

            // 在这里可以添加处理被点击物体的逻辑
        }
        else
        {
            return null;
        }
    }

    // 用于鼠标输入检测
    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0) && MouseClick() == board && !point)
        {

            S_Pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            point = true;
        }

        if (Input.GetMouseButtonUp(0) && point)
        {
            print("321");
            Vector2 E_Pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            point = false;

            // 检测手指滑动方向
            Define.Direction mDirection = HandDirection(S_Pos, E_Pos);
            Debug.Log("mDirection: " + mDirection.ToString());
        }
    }

    // 用于移动设备触摸输入检测
    private void MobileInput()
    {
        if (Input.touchCount <= 0)
            return;

        // 一个手指触碰屏幕
        if (Input.touchCount == 1)
        {
            // 开始触碰
            if (Input.touches[0].phase == TouchPhase.Began && MouseClick() == board)
            {
                Debug.Log("Began");
                // 记录触碰位置
                S_Pos = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                Debug.Log("Moved");
            }

            // 手指离开屏幕
            if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Debug.Log("Ended");
                Vector2 E_Pos = Input.touches[0].position;

                // 检测手指滑动方向
                Define.Direction mDirection = HandDirection(S_Pos, E_Pos);
                Debug.Log("mDirection: " + mDirection.ToString());
            }
        }
    }

    // 根据触摸位置计算手指滑动方向
    Define.Direction HandDirection(Vector2 StartPos, Vector2 EndPos)
    {
        Define.Direction mDirection;

        // 手指水平移动
        if (Mathf.Abs(StartPos.x - EndPos.x) > Mathf.Abs(StartPos.y - EndPos.y))
        {
            if (StartPos.x - Touch_buffer > EndPos.x)
            {
                // 手指向左滑动
                mDirection = Define.Direction.Left;

                Move(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (StartPos.x + Touch_buffer < EndPos.x)
            {
                // 手指向右滑动
                mDirection = Define.Direction.Right;

                Move(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
            else
            {
                mDirection = Define.Direction.None;
            }
        }
        else
        {
            if (StartPos.y - Touch_buffer > EndPos.y)
            {
                // 手指向下滑动
                mDirection = Define.Direction.Down;

                Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (StartPos.y + Touch_buffer < EndPos.y)
            {
                // 手指向上滑动
                mDirection = Define.Direction.Up;

                Move(Vector2Int.up, 0, 1, 1, 1);
            }
            else
            {
                mDirection = Define.Direction.None;
            }
        }
        return mDirection;
    }

    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    //方塊順序
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }

    }

    bool MoveTile(Tile tile, Vector2Int direction)      //檢測是否能移動
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    bool CanMerge(Tile a, Tile b)       //檢測是否能合併
    {
        return (a.state.Number == b.state.Number || a.state.Number == b.state.Number + 4 || a.state.Number + 4 == b.state.Number) && !b.locked;
    }

    void MergeTiles(Tile a, Tile b)     //合併方塊
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        WeaponState maxstate;
        if (a.state.Number >= b.state.Number)
        {
            maxstate = a.state;
        }
        else
        {
            maxstate = b.state;
        }

        int index = IndexOf(maxstate) + 1;
        if (index > weaponStates.Length - 1)
        {
            index = 0;
        }

        b.SetState(weaponStates[index]);
        if (index < 4)
        {
            bonus = index + 1;
        }
        else if (index >= 4)
        {
            bonus = index - 3;
        }
    }

    int IndexOf(WeaponState state)      //合併方塊中的運算
    {
        for (int i = 0; i < weaponStates.Length; i++)
        {
            if (state == weaponStates[i])
            {
                return i;
            }
        }

        return -1;
    }

    IEnumerator WaitForChanges()        //方塊移動中的等待
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (CheckForFullpage())
        {
            popBoxController.showPop_Transparency(FullBoardPopScreen);
        }

        foreach (var tile in tiles)     //
        {
            if (tile.number == 1 || tile.number == 5)
            {
                swordcount++;
            }
            else if (tile.number == 2 || tile.number == 6)
            {
                axcount++;
            }
            else if (tile.number == 3 || tile.number == 7)
            {
                spearcount++;
            }
            else if (tile.number == 4 || tile.number == 8)
            {
                bowcount++;
            }

        }

        movetimes--;

        StartCoroutine(MovetimesSelect());
    }

    IEnumerator MovetimesSelect()
    {
        if (movetimes == 0)     //移動回合結束
        {
            StartCoroutine(hPBarController.HPFlow(MonsterHP, MonsterHPAni, attack.DamageCalculation(swordcount, axcount, spearcount, bowcount, bonus)));

            yield return new WaitForSeconds(1);

            swordcount = 0;
            axcount = 0;
            spearcount = 0;
            bowcount = 0;
            bonus = 1;

            movetimes = 3;

            round -= 1;
            roundtext.text = round.ToString();

            if (MonsterHP.value == 0)     //怪物死亡
            {
                GameManager.Level++;
                if (GameManager.Level == enemyStates.Length)
                {
                    GameManager.Level = 0;
                }
                CreateEnemy(GameManager.Level);

                movetimes = 3;
            }
            else if (round == 0)     //怪物回合倒數結束
            {
                StartCoroutine(hPBarController.HPFlow(PlayerHP, PlayerHPAni, enemy.damage));

                yield return new WaitForSeconds(1);

                round = enemy.round;
                roundtext.text = round.ToString();
            }

            waiting = false;

        }
        else
        {
            waiting = false;
        }

        if (tiles.Count != grid.size)
        {
            CreateTile();
        }

        Debug.Log(movetimes);
        yield return null;
    }

    bool CheckForFullpage()     //檢測是否無法動彈
    {
        if (tiles.Count != grid.size)
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        waiting = true;
        return true;
    }

    public void FullPageStart()
    {
        foreach (var tile in tiles)
        {
            tile.FullpageAni();
        }
    }
}
