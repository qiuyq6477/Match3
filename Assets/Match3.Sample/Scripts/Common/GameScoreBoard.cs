using System.Text;
using UnityEngine;

namespace Match3
{
    public class GameScoreBoard : ISolvedSequencesConsumer<IUnityGridSlot>
    {
        public void OnSequencesSolved(SolvedData<IUnityGridSlot> solvedData)
        {
            foreach (var sequence in solvedData.SolvedSequences)
            {
                RegisterSequenceScore(sequence);
            }
        }

        private void RegisterSequenceScore(ItemSequence<IUnityGridSlot> sequence)
        {
            Debug.Log(GetSequenceDescription(sequence));
        }

        private string GetSequenceDescription(ItemSequence<IUnityGridSlot> sequence)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("ContentId <color=yellow>");
            stringBuilder.Append(sequence.SolvedGridSlots[0].Item.ContentId);
            stringBuilder.Append("</color> | <color=yellow>");
            stringBuilder.Append(sequence.SequenceDetectorType.Name);
            stringBuilder.Append("</color> sequence of <color=yellow>");
            stringBuilder.Append(sequence.SolvedGridSlots.Count);
            stringBuilder.Append("</color> elements");

            return stringBuilder.ToString();
        }
    }
}