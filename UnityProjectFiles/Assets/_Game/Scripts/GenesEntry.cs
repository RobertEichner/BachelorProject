using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenesEntry
{
    private Genes genes;
    private float fitnessValue;
    private List<(Action, int)> actionHistogram = new List<(Action, int)>();

    public GenesEntry(Genes genes, float fitnessValue, List<(Action, int)> actionHistogram)
    {
        this.genes = genes;
        this.fitnessValue = fitnessValue;
        this.actionHistogram = actionHistogram;
    }

    public Genes Genes => genes;

    public float FitnessValue => fitnessValue;

    public List<(Action, int)> ActionHistogram => actionHistogram;
}
