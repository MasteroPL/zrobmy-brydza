using UnityEngine.UI;

namespace Assets.Code.UI
{
    class ScrollRect_fix : ScrollRect
    {
        override protected void LateUpdate()
        {
            base.LateUpdate();
            if (this.verticalScrollbar)
            {
                this.verticalScrollbar.size = 0f;
            }
        }

        override public void Rebuild(CanvasUpdate executing)
        {
            base.Rebuild(executing);
            if (this.verticalScrollbar)
            {
                this.verticalScrollbar.size = 0f;
            }
        }
    }
}