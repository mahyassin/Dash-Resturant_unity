using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEditor.PackageManager;

public class ViewsRigistry
{
    private Dictionary<int , ITileView> _views = new();


    public void AddView(int id, ITileView view)
    {
        if (view == null) return;
        _views.Add(id, view);
    }


    public ITileView GetOnTile(int id)
    {
        if (id == -1) return null;
        return _views[id];


    }
   
}