
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _MyScripts.Example_5
{
    
    public class LoopList : MonoBehaviour
    {

        private int _containCountMax;  //View最多显示item数量
        private float _viewHeight;     //视图整体高度
        private float _itemSpacingY;    //item之间的y轴间距
        private int _itemCount;        //item 总数量
        private RectTransform myRect;
        private RectTransform parentRect;
        private GameObject _itemPrefab;      //item预制体资源
        private Sprite[] _itemSprites;
        private List<LoopListItem> _items;
        private LoopListModel curItemModel;

        private float curScrollY;
        private int curIndex;
        private void Start()
        {
            _itemSpacingY = 0;
            curIndex = 0;
            _itemCount = 14;
            curScrollY = 1;
            
            _items=new List<LoopListItem>();
            _itemPrefab = Resources.Load<GameObject>("LoopListItem");
            _itemSprites = Resources.LoadAll<Sprite>("Icons/");
            
            myRect = GetComponent<RectTransform>();
            parentRect = myRect.Find("Viewport/Content").GetComponent<RectTransform>();
            _viewHeight = myRect.rect.height;
            float itemHeight = _itemPrefab.GetComponent<RectTransform>().rect.height;
            _containCountMax = GetItemMaxCount(_viewHeight,itemHeight);
            

            InitContentSize(parentRect,_itemCount,itemHeight,_itemSpacingY);
            CreateItems(_containCountMax, parentRect, _itemSpacingY, itemHeight);

            //添加滚动逻辑
            myRect.GetComponent<ScrollRect>().onValueChanged.AddListener(v2 =>
            {
                if (v2.y > curScrollY)
                {      LoopItems(itemHeight, _itemSpacingY,Direction.down);}
                else if (v2.y < curScrollY)
                {
                    LoopItems(itemHeight, _itemSpacingY,Direction.up);
                }

                curScrollY = v2.y;
            });

        }
        /// <summary>
        /// content尺寸适应
        /// </summary>
        /// <param name="parentRect"></param>
        /// <param name="itemCount"></param>
        /// <param name="itemHeight"></param>
        /// <param name="itemSpacing"></param>
        private void InitContentSize(RectTransform parentRect,int itemCount,float itemHeight,float itemSpacing)
        {
            parentRect.sizeDelta=new Vector2(parentRect.sizeDelta.x,itemCount*(itemHeight+itemSpacing));
        }
         /// <summary>
         /// 获取最大创建item数量
         /// </summary>
         /// <param name="viewHeight"></param>
         /// <param name="itemHeight"></param>
         /// <returns></returns>
        private int GetItemMaxCount(float viewHeight,float itemHeight)
        {
           return  Mathf.CeilToInt(viewHeight / (itemHeight+_itemSpacingY));
        }

        /// <summary>
        /// 创建Item
        /// </summary>
        /// <param name="count"></param>
        /// <param name="parent"></param>
        /// <param name="itemSpacing"></param>
        /// <param name="itemHeight"></param>
        private void CreateItems(int count,Transform parent,float itemSpacing,float itemHeight)
        {
            LoopListItem item = null;
            Vector2 tempPos=new Vector2();
            for (int i = 0; i < count; i++)
            {
                item = Instantiate(_itemPrefab, parent).AddComponent<LoopListItem>();
                tempPos=new Vector2(0,0-i*(itemSpacing+itemHeight));
                item.SetPos(tempPos);
                item.SetId(i);
                item.MyModel = curItemModel;
                item.SetImage();
                _items.Add(item);
            }
        }
       
        /// <summary>
        /// 设置当前需要改变model数据的Item新model数据（暂存字段中）
        /// </summary>
        /// <param name="index"></param>
        private void SetCurItemModel(int index)
        {
            if(index<0||index>=_itemCount)
                  curItemModel=new LoopListModel();
            else
                curItemModel=new LoopListModel(_itemSprites[index]);
        }


       /// <summary>
       /// 滚动视图主逻辑
       /// </summary>
       /// <param name="itemHeight"></param>
       /// <param name="itemSpacing"></param>
       /// <param name="dir"></param>
        private void LoopItems(float  itemHeight,float itemSpacing,Direction dir)
        {
            int index=Mathf.FloorToInt(parentRect.anchoredPosition.y / (itemSpacing + itemHeight));
            if (curIndex == index) return;
            curIndex = index;
            Debug.Log(index);
            int offset = 0;
            if(index>=0&&index<=(_itemCount-_containCountMax-1))
            {
                int startId = index;
                int endId = index + _containCountMax-1;
                if (dir == Direction.up)
                {
                    offset = startId - (_items[index-1].MyId) - 1;
                    Vector2 tempPos = new Vector2(0, 0 - (endId-offset) * (itemSpacing + itemHeight));
                    _items[index-1].SetPos(tempPos);
                    _items[index-1].SetId(endId-offset);
                    _items[index-1].MyModel = curItemModel;
                    _items[index-1].SetImage();
                }
                else
                {
                    foreach (var item in _items)
                    {
                        if(item.MyId==endId+1)
                        {
                           offset = item.MyId - endId - 1;
                            Vector2 tempPos = new Vector2(0, 0-(startId+offset)*(itemSpacing + itemHeight));
                            item.SetPos(tempPos);
                            item.SetId(startId+offset);
                            item.MyModel = curItemModel;
                            item.SetImage();
                            return;
                        }
                    }

                }
            }
        }

        
        
    }
}

/// <summary>
/// 滚动方向
/// </summary>
enum Direction
{
    up,
    down
}
