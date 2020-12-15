

namespace Steamwar.Interaction
{
    /// <summary>
    /// Listens to selection updates.
    /// 
    /// Currently used for updating the unit info UI and the move nodes.
    /// </summary>
    public interface ISelectionListener
    {
        /// <summary>
        /// Called if an object gets selected
        /// </summary>
        /// <param name="data">The new selction data</param>
        /// <param name="oldData">The selection data of the previous selected object.</param>
        void OnSelection(SelectionData data, SelectionData oldData);

        /// <summary>
        /// Called if an object gets deselected
        /// </summary>
        /// <param name="oldData"></param>
        void OnDeselection(SelectionData oldData);

        /// <summary>
        /// Called if an object is selected and the player interacts with the world.
        /// 
        /// Can be used to move a unit or to attack an object.
        /// </summary>
        /// <param name="data">The currently selected data</param>
        /// <param name="deselect">If the object should be deselected after the interaction.</param>
        /// <param name="context">The context of the interaction</param>
        /// <returns>True if the listener interacted with the world.</returns>
        bool OnInteraction(SelectionData data, InteractionContext context, out bool deselect);

        /// <summary>
        /// Called if an object is selected and the mouse moves.
        /// </summary>
        /// <param name="data"></param>
        void OnSelectionMouseMove(SelectionData data);
    }
}
