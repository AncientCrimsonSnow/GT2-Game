using Features;

public class RestorableFocus_SO<T> : Focus_SO<T>
{
    private T _restoreValue;
    
    public override void Restore()
    {
        if (_restoreValue != null)
        {
            SetFocus(_restoreValue);
        }
        else
        {
            base.Restore();
        }
    }
    
    public void SetAsRestore(T restoreValue)
    {
        _restoreValue = restoreValue;
    }

    public void SetCurrentAsRestore()
    {
        _restoreValue = Focus;
    }
}
