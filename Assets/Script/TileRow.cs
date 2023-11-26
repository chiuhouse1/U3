using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] cells { get; set; }
    private void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();
    }
}
