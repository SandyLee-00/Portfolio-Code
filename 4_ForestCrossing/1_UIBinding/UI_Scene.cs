
/// <summary>
/// Scene에 기본적으로 배치될 UI
/// </summary>
public class UI_Scene : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.SetCanvas(gameObject, false);
        return true;
    }
}