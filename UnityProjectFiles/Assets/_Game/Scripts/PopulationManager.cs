using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PopulationManager : MonoBehaviour
{

    [SerializeField] private Agent agent;
    [SerializeField] private WorldState worldState;
    private List<Genes> agentGenes = new List<Genes>();
    
    //UI
    [SerializeField] private Text bestGeneText;
    [SerializeField] private GameObject contentHolder;
    [SerializeField] private GameObject currentGenPrefab;
    
    
    private Dictionary<Genes, float> geneRankingCost = new Dictionary<Genes, float>();
    private Dictionary<Genes, Dictionary<Action, int>> geneRankingHistogramm = new Dictionary<Genes, Dictionary<Action, int>>();
    
    private List<Genes> geneRankingList = new List<Genes>();
    
    private (Genes, float) currentBestGene = (null, Single.MaxValue);

    private int currentGeneration = 0;
    
    
    [Header("Algorithmen Choices")]
    [SerializeField] private int generationCount = 100;
    [SerializeField] private int populationCount = 50;
    [SerializeField] private List<DynamicBoolArray> worldStateChanges = new List<DynamicBoolArray>();
    [SerializeField] private List<Action> actionEstimate = new List<Action>();
    [SerializeField] private float mutationChance = 0.1f;
    [SerializeField] private float meanMulti = 1.0f;
    [SerializeField] private float rankMulti = 1.0f;

    void Start()
    {
       InitPopulation();
       agent.ResetAgent();
       
    }
    
    public void InitPopulation()
    {
        agentGenes = new List<Genes>();
        currentGeneration = 0;
        for (int i = 0; i < populationCount; i++)
        {
            agentGenes.Add(new Genes(agent.AvailableActions.Count, worldState.CurrentWorldState.Count * worldState.CurrentWorldState.Count));
        }
    }

    public void ResetGeneration()
    {
        geneRankingHistogramm.Clear();
        geneRankingCost.Clear();

        foreach (var gene in agentGenes)
        {
            gene.Randomize();
        }
    }
    

    private void ApplyPermutationToWorldState(int index)
    {
        int i = 0;
        foreach (var state in worldState.CurrentWorldState.Keys.ToList())
        {
            worldState.CurrentWorldState[state] = worldStateChanges[index].values[i];
            i++;
        }
    }

    private void ApplyNewCosts(Genes genToApply)
    {
        for (int i = 0; i < genToApply.Costs.Count; i++)
        {
            agent.AvailableActions[i].ActionCost = genToApply.Costs[i];
        }
    }


    private void RankGenes()
    {
        geneRankingList = geneRankingCost.OrderBy(x => x.Value).
            ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
    }

    private void CrossoverAll()
    {
        int randomCrossoverPoint = Random.Range(1, agent.AvailableActions.Count);

        Genes fittest = new Genes(geneRankingList[0].Costs);
        Genes secondFittest = geneRankingList[1];
        fittest.Crossover(secondFittest, randomCrossoverPoint);

        foreach (var aGene in agentGenes)
        {
            aGene.Costs = new List<int>(fittest.Costs);
        }

    }
    

    public void GenerateFitnessLevel(Genes gene, Dictionary<Action, int> histogram)
    {
        float mean = (float)worldStateChanges.Count / actionEstimate.Count;
        float fitnessScore = 0;


        foreach (var entry in histogram.Keys.ToList())
        {
            fitnessScore += Mathf.Pow(2, Mathf.Abs(histogram[entry] - mean)) * meanMulti;
        }
        
        var histogramRanking = histogram.OrderBy(x => x.Value).
            ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
        
        foreach (var entry in histogram.Keys.ToList())
        {
            fitnessScore += Mathf.Pow(2,Mathf.Abs(actionEstimate.IndexOf(entry) - histogramRanking.IndexOf(entry))) * rankMulti;
        }

    
        geneRankingCost.Add(gene, fitnessScore);
        geneRankingHistogramm.Add(gene, histogram);
    }
    
    private void MutateAll()
    {
        foreach (var gene in agentGenes)
        {
            gene.Mutate(mutationChance);
        }
    }

    private void MakeEntry()
    {
        var tmp = Instantiate(currentGenPrefab, contentHolder.transform);
        Text tmpText = tmp.GetComponentInChildren<Text>();

        float val = geneRankingCost[geneRankingList[0]];

        if (val < currentBestGene.Item2)
        {
            currentBestGene = (new Genes(geneRankingList[0].Costs), val);
            
            bestGeneText.text = "Current Best Gene " + "\nGeneration: " + currentGeneration + "\nFitness Score: " + val;
        }
        
        

        tmpText.text = "Generation: " + currentGeneration + "\nBest Gene \nwith Fitness Score: " + val;

        foreach (var pair in geneRankingHistogramm[geneRankingList[0]])
        {
            tmpText.text += "\n" + pair.Key.ActionName + "(" + pair.Key.ActionCost + ")" + ": " + pair.Value + "x";
        }
        
        tmpText.text += "\nWorst Gene \nwith Fitness Score: " + geneRankingCost[geneRankingList[geneRankingList.Count-1]];

        foreach (var pair in geneRankingHistogramm[geneRankingList[geneRankingList.Count-1]])
        {
            tmpText.text += "\n" + pair.Key.ActionName + "(" + pair.Key.ActionCost + ")" + ": " + pair.Value + "x";
        }

        float averageCost = 0.0f;

        foreach (var value in geneRankingCost.Values)
        {
            averageCost += value;
        }

        averageCost /= geneRankingCost.Count;

        tmpText.text += "\nWith Average Fitness Score: " + averageCost;

    }

    private void ResetUI()
    {
        bestGeneText.text = "Current best Gene:";
        currentBestGene = (null, Single.MaxValue);
        
        foreach (Transform child in contentHolder.transform) {
            Destroy(child.gameObject);
        }
    }


    public void StartGA()
    {
        ResetUI();
        StopAllCoroutines();
        StartCoroutine(StartGeneticAlgorithm());
    }

    private IEnumerator StartGeneticAlgorithm()
    {

        currentGeneration = 0;
        for (int o = 0; o < generationCount; o++)
        {
            ResetGeneration();
            currentGeneration++;
            
            foreach (var aGen in agentGenes)
            {
                Dictionary<Action, int> actionHistogramm = new Dictionary<Action, int>();
                foreach (var action in actionEstimate)
                {
                    actionHistogramm.Add(action, 0);
                }

                ApplyNewCosts(aGen);

                for (int i = 0; i < worldStateChanges.Count; i++)
                {
                    ApplyPermutationToWorldState(i);
                    var returnList = agent.CreatePlan(worldState);
                    

                    foreach (var actionLists in returnList)
                    {
                        foreach (var action in actionLists)
                        {
                            if(actionHistogramm.ContainsKey(action))
                                actionHistogramm[action] += 1;
                        }
                    }
                }

                GenerateFitnessLevel(aGen, actionHistogramm);
                
            }
            
            RankGenes();
            CrossoverAll();
            MutateAll();

            MakeEntry();
            yield return null;
        }

        yield return null;
    }
    

}
