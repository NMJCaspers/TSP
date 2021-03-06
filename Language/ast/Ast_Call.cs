﻿// Copyright (c) 2020-2021 Marco Caspers. All Rights Reserved.
// Copyright (c) 2021 DiBiAsi Software.
// Licensed under the Mozila Public License, version 2.0.

using System.Text;

namespace Language
{

    public enum CallType { Function, Procedure, Lambda };
    public class Ast_Call : Ast_Base
    {
        public CallType CallType { get; set; }
        private readonly Libraries Libraries;
        public string Name
        {
            get
            {
                return Token?.Lexeme;
            }
        }
        public Ast_Call(Token token, Libraries libraries) : base(token)
        {
            Type = AstType.Call;
            Libraries = libraries;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"call {Name}(");
            if (Block?.Count > 0)
            {
                foreach (Ast_Base arg in Block)
                {
                    sb.Append(arg.ToString().Trim());
                    sb.Append(',');
                }
                if (sb.Length > 1)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            sb.Append(')');
            return sb.ToString();
        }

        public override dynamic Execute(Ast_Scope scope)
        {
            Ast_Lambda exec;
            Ast_Scope execScope = scope;
            if (scope.VariableExists(Name))
            {
                var v = scope.GetVariable(Name);
                var value = v.Value;
                exec = value.Value;
                //if (Name.Contains('.'))
                //{
                //    execScope = scope.GetStructScope(Name);
                //}
            }
            else
            {
                exec = Libraries.GetMethodOrFunction(Name);
                exec.Execute(execScope); // run the prepwork.
            }
            if (exec == null)
            {
                throw new SyntaxError(Token, $"Function or procedure not found ({Name}).");
            }

            if (Block?.Count != exec.Args.Count)
            {
                if (!exec.Args.ContainsParams())
                {
                    throw new SyntaxError(Token, "Invalid number of parameters.");
                }
            }

            var i = 0;
            foreach (Ast_Expression expr in Block)
            {
                var expressionValue = expr.Execute(execScope);

                if (exec.Args[i].Value.Type == ValueType.Params)
                {
                    exec.Args[i].Value.Value.Add
                        (new VT_Any { Value = expressionValue });
                }
                else
                {
                    exec.Args[i].DoSetValue(expressionValue);
                    i++;
                }
            }

            if (exec.Type == AstType.Function || exec.Type == AstType.Procedure)
            {
                return exec.ExecuteCall(execScope);
            }
            return exec.Execute(execScope);
        }
    }
}
