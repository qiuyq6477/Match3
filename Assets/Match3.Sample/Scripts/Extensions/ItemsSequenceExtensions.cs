﻿using System.Collections.Generic;



namespace Match3
{
    public static class ItemsSequenceExtensions
    {
        public static IEnumerable<TGridSlot> GetUniqueSolvedGridSlots<TGridSlot>(this SolvedData<TGridSlot> solvedData,
            bool onlyMovable = false) where TGridSlot : IGridSlot
        {
            var solvedGridSlots = new HashSet<TGridSlot>();

            foreach (var solvedGridSlot in solvedData.GetSolvedGridSlots(onlyMovable))
            {
                if (solvedGridSlots.Add(solvedGridSlot) == false)
                {
                    continue;
                }

                yield return solvedGridSlot;
            }

            solvedGridSlots.Clear();
        }
    }
}