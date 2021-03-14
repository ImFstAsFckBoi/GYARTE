namespace GYARTE.menu
{
    public interface IMenu
    {
        string Name { get; }
        bool IsHovering { get; set; }

        void OnClick();
    }
} 