using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YantraJS.Core;

namespace Esprima.Ast
{
    public class HoistingScope
    {

        public SparseList<VariableDeclaration> VariableDeclarations { get; } 
            = new SparseList<VariableDeclaration>();

        public SparseList<ClassDeclaration> ClassDeclarations { get; }
            = new SparseList<ClassDeclaration>();

        public SparseList<FunctionDeclaration> FunctionDeclarations { get; }
            = new SparseList<FunctionDeclaration>();

    }
}
