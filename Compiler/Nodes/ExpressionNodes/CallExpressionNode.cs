namespace Compiler.Nodes
{
    /// <summary>
    /// A node corresponding to a call expression
    /// </summary>
    public class CallExpressionNode : IExpressionNode
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public IdentifierNode Identifier { get; }

        /// <summary>
        /// The parameter
        /// </summary>
        public IParameterNode Parameter { get; set; }
        
        /// <summary>
        /// The type of the node
        /// </summary>
        public SimpleTypeDeclarationNode Type { get; set; }
        
        /// <summary>
        /// The position in the code where the content associated with the node begins
        /// </summary>
        public Position Position { get { return Identifier.Position; } }

        /// <summary>
        /// Creates a new call expression node
        /// </summary>
        /// <param name="identifier">The identifier</param>
        public CallExpressionNode(IdentifierNode identifier, IParameterNode parameter)
        {
            Identifier = identifier;
            Parameter = parameter;
        }
    }
}