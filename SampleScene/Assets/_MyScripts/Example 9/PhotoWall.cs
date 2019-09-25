using System.Collections;
using System.Collections.Generic;
using _MyScripts.Example_9;
using DG.Tweening;
using UnityEngine;

public class PhotoWall : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _itemPrefab;
    private RectTransform _itemRect;
    private float offsetY;
    private float offsetX;
    private List<PhotoWallItem> _photoItems;
    void Start()
    {
        offsetX = 5;
        offsetY = 5;
        _photoItems=new List<PhotoWallItem>();
        _itemPrefab = transform.Find("Image").gameObject;
        _itemRect = _itemPrefab.GetComponent<RectTransform>();
        var rect = _itemRect.rect;
        SpawnItems(_itemPrefab,rect.height,rect.width);
        Destroy(_itemPrefab);
    }
    
    
    private void SpawnItems(GameObject itemPrefab,float itemHeight,float itemWidth)
    {
        int id = 0;        
        int row = Mathf.FloorToInt(Screen.height / (itemHeight+offsetY));
        int column=Mathf.FloorToInt(Screen.width / (itemWidth+offsetX));
        Vector3 firstPos=new Vector3(Screen.width+itemWidth*0.5f,-itemHeight*0.5f);
        int totalNum = row * column;
        Vector3 curItemPos;
        PhotoWallItem itemTemp;
        RectTransform itemRect;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                curItemPos = firstPos + new Vector3(j * (offsetX + itemWidth), -i * (offsetY + itemHeight));
                itemRect=Instantiate<GameObject>(itemPrefab,transform).GetComponent<RectTransform>();
                itemRect.anchoredPosition = curItemPos;
                itemRect.DOAnchorPosX(curItemPos.x - Screen.width, 1.0f);
                itemRect.gameObject.AddComponent<CanvasGroup>();
                itemTemp = itemRect.gameObject.AddComponent<PhotoWallItem>();
                itemTemp.Init(id,column,totalNum,GetItem);
                _photoItems.Add(itemTemp);
                id++;
            }
            _photoItems[id - column].IsleftBorder = true;
            _photoItems[id - 1].IsRightBorder = true;
        }
    }

    
    private PhotoWallItem GetItem(int index)
    {
        if (index >= 0 && index < _photoItems.Count)
        {
            return _photoItems[index];
        }
        else
        {
            return null;
        }
    }
}
