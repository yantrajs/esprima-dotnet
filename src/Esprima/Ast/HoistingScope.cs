using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esprima.Ast
{
    public class HoistingScope
    {

        public List<VariableDeclaration> VariableDeclarations { get; } 
            = new List<VariableDeclaration>();

        public List<ClassDeclaration> ClassDeclarations { get; }
            = new List<ClassDeclaration>();

        public List<FunctionDeclaration> FunctionDeclarations { get; }
            = new List<FunctionDeclaration>();

    }
}
