using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//quicc draft for the inflation game idea
//Speculating the very near future regarding the ongoing inflation 
//possibly prequel to Human error,  where the society collapses at the end and tech companies rises 

public class InflationSimTest : MonoBehaviour
{
    //time & action limit
    public int cycle = 0; // convert into day> week > month > year
    public int AP = 3; //renews every cycle (depending on ur health)

    //player stats (simplified)
    public float health = 10f; //can be divided to different body parts
    public float sanity =  10f;// can be divided to different emotions?? like morale,self-actualization,social belonging etc
    public float money = 2000f; // regular standard currency. stock / cryptocurrency will be added later. inflates over time

    //stats variables (simplified)
    //rn it's default to "daily" but
    public float income = 80f; // goes up linearly
    public float expense = 40f; //goes up exponentially 

    //piles up this cycle 
    public float moneyThisCycle;
    public float healthThisCycle;
    public float sanityThisCycle;

    //inflation variables
    public float inflationRate = 1.025f;
    public float inflationAccelerationRate = 2f; //the rate at which the inflation accelerates 
    public int inflationRaiseFreq = 5; // inflation rate raises every x cycles 
    public int inflationTriggerFreq = 3; //inflation happens every x cycles
    public float turbulanceRate = 0.15f; //likelyhood of inflationraiseRate lowering at the end of each cycle
    

    void Start()
    {
    }

    //stable income
    public void Work()
    {
        //if earning is ok
        if(income/expense > 1.5f)
        {
            GainStats(income,0,0.2f);
        }else if(income/expense <1.5f&& income / expense > 1.0f) //just enough to get by
        {
            GainStats(income,0,0f); //no morale boost
        }else if (income<expense)
        {
            GainStats(income, -0.5f, -0.5f); //not earning enough, health suffers (hard coding this for now) implement expense choice later
        }

    }

    //risky income
    public void Invest(float riskIndicator, float moneyIn)
    {
        turbulanceRate += riskIndicator * 0.2f; //the more you engage in risky investment, the faster inflation rate accelerates
    }


    //basic Gamble model for risky income like investments
    public float Gamble(float riskIndicator, float moneyIn)
    {
        float moneyOut = riskIndicator * Random.Range(-15, 10) * moneyIn/10 + moneyIn;
        if(moneyOut <= 0)
        {
            moneyOut = 0;
        }
        return moneyOut;
    }

    //changes stats 
    public void GainStats(float moneyGain, float healthGain, float sanityGain)
    {
        //rounding
        moneyGain = Mathf.Round(moneyGain*100f)/100f;
        moneyGain = Mathf.Round(moneyGain * 100f) / 100f;
        moneyGain = Mathf.Round(moneyGain * 100f) / 100f;

        moneyThisCycle += moneyGain;
        healthThisCycle += healthGain;
        sanityThisCycle += sanityGain;

        Debug.Log("money: "+ moneyGain +"; health: "+healthGain+"; sanity: ");
    }

    public void NewCycle()
    {
        //cycle accumulation clears
        moneyThisCycle = 0;
        healthThisCycle = 0;
        sanityThisCycle = 0;

        //changes to the economy 
        if (cycle < 5)
        {
            //do nothing, everything is ok at first to give a false sense of stability
        }
        else if (cycle >5 && cycle < 10)
        {
            //inflation starts
            expense *= inflationRate;
            expense= Mathf.Round(expense*100f)/100f;//round to 2decimals
        }
        else if (cycle > 10 && cycle <15)
        {
            
        }
    }

    public void Inflate()
    {
        if (cycle % inflationTriggerFreq==0)
        {
            expense *= inflationRate;
        }

        if (cycle % inflationRaiseFreq == 0)
        {
            inflationRate *= inflationAccelerationRate;
        }


    }

}
