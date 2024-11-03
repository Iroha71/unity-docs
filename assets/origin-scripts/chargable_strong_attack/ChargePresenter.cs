using UnityEngine;

[DefaultExecutionOrder(6)]
public class ChargePresenter : MonoBehaviour
{
    [SerializeField]
    private ChargeView view;
    [SerializeField]
    private StrongAttackCharger model;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RegistEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RegistEvents()
    {
        model.OnChangedValue += view.UpdateCircle;
        model.OnChangedChargeLevel += view.UpdateChargeLevel;
    }
}
