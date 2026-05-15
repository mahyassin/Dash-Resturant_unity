using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerView : MonoBehaviour, ITileView, IcontainerView
{
    [SerializeField] private Transform anchor;

    public Transform Anchor => anchor;

    public Type Type => Type.Container;
    public ContentPubleView contentPubleView;

    public ContentPubleView PubleView => contentPubleView;

}

public interface IcontainerView
{
    public ContentPubleView PubleView {get;}
}
