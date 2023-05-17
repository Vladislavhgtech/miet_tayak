using System;
using Lab4_5.Interpreter.Primitives;

namespace Lab4_5.Interpreter
{
    internal class Scan : IExpression
    {
        private Identifier Id;
        private int Value;

        public Scan(Identifier identifier, int value)
        {
            Id = identifier;
            Value = value;
        }

        public int Interpret(Context context)
        {
            context.SetVariable(Id.GetId(), Value);
            return 0;
        }
    }
}
