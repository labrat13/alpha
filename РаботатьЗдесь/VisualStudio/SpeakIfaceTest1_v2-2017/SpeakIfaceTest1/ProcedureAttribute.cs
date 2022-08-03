using System;
using System.Collections.Generic;
using System.Text;

namespace Operator
{
    /// <summary>
    /// Element type mark as code realization stage
    /// </summary>
    public enum ImplementationState
    {
        /// <summary>
        /// Method realization in progress
        /// </summary>
        NotRealized = 0,
        /// <summary>
        /// Method, class, assembly is realized, but not tested
        /// </summary>
        NotTested,
        /// <summary>
        /// Method, class, assembly is full tested and ready to use
        /// </summary>
        Ready,
    }

    /// <summary>
    /// Mark element (assembly, class, method) as a method implementation element with specify an implementation stage
    /// </summary>
    /// <remarks>This attribute marks assemblies, classes and functions as usable in method execution process.
    /// If assembly, class, function has this attribute, user can view and select as method realization.</remarks>
    public class ProcedureAttribute : Attribute
    {
        private ImplementationState elem;

        /// <summary>
        /// Params constructor
        /// </summary>
        /// <param name="ie">one of ImplementationElement values</param>
        public ProcedureAttribute(ImplementationState ie)
        {
            elem = ie;
        }
        /// <summary>
        /// Implementation element type
        /// </summary>
        public ImplementationState ElementValue
        {
            get { return elem; }
            set { elem = value; }
        }


    }
}
