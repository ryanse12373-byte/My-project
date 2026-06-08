using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MuscleManager : MonoBehaviour
{
    private DecalProjector decal;
    [SerializeField] private HumanoidStates state;
    Material matInstance;

void Awake()
{
    decal = GetComponent<DecalProjector>();
    matInstance = Instantiate(decal.material);
    decal.material = matInstance;
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
