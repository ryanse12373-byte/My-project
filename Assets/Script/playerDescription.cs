using UnityEngine;

public class playerDescription : MonoBehaviour
{
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private float range;
    private Creature selectedCreature;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Debug.DrawRay(playerOrientation.position + playerOrientation.forward , playerOrientation.forward * range, Color.red);

            if (Physics.Raycast(playerOrientation.position + playerOrientation.forward , playerOrientation.forward, out hit, range))
            {
                Creature creature = hit.collider.GetComponent<Creature>();
                if(creature != null)
                {
                    CreatureDescription.Instance.instantiateDescription(creature);
                    if (!CreatureDescription.Instance.description.activeSelf)
                    {
                        CreatureDescription.Instance.ShowDescription(creature);
                        selectedCreature = creature;
                    }
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CreatureDescription.Instance.description.activeSelf)
            {
                CreatureDescription.Instance.HideDescription(selectedCreature);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerOrientation.position + playerOrientation.forward , playerOrientation.forward, out hit, range))
            {
                Creature creature = hit.collider.GetComponent<Creature>();
                if(creature != null)
                {
                    creature.health.Die();
                }
                
            }
        }
    }
}
