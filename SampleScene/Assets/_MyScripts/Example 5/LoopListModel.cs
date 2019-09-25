using UnityEngine;

namespace _MyScripts.Example_5
{
    public class LoopListModel
    {
        private Sprite iconSprite;

        public Sprite IconSprite
        {
            get => iconSprite;
            set => iconSprite = value;
        }

        public LoopListModel()
        {
            
        }

        public LoopListModel(Sprite icon)
        {
            IconSprite = icon;
        }
    }
}
