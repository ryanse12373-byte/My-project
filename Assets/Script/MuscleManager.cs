using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuscleManager : MonoBehaviour
{
    private DecalProjector decal;
    [SerializeField] private HumanoidStates state;


    void Awake()
    {
        decal = GetComponent<DecalProjector>();
    }

    void updateMuscle()
    {
        decal.fadeFactor= state.strenght/100;
    }
    void Start()
    {
        updateMuscle();
    }
}
