using System;
using System.Collections.Generic;
using System.Linq;

public class OptimalAccountingBalance
{
    private List<int[]> bestTransactions;

    public int MinTransfers(List<int[]> transactions)
    {
        Dictionary<int, int> memberVsBalance = new Dictionary<int, int>();

        // Compute the overall balance (Incoming - Outgoing) for each member
        foreach (int[] txn in transactions)
        {
            int from = txn[0];
            int to = txn[1];
            int amount = txn[2];

            memberVsBalance[from] = memberVsBalance.GetValueOrDefault(from, 0) - amount;
            memberVsBalance[to] = memberVsBalance.GetValueOrDefault(to, 0) + amount;
        }

        List<int> balances = new List<int>();
        foreach (int amount in memberVsBalance.Values)
        {
            if (amount != 0)
            {
                balances.Add(amount);
            }
        }

        bestTransactions = new List<int[]>(); // Yeni liste oluşturuluyor
        FindMinimumTxns(balances, 0, new List<int[]>());

        Console.WriteLine("Minimum Transaction Count: " + bestTransactions.Count);
        PrintTransactions();

        return bestTransactions.Count;
    }

    private void FindMinimumTxns(List<int> balances, int currentIndex, List<int[]> currentTransactions)
    {
        if (currentIndex >= balances.Count)
        {
            if (currentTransactions.Count > bestTransactions.Count)
            {
                bestTransactions = new List<int[]>(currentTransactions);
            }
            return;
        }

        int currentVal = balances[currentIndex];
        if (currentVal == 0)
        {
            FindMinimumTxns(balances, currentIndex + 1, currentTransactions);
            return;
        }

        for (int txnIndex = currentIndex + 1; txnIndex < balances.Count; txnIndex++)
        {
            int nextVal = balances[txnIndex];
            if (currentVal * nextVal < 0)
            {
                balances[txnIndex] = currentVal + nextVal;
                currentTransactions.Add(new int[] { currentIndex, txnIndex, Math.Min(currentVal, -nextVal) });

                FindMinimumTxns(balances, currentIndex + 1, currentTransactions);

                balances[txnIndex] = nextVal;
                currentTransactions.RemoveAt(currentTransactions.Count - 1);

                if (currentVal + nextVal == 0)
                {
                    break;
                }
            }
        }
    }

    private void PrintTransactions()
    {
        Console.WriteLine("Optimal Transactions:");

        foreach (int[] txn in bestTransactions)
        {
            int from = txn[0];
            int to = txn[1];
            int amount = txn[2];

            if (amount < 0)
            {
                Console.WriteLine($"From: {to}, To: {from}, Amount: {-amount}");
            }
            else
            {
                Console.WriteLine($"From: {from}, To: {to}, Amount: {amount}");
            }
        }
    }


    public static void Main(string[] args)
    {
        //0 ATA
        //1 İDİL
        //2 UĞUR
        //3 SERAV
        OptimalAccountingBalance minTxnsObj = new OptimalAccountingBalance();
        List<int[]> txns = new List<int[]> {
            new int[] { 0, 1, 300 },
            new int[] { 0, 2, 300 },
            new int[] { 0, 3, 300 },
            new int[] { 2, 0, 200 },
            new int[] { 2, 1, 200 },
        };

        int output = minTxnsObj.MinTransfers(txns);
        Console.WriteLine(output);
    }
}
