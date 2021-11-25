using System.Linq.Expressions;

namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a for command
    /// </summary>
    public class ForCommandNode : ICommandNode
    {
        /// <summary>
        /// The initialiser associated with the loop
        /// </summary>
        public ICommandNode Initialisation { get; }

        /// <summary>
        /// The condition associated with the loop
        /// </summary>
        public IExpressionNode Condition { get; }
        
        /// <summary>
        /// The update associated with the loop
        /// </summary>
        public ICommandNode Update { get; }

        /// <summary>
        /// The command inside the loop
        /// </summary>
        public ICommandNode Command { get; }
        
        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Creates a new for node
        /// </summary>
        /// <param name="expression">The condition associated with the loop</param>
        /// <param name="command">The command inside the loop</param>
        /// <param name="position">The position in the code where the content associated with the node begins</param>
        public ForCommandNode(ICommandNode initialisation, IExpressionNode condition, ICommandNode update, ICommandNode command, Position position)
        {
            Initialisation = initialisation;
            Condition = condition;
            Update = update;
            Command = command;
            Position = position;
        }
    }
}