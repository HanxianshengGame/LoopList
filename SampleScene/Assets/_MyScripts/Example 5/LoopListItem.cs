using System;
using UnityEngine;
using UnityEngine.UI;

namespace _MyScripts.Example_5
{
    public class LoopListItem : MonoBehaviour
    {
        private RectTransform myRect;
        private Image _iconImage;   //子物体的image  作为父物体的icon显示
        private LoopListModel myModel; //自身数据模型
        private int myId;
        private Text _idText;

        private Text IdText
        {
            get
            {
                if (_idText==null)
                {
                    _idText = transform.Find("Image/Text").GetComponent<Text>();
                    
                }

                return _idText;

            }
        }
        public int MyId
        {
            get => myId;
            set => myId = value;
        }
        public LoopListModel MyModel
        {
            get => myModel;
            set => myModel = value;
        }

        private RectTransform MyRect
        {
            get
            {
                if (myRect == null)
                {
                    myRect = GetComponent<RectTransform>();
                }

                return myRect;
            }
        }

        private Image IconImage
        {
            get
            {
                if (_iconImage == null) _iconImage = transform.Find("Image").GetComponent<Image>();
                return _iconImage;
            }
        }

        public void SetPos(Vector2 pos)
        {
            MyRect.anchoredPosition = pos;
        }

        public void SetId(int id)
        {
            myId = id;
            IdText.text = id.ToString();
            SendMessageUpwards("SetCurItemModel", myId);
        }

        public void SetImage()
        {
            IconImage.sprite = MyModel.IconSprite;
        }
    }
}
