using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public WeaponState state { get; private set; }
    public TileCell cell { get; private set; }

    public int number { get; private set; }

    public float magnification { get; private set; }

    public bool locked { get; set; }

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetState(WeaponState state)     //設置狀態
    {
        this.state = state;

        number = state.Number;
        magnification = state.Magnification;
        spriteRenderer.sprite = state.WeaponPic;
    }

    public void Spawn(TileCell cell)        //將生成物件移動至隨機空格
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTo(TileCell cell)       //方塊移動
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)        //方塊合成
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }

    IEnumerator Animate(Vector3 to, bool merging)       //方塊移動動畫Lerp
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging)
        {
            Destroy(gameObject);
        }
    }


    public void FullpageAni()
    {
        Vector3 RandomDirection;
        int a = Random.Range(0, 3);
        if (a == 0)
        {
            RandomDirection = Vector3.left;
        }
        else if (a == 1)
        {
            RandomDirection = Vector3.right;
        }
        else
        {
            RandomDirection = Vector3.down;
        }

        StartCoroutine(FullpageLeave(RandomDirection, 5.0f, 1.0f));
    }

    IEnumerator FullpageLeave(Vector3 Direction, float moveSpeed, float fadeDuration)
    {
        float elapsedTime = 0;
        Color startColor = GetComponent<SpriteRenderer>().color;
        Color targetColor = startColor;
        targetColor.a = 0;

        while (elapsedTime < fadeDuration)
        {
            Debug.Log(elapsedTime);
            // 在渐变曲线上插值
            float t = elapsedTime / fadeDuration;

            // 移动
            transform.Translate(Direction * moveSpeed * Time.deltaTime);

            // 设置透明度
            GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, targetColor, t);

            // 更新计时器
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
