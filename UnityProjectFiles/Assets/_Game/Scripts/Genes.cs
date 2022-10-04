using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genes 
{
    private List<int> costs = new List<int>();

    public List<int> Costs
    {
        get => costs;
        set => costs = value;
    }

    private int randomUpperRange = 0;


    public Genes(int geneLength, int upperRange)
    {
        randomUpperRange = upperRange;
        for (int i = 0; i < geneLength; i++)
        {
            costs.Add(Random.Range(0, randomUpperRange));
        }
    }
    

    public Genes(List<int> newGenPath)
    {
        costs.Clear();
        for (int i = 0; i < newGenPath.Count; i++)
        {
            costs.Add(newGenPath[i]);
        }
    }

    public void Randomize()
    {
        int geneLength = Costs.Count;
        Costs.Clear();
        for (int i = 0; i < geneLength; i++)
        {
            costs.Add(Random.Range(0, randomUpperRange));
        } 
    }

    

    public void Mutate(float mutationChance)
    {
        for (int i = 0; i < costs.Count; i++)
        {

            if (Random.Range(0.0f, 1.0f) <= mutationChance)
            {
                costs[i] = Random.Range(0, randomUpperRange);
            }
        }
    }

    public void Crossover(Genes other, int crossoverPoint)
    {
        var newGenPath = costs.GetRange(0, crossoverPoint); 
        newGenPath.AddRange(other.costs.GetRange(crossoverPoint, other.costs.Count-crossoverPoint));
        costs = newGenPath;
    }
}
