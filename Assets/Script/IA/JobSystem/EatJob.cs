using UnityEngine;

public class EatJob : AIJob
{
    private NeedsComponent needs;

    public EatJob(
        AIController controller,
        NeedsComponent needs)
        : base(controller)
    {
        this.needs = needs;
    }

    public override float GetPriority()
    {
        return needs.hunger;
    }

    public override bool CanRun()
    {
        return needs.hunger > 20;
    }

    public override void Start()
    {
        Debug.Log("Commence à manger");
    }

    public override void Tick()
    {
        needs.hunger -= Time.deltaTime * 10;
    }

    public override void Stop()
    {
        Debug.Log("je v me mettre la kipa dans le q si c cette ligne de code qui fait tout bugger");
    }
}