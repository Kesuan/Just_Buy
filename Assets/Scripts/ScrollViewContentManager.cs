using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewContentManager : MonoBehaviour
{
    private RectTransform rectTransform;
    private float childHeight;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        childHeight = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;

        float height = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            height += childHeight;
        }
        height += 192f;

        Debug.Log(rectTransform.sizeDelta.y);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }
}
