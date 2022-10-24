using UnityEngine.UIElements;

namespace Game.AI.BehaviorTree.Window
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits>
        {

        }

        public SplitView()
        {
            this.StretchToParentSize();
        }
    }
}
