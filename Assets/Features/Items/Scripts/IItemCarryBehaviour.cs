namespace Features.Items.Scripts
{
    public interface IItemCarryBehaviour
    {
        bool IsCarrying();
        bool CanCarryMore();
        BaseItem_SO GetNextCarried();
        void DropItem(BaseItem_SO droppedItemType);
        void PickupItem(BaseItem_SO newBaseItem);
    }
}
