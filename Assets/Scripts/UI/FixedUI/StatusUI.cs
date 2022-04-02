using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    private StatsObject playerStats;

    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider hungerSlider;
    public Slider thirstSlider;

    void Start()
    {
        healthSlider.value = playerStats.statuses[StatusType.HP].Percentage;
        staminaSlider.value = playerStats.statuses[StatusType.SP].Percentage;
        hungerSlider.value = playerStats.statuses[StatusType.Hunger].Percentage;
        thirstSlider.value = playerStats.statuses[StatusType.Thirst].Percentage;
    }

    private void OnEnable()
    {
        playerStats.OnStatChanged += OnStatChanged;
    }

    private void OnDisable()
    {
        playerStats.OnStatChanged -= OnStatChanged;
    }

    private void OnStatChanged(StatsObject obj)
    {
        healthSlider.value = playerStats.statuses[StatusType.HP].Percentage;
        staminaSlider.value = playerStats.statuses[StatusType.SP].Percentage;
        hungerSlider.value = playerStats.statuses[StatusType.Hunger].Percentage;
        thirstSlider.value = playerStats.statuses[StatusType.Thirst].Percentage;
    }
}
