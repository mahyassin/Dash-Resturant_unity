public class FactoryContext
{
    public ViewFactory ViewFactory;
    public EntitiesFactory EntitiesFactory;
    public Identfier Identfier;
    public ViewsRigistry ViewsRigistry;

    public FactoryContext(ViewFactory viewFactory, EntitiesFactory entitiesFactory, Identfier identfier, ViewsRigistry viewsRigistry)
    {
        ViewFactory = viewFactory;
        EntitiesFactory = entitiesFactory;
        Identfier = identfier;
        ViewsRigistry = viewsRigistry;
    }
}