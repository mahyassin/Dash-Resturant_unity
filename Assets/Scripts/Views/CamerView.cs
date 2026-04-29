using UnityEngine;

public class CamerView : MonoBehaviour
{
    [SerializeField] private int speed;
    private int _intialV = 0;
    private IViewable target;

    void Update()
    {
        if(target == null) return;
        var dis = (target.Pos - transform.position).magnitude;

        transform.position = Vector3.MoveTowards(transform.position, target.Pos, speed * dis * Time.deltaTime);
    }

    public void FollowTarget(IViewable targetToFollow)
    {
        target = targetToFollow;
    }
}

public interface IViewable
{
    public Vector3 Pos {get;}
}

